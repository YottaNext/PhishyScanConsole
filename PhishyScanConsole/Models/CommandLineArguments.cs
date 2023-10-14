using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
namespace PhishyScanConsole.Models
{
    public class CommandLineArguments
    {
        [Option('s', "server", Required = false, HelpText = "Server nick name to scan.")]
        public string ServerName { get; set; }

        [Option('c', "configure", Required = false, HelpText = "Configuration file to load.")]
        public string ConfigurationFile { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('u', "username", Required = false, HelpText = "Username for the email account.")]
        public string UserName { get; set; }
        [Option('p', "password", Required = false, HelpText = "Password for the email account.")]
        public string Password { get; set; }
        [Option('h', "host", Required = false, HelpText = "Host for the email account.")]
        public string Host { get; set; }
        [Option('t', "port", Required = false, HelpText = "Port for the email account.")]
        public int Port { get; set; }
        [Option('f', "folder", Required = false, HelpText = "Folder on the server for the email account to scan.")]
        public string Folder { get; set; }
    }
}
