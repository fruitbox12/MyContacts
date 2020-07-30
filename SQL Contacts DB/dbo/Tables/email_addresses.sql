CREATE TABLE [dbo].[email_addresses]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [email_address] NVARCHAR(100) NOT NULL, 
)

GO

CREATE UNIQUE INDEX [IX_email_addresses_email_address] ON [dbo].[email_addresses] ([email_address])
