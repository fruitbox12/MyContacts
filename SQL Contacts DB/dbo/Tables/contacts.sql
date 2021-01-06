
CREATE TABLE [dbo].[contacts]

(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [first_name] NVARCHAR(100) NOT NULL, 
    [middle_name] NVARCHAR(100) NULL, 
    [last_name] NVARCHAR(100) NOT NULL,
)

GO

CREATE INDEX [IX_contacts_Column] ON [dbo].[contacts] ([last_name], [first_name])
