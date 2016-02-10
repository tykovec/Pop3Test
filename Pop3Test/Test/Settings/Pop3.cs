﻿using System;
using System.Configuration;

namespace Test.Settings
{
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class Pop3
    {
        public string HostName{ get; set; }

        public int Port { get; set; }

        public bool UseSSL { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}