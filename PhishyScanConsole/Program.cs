// See https://aka.ms/new-console-template for more information

using CommandLine;
using MailKit.Net.Imap;
using PhishyScanConsole.Models;
using Config.Net;

const string PhishyScanEndpoint = "https://phishyscan-api.p.rapidapi.com/scan";



Parser.Default.ParseArguments<CommandLineArguments>(args).WithParsed(o =>
{
    if (o.ConfigurationFile.Length > 0)
    {
        var config = new ConfigurationBuilder<CommandLineArguments>()
            .UseIniFile(o.ConfigurationFile)
            .Build();

        if (o.Verbose) Console.WriteLine($"Configuration loaded for {o.ServerName}");
    }



    Console.WriteLine($"Scanning folder: {o.Folder} ({o.ServerName}");

    using var client = new ImapClient();
    client.Connect(o.Host, o.Port, useSsl: true);

});


