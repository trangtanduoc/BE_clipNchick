BEGIN TRY
    BEGIN TRAN;

    -- Add new nullable foreign key columns to Image if they do not already exist
    IF COL_LENGTH('dbo.Image', 'baseId') IS NULL
        ALTER TABLE dbo.Image ADD baseId INT NULL;

    IF COL_LENGTH('dbo.Image', 'charmId') IS NULL
        ALTER TABLE dbo.Image ADD charmId INT NULL;

    IF COL_LENGTH('dbo.Image', 'productId') IS NULL
        ALTER TABLE dbo.Image ADD productId INT NULL;

    IF COL_LENGTH('dbo.Image', 'blindBoxId') IS NULL
        ALTER TABLE dbo.Image ADD blindBoxId INT NULL;

    -- Migrate existing relationships from legacy columns/tables into the new Image columns
    IF COL_LENGTH('dbo.Base', 'imageId') IS NOT NULL AND COL_LENGTH('dbo.Image', 'baseId') IS NOT NULL
        UPDATE img
        SET baseId = b.id
        FROM dbo.Image AS img
        INNER JOIN dbo.Base AS b ON b.imageId = img.id
        WHERE img.baseId IS NULL;

    IF COL_LENGTH('dbo.Charm', 'imageId') IS NOT NULL AND COL_LENGTH('dbo.Image', 'charmId') IS NOT NULL
        UPDATE img
        SET charmId = c.id
        FROM dbo.Image AS img
        INNER JOIN dbo.Charm AS c ON c.imageId = img.id
        WHERE img.charmId IS NULL;

    IF OBJECT_ID(N'dbo.ProductPic', N'U') IS NOT NULL AND COL_LENGTH('dbo.Image', 'productId') IS NOT NULL
        UPDATE img
        SET productId = pp.productId
        FROM dbo.Image AS img
        INNER JOIN dbo.ProductPic AS pp ON pp.imageId = img.id
        WHERE img.productId IS NULL;

    IF OBJECT_ID(N'dbo.BlindPic', N'U') IS NOT NULL AND COL_LENGTH('dbo.Image', 'blindBoxId') IS NOT NULL
        UPDATE img
        SET blindBoxId = bp.blindId
        FROM dbo.Image AS img
        INNER JOIN dbo.BlindPic AS bp ON bp.imageId = img.id
        WHERE img.blindBoxId IS NULL;

    -- Drop foreign key constraints from Base/Charm to Image that relied on the legacy imageId column
    DECLARE @dropFkSql NVARCHAR(MAX) = N'';

    SELECT @dropFkSql = STRING_AGG('ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fk.parent_object_id)) + '.' +
                                   QUOTENAME(OBJECT_NAME(fk.parent_object_id)) + ' DROP CONSTRAINT ' +
                                   QUOTENAME(fk.name) + ';', CHAR(13) + CHAR(10))
    FROM sys.foreign_keys AS fk
    WHERE fk.referenced_object_id = OBJECT_ID(N'dbo.Image')
      AND fk.parent_object_id IN (OBJECT_ID(N'dbo.Base'), OBJECT_ID(N'dbo.Charm'));

    IF @dropFkSql IS NOT NULL AND LEN(@dropFkSql) > 0
        EXEC sp_executesql @dropFkSql;

    -- Remove the obsolete imageId columns now that relationships are stored on the Image table
    IF COL_LENGTH('dbo.Base', 'imageId') IS NOT NULL
        ALTER TABLE dbo.Base DROP COLUMN imageId;

    IF COL_LENGTH('dbo.Charm', 'imageId') IS NOT NULL
        ALTER TABLE dbo.Charm DROP COLUMN imageId;

    -- Decommission legacy mapping tables after data migration
    IF OBJECT_ID(N'dbo.ProductPic', N'U') IS NOT NULL
        DROP TABLE dbo.ProductPic;

    IF OBJECT_ID(N'dbo.BlindPic', N'U') IS NOT NULL
        DROP TABLE dbo.BlindPic;

    -- Ensure unique image per Base as enforced by the EF Core model
    IF COL_LENGTH('dbo.Image', 'baseId') IS NOT NULL
       AND NOT EXISTS (SELECT 1
                       FROM sys.indexes
                       WHERE name = N'IX_Image_baseId'
                         AND object_id = OBJECT_ID(N'dbo.Image'))
        EXEC(N'CREATE UNIQUE INDEX IX_Image_baseId ON dbo.Image (baseId) WHERE baseId IS NOT NULL;');

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN;

    THROW;
END CATCH;
