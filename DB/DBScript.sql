/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2014 (12.0.2000)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [CodeCampNYC2018]
GO
/****** Object:  StoredProcedure [dbo].[InsertSmartContractTransaction]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[InsertSmartContractTransaction]
GO
/****** Object:  StoredProcedure [dbo].[InsertSmartContractFunction]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[InsertSmartContractFunction]
GO
/****** Object:  StoredProcedure [dbo].[InsertSmartContractDeployedInstance]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[InsertSmartContractDeployedInstance]
GO
/****** Object:  StoredProcedure [dbo].[InsertSmartContract]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[InsertSmartContract]
GO
/****** Object:  StoredProcedure [dbo].[GetUserDltAccounts]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetUserDltAccounts]
GO
/****** Object:  StoredProcedure [dbo].[GetUserDltAccountByLoginId]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetUserDltAccountByLoginId]
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractTransactionsForDeployedInstance]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetSmartContractTransactionsForDeployedInstance]
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractTransactionInfoByHash]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetSmartContractTransactionInfoByHash]
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContracts]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetSmartContracts]
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractFunctions]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetSmartContractFunctions]
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractDeployedInstances]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetSmartContractDeployedInstances]
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractDeployedInstance]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetSmartContractDeployedInstance]
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContract]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP PROCEDURE [dbo].[GetSmartContract]
GO
/****** Object:  Table [dbo].[UserDltAccount]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP TABLE [dbo].[UserDltAccount]
GO
/****** Object:  Table [dbo].[SmartContractTransaction]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP TABLE [dbo].[SmartContractTransaction]
GO
/****** Object:  Table [dbo].[SmartContractFunction]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP TABLE [dbo].[SmartContractFunction]
GO
/****** Object:  Table [dbo].[SmartContractDeployedInstance]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP TABLE [dbo].[SmartContractDeployedInstance]
GO
/****** Object:  Table [dbo].[SmartContract]    Script Date: 10/3/2018 9:34:30 PM ******/
DROP TABLE [dbo].[SmartContract]
GO
/****** Object:  Table [dbo].[SmartContract]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmartContract](
	[SmartContractId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Abi] [text] NOT NULL,
	[ByteCode] [text] NOT NULL,
	[CreatedByUserLoginId] [varchar](100) NOT NULL,
	[CreatedDatetime] [datetime] NULL,
	[UpdatedDatetime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SmartContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmartContractDeployedInstance]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmartContractDeployedInstance](
	[SmartContractDeployedInstanceId] [int] IDENTITY(1,1) NOT NULL,
	[SmartContractId] [int] NOT NULL,
	[DeployedAddress] [varchar](100) NOT NULL,
	[InitialData] [text] NOT NULL,
	[DeployedBYUserLoginId] [varchar](100) NULL,
	[DeployedInstanceDisplayName] [varchar](500) NULL,
	[CreatedDatetime] [datetime] NULL,
	[UpdatedDatetime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SmartContractDeployedInstanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmartContractFunction]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmartContractFunction](
	[SmartContractFunctionId] [int] IDENTITY(1,1) NOT NULL,
	[SmartContractId] [int] NOT NULL,
	[FunctionName] [nvarchar](1000) NOT NULL,
	[FunctionType] [nvarchar](1000) NOT NULL,
	[Sequence] [int] NOT NULL,
	[CreatedDatetime] [datetime] NULL,
	[UpdatedDatetime] [datetime] NULL,
 CONSTRAINT [PK_SmartContractFunction] PRIMARY KEY CLUSTERED 
(
	[SmartContractFunctionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmartContractTransaction]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmartContractTransaction](
	[SmartContractTransactionId] [int] IDENTITY(1,1) NOT NULL,
	[SmartContractDeployedInstanceId] [int] NOT NULL,
	[TransactionHash] [nvarchar](100) NOT NULL,
	[TransactionUser] [nvarchar](100) NOT NULL,
	[SmartContractFunction] [nvarchar](500) NOT NULL,
	[SmartContractFunctionParameters] [text] NOT NULL,
	[CreatedDatetime] [datetime] NULL,
	[UpdatedDatetime] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDltAccount]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDltAccount](
	[UserAccountId] [int] IDENTITY(1,1) NOT NULL,
	[UserLoginId] [varchar](100) NOT NULL,
	[AccountAddress] [nvarchar](200) NOT NULL,
	[Passphrase] [nvarchar](200) NOT NULL,
	[CreatedDatetime] [datetime] NULL,
	[UpdatedDatetime] [datetime] NULL,
 CONSTRAINT [PK_UserDltAccount] PRIMARY KEY CLUSTERED 
(
	[UserAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContract]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSmartContract]
	-- Add the parameters for the stored procedure here
	@smartContractId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
			[SmartContractId]
		  ,[Name]
		  ,[Abi]
		  ,[ByteCode]
		  ,[CreatedByUserLoginId]
		  ,[CreatedDatetime]
		  ,[UpdatedDatetime]
	FROM 
		[SmartContract]
	WHERE
		[SmartContractId] = @smartContractId
END
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractDeployedInstance]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSmartContractDeployedInstance]
	-- Add the parameters for the stored procedure here
	@smartContractDeployedInstanceId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		[SmartContractDeployedInstanceId]
      ,[SmartContractId]
      ,[DeployedAddress]
      ,[InitialData]
      ,[DeployedBYUserLoginId]
      ,[DeployedInstanceDisplayName]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContractDeployedInstance]
	WHERE 
		[SmartContractDeployedInstanceId] = @smartContractDeployedInstanceId
END
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractDeployedInstances]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSmartContractDeployedInstances]
	-- Add the parameters for the stored procedure here
	@smartContractId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		[SmartContractDeployedInstanceId]
      ,[SmartContractId]
      ,[DeployedAddress]
      ,[InitialData]
      ,[DeployedBYUserLoginId]
      ,[DeployedInstanceDisplayName]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContractDeployedInstance]
	WHERE 
		[SmartContractId] = @smartContractId
END
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractFunctions]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSmartContractFunctions]
	@smartContractId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/****** Script for SelectTopNRows command from SSMS  ******/
	SELECT 
		[SmartContractFunctionId]
      ,[SmartContractId]
      ,[FunctionName]
      ,[FunctionType]
      ,[Sequence]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContractFunction]
	WHERE
		[SmartContractId] = @smartContractId
