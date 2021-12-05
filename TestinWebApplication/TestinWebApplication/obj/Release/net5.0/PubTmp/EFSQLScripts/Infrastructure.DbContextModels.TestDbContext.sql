IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211205214940__FirstMigration')
BEGIN
    CREATE TABLE [Transactions] (
        [Id] int NOT NULL IDENTITY,
        [FinancialInstitution] nvarchar(max) NULL,
        [FXSettlementDate] nvarchar(max) NULL,
        [ReconciliationFileID] nvarchar(max) NULL,
        [TransactionCurrency] nvarchar(max) NULL,
        [ReconciliationCurrency] nvarchar(max) NULL,
        CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211205214940__FirstMigration')
BEGIN
    CREATE TABLE [SubTransactions] (
        [Id] int NOT NULL IDENTITY,
        [SettlementCategory] nvarchar(max) NULL,
        [TransactionAmountcredit] decimal(18,2) NOT NULL,
        [ReconciliationAmntCredit] decimal(18,2) NOT NULL,
        [FeeAmountCredit] decimal(18,2) NOT NULL,
        [TransactionAmountDebit] decimal(18,2) NOT NULL,
        [ReconciliationAmntDebit] decimal(18,2) NOT NULL,
        [FeeAmountDebit] decimal(18,2) NOT NULL,
        [CountTotal] int NOT NULL,
        [NetValue] decimal(18,2) NOT NULL,
        [TransactionId] int NOT NULL,
        CONSTRAINT [PK_SubTransactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SubTransactions_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211205214940__FirstMigration')
BEGIN
    CREATE INDEX [IX_SubTransactions_TransactionId] ON [SubTransactions] ([TransactionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211205214940__FirstMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211205214940__FirstMigration', N'5.0.0');
END;
GO

COMMIT;
GO

