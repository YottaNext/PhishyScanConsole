using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;
namespace PhishyScanConsole.Models
{
    public interface IConfiguration
    {
        [Option(Alias = "host.verbose")]
        bool Verbose { get; set; }

        [Option(Alias = "host.username")]
        string? UserName { get; set; }

        [Option(Alias = "host.password")]
        string? Password { get; set; }

        [Option(Alias = "host.host_name")]
        string? Host { get; set; }

        [Option(Alias = "host.port")]
        int? Port { get; set; }

        [Option(Alias = "host.folder")]
        string? Folder { get; set; }

        [Option(Alias = "host.level")]
        int? Level { get; set; }

        [Option(Alias = "api.limit")]
        int? ApiLimit { get; set; }

        [Option(Alias = "api.host")]
        string? ApiHost { get; set; }

        [Option(Alias = "api.key")]
        string? ApiKey { get; set; }
    }
}
