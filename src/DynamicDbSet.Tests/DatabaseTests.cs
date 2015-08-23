using System.Linq;

using NUnit.Framework;

using DynamicDbSet.Database;

namespace DynamicDbSet.Tests
{
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
    }
}
