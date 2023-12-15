using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
namespace PhishyScanConsole.Models
{
    public class CommandLineArguments: IConfiguration
    {

        [Option('v', "verbose", Required = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('u', "username", Required = false, HelpText = "Username for the email account.")]
        public string? UserName { get; set; }
        [Option('p', "password", Required = false, HelpText = "Password for the email account.")]
        public string? Password { get; set; }
        [Option('h', "host", Required = false, HelpText = "Host for the email account.")]
        public string? Host { get; set; }
        [Option('t', "port", Required = false, HelpText = "Port for the email account.")]
        public int? Port { get; set; }
        [Option('f', "folder", Required = false, HelpText = "Folder on the server for the email account to scan.")]
        public string? Folder { get; set; }

        [Option('l', "limit", Required = false, HelpText = "Limit for the number of emails to scan, starting from the most recent.")]
        public int? ApiLimit { get; set; }

        [Option('k', "apikey", Required = false, HelpText = "Limit for the number of emails to scan, starting from the most recent.")]
        public string? ApiKey { get; set; }

        [Option('a', "apihost", Required = false, HelpText = "Limit for the number of emails to scan, starting from the most recent.")]
        public string? ApiHost { get; set; }

        [Option('d', "level", Required = false, HelpText = "Detection level. 0 = include all results; 1 = include Suspicious and Phishing; 2 = include Phishing only.")]
        public int? Level { get; set; }
    }
}
