CREATE TABLE [dbo].[contacts_phone_numbers]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [contact_id] INT NOT NULL, 
    [phone_number_id] INT NOT NULL, 
    CONSTRAINT [FK_contacts_phone_numbers_contacts] FOREIGN KEY ([contact_id]) REFERENCES [contacts]([id]), 
    CONSTRAINT [FK_contact_phone_numbers_phone_numbers] FOREIGN KEY ([phone_number_id]) REFERENCES [phone_numbers]([id])
)

GO

CREATE UNIQUE INDEX [IX_contact_phone_numbers] ON [dbo].[contacts_phone_numbers] ([phone_number_id])
 