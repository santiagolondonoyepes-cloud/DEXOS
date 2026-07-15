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
CREATE TABLE [Customers] (
    [Id] uniqueidentifier NOT NULL,
    [TenantId] uniqueidentifier NOT NULL,
    [FullName] nvarchar(200) NOT NULL,
    [Email] nvarchar(200) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [Segment] nvarchar(50) NOT NULL DEFAULT N'new',
    [LoyaltyPoints] int NOT NULL DEFAULT 0,
    [Preferences] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

CREATE TABLE [InventoryMovements] (
    [Id] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [TenantId] uniqueidentifier NOT NULL,
    [Type] nvarchar(20) NOT NULL,
    [Quantity] int NOT NULL,
    [Reason] nvarchar(500) NOT NULL,
    [OccurredAt] datetimeoffset NOT NULL,
    CONSTRAINT [PK_InventoryMovements] PRIMARY KEY ([Id])
);

CREATE TABLE [KitchenOrders] (
    [Id] uniqueidentifier NOT NULL,
    [OrderId] uniqueidentifier NOT NULL,
    [TenantId] uniqueidentifier NOT NULL,
    [Station] nvarchar(50) NOT NULL DEFAULT N'kitchen',
    [Status] nvarchar(20) NOT NULL DEFAULT N'PREPARING',
    [Notes] nvarchar(1000) NOT NULL,
    CONSTRAINT [PK_KitchenOrders] PRIMARY KEY ([Id])
);

CREATE TABLE [Orders] (
    [Id] uniqueidentifier NOT NULL,
    [TenantId] uniqueidentifier NOT NULL,
    [BranchId] uniqueidentifier NOT NULL,
    [CustomerId] uniqueidentifier NULL,
    [Number] nvarchar(50) NOT NULL,
    [Status] nvarchar(20) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id])
);

CREATE TABLE [Products] (
    [Id] uniqueidentifier NOT NULL,
    [TenantId] uniqueidentifier NOT NULL,
    [Sku] nvarchar(50) NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(1000) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [UnitOfMeasure] nvarchar(20) NOT NULL,
    [Stock] int NOT NULL,
    [MinStock] int NOT NULL,
    [MaxStock] int NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);

CREATE TABLE [OrderItems] (
    [Id] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [Quantity] int NOT NULL,
    [OrderId] uniqueidentifier NULL,
    [UnitPriceAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Customers_TenantId] ON [Customers] ([TenantId]);

CREATE UNIQUE INDEX [IX_Customers_TenantId_Email] ON [Customers] ([TenantId], [Email]);

CREATE INDEX [IX_InventoryMovements_OccurredAt] ON [InventoryMovements] ([OccurredAt]);

CREATE INDEX [IX_InventoryMovements_TenantId_ProductId_OccurredAt] ON [InventoryMovements] ([TenantId], [ProductId], [OccurredAt]);

CREATE INDEX [IX_KitchenOrders_OrderId] ON [KitchenOrders] ([OrderId]);

CREATE INDEX [IX_KitchenOrders_TenantId_Status] ON [KitchenOrders] ([TenantId], [Status]);

CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);

CREATE INDEX [IX_Orders_BranchId] ON [Orders] ([BranchId]);

CREATE INDEX [IX_Orders_CreatedAt] ON [Orders] ([CreatedAt]);

CREATE INDEX [IX_Orders_TenantId] ON [Orders] ([TenantId]);

CREATE INDEX [IX_Orders_TenantId_Status] ON [Orders] ([TenantId], [Status]);

CREATE INDEX [IX_Products_TenantId] ON [Products] ([TenantId]);

CREATE UNIQUE INDEX [IX_Products_TenantId_Sku] ON [Products] ([TenantId], [Sku]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260714223944_V20260714_Phase3Foundation', N'9.0.0');

COMMIT;
GO

