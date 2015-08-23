using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using DynamicDbSet.Models;

namespace DynamicDbSet.Database
{
    public class EntityCodeGenerator
    {
        public Dictionary<string, Type> Entities = new Dictionary<string, Type>();
        public Dictionary<string, Type> EntityAttributes = new Dictionary<string, Type>();
        public Dictionary<string, Type> EntityAttributeTypes = new Dictionary<string, Type>();
        public Dictionary<string, Type> EntityRelations = new Dictionary<string, Type>();
        public Dictionary<string, Type> EntityRelationTypes = new Dictionary<string, Type>();

        private EntityCodeGenerator()
        {
        }

        public static EntityCodeGenerator Create(
            string nameSpace,
            string assemblyName,
            IEnumerable<EntityClass> entityClasses)
        {
            var generator = new EntityCodeGenerator();

            //
            // Generate code for entity classes.
            //

            var code = "";
            foreach (var entityClass in entityClasses)
            {
                var name = entityClass.Name;

                code +=
                    $@"
                    public class {name} : Entity<{name}, {name}Attribute, {name}AttributeType, {name}Relation, {name}RelationType> 
                    {{                        
                        [Key, Column(""{name}Id"")]
                        public override long Id {{ get; set; }}
                    }}

                    public class {name}Attribute : EntityAttribute<{name}, {name}Attribute, {name}AttributeType, {name}Relation, {name}RelationType> 
                    {{
                        [Key, Column(""{name}AttributeId"")]
                        public override long Id {{ get; set; }}

                        [Column(""{name}AttributeTypeId"")]
                        public override long TypeId {{ get; set; }}

                        [Column(""{name}Id"")]
                        public override long EntityId {{ get; set; }}

                        [Column(""{name}RelationId"")]
                        public override long? RelationId {{ get; set; }}
                    }}

                    public class {name}AttributeType : EntityAttributeType<{name}, {name}Attribute, {name}AttributeType, {name}Relation, {name}RelationType> 
                    {{
                        [Key, Column(""{name}AttributeTypeId"")]
                        public override long Id {{ get; set; }}

                        [Column(""{name}RelationTypeId"")]
                        public override long? RelationTypeId {{ get; set; }}
                    }}

                    public class {name}Relation : EntityRelation<{name}, {name}Attribute, {name}AttributeType, {name}Relation, {name}RelationType> 
                    {{
                        [Key, Column(""{name}RelationId"")]
                        public override long Id {{ get; set; }}

                        [Column(""{name}Id"")]
                        public override long EntityId {{ get; set; }}

                        [Column(""{name}RelationTypeId"")]
                        public override long TypeId {{ get; set; }}
                    }}

                    public class {name}RelationType : EntityRelationType<{name}, {name}Attribute, {name}AttributeType, {name}Relation, {name}RelationType> 
                    {{
                        [Key, Column(""{name}RelationTypeId"")]
                        public override long Id {{ get; set; }}
                    }}";
            }

            code = 
                $@"
                using System.ComponentModel.DataAnnotations;
                using System.ComponentModel.DataAnnotations.Schema;

                namespace {nameSpace} 
                {{ 
                    {code} 
                }}";

            //
            // Compile code and load into dynamic assembly.
            //

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IEntity).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(KeyAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ColumnAttribute).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var assembly = (Assembly)null;
            using (var output = new MemoryStream())
            {
                var result = compilation.Emit(output);

                if (!result.Success)
                {
                    var errors = result.Diagnostics
                        .Where(o => o.IsWarningAsError || o.Severity == DiagnosticSeverity.Error)
                        .ToList();

                    foreach (var error in errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"{error.Id}: {error.GetMessage()}");
                    }

                    throw new Exception("Entity class generation failed with compilation errors.");
                }

                output.Seek(0, SeekOrigin.Begin);
                assembly = Assembly.Load(output.ToArray());
            }

            //
            // Add class types from dynamic assembly to class map.
            //

            foreach (var entityClass in entityClasses)
            {
                var name = entityClass.Name;

                generator.Entities[name] = assembly.GetType($"{nameSpace}.{name}");
                generator.EntityAttributes[name] = assembly.GetType($"{nameSpace}.{name}Attribute");
                generator.EntityAttributeTypes[name] = assembly.GetType($"{nameSpace}.{name}AttributeType");
                generator.EntityRelations[name] = assembly.GetType($"{nameSpace}.{name}Relation");
                generator.EntityRelationTypes[name] = assembly.GetType($"{nameSpace}.{name}RelationType");
            }

            return generator;
        }
    }
}
