using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Services.FootballDataApi
{
    public class FootballDataApiSettings
    {
        public const string SettingKey = nameof(FootballDataApiSettings);
        public string FootballServiceApiKey { get; }
        public string FootballServiceApiHost { get; }
        public string Version { get; set; }
    }
}
