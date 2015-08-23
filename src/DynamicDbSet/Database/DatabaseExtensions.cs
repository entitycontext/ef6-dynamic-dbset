using System.Linq;

namespace DynamicDbSet.Database
{
    public static class DatabaseExtensions
    {
        private static string schema
        {
            get { return DatabaseSettings.SchemaName; }
        }

        public static void DropTable(
            this System.Data.Entity.Database db,
            string table)
        {
            db.ExecuteSqlCommand($@"
                DROP TABLE [{schema}].[{table}]
            ");
        }

        public static bool SchemaExists(
            this System.Data.Entity.Database db)
        {
            var result = db.SqlQuery<int>($@"
                IF EXISTS (SELECT 1 
                    FROM SYS.SCHEMAS 
                    WHERE NAME LIKE '{schema}') 
                SELECT 1 ELSE SELECT 0;
            ");

            return result.FirstOrDefault() == 1;
        }

        public static bool TableExists(
            this System.Data.Entity.Database db,
            string table)
        {
            var result = db.SqlQuery<int>($@"
                IF EXISTS (SELECT 1 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_TYPE = 'BASE TABLE'
                    AND TABLE_SCHEMA LIKE '{schema}'
                    AND TABLE_NAME LIKE '{table}') 
                SELECT 1 ELSE SELECT 0;
            ");

            return result.FirstOrDefault() == 1;
        }
    }
}
