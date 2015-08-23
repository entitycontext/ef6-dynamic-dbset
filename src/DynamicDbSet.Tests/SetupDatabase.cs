using System.Collections.Generic;

using NUnit.Framework;

using DynamicDbSet.Database;
using DynamicDbSet.Models;

namespace DynamicDbSet.Tests
{
    [SetUpFixture]
    public class SetupDatabase
    {
        [SetUp]
        public void Setup()
        {
            //
            // Create entity classes and related tables.
            //

            var entityClasses = new Dictionary<string, EntityClass>
            {
                {  "Band", new EntityClass { Name = "Band" } },
                {  "Person", new EntityClass { Name = "Person" } },
                {  "Album", new EntityClass { Name = "Album" } }
            };

            using (var db = new DatabaseContext())
            {
                db.Database.Delete();

                foreach (var entityClass in entityClasses.Values)
                {
                    db.GenerateEntityClass(entityClass);
                }
            }

            DatabaseContext.RefreshEntityModel();

            //
            // Create entity attribute types and relation types.
            //

            var relationTypes = new Dictionary<string, IEntityRelationType>();
            var attributeTypes = new Dictionary<string, IEntityAttributeType>();

            using (var db = new DatabaseContext())
            {
                relationTypes["Band.Member"] = db.CreateEntityRelationType("Band");
                relationTypes["Band.Member"].Name = "Member";
                relationTypes["Band.Member"].ToClassId = entityClasses["Person"].Id;
                db.EntityRelationTypes("Band").Add(relationTypes["Band.Member"]);

                relationTypes["Album.Band"] = db.CreateEntityRelationType("Album");
                relationTypes["Album.Band"].Name = "Band";
                relationTypes["Album.Band"].ToClassId = entityClasses["Band"].Id;
                db.EntityRelationTypes("Album").Add(relationTypes["Album.Band"]);

                attributeTypes["Person.DateOfBirth"] = db.CreateEntityAttributeType("Person");
                attributeTypes["Person.DateOfBirth"].Name = "Date of Birth";
                db.EntityAttributeTypes("Person").Add(attributeTypes["Person.DateOfBirth"]);

                attributeTypes["Band.Member.YearJoined"] = db.CreateEntityAttributeType("Band");
                attributeTypes["Band.Member.YearJoined"].Name = "Year Joined";
                attributeTypes["Band.Member.YearJoined"].RelationType = relationTypes["Band.Member"];
                db.EntityAttributeTypes("Band").Add(attributeTypes["Band.Member.YearJoined"]);

                attributeTypes["Album.YearReleased"] = db.CreateEntityAttributeType("Album");
                attributeTypes["Album.YearReleased"].Name = "Year Released";
                db.EntityAttributeTypes("Album").Add(attributeTypes["Album.YearReleased"]);

                db.SaveChanges();
            }

            //
            // Create entities, attributes and relations.
            //

            using (var db = new DatabaseContext())
            { 
                var bands = new[]
                {
                    new
                    {
                        Name = "The Beatles",
                        Members = new[]
                        {
                            new { Name = "John Lennon", DateOfBirth = "1940-10-09", YearJoined = "1960" },
                            new { Name = "Paul McCartney", DateOfBirth = "1942-06-18", YearJoined = "1960" },
                            new { Name = "George Harrison", DateOfBirth = "1943-02-25", YearJoined = "1960" },
                            new { Name = "Ringo Starr", DateOfBirth = "1940-07-07", YearJoined = "1962" }
                        },
                        Albums = new[]
                        {
                            new { Name = "Please Please Me", YearReleased = "1963" },
                            new { Name = "With The Beatles", YearReleased = "1963" },
                            new { Name = "A Hard Day's Night", YearReleased = "1964" },
                            new { Name = "Beatles for Sale", YearReleased = "1964" },
                            new { Name = "Help!", YearReleased = "1965" },
                            new { Name = "Rubber Soul", YearReleased = "1965" },
                            new { Name = "Revolver", YearReleased = "1966" },
                            new { Name = "Sgt. Pepper's Lonely Hearts Club Band", YearReleased = "1967" },
                            new { Name = "The Beatles", YearReleased = "1968" },
                            new { Name = "Yellow Submarine", YearReleased = "1969" },
                            new { Name = "Abbey Road", YearReleased = "1969" },
                            new { Name = "Let It Be", YearReleased = "1970" },
                        }
                    }
                };

                foreach (var band in bands)
                {
                    var bandEntity = db.CreateEntity("Band");
                    bandEntity.Name = band.Name;
                    db.Entities("Band").Add(bandEntity);
                    db.SaveChanges();

                    foreach (var person in band.Members)
                    {
                        var personEntity = db.CreateEntity("Person");
                        personEntity.Name = person.Name;
                        db.Entities("Person").Add(personEntity);
                        db.SaveChanges();

                        var personDateOfBirth = db.CreateEntityAttribute("Person");
                        personDateOfBirth.TypeId = attributeTypes["Person.DateOfBirth"].Id;
                        personDateOfBirth.EntityId = personEntity.Id;
                        personDateOfBirth.Value = person.DateOfBirth;
                        db.EntityAttributes("Person").Add(personDateOfBirth);
                        db.SaveChanges();

                        var bandMemberRelation = db.CreateEntityRelation("Band");
                        bandMemberRelation.TypeId = relationTypes["Band.Member"].Id;
                        bandMemberRelation.EntityId = bandEntity.Id;
                        bandMemberRelation.ToEntityId = personEntity.Id;
                        db.EntityRelations("Band").Add(bandMemberRelation);
                        db.SaveChanges();

                        var bandMemberYearJoined = db.CreateEntityAttribute("Band");
                        bandMemberYearJoined.TypeId = attributeTypes["Band.Member.YearJoined"].Id;
                        bandMemberYearJoined.EntityId = bandEntity.Id;
                        bandMemberYearJoined.RelationId = bandMemberRelation.Id;
                        bandMemberYearJoined.Value = person.YearJoined;
                        db.EntityAttributes("Band").Add(bandMemberYearJoined);
                        db.SaveChanges();
                    }

                    foreach (var album in band.Albums)
                    {
                        var albumEntity = db.CreateEntity("Album");
                        albumEntity.Name = album.Name;
                        db.Entities("Album").Add(albumEntity);
                        db.SaveChanges();

                        var albumYearReleased = db.CreateEntityAttribute("Album");
                        albumYearReleased.TypeId = attributeTypes["Album.YearReleased"].Id;
                        albumYearReleased.EntityId = albumEntity.Id;
                        albumYearReleased.Value = album.YearReleased;
                        db.EntityAttributes("Album").Add(albumYearReleased);
                        db.SaveChanges();

                        var albumBandRelation = db.CreateEntityRelation("Album");
                        albumBandRelation.TypeId = relationTypes["Album.Band"].Id;
                        albumBandRelation.EntityId = albumEntity.Id;
                        albumBandRelation.ToEntityId = bandEntity.Id;
                        db.EntityRelations("Album").Add(albumBandRelation);
                        db.SaveChanges();
                    }
                }
            }
        }

        [TearDown]
        public void Teardown()
        {
            using (var db = new DatabaseContext())
            {
                db.Database.Delete();
            }
        }
    }
}
