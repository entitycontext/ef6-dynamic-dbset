using System.Data.Entity;

namespace Dynamic.Database
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
    }
}
