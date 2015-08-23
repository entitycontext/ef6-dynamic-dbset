using System.Configuration;

namespace DynamicDbSet.Database
{
    public static class DatabaseSettings
    {
        private const string SCHEMA_NAME = "SchemaName";
        public static string SchemaName
        {
            get { return ConfigurationManager.AppSettings[SCHEMA_NAME] ?? "dbo"; }
        }
    }
}
