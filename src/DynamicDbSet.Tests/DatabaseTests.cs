using System.Linq;

using NUnit.Framework;

using DynamicDbSet.Database;

namespace DynamicDbSet.Tests
{
    //
    // NOTE: SetupDatabase is configured as the setup/teardown fixture class, so that code is run before/after these tests.
    //

    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void CheckEntityTables()
        {
            //
            // Entity classes have entries in the EntityClass table and also have the following tables:
            //
            // {Entity}
            // {Entity}Attribute 
            // {Entity}AttributeType
            // {Entity}Relation
            // {Entity}RelationType
            //

            using (var db = new DatabaseContext())
            {
                var entityClasses = db.EntityClasses.ToList();

                Assert.NotNull(entityClasses.FirstOrDefault(o => o.Name == "Album"));
                Assert.NotNull(entityClasses.FirstOrDefault(o => o.Name == "Band"));
                Assert.NotNull(entityClasses.FirstOrDefault(o => o.Name == "Person"));

                foreach (var entityClass in entityClasses)
                {
                    var name = entityClass.Name;
                    Assert.True(db.Database.TableExists(name));
                    Assert.True(db.Database.TableExists(name + "Attribute"));
                    Assert.True(db.Database.TableExists(name + "AttributeType"));
                    Assert.True(db.Database.TableExists(name + "Relation"));
                    Assert.True(db.Database.TableExists(name + "RelationType"));
                }
            }
        }

        [Test]
        public void CheckSqlGenerator()
        {
            //
            // The EntitySqlGenerator generates SQL DDL statements for creating the entity tables.
            // Since the built-in Entity Framework SQL generator is leveraged, this should produce
            // SQL DDL statements that are compatible with the current database provider.
            //
            // If the entity tables need to be dropped, they can be dropped in the order below 
            // to avoid foreign key dependency errors.  Please note that the helper extension methods
            // DropTable, TableExists, etc. found in DatabaseExtensions.cs are specific to SQL Server.
            //

            using (var db = new DatabaseContext())
            {
                var sql = EntitySqlGenerator.GenerateCreateTables("Foo");

                db.Database.ExecuteSqlCommand(sql);

                Assert.True(db.Database.TableExists("Foo"));
                Assert.True(db.Database.TableExists("FooAttribute"));
                Assert.True(db.Database.TableExists("FooAttributeType"));
                Assert.True(db.Database.TableExists("FooRelation"));
                Assert.True(db.Database.TableExists("FooRelationType"));

                db.Database.DropTable("FooAttribute");
                db.Database.DropTable("FooAttributeType");
                db.Database.DropTable("FooRelation");
                db.Database.DropTable("FooRelationType");
                db.Database.DropTable("Foo");

                Assert.False(db.Database.TableExists("Foo"));
                Assert.False(db.Database.TableExists("FooAttribute"));
                Assert.False(db.Database.TableExists("FooAttributeType"));
                Assert.False(db.Database.TableExists("FooRelation"));
                Assert.False(db.Database.TableExists("FooRelationType"));
            }
        }
    }
}
