CREATE TABLE Companies(
    Id BIGINT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    TaxNumber NVARCHAR(20) NOT NULL UNIQUE,
    CurrencyCode CHAR(3) NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL,
    CreatedBy NVARCHAR(100) NOT NULL,
    UpdatedAtUtc DATETIME2 NULL,
    UpdatedBy NVARCHAR(100) NULL,
    IsDeleted BIT NOT NULL DEFAULT(0),
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE TransferOrders(
    Id BIGINT IDENTITY PRIMARY KEY,
    CompanyId BIGINT NOT NULL,
    ReferenceNo NVARCHAR(50) NOT NULL UNIQUE,
    SourceIban NVARCHAR(34) NOT NULL,
    DestinationIban NVARCHAR(34) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    CurrencyCode CHAR(3) NOT NULL,
    State INT NOT NULL,
    IdempotencyKey NVARCHAR(64) NOT NULL UNIQUE,
    CreatedAtUtc DATETIME2 NOT NULL,
    CreatedBy NVARCHAR(100) NOT NULL,
    UpdatedAtUtc DATETIME2 NULL,
    UpdatedBy NVARCHAR(100) NULL,
    IsDeleted BIT NOT NULL DEFAULT(0),
    RowVersion ROWVERSION NOT NULL,
    CONSTRAINT FK_TransferOrders_Companies FOREIGN KEY (CompanyId) REFERENCES Companies(Id)
);
CREATE INDEX IX_TransferOrders_State_CreatedAtUtc ON TransferOrders(State, CreatedAtUtc);
