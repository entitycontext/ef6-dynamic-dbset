using System.Data.Entity;

namespace DynamicDbSet.Database
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
    }
}
