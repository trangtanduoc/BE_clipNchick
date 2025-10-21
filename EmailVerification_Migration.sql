-- Add isEmailVerified column to User table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'User' AND COLUMN_NAME = 'isEmailVerified')
BEGIN
    ALTER TABLE [User]
    ADD isEmailVerified BIT NOT NULL DEFAULT 0;
END;

-- Create EmailVerificationToken table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EmailVerificationToken')
BEGIN
    CREATE TABLE [EmailVerificationToken] (
        [id] INT PRIMARY KEY IDENTITY(1,1),
        [userId] INT NOT NULL,
        [token] NVARCHAR(255) NOT NULL,
        [expiryDate] DATETIME NOT NULL,
        [isUsed] BIT NOT NULL DEFAULT 0,
        [createdDate] DATETIME NOT NULL,
        CONSTRAINT [FK_EmailVerificationToken_User] FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_EmailVerificationToken_Token] ON [EmailVerificationToken]([token]);
    CREATE INDEX [IX_EmailVerificationToken_UserId] ON [EmailVerificationToken]([userId]);
END;
