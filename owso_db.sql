CREATE DATABASE [owso_db1]
 CONTAINMENT = NONE WITH CATALOG_COLLATION = DATABASE_DEFAULT
 GO

 USE [owso_db1]
 GO

 CREATE TABLE [dbo].[tbl_test](
	[code] [nvarchar](50) NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[date] [datetime] NOT NULL
) ON [PRIMARY]
GO

INSERT INTO [dbo].[tbl_test]
           ([code]
           ,[status]
           ,[date])
     VALUES('010177889901','completed','2020-06-23 10:00:00.000')
GO

INSERT INTO [dbo].[tbl_test]
           ([code]
           ,[status]
           ,[date])
     VALUES('010177889902','completed','2020-06-29 11:00:00.000')
GO

INSERT INTO [dbo].[tbl_test]
           ([code]
           ,[status]
           ,[date])
     VALUES('010177889903','incompete','2020-06-30 10:00:00.000')
GO

INSERT INTO [dbo].[tbl_test]
           ([code]
           ,[status]
           ,[date])
     VALUES('010177889904','incompete','2020-07-01 01:00:00.000')
GO

INSERT INTO [dbo].[tbl_test]
           ([code]
           ,[status]
           ,[date])
     VALUES('010177889905','completed','2020-06-28 23:00:00.000')
GO

INSERT INTO [dbo].[tbl_test]
           ([code]
           ,[status]
           ,[date])
     VALUES('010177889906','completed','2020-06-23 10:00:00.000')
GO

INSERT INTO [dbo].[tbl_test]
           ([code]
           ,[status]
           ,[date])
     VALUES('010177889907','incompete','2020-06-29 11:00:00.000')
GO

INSERT INTO [dbo].[tbl_test]
           ([code]
           ,[status]
           ,[date])
     VALUES('010177889908','incompete','2020-06-30 10:00:00.000')
GO

