USE [master]
GO
/****** Object:  Database [owso_db]    Script Date: 7/7/2020 10:20:38 PM ******/
CREATE DATABASE [owso_db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'owso_db', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\owso_db.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'owso_db_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\owso_db_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [owso_db] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [owso_db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [owso_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [owso_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [owso_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [owso_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [owso_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [owso_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [owso_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [owso_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [owso_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [owso_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [owso_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [owso_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [owso_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [owso_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [owso_db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [owso_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [owso_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [owso_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [owso_db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [owso_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [owso_db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [owso_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [owso_db] SET RECOVERY FULL 
GO
ALTER DATABASE [owso_db] SET  MULTI_USER 
GO
ALTER DATABASE [owso_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [owso_db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [owso_db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [owso_db] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [owso_db] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'owso_db', N'ON'
GO
ALTER DATABASE [owso_db] SET QUERY_STORE = OFF
GO
USE [owso_db]
GO
/****** Object:  User [chandara]    Script Date: 7/7/2020 10:20:39 PM ******/
CREATE USER [chandara] FOR LOGIN [chandara] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 7/7/2020 10:20:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[FormID] [int] NOT NULL,
	[DistGIS] [nvarchar](4) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[NameLatin] [nvarchar](50) NULL,
	[Sex] [int] NOT NULL,
	[Nationality] [nvarchar](50) NOT NULL,
	[HouseNum] [nvarchar](50) NOT NULL,
	[Street] [nvarchar](50) NOT NULL,
	[VillGIS] [nvarchar](8) NULL,
	[Tel] [nvarchar](50) NULL,
	[ServiceTypeID] [uniqueidentifier] NOT NULL,
	[ServiceDescription] [nvarchar](255) NOT NULL,
	[S1Brand] [nvarchar](50) NULL,
	[S1Type] [int] NULL,
	[S1Color] [nvarchar](50) NULL,
	[S1EngineNum] [nvarchar](50) NULL,
	[S1FrameNum] [nvarchar](50) NULL,
	[S6Power] [nvarchar](5) NULL,
	[S6CurReceipt] [nvarchar](50) NULL,
	[S2NumCopies] [int] NULL,
	[S3BizName] [nvarchar](50) NULL,
	[S3HouseNum] [nvarchar](50) NULL,
	[S3Street] [nvarchar](50) NULL,
	[S3VillGIS] [nvarchar](8) NULL,
	[S3Tel] [nvarchar](50) NULL,
	[RequestedDate] [datetime] NOT NULL,
	[ProjectionDate] [datetime] NOT NULL,
	[RenewFrom] [uniqueidentifier] NULL,
	[Status] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[Modifiedby] [uniqueidentifier] NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl-delivery]    Script Date: 7/7/2020 10:20:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl-delivery](
	[ID] [nvarchar](255) NULL,
	[FormID] [float] NULL,
	[DistGIS] [float] NULL,
	[ServiceID] [nvarchar](255) NULL,
	[ApprovedStatus] [float] NULL,
	[ApprovedDate] [datetime] NULL,
	[LicenseNum] [nvarchar](255) NULL,
	[IssuedDate] [nvarchar](255) NULL,
	[ExpiredDate] [nvarchar](255) NULL,
	[IDnum] [nvarchar](255) NULL,
	[PlatNum] [nvarchar](255) NULL,
	[AdminNum] [nvarchar](255) NULL,
	[DeliveryDate] [datetime] NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[Modifiedby] [nvarchar](255) NULL,
	[Timestamp] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl-services]    Script Date: 7/7/2020 10:20:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl-services](
	[ID] [nvarchar](255) NULL,
	[FormID] [float] NULL,
	[DistGIS] [float] NULL,
	[Name] [nvarchar](255) NULL,
	[NameLatin] [nvarchar](255) NULL,
	[Sex] [float] NULL,
	[Nationality] [nvarchar](255) NULL,
	[HouseNum] [nvarchar](255) NULL,
	[Street] [nvarchar](255) NULL,
	[VillGIS] [float] NULL,
	[Tel] [nvarchar](255) NULL,
	[ServiceTypeID] [nvarchar](255) NULL,
	[ServiceDescription] [nvarchar](255) NULL,
	[S1Brand] [nvarchar](255) NULL,
	[S1Type] [nvarchar](255) NULL,
	[S1Color] [nvarchar](255) NULL,
	[S1EngineNum] [nvarchar](255) NULL,
	[S1FrameNum] [nvarchar](255) NULL,
	[S6Power] [nvarchar](255) NULL,
	[S6CurReceipt] [nvarchar](255) NULL,
	[S2NumCopies] [float] NULL,
	[S3BizName] [nvarchar](255) NULL,
	[S3HouseNum] [nvarchar](255) NULL,
	[S3Street] [nvarchar](255) NULL,
	[S3VillGIS] [nvarchar](255) NULL,
	[S3Tel] [nvarchar](255) NULL,
	[RequestedDate] [datetime] NULL,
	[ProjectionDate] [datetime] NULL,
	[RenewFrom] [nvarchar](255) NULL,
	[Status] [float] NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[Modifiedby] [nvarchar](255) NULL,
	[Timestamp] [nvarchar](255) NULL
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [owso_db] SET  READ_WRITE 
GO
