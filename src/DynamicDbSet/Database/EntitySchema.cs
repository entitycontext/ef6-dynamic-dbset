namespace DynamicDbSet.Database
{
    public static class EntitySchema
    {
        public static void CreateEntityTables(
            System.Data.Entity.Database db,
            string schema,
            string name)
        {
            var sql = $@"
                CREATE TABLE [{schema}].[{name}] (
                    [{name}Id] BIGINT IDENTITY NOT NULL,
                    [Name] NVARCHAR(255) NOT NULL,
                    CONSTRAINT [PK_{schema}.{name}] PRIMARY KEY CLUSTERED ([{name}Id] ASC) 
                )

                CREATE TABLE [{schema}].[{name}Attribute] (
                    [{name}AttributeId] BIGINT IDENTITY NOT NULL,
                    [{name}AttributeTypeId] BIGINT NOT NULL,
                    [{name}Id] BIGINT NOT NULL,
                    [{name}RelationId] BIGINT NULL,
                    [Value] NVARCHAR(255) NOT NULL,
                    CONSTRAINT [PK_{schema}.{name}Attribute] PRIMARY KEY CLUSTERED ([{name}AttributeId] ASC) 
                )

                CREATE TABLE [{schema}].[{name}AttributeType] (
                    [{name}AttributeTypeId] BIGINT IDENTITY NOT NULL,
                    [{name}RelationTypeId] BIGINT NULL,
                    [Name] NVARCHAR(255) NOT NULL,
                    CONSTRAINT [PK_{schema}.{name}AttributeType] PRIMARY KEY CLUSTERED ([{name}AttributeTypeId] ASC) 
                )

                CREATE TABLE [{schema}].[{name}Relation] (
                    [{name}RelationId] BIGINT IDENTITY NOT NULL,
                    [{name}RelationTypeId] BIGINT NOT NULL,
                    [{name}Id] BIGINT NOT NULL,
                    [ToEntityId] BIGINT NOT NULL,
                    CONSTRAINT [PK_{schema}.{name}Relation] PRIMARY KEY CLUSTERED ([{name}RelationId] ASC) 
                )

                CREATE TABLE [{schema}].[{name}RelationType] (
                    [{name}RelationTypeId] BIGINT IDENTITY NOT NULL,
                    [ToClassId] BIGINT NOT NULL,                    
                    [Name] NVARCHAR(255) NOT NULL,
                    CONSTRAINT [PK_{schema}.{name}RelationType] PRIMARY KEY CLUSTERED ([{name}RelationTypeId] ASC) 
                )
            ";

            db.ExecuteSqlCommand(sql);
        }
    }
}
