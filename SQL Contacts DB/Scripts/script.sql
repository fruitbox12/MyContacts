USE [SQLContactsDBDylan]
GO
/****** Object:  Table [dbo].[contact_email_addresses]    Script Date: 7/22/2020 3:17:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contact_email_addresses](
	[id] [int] NOT NULL,
	[contact_id] [int] NOT NULL,
	[email_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[contact_phone_numbers]    Script Date: 7/22/2020 3:17:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contact_phone_numbers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[contact_id] [int] NOT NULL,
	[phone_number_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[contacts]    Script Date: 7/22/2020 3:17:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contacts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [nvarchar](100) NOT NULL,
	[middle_name] [nvarchar](100) NULL,
	[last_name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[email_addresses]    Script Date: 7/22/2020 3:17:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[email_addresses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email_address] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[phone_numbers]    Script Date: 7/22/2020 3:17:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[phone_numbers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[phone_number] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[contact_email_addresses]  WITH CHECK ADD  CONSTRAINT [FK_contact_email_addresses_contacts] FOREIGN KEY([contact_id])
REFERENCES [dbo].[contacts] ([id])
GO
ALTER TABLE [dbo].[contact_email_addresses] CHECK CONSTRAINT [FK_contact_email_addresses_contacts]
GO
ALTER TABLE [dbo].[contact_email_addresses]  WITH CHECK ADD  CONSTRAINT [FK_contact_email_addresses_email_id] FOREIGN KEY([email_id])
REFERENCES [dbo].[email_addresses] ([id])
GO
ALTER TABLE [dbo].[contact_email_addresses] CHECK CONSTRAINT [FK_contact_email_addresses_email_id]
GO
ALTER TABLE [dbo].[contact_phone_numbers]  WITH CHECK ADD  CONSTRAINT [FK_contact_phone_numbers_phone_numbers] FOREIGN KEY([phone_number_id])
REFERENCES [dbo].[phone_numbers] ([id])
GO
ALTER TABLE [dbo].[contact_phone_numbers] CHECK CONSTRAINT [FK_contact_phone_numbers_phone_numbers]
GO
ALTER TABLE [dbo].[contact_phone_numbers]  WITH CHECK ADD  CONSTRAINT [FK_contacts_phone_numbers_contacts] FOREIGN KEY([contact_id])
REFERENCES [dbo].[contacts] ([id])
GO
ALTER TABLE [dbo].[contact_phone_numbers] CHECK CONSTRAINT [FK_contacts_phone_numbers_contacts]
GO
ALTER TABLE [dbo].[phone_numbers]  WITH CHECK ADD  CONSTRAINT [CK_phone_numbers_phone_number] CHECK  ((NOT [phone_number] like '%[^0-9]%'))
GO
ALTER TABLE [dbo].[phone_numbers] CHECK CONSTRAINT [CK_phone_numbers_phone_number]
GO
