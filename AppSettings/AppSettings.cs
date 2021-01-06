﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Settings
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            ReadJsonSettings();

        }
        public string CurrentConnectionName { get; set; }

        public string CurrentConnectionString { get; set; }

        public bool Trace { get; set; } = false;

        private void ReadJsonSettings()
        {
            CurrentConnectionName = _configuration.GetSection("ConnectionStrings").GetSection("CurrentConnectionName").Value;
            CurrentConnectionString = _configuration.GetSection("ConnectionStrings").GetSection(CurrentConnectionName).Value;
            string trace = _configuration.GetSection("Trace").Value;

            if (trace.Equals("true", StringComparison.OrdinalIgnoreCase)) {
                Trace = true;
            }

        }

    }
}
