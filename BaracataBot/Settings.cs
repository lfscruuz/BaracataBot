using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaracataBot
{
    internal class Settings
    {
        public string? DiscordBotToken { get; set; }
        public ConnectionStrings? ConnectionStrings { get; set; }
    }

    internal class ConnectionStrings
    {
        public string? defaultConnection { get; set; }
    }
}
