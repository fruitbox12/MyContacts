CREATE TABLE [dbo].[contacts_email_addresses]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [contact_id] INT NOT NULL, 
    [email_id] INT NOT NULL, 

    CONSTRAINT [FK_contact_email_addresses_contacts] FOREIGN KEY ([contact_id]) REFERENCES [contacts]([id]), 
    CONSTRAINT [FK_contact_email_addresses_email_id] FOREIGN KEY ([email_id]) REFERENCES [email_addresses]([id])
)

GO

CREATE UNIQUE INDEX [IX_contact_email] ON [dbo].[contacts_email_addresses] ([email_id])
