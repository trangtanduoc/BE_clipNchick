-- Script to verify that date columns are using DATETIME2 type
-- Run this against your ClipNChic database to confirm the column types

USE ClipNChic;
GO

SELECT 
    t.TABLE_NAME,
    c.COLUMN_NAME,
    c.DATA_TYPE,
    c.IS_NULLABLE
FROM INFORMATION_SCHEMA.TABLES t
INNER JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME
WHERE 
    t.TABLE_SCHEMA = 'dbo' 
    AND (
        (t.TABLE_NAME = 'User' AND c.COLUMN_NAME = 'createDate')
        OR (t.TABLE_NAME = 'Product' AND c.COLUMN_NAME = 'createDate')
        OR (t.TABLE_NAME = 'Order' AND c.COLUMN_NAME = 'createDate')
        OR (t.TABLE_NAME = 'Voucher' AND c.COLUMN_NAME = 'start')
        OR (t.TABLE_NAME = 'Voucher' AND c.COLUMN_NAME = 'end')
    )
ORDER BY t.TABLE_NAME, c.COLUMN_NAME;