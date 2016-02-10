using System;
using System.Configuration;

namespace Test.Settings
{
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class Db
    {
        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        public int Status { get; set; }
    }
}