// See https://aka.ms/new-console-template for more information

using CommandLine;
using MailKit.Net.Imap;
using PhishyScanConsole.Models;
using Config.Net;
using MailKit;
using MimeKit;
using HtmlAgilityPack;
using PhishyScanConsole;
using Flurl.Http;
using System.Configuration;

const string PhishyScanEndpoint = "/scan";
const string DefaultConfigurationFile = "config.ini";
const int MaximumContentLength = 4097;
Parser.Default.ParseArguments<CommandLineArguments>(args).WithParsed(ops =>
{

    try
    {
        var configurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultConfigurationFile);

        if (File.Exists(configurationFile))
        {
            var configuration = new ConfigurationBuilder<IConfiguration>()
                .UseIniFile(configurationFile)
                .Build();

            if ((bool)configuration?.Verbose) Console.WriteLine($"Configuration loaded from file: {Path.GetFileName(configurationFile)}");

            Parser.Default.ParseArguments<CommandLineArguments>(args)
                .WithParsed(o =>
                {

                    ops.Host = o?.Host ?? configuration?.Host;
                    ops.UserName = o?.UserName ?? configuration?.UserName;
                    ops.Password = o?.Password ?? configuration?.Password;
                    ops.Host = o?.Host ?? configuration?.Host;
                    ops.Port = o?.Port ?? configuration?.Port;
                    ops.Folder = o?.Folder ?? configuration?.Folder;
                    ops.Level = o?.Level ?? configuration?.Level;
                    ops.ApiLimit = o?.ApiLimit ?? configuration?.ApiLimit;
                    ops.ApiHost = o?.ApiHost ?? configuration?.ApiHost;
                    ops.ApiKey = o?.ApiKey ?? configuration?.ApiKey;

                });

        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading configuration file: {ex.Message}");
    }

    if (ops?.Host == null)
    {
        Console.WriteLine("-h, Host name is required.");
        return;
    }

    if (ops?.UserName == null)
    {
        Console.WriteLine("-u, Username is required.");
        return;
    }

    if (ops?.Password == null)
    {
        Console.WriteLine("-p, Password is required.");
        return;
    }

    if (ops?.Host == null)
    {
        Console.WriteLine("-h, Host is required.");
        return;
    }

    if (ops.Port == null)
    {
        Console.WriteLine("-t, Port is required.");
        return;
    }

    try
    {
        Console.WriteLine($"Scanning will be limited to the following email count, starting from most recent: {ops.ApiLimit} ...");
        Console.WriteLine($"Scanning will be performed on folder: {ops.Folder} ({ops.Host})");
        Console.WriteLine($"Connecting to email server: {ops.Host} ...");
        using var client = new ImapClient();
        client.Connect(ops.Host, (int)(ops.Port), useSsl: true);
        Console.WriteLine($"Authenticating user (with password): {ops.UserName} ...");
        client.Authenticate(ops.UserName, ops.Password);

        // The Inbox folder is always available on all IMAP servers...
        Console.WriteLine($"The following folder will be scanned: {ops.UserName} ...");
        var folder = client.GetFolder(ops.Folder);
        folder.Open(FolderAccess.ReadOnly);

        Console.WriteLine($"Scanning. Please wait ...");


        for (int i = 1; i <= ops.ApiLimit; i++)
        {
            MimeMessage? message = null;
            var sender = "";
            try
            {
                if (i > folder.Count) break;

                message = folder.GetMessage(folder.Count - i);
                sender = ((MailboxAddress)message.From.FirstOrDefault()).Address;
                sender = ((MailboxAddress)message.From.FirstOrDefault()).Address;
                var doc = new HtmlDocument()
                {
                    OptionFixNestedTags = true,
                    OptionCheckSyntax = true,
                    OptionAutoCloseOnEnd = true
                };
                doc.LoadHtml(message.TextBody?.Length > 0 ? message.TextBody : message?.HtmlBody);

                var request = new Email()
                {
                    Address = sender,
                    Id = i.ToString(),
                    Message = doc.DocumentNode.InnerText.Take(MaximumContentLength).ToString(),
                    Title = message?.Subject
                };
                var phishyApi = new Uri("https://" + ops?.ApiHost + PhishyScanEndpoint).ToString();
                var response = phishyApi.WithTimeout(TimeSpan.FromMinutes(1)).WithHeaders(new[] { ("X-RapidAPI-Host", ops.ApiHost), ("X-RapidAPI-Key", ops.ApiKey) }).PostJsonAsync(request);
                var result = response.Result.GetJsonAsync<EmailSecurityResult>().Result;
                if (ops.Level == 0 & result.Level == EmailSecurityLevel.Passed)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i} - [PASSED] {message.Date} - Sender: {sender}, Subject: {message.Subject}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (ops.Level >= 1 & result.Level == EmailSecurityLevel.Warn)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{i} - [SUSPICIOUS] {message.Date}) - Sender: {sender}, Subject: {message.Subject}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (ops.Level >= 2 & result.Level == EmailSecurityLevel.Failed)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{i} - [PHISHING] {message.Date} - Sender: {sender}, Subject: {message.Subject}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (FlurlHttpException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{i} - [ERROR] {message.Date} - Sender: {sender}, Subject: {message?.Subject} {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        client.Disconnect(true);


    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error connecting to server: {ex.Message}");
    }
});