END
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContracts]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSmartContracts] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
			[SmartContractId]
		  ,[Name]
		  ,[Abi]
		  ,[ByteCode]
		  ,[CreatedByUserLoginId]
		  ,[CreatedDatetime]
		  ,[UpdatedDatetime]
	FROM 
		[SmartContract]
END
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractTransactionInfoByHash]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSmartContractTransactionInfoByHash] 
	@transactionHash VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[SmartContractTransactionId]
      ,[SmartContractDeployedInstanceId]
      ,[TransactionHash]
      ,[TransactionUser]
      ,[SmartContractFunction]
      ,[SmartContractFunctionParamters]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContractTransaction]
	WHERE
		[TransactionHash] = @transactionHash
END
GO
/****** Object:  StoredProcedure [dbo].[GetSmartContractTransactionsForDeployedInstance]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSmartContractTransactionsForDeployedInstance] 
	@smartContractDeployedInstanceId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[SmartContractTransactionId]
      ,[SmartContractDeployedInstanceId]
      ,[TransactionHash]
      ,[TransactionUser]
      ,[SmartContractFunction]
      ,[SmartContractFunctionParamters]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContractTransaction]
	WHERE
		[SmartContractDeployedInstanceId] = @smartContractDeployedInstanceId
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserDltAccountByLoginId]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserDltAccountByLoginId] 
	@userLoginId VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[UserAccountId]
      ,[UserLoginId]
      ,[AccountAddress]
      ,[Passphrase]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[UserDltAccount]
	WHERE
		[UserLoginId] = @userLoginId
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserDltAccounts]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetUserDltAccounts] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		[UserAccountId]
      ,[UserLoginId]
      ,[AccountAddress]
      ,[Passphrase]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[UserDltAccount]
END
GO
/****** Object:  StoredProcedure [dbo].[InsertSmartContract]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertSmartContract]
	@name varchar(100),
    @abi text,
    @byteCode text,
    @createdByUserLoginId varchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[SmartContract]
           ([Name]
           ,[Abi]
           ,[ByteCode]
           ,[CreatedByUserLoginId]
           ,[CreatedDatetime]
           ,[UpdatedDatetime])
     VALUES
           (@name
           ,@abi
           ,@byteCode
		    ,@createdByUserLoginId
           ,GETDATE()
           ,GETDATE()
           )

	SELECT TOP (1000) [SmartContractId]
      ,[Name]
      ,[Abi]
      ,[ByteCode]
      ,[CreatedByUserLoginId]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContract] WITH (NOLOCK)
	WHERE 
		SmartContractId = SCOPE_IDENTITY()

END
GO
/****** Object:  StoredProcedure [dbo].[InsertSmartContractDeployedInstance]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertSmartContractDeployedInstance] 
	 @smartContractId int
    ,@deployedAddress varchar(100)
    ,@initialData text
    ,@deployedBYUserLoginId varchar(100)
    ,@deployedInstanceDisplayName varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    

	INSERT INTO [dbo].[SmartContractDeployedInstance]
           ([SmartContractId]
           ,[DeployedAddress]
           ,[InitialData]
           ,[DeployedBYUserLoginId]
           ,[DeployedInstanceDisplayName]
           ,[CreatedDatetime]
           ,[UpdatedDatetime])
     VALUES
           (@smartContractId
           ,@deployedAddress
           ,@initialData
           ,@deployedBYUserLoginId
		   ,@deployedInstanceDisplayName
           ,GETDATE()
           ,GETDATE())


	SELECT
	   [SmartContractDeployedInstanceId]
      ,[SmartContractId]
      ,[DeployedAddress]
      ,[InitialData]
      ,[DeployedBYUserLoginId]
      ,[DeployedInstanceDisplayName]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContractDeployedInstance]
	WHERE
		SmartContractDeployedInstanceId = SCOPE_IDENTITY()

END
GO
/****** Object:  StoredProcedure [dbo].[InsertSmartContractFunction]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertSmartContractFunction] 
	@smartContractId int
	,@functionName nvarchar(1000)
	,@functionType nvarchar(1000)
	,@sequence int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [dbo].[SmartContractFunction]
           ([SmartContractId]
           ,[FunctionName]
           ,[FunctionType]
           ,[Sequence]
           ,[CreatedDatetime]
           ,[UpdatedDatetime])
     VALUES
           (@smartContractId
           ,@functionName
           ,@functionType 
           ,@sequence
           ,GETDATE()
           ,GETDATE())

	SELECT [SmartContractFunctionId]
      ,[SmartContractId]
      ,[FunctionName]
      ,[FunctionType]
      ,[Sequence]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContractFunction]
	WHERE
		[SmartContractFunctionId] = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[InsertSmartContractTransaction]    Script Date: 10/3/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertSmartContractTransaction]
	-- Add the parameters for the stored procedure here
	@smartContractDeployedInstanceId int
	,@transactionHash nvarchar(100)
	,@transactionUser nvarchar(100)
	,@smartContractFunction nvarchar(500)
	,@smartContractFunctionParameters text	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [SmartContractTransaction]
           ([SmartContractDeployedInstanceId]
           ,[TransactionHash]
           ,[TransactionUser]
           ,[SmartContractFunction]
           ,[SmartContractFunctionParameters]
           ,[CreatedDatetime]
           ,[UpdatedDatetime])
     VALUES
           (@smartContractDeployedInstanceId
           ,@transactionHash
           ,@transactionUser
           ,@smartContractFunction
           ,@smartContractFunctionParameters
           ,GETDATE()
           ,GETDATE())

	SELECT 
		[SmartContractTransactionId]
      ,[SmartContractDeployedInstanceId]
      ,[TransactionHash]
      ,[TransactionUser]
      ,[SmartContractFunction]
      ,[SmartContractFunctionParameters]
      ,[CreatedDatetime]
      ,[UpdatedDatetime]
	FROM 
		[SmartContractTransaction]
	WHERE
		SmartContractTransactionId = SCOPE_IDENTITY()
END
GO
