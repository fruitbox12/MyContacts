CREATE TABLE [dbo].[phone_numbers]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [phone_number] NVARCHAR(20) NOT NULL, 
    CONSTRAINT [CK_phone_numbers_phone_number] CHECK (phone_number not like '%[^0-9]%')
)

GO

CREATE UNIQUE INDEX [IX_phone_numbers_phone_number] ON [dbo].[phone_numbers] ([phone_number])