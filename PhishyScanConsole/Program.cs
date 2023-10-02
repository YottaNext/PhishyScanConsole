// See https://aka.ms/new-console-template for more information

using CommandLine;
using MailKit.Net.Imap;
using PhishyScanConsole.Models;


const string PhishyScanEndpoint = "https://phishyscan-api.p.rapidapi.com/scan";

Parser.Default.ParseArguments<CommandLineArguments>(args).WithParsed<CommandLineArguments>(o =>
{
    if (o.Configure)
    {

        Console.WriteLine($"Configuration saved for server {o.ServerName}");
    }
    else
    {
        Console.WriteLine($"Scanning folder: {o.Folder} ({o.ServerName}");

        using var client = new ImapClient();
        client.Connect(o.Host, o.Port, useSsl: true);
    }
});


