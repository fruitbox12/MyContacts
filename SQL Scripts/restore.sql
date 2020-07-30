USE [SQLContactsDBDylan]
GO
SET IDENTITY_INSERT [dbo].[email_addresses] ON 
GO
INSERT [dbo].[email_addresses] ([id], [email_address]) VALUES (1, N'dylanwong007@gmail.com')
GO
INSERT [dbo].[email_addresses] ([id], [email_address]) VALUES (2, N'ryanjwong007@gmail.com')
GO
SET IDENTITY_INSERT [dbo].[email_addresses] OFF
GO
SET IDENTITY_INSERT [dbo].[contacts] ON 
GO
INSERT [dbo].[contacts] ([id], [first_name], [middle_name], [last_name]) VALUES (5, N'Dylan', N'J', N'Wong')
GO
INSERT [dbo].[contacts] ([id], [first_name], [middle_name], [last_name]) VALUES (6, N'Ryan', NULL, N'Wong')
GO
SET IDENTITY_INSERT [dbo].[contacts] OFF
GO
INSERT [dbo].[contact_email_addresses] ([id], [contact_id], [email_id]) VALUES (1, 5, 1)
GO
INSERT [dbo].[contact_email_addresses] ([id], [contact_id], [email_id]) VALUES (2, 6, 2)
GO
SET IDENTITY_INSERT [dbo].[phone_numbers] ON 
GO
INSERT [dbo].[phone_numbers] ([id], [phone_number]) VALUES (3, N'3104630301')
GO
INSERT [dbo].[phone_numbers] ([id], [phone_number]) VALUES (1, N'9096558473')
GO
SET IDENTITY_INSERT [dbo].[phone_numbers] OFF
GO
SET IDENTITY_INSERT [dbo].[contact_phone_numbers] ON 
GO
INSERT [dbo].[contact_phone_numbers] ([id], [contact_id], [phone_number_id]) VALUES (4, 5, 1)
GO
INSERT [dbo].[contact_phone_numbers] ([id], [contact_id], [phone_number_id]) VALUES (5, 6, 3)
GO
SET IDENTITY_INSERT [dbo].[contact_phone_numbers] OFF
GO
