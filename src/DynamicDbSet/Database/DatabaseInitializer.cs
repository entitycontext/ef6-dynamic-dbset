using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DynamicDbSet.Database
{
    public class DatabaseInitializer : IDatabaseInitializer<DatabaseContext>
    {
        public void InitializeDatabase(
            DatabaseContext db)
        {
            if (db.Database.Exists())
            {
                if (db.Database.TableExists("EntityClass"))
                {
                    return;
                }

                var createTables = ((IObjectContextAdapter)db).ObjectContext.CreateDatabaseScript();
                db.Database.ExecuteSqlCommand(createTables);
            }
            else
            {
                db.Database.Create();
                if (db.Database.TableExists("__MigrationHistory"))
                {
                    db.Database.DropTable("__MigrationHistory");
                }
            }
        }
    }
}
