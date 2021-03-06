USE [master]
GO
/****** Object:  Database [Bismark]    Script Date: 10/22/2020 1:14:56 PM ******/
CREATE DATABASE [Bismark]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Bismark', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Bismark.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Bismark_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Bismark_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Bismark] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Bismark].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Bismark] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Bismark] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Bismark] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Bismark] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Bismark] SET ARITHABORT OFF 
GO
ALTER DATABASE [Bismark] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Bismark] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Bismark] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Bismark] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Bismark] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Bismark] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Bismark] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Bismark] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Bismark] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Bismark] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Bismark] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Bismark] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Bismark] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Bismark] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Bismark] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Bismark] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Bismark] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Bismark] SET RECOVERY FULL 
GO
ALTER DATABASE [Bismark] SET  MULTI_USER 
GO
ALTER DATABASE [Bismark] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Bismark] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Bismark] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Bismark] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Bismark] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Bismark', N'ON'
GO
ALTER DATABASE [Bismark] SET QUERY_STORE = OFF
GO
USE [Bismark]
GO
/****** Object:  UserDefinedTableType [dbo].[IDTABLE]    Script Date: 10/22/2020 1:14:56 PM ******/
CREATE TYPE [dbo].[IDTABLE] AS TABLE(
	[Id] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[ITEMTABLE]    Script Date: 10/22/2020 1:14:56 PM ******/
CREATE TYPE [dbo].[ITEMTABLE] AS TABLE(
	[Name] [varchar](30) NULL,
	[Description] [varchar](50) NULL,
	[Price] [money] NULL,
	[Quantity] [int] NULL,
	[Justification] [varchar](100) NULL,
	[Source] [varchar](100) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[SALARYADJTABLE]    Script Date: 10/22/2020 1:14:56 PM ******/
CREATE TYPE [dbo].[SALARYADJTABLE] AS TABLE(
	[SalaryAdjustmentId] [int] NULL,
	[EmployeeId] [int] NULL,
	[PercentIncrease] [decimal](5, 4) NULL,
	[EffectiveDate] [date] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[Test]    Script Date: 10/22/2020 1:14:56 PM ******/
CREATE TYPE [dbo].[Test] AS TABLE(
	[Name] [varchar](30) NULL,
	[Description] [varchar](100) NULL,
	[Price] [money] NULL,
	[Quantity] [int] NULL,
	[Justification] [varchar](100) NULL,
	[Source] [varchar](100) NULL
)
GO
/****** Object:  UserDefinedFunction [dbo].[GET_CURRENT_PAYPERIOD_STARTDATE]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GET_CURRENT_PAYPERIOD_STARTDATE]()
RETURNS DATE
AS
	BEGIN
		DECLARE @Return date
				
		;WITH
		cteTally AS
		(
		SELECT TOP 27
			ROW_NUMBER() OVER (ORDER BY ID) -1 AS N
			FROM master.sys.syscolumns
		)

		SELECT @Return = MAX(DATEADD(dd,t.n*14,'20120106'))
			FROM cteTally t
			WHERE DATEADD(dd,t.n*14,'20120106') < GETDATE()				
		
	RETURN @Return
	END
GO
/****** Object:  UserDefinedFunction [dbo].[GetOrderTotals]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: May 1, 2012
-- Description:	This function returns the order totals.
-- =============================================
CREATE FUNCTION [dbo].[GetOrderTotals]
(
	-- Add the parameters for the function here
	@orderId INT,
	@type INT
)
RETURNS MONEY
AS
BEGIN
	IF EXISTS(SELECT 1 FROM [Order] WHERE OrderId = @orderId)
    BEGIN
		DECLARE @itemTable AS TABLE (
			Price MONEY,
			Quantity INT
		)
		DECLARE @price MONEY
		DECLARE @quantity INT
		DECLARE @subTotal MONEY = 0
		DECLARE @taxTotal MONEY = 0
		DECLARE @grandTotal MONEY = 0
		INSERT @itemTable SELECT Price, Quantity FROM Item WHERE OrderId = @orderId 
		AND [Status] <> 2
		WHILE EXISTS(SELECT TOP 1 * FROM @itemTable)
		BEGIN
			SELECT TOP 1 @price = Price FROM @itemTable
			SELECT TOP 1 @quantity = Quantity FROM @itemTable
			SET @subTotal += @price * @quantity
			DELETE TOP (1) FROM @itemTable	
		END
		SET @taxTotal = @subTotal * 0.13
		SET @grandTotal = @subTotal + @taxTotal	
		IF @type = 0
			RETURN @subTotal
		IF @type = 1
			RETURN @taxTotal
		IF @type = 2
			RETURN @grandTotal
	END
	RETURN 0
END
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] IDENTITY(10000000,1) NOT NULL,
	[FirstName] [varchar](25) NOT NULL,
	[LastName] [varchar](25) NOT NULL,
	[MiddleInitial] [char](1) NULL,
	[Address] [varchar](50) NOT NULL,
	[City] [varchar](20) NOT NULL,
	[Province] [char](2) NOT NULL,
	[PostalCode] [char](6) NOT NULL,
	[Phone] [char](10) NOT NULL,
	[CellPhone] [char](10) NULL,
	[Email] [varchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[SIN] [char](9) NOT NULL,
	[Status] [int] NOT NULL,
	[HireDate] [date] NOT NULL,
	[JobStartDate] [date] NOT NULL,
	[JobId] [int] NOT NULL,
	[Salary] [money] NOT NULL,
	[SalaryEffectiveDate] [date] NOT NULL,
	[PrevSalary] [money] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ActiveEmployees]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ActiveEmployees]
AS
SELECT     EmployeeId, FirstName, LastName, MiddleInitial, Address, City, Province, PostalCode, Phone, CellPhone, Email, DateOfBirth, SIN, Status, HireDate, JobStartDate, 
                      JobId, Salary, SalaryEffectiveDate, PrevSalary
FROM         dbo.Employee
WHERE     (Status = 1)
GO
/****** Object:  Table [dbo].[Bonus]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bonus](
	[BonusId] [int] IDENTITY(1,1) NOT NULL,
	[PayPeriod] [date] NOT NULL,
	[PercentBonus] [decimal](5, 4) NULL,
	[FixedBonus] [money] NULL,
 CONSTRAINT [PK_Bonus] PRIMARY KEY CLUSTERED 
(
	[BonusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeBonus]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeBonus](
	[EmployeeId] [int] NOT NULL,
	[BonusId] [int] NOT NULL,
 CONSTRAINT [PK_EmployeeBonus] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC,
	[BonusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeLogin]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeLogin](
	[EmployeeId] [int] NOT NULL,
	[Password] [varchar](12) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeSalaryAdjustment]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeSalaryAdjustment](
	[EmployeeId] [int] NOT NULL,
	[SalaryAdjustmentId] [int] NOT NULL,
 CONSTRAINT [PK_EmployeeSalaryAdjustment] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC,
	[SalaryAdjustmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Price] [money] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Justification] [varchar](100) NOT NULL,
	[Source] [varchar](100) NOT NULL,
	[Status] [int] NOT NULL,
	[Reason] [varchar](100) NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job](
	[JobId] [int] IDENTITY(1,1) NOT NULL,
	[Department] [int] NOT NULL,
	[Title] [varchar](30) NOT NULL,
	[MaxSalary] [money] NOT NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[JobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[OrderDate] [date] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayStub]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayStub](
	[PayStubId] [int] IDENTITY(100,1) NOT NULL,
	[PayPeriod] [date] NOT NULL,
	[GrossPay] [money] NOT NULL,
	[YTDGrossPay] [money] NOT NULL,
	[BonusPay] [money] NOT NULL,
	[YTDBonusPay] [money] NOT NULL,
	[IncomeTaxDeduction] [money] NOT NULL,
	[YTDIncomeTaxDeduction] [money] NOT NULL,
	[CPPDeduction] [money] NOT NULL,
	[YTDCPPDeduction] [money] NOT NULL,
	[EIDeduction] [money] NOT NULL,
	[YTDEIDeduction] [money] NOT NULL,
	[PensionDeduction] [money] NOT NULL,
	[YTDPensionDeduction] [money] NOT NULL,
	[NetPay] [money] NOT NULL,
	[YTDNetPay] [money] NOT NULL,
	[EmployeeId] [int] NOT NULL,
 CONSTRAINT [PK_PayStub] PRIMARY KEY CLUSTERED 
(
	[PayStubId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalaryAdjustment]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalaryAdjustment](
	[SalaryAdjustmentId] [int] IDENTITY(100,1) NOT NULL,
	[PercentIncrease] [decimal](5, 4) NOT NULL,
	[EffectiveDate] [date] NOT NULL,
 CONSTRAINT [PK_SalaryAdjustment] PRIMARY KEY CLUSTERED 
(
	[SalaryAdjustmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SickDay]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SickDay](
	[SickDayId] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[IsFullDay] [bit] NOT NULL,
	[EmployeeId] [int] NOT NULL,
 CONSTRAINT [PK_SickDay] PRIMARY KEY CLUSTERED 
(
	[SickDayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Item] ADD  CONSTRAINT [DF_Item_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Item] ADD  CONSTRAINT [DF_Item_DenialReason]  DEFAULT (NULL) FOR [Reason]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_OrderDate]  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_IsClosed]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobId])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Job]
GO
ALTER TABLE [dbo].[EmployeeBonus]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeBonus_Bonus] FOREIGN KEY([BonusId])
REFERENCES [dbo].[Bonus] ([BonusId])
GO
ALTER TABLE [dbo].[EmployeeBonus] CHECK CONSTRAINT [FK_EmployeeBonus_Bonus]
GO
ALTER TABLE [dbo].[EmployeeBonus]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeBonus_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[EmployeeBonus] CHECK CONSTRAINT [FK_EmployeeBonus_Employee]
GO
ALTER TABLE [dbo].[EmployeeSalaryAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeSalaryAdjustment_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[EmployeeSalaryAdjustment] CHECK CONSTRAINT [FK_EmployeeSalaryAdjustment_Employee]
GO
ALTER TABLE [dbo].[EmployeeSalaryAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeSalaryAdjustment_SalaryAdjustment] FOREIGN KEY([SalaryAdjustmentId])
REFERENCES [dbo].[SalaryAdjustment] ([SalaryAdjustmentId])
GO
ALTER TABLE [dbo].[EmployeeSalaryAdjustment] CHECK CONSTRAINT [FK_EmployeeSalaryAdjustment_SalaryAdjustment]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Order]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Employee]
GO
ALTER TABLE [dbo].[PayStub]  WITH CHECK ADD  CONSTRAINT [FK_PayStub_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[PayStub] CHECK CONSTRAINT [FK_PayStub_Employee]
GO
ALTER TABLE [dbo].[SickDay]  WITH CHECK ADD  CONSTRAINT [FK_SickDay_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[SickDay] CHECK CONSTRAINT [FK_SickDay_Employee]
GO
/****** Object:  StoredProcedure [dbo].[CheckCEO]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CheckCEO]
	-- Add the parameters for the stored procedure here
	@employeeId INT,
	@result BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
    BEGIN TRY
    IF NOT EXISTS(SELECT 1 FROM Employee WHERE Employee.EmployeeId = @employeeId)
		RAISERROR('The employee Id does not exist.',15,1)
	ELSE
	BEGIN
		IF ((SELECT Job.Title FROM Job
			 JOIN Employee ON Employee.JobId = Job.JobId
			 WHERE Employee.EmployeeId = @employeeId) = 'CEO')
			 SET @result = 1
		ELSE
			SET @result = 0
	END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[CheckIfItemsArePending]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: May 2, 2012
-- Description:	This procedure verifies if there are more items pending and can safely close the order.
-- =============================================
CREATE PROCEDURE [dbo].[CheckIfItemsArePending] 
	-- Add the parameters for the stored procedure here
	@result BIT OUTPUT,
	@orderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    BEGIN TRY
		IF EXISTS(SELECT 1 FROM [Order] WHERE OrderId = @orderId)
		BEGIN
			-- If a status of 0 is found on an item in the given order, it is pending.
			IF EXISTS(SELECT 1 FROM Item WHERE OrderId = @orderId AND [Status] = 0)
				-- Found a pending status, return true.
				SET @result = 1
			ELSE
				-- No pending status found on any items, return false.
				SET @result = 0
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[CheckIfOrderIsClosed]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns the purchase orders by employee.
-- =============================================
CREATE PROCEDURE [dbo].[CheckIfOrderIsClosed]
	-- Add the parameters for the stored procedure here
	@result BIT OUTPUT,
	@orderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	
    -- Insert statements for procedure here
 	
 	-- Check to ensure that the order is not already closed.
 	BEGIN TRY
	IF (SELECT [Status] FROM [Order] WHERE OrderId = @orderId) = 0
		SET @result = 0
	ELSE
		-- Order still open.
		SET @result = 1
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[CheckSupervisor]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CheckSupervisor]
	-- Add the parameters for the stored procedure here
	@employeeId INT,
	@result BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    BEGIN TRY
    IF NOT EXISTS(SELECT 1 FROM Employee WHERE Employee.EmployeeId = @employeeId)
		RAISERROR('The employee Id does not exist.',15,1)
	ELSE
	BEGIN
		IF ((SELECT Job.Title FROM Job
			 JOIN Employee ON Employee.JobId = Job.JobId
			 WHERE Employee.EmployeeId = @employeeId) = 'Supervisor')
			 SET @result = 1
		ELSE
			SET @result = 0
	END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[CloseOrder]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns the purchase orders by employee.
-- =============================================
CREATE PROCEDURE [dbo].[CloseOrder]
	-- Add the parameters for the stored procedure here
	@employeeId INT,
	@orderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	
    -- Insert statements for procedure here
    BEGIN TRY
    IF NOT EXISTS(SELECT 1 FROM [Order] WHERE OrderId = @orderId)
		RAISERROR('There is no order associated with the given order Id.',15,1)
	ELSE
    BEGIN
		DECLARE @result BIT = 0
		EXEC CheckIfItemsArePending @result OUTPUT, @orderId
		IF @result = 1
			RAISERROR('Unable to close order due to one or more pending items associated with this order.',15,1)
		ELSE
		BEGIN
			IF (SELECT [Order].EmployeeId FROM [Order] WHERE [Order].OrderId = @orderId) = @employeeId
				RAISERROR('Unable to close your own order.',15,1)
			UPDATE [Order] SET [Status] = 1 WHERE OrderId = @orderId
		END
	END   
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[CreateBonus]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 29, 2012
-- Description:	Creates bonus record for one or more employees
-- =============================================
CREATE PROCEDURE [dbo].[CreateBonus]
	-- Add the parameters for the stored procedure here
	@employeeIdTable AS IDTABLE READONLY,
	@payPeriod AS DATE,
	@percent AS DECIMAL(5,4) = 0.0000,
	@fixedBonus AS MONEY = 0
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY
		BEGIN TRAN
			
			IF @payPeriod < dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
				RAISERROR('Bonus cannot be applied to a previous pay period.',16,1)
								
			-- IF PERCENT BONUS
			IF @percent > 0				
				INSERT INTO Bonus(PayPeriod, PercentBonus)
				VALUES (@payPeriod, @percent)
			-- IF FIXED BONUS				
			ELSE IF @fixedBonus > 0
				INSERT INTO Bonus(PayPeriod, FixedBonus)
				VALUES (@payPeriod, @fixedBonus)
			
			DECLARE @bonusId int = SCOPE_IDENTITY()
			DECLARE @employeeId int
			
			SELECT * INTO #IdList FROM @employeeIdTable
			
			IF ((SELECT COUNT(*)FROM #IdList) = 0)
				RAISERROR('No Employees Selected',16,1)
						
			-- LOOP TO LINK BONUS WITH EMPLOYEES
			WHILE EXISTS(SELECT TOP 1 * FROM #IdList)
				BEGIN
					SELECT TOP 1 @employeeId = Id FROM #IdList	
					
					-- Links the Bonus to the applicable employees							
					INSERT INTO EmployeeBonus (EmployeeId, BonusId)
					VALUES (@employeeId, @bonusId)
							
					DELETE TOP (1) FROM #IdList
				END			
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000)
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		IF @@TRANCOUNT > 0
			BEGIN
			ROLLBACK TRAN
				SELECT
				@ErrorMessage = 'Error number ' + CONVERT(VARCHAR, ERROR_NUMBER(),1)
				+ ' Transaction rolled back - ' + ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			END
			ELSE
			BEGIN
				SELECT
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
			END
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[CreateOrder]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateOrder] 
	-- Add the parameters for the stored procedure here
	@orderId INT OUTPUT,
	@employeeId INT,
	@itemTable ITEMTABLE READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @successCount INT = 0
    BEGIN TRY
		BEGIN TRAN
			INSERT INTO [Order]
				(EmployeeId)
			VALUES
				(@employeeId)	
			SET @orderId = SCOPE_IDENTITY()	
			EXEC CreateOrderItems @successCount OUTPUT, @orderId, @itemTable				
		COMMIT TRAN			
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);	
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[CreateOrderItems]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 26, 2012
-- Description:	This procedure inserts a new Item record into the Item table.
-- =============================================
CREATE PROCEDURE [dbo].[CreateOrderItems]
	-- Add the parameters for the stored procedure here
	@successCount INT OUTPUT,
	@orderId INT,
	@itemTable ITEMTABLE READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here    
    DECLARE @name VARCHAR(30)
    DECLARE @description VARCHAR(100)
    DECLARE @price MONEY
    DECLARE @quantity INT
    DECLARE @justification VARCHAR(100)
    DECLARE @source VARCHAR(100)
    
    -- Add all the items from the @itemTable variable into a temporary table.
    SELECT * INTO #itemList FROM @itemTable
    
    -- Loop through the list of items in the temporary table and insert each as a record.
    WHILE EXISTS(SELECT TOP 1 * FROM #itemList)
    BEGIN		
		-- Declare a variable for each item field and assign it a value from
		-- the matching column value in the temporary table 
		SELECT TOP 1 @name = Name FROM #itemList
		SELECT TOP 1 @description = [Description] FROM #itemList
		SELECT TOP 1 @price = Price FROM #itemList
		SELECT TOP 1 @quantity = Quantity FROM #itemList
		SELECT TOP 1 @justification = Justification FROM #itemList
		SELECT TOP 1 @source = [Source] FROM #itemList
		
		BEGIN TRY    
			BEGIN TRAN
				INSERT INTO Item(
								 OrderId,
								 Name,
								 [Description],
								 Price,
								 Quantity,
								 Justification,
								 [Source])
						  VALUES(
								 @orderId,
								 @name,
								 @description,
								 @price,
								 @quantity,
								 @justification,
								 @source)
			COMMIT TRAN		
			-- Item record added successfully, increment the count.
			SET @successCount += 1 
		END TRY
		
		BEGIN CATCH
			IF @@TRANCOUNT > 0
				ROLLBACK TRAN
		END CATCH	
			
		-- Remove successfully inserted record from the list of items.
		DELETE TOP (1) FROM #itemList 
	END
END
GO
/****** Object:  StoredProcedure [dbo].[CreateSalaryAdjustment]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 27, 2012
-- Description:	Creates a salary adjustment record for one or many employees
-- =============================================
CREATE PROCEDURE [dbo].[CreateSalaryAdjustment]
	-- Parameters
	@employeeIdTable as IDTABLE readonly, -- List of IDs if this adjustment applies to many
	@percentIncrease decimal(5,4), -- 0.0000 format		
	@effectiveDate date 	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY			
			IF @effectiveDate < dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
				RAISERROR('Effective date invalid. Payroll has already run for specified effective date',16,1)
			DECLARE @employeeId int			
			SELECT * INTO #IdList FROM @employeeIdTable			
			BEGIN TRAN
				-- Inserts the Salary Adjustment record
				INSERT INTO SalaryAdjustment (PercentIncrease, EffectiveDate)
				VALUES (@percentIncrease, @effectiveDate)
				DECLARE @salaryAdjId int = SCOPE_IDENTITY() -- Gets the ID of the last insert
				
				WHILE EXISTS(SELECT TOP 1 * FROM #IdList)
				BEGIN
					SELECT TOP 1 @employeeId = Id FROM #IdList	
					
					-- Links the Salary Adjustment to the applicable employees							
					INSERT INTO EmployeeSalaryAdjustment (EmployeeId, SalaryAdjustmentId)
					VALUES (@employeeId, @salaryAdjId)
							
					DELETE TOP (1) FROM #IdList
				END
			COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000)
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		IF @@TRANCOUNT > 0
			BEGIN
			ROLLBACK TRAN
				SELECT
				@ErrorMessage = 'Error number ' + CONVERT(VARCHAR, ERROR_NUMBER(),1)
				+ ' Transaction rolled back - ' + ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			END
			ELSE
			BEGIN
				SELECT
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
			END
	END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteBonus]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 29, 2012
-- Description:	Deletes a Bonus
-- =============================================
CREATE PROCEDURE [dbo].[DeleteBonus]
	-- Add the parameters for the stored procedure here
	@bonusId int
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRY
	
		-- Check to ensure salary adjustment hasn't been run in a previous payroll
		IF(SELECT PayPeriod FROM Bonus
			WHERE BonusId = @bonusId)
			< dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
			RAISERROR('Unable to delete bonuses that have already been processed by payroll',16,1)
		
		BEGIN TRAN		
				
			DELETE FROM EmployeeBonus
			WHERE BonusId = @bonusId
			
			DELETE FROM Bonus
			WHERE BonusId = @bonusId
			
			-- IF No other employees are linked to that bonus Delete it
			IF NOT EXISTS(SELECT * FROM EmployeeBonus
							WHERE BonusId = @bonusId)						
				DELETE FROM Bonus
				WHERE BonusId = @bonusId					
				
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000)
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		IF @@TRANCOUNT > 0
			BEGIN
			ROLLBACK TRAN
				SELECT
				@ErrorMessage = 'Error number ' + CONVERT(VARCHAR, ERROR_NUMBER(),1)
				+ ' Transaction rolled back - ' + ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			END
			ELSE
			BEGIN
				SELECT
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
			END
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteSalaryAdjustmentById]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 28, 2012
-- Description:	Deletes specified salary adjustment record(s)
-- =============================================
CREATE PROCEDURE [dbo].[DeleteSalaryAdjustmentById]
	-- Parameters
	@salaryAdjId int	
AS
BEGIN
	 -- Insert statements for procedure here
	BEGIN TRY
	
		-- Check to ensure salary adjustment hasn't been run in a previous payroll
		IF(SELECT EffectiveDate FROM SalaryAdjustment
			WHERE SalaryAdjustmentId = @salaryAdjId)
			< dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
			RAISERROR('Unable to delete salary adjustment that have already been processed by payroll',16,1)
		
		BEGIN TRAN		
				
			DELETE FROM EmployeeSalaryAdjustment
			WHERE SalaryAdjustmentId = @salaryAdjId
			
			DELETE FROM SalaryAdjustment
			WHERE SalaryAdjustmentId = @salaryAdjId			
				
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000)
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		IF @@TRANCOUNT > 0
			BEGIN
			ROLLBACK TRAN
				SELECT
				@ErrorMessage = 'Error number ' + CONVERT(VARCHAR, ERROR_NUMBER(),1)
				+ ' Transaction rolled back - ' + ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			END
			ELSE
			BEGIN
				SELECT
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
			END
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteSingleBonusByEmpId]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 1, 2012
-- Description:	Deletes bonus record for employee
-- =============================================
CREATE PROCEDURE [dbo].[DeleteSingleBonusByEmpId]
	-- Add the parameters for the stored procedure here
	@employeeId int,
	@bonusId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    BEGIN TRY
		-- Check to ensure bonus hasn't already been applied
		IF(SELECT PayPeriod FROM Bonus
			WHERE BonusId = @bonusId)
			< dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
			RAISERROR('Unable to delete bonuses that have already been processed by payroll',16,1)
		
		BEGIN TRAN
			DELETE EmployeeBonus
			WHERE EmployeeId = @employeeId 
				AND BonusId = @bonusId
			
			-- IF No other employees are linked to that bonus Delete it
			IF NOT EXISTS(SELECT * FROM EmployeeBonus
							WHERE BonusId = @bonusId)						
				DELETE FROM Bonus
				WHERE BonusId = @bonusId			
		COMMIT
		
    END TRY
    BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000)
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		IF @@TRANCOUNT > 0
			BEGIN
			ROLLBACK TRAN
				SELECT
				@ErrorMessage = 'Error number ' + CONVERT(VARCHAR, ERROR_NUMBER(),1)
				+ ' Transaction rolled back - ' + ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			END
			ELSE
			BEGIN
				SELECT
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
			END
    END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteSingleEmployeeSalaryAdjustment]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 28, 2012
-- Description:	Deletes specified salary adjustment record(s)
-- =============================================
CREATE PROCEDURE [dbo].[DeleteSingleEmployeeSalaryAdjustment]
	-- Parameters
	@salaryAdjId int,
	@employeeId int	
AS
BEGIN
	 -- Insert statements for procedure here
	BEGIN TRY
	
		-- Check to ensure salary adjustment hasn't been run in a previous payroll
		IF(SELECT EffectiveDate FROM SalaryAdjustment
			WHERE SalaryAdjustmentId = @salaryAdjId)
			< dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
			RAISERROR('Unable to delete salary adjustment that have already been processed by payroll',16,1)
		
		BEGIN TRAN		
				
			DELETE FROM EmployeeSalaryAdjustment
			WHERE SalaryAdjustmentId = @salaryAdjId AND
				EmployeeId = @employeeId
			
			-- IF No other employees are linked to that salary adjustment Delete it
			IF NOT EXISTS(SELECT * FROM EmployeeSalaryAdjustment
							WHERE SalaryAdjustmentId = @salaryAdjId)						
				DELETE FROM SalaryAdjustment
				WHERE SalaryAdjustmentId = @salaryAdjId			
				
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000)
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		IF @@TRANCOUNT > 0
			BEGIN
			ROLLBACK TRAN
				SELECT
				@ErrorMessage = 'Error number ' + CONVERT(VARCHAR, ERROR_NUMBER(),1)
				+ ' Transaction rolled back - ' + ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			END
			ELSE
			BEGIN
				SELECT
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
			END
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllActiveEmployees]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 30, 2012
-- Description:	Gets all active employees
-- =============================================
CREATE PROCEDURE [dbo].[GetAllActiveEmployees] 
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EmployeeId, FirstName, LastName, MiddleInitial, [Address],
			City, Province, PostalCode, Phone, CellPhone, Email, DateOfBirth,
			[SIN], [Status], HireDate, JobStartDate, ActiveEmployees.JobId, 
			Salary, SalaryEffectiveDate, PrevSalary, Job.Title, Job.Department
			   FROM ActiveEmployees JOIN Job ON
		ActiveEmployees.JobId = Job.JobId
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllEmployees]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 15, 2012
-- Description:	Get All Employees
-- =============================================
CREATE PROCEDURE [dbo].[GetAllEmployees]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EmployeeId, FirstName, LastName, MiddleInitial, [Address],
			City, Province, PostalCode, Phone, CellPhone, Email, DateOfBirth,
			[SIN], [Status], HireDate, JobStartDate, Employee.JobId, 
			Salary, SalaryEffectiveDate, PrevSalary, Job.Title, Job.Department
			   FROM Employee JOIN Job ON
		Employee.JobId = Job.JobId
END
GO
/****** Object:  StoredProcedure [dbo].[GetBonusByEmpId]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 1, 2012
-- Description:	Gets the bonuses for employee
-- =============================================
CREATE PROCEDURE [dbo].[GetBonusByEmpId] 
	-- Add the parameters for the stored procedure here
	@employeeId int,
	@getType int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @getType = 0
		SELECT * FROM Bonus JOIN EmployeeBonus ON
			Bonus.BonusId = EmployeeBonus.BonusId
			WHERE EmployeeId = @employeeId AND PayPeriod > dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
	ELSE
		SELECT * FROM Bonus JOIN EmployeeBonus ON
			Bonus.BonusId = EmployeeBonus.BonusId
			WHERE EmployeeId = @employeeId AND PayPeriod < dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
END
GO
/****** Object:  StoredProcedure [dbo].[GetBonusById]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 1, 2012
-- Description:	Gets a bonus record by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetBonusById]
	-- Add the parameters for the stored procedure here
	@bonusId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Bonus WHERE BonusId = @bonusId
END
GO
/****** Object:  StoredProcedure [dbo].[GetCurrentPayPeriodStartDate]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 06, 2012
-- Description:	Returns the start date of the current pay period
-- =============================================
CREATE PROCEDURE [dbo].[GetCurrentPayPeriodStartDate] 
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
END
GO
/****** Object:  StoredProcedure [dbo].[GetDeptSupervisor]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 27, 2012
-- Description:	This procedure returns the name of the supervisor relevant to the employee.
-- Modified: By Joel Romero
-- Change: Returns the name of the supervisor of specified department
-- =============================================
CREATE PROCEDURE [dbo].[GetDeptSupervisor]
	-- Add the parameters for the stored procedure here
	@deptId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here	
	SELECT EmployeeId, FirstName, LastName, MiddleInitial FROM Employee
	JOIN Job ON Employee.JobId = Job.JobId
	WHERE Title = 'Supervisor' AND Job.Department = @deptId
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeById]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 30, 2012
-- Description:	Gets employee by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetEmployeeById]
	-- Add the parameters for the stored procedure here
	@employeeId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EmployeeId, FirstName, LastName, MiddleInitial, [Address],
			City, Province, PostalCode, Phone, CellPhone, Email, DateOfBirth,
			[SIN], [Status], HireDate, JobStartDate, ActiveEmployees.JobId, 
			Salary, SalaryEffectiveDate, PrevSalary, Job.Title, Job.Department
			   FROM ActiveEmployees JOIN Job ON
		ActiveEmployees.JobId = Job.JobId 
		WHERE EmployeeId = @employeeId
END
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeByName]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 30, 2012
-- Description:	Gets employees by name search
-- =============================================
CREATE PROCEDURE [dbo].[GetEmployeeByName]
	-- Add the parameters for the stored procedure here
	@firstName varchar(25) = NULL,
	@lastName varchar(25) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @firstName IS NULL		
		SELECT EmployeeId, FirstName, LastName, MiddleInitial, [Address],
			City, Province, PostalCode, Phone, CellPhone, Email, DateOfBirth,
			[SIN], [Status], HireDate, JobStartDate, ActiveEmployees.JobId, 
			Salary, SalaryEffectiveDate, PrevSalary, Job.Title, Job.Department
			   FROM ActiveEmployees JOIN Job ON
		ActiveEmployees.JobId = Job.JobId 
		WHERE LastName LIKE @lastName
	
	ELSE IF @lastName IS NULL
		SELECT EmployeeId, FirstName, LastName, MiddleInitial, [Address],
			City, Province, PostalCode, Phone, Email, DateOfBirth,
			[SIN], [Status], HireDate, JobStartDate, ActiveEmployees.JobId, 
			Salary, SalaryEffectiveDate, PrevSalary, Job.Title, Job.Department
			   FROM ActiveEmployees JOIN Job ON
		ActiveEmployees.JobId = Job.JobId 
		WHERE FirstName LIKE @firstName
	
	ELSE
		SELECT EmployeeId, FirstName, LastName, MiddleInitial, [Address],
			City, Province, PostalCode, Phone, Email, DateOfBirth,
			[SIN], [Status], HireDate, JobStartDate, ActiveEmployees.JobId, 
			Salary, SalaryEffectiveDate, PrevSalary, Job.Title, Job.Department
			   FROM ActiveEmployees JOIN Job ON
		ActiveEmployees.JobId = Job.JobId 
		WHERE FirstName LIKE @firstName 
		AND LastName LIKE @lastName
		
END
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeLookup]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns a list of names of employees with associated Id.
-- =============================================
CREATE PROCEDURE [dbo].[GetEmployeeLookup]
	-- Add the parameters for the stored procedure here
	@department INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT Employee.EmployeeId, Employee.LastName, Employee.FirstName FROM Employee 
    JOIN Job ON Employee.JobId = Job.JobId Where Job.Department = @department
    ORDER BY Employee.LastName, Employee.FirstName
END
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeesByDept]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 30, 2012
-- Description:	Gets all active employees in department
-- =============================================
CREATE PROCEDURE [dbo].[GetEmployeesByDept]
	-- Add the parameters for the stored procedure here
	@deptId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM ActiveEmployees JOIN Job ON
		ActiveEmployees.JobId = Job.JobId
	WHERE Department = @deptId
END
GO
/****** Object:  StoredProcedure [dbo].[GetEmployeeSummaryById]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 26, 2012
-- Description: Gets an employee details by ID 
-- =============================================
CREATE PROCEDURE [dbo].[GetEmployeeSummaryById]
	-- Parameters
	@employeeId	int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Employee.EmployeeId, FirstName, LastName, MiddleInitial, [Address], City, Province,
			PostalCode, Phone, Email, DateOfBirth, [SIN], Employee.[Status], HireDate,
			JobStartDate
	
	FROM Employee 
	JOIN Department ON Employee.DepartmentId = Department.DepartmentId
	
		
	WHERE Employee.EmployeeId = @employeeId
	
END
GO
/****** Object:  StoredProcedure [dbo].[getHighestFiveEarningYears]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 13, 2012
-- Description:	Gets the 5 highest paid years for employee
-- =============================================
CREATE PROCEDURE [dbo].[getHighestFiveEarningYears]
	-- Add the parameters for the stored procedure here
	@empId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 5 SUM(GrossPay + BonusPay) AS [YTD], YEAR(PayPeriod) AS [YEAR] FROM PayStub
	WHERE EmployeeId = @empId
	GROUP BY YEAR(PayPeriod)
	ORDER BY [YTD] DESC
END
GO
/****** Object:  StoredProcedure [dbo].[GetItemById]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns a single item record.
-- =============================================
CREATE PROCEDURE [dbo].[GetItemById]
	-- Add the parameters for the stored procedure here
	@itemId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	
    -- Insert statements for procedure here
    SELECT * FROM Item WHERE ItemId = @itemId
END
GO
/****** Object:  StoredProcedure [dbo].[GetItemsByOrderId]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns the purchase orders by employee.
-- =============================================
CREATE PROCEDURE [dbo].[GetItemsByOrderId]
	-- Add the parameters for the stored procedure here
	@orderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	
    -- Insert statements for procedure here
    
    SELECT * FROM Item WHERE OrderId = @orderId
END
GO
/****** Object:  StoredProcedure [dbo].[GetJobDetails]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 30, 2012
-- Description:	Gets the specified job details
-- =============================================
CREATE PROCEDURE [dbo].[GetJobDetails] 
	-- Add the parameters for the stored procedure here
	@jobId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Job
	WHERE JobId = @jobId
END
GO
/****** Object:  StoredProcedure [dbo].[GetJobsByDept]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 14, 2012
-- Description:	Gets Jobs by Department
-- =============================================
CREATE PROCEDURE [dbo].[GetJobsByDept]
	-- Add the parameters for the stored procedure here
	@dept int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT jobId, Title FROM JOB
	WHERE Department = @dept
	ORDER BY Title
END
GO
/****** Object:  StoredProcedure [dbo].[GetLoginAuthorization]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLoginAuthorization] 
	-- Add the parameters for the stored procedure here
	@authLevel INT OUTPUT,
	@employeeId INT,
	@password VARCHAR(12)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRY
	IF EXISTS(SELECT 1 FROM EmployeeLogin WHERE EmployeeId = @employeeId AND [Password] = @password)
	BEGIN
		IF EXISTS(SELECT 1 FROM Employee WHERE EmployeeId = @employeeId AND [Status] = 1)
		BEGIN
			DECLARE @result BIT
			EXEC CheckSupervisor @employeeId, @result OUTPUT
			IF @result = 0
				SET @authLevel = 0
			ELSE
				SET @authLevel = 1
			EXEC CheckCEO @employeeId, @result OUTPUT
			IF @result = 1
				SET @authLevel = 2
		END
		ELSE
			RAISERROR('You are no longer authorized to view this page. Please check with human resources.',15,1)
	END
	ELSE
		RAISERROR('We were unable to grant access with the given credentials. Please try again, or check with your supervisor.',15,1)
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[GetOrderById]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns the purchase order by it's Id.
-- =============================================
CREATE PROCEDURE [dbo].[GetOrderById]
	-- Add the parameters for the stored procedure here
	@employeeId INT,
	@orderId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
    BEGIN TRY
    IF EXISTS
		(SELECT 1 FROM [Order] WHERE orderId = @orderId)
	BEGIN
		DECLARE @myDepartment INT
		DECLARE @isSupervisor BIT
		DECLARE @orderEmployeeId INT
		DECLARE @orderDepartment INT
		
		SET @myDepartment = (SELECT Job.Department FROM Job
						     JOIN Employee ON Employee.JobId = Job.JobId
							 WHERE Employee.EmployeeId = @employeeId)
		EXEC CheckSupervisor @employeeId, @isSupervisor OUTPUT
		
		SET @orderEmployeeId = (SELECT EmployeeId From [Order] WHERE OrderId = @orderId)
		SET @orderDepartment = (SELECT Job.Department FROM Job 
					               JOIN Employee ON Employee.JobId = Job.JobId
					               WHERE Employee.EmployeeId = @orderEmployeeId)
		IF (@myDepartment <> @orderDepartment)
			RAISERROR('This order is restricted from view as it is from another department.',15,1)
		IF (@employeeId <> @orderEmployeeId)
		BEGIN
			IF @isSupervisor = 0
				RAISERROR('This order is restrictred from view as you are not the creator, nor the supervisor of the department',15,1)
		END

				SELECT [Order].OrderId, [Order].EmployeeId,
			    Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
			    [Order].OrderDate,
			    [Order].[Status],
			    Job.Department,
			    dbo.GetOrderTotals(OrderId,0) AS SubTotal,
			    dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
			    dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order] 
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE [Order].OrderId = @orderId
				Order BY [Order].OrderDate DESC
				
	END
	ELSE
		RAISERROR('There are no orders by the given order number.',15,1)
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[GetOrdersByDate]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns the purchase orders by date range.
-- =============================================
CREATE PROCEDURE [dbo].[GetOrdersByDate]
	-- Add the parameters for the stored procedure here
	@department INT,
	@dateFrom DATE,
	@dateTo DATE,
	@filter INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @dateTo < @dateFrom
		RAISERROR('Date from cannot be before the date to.',15,1)
    IF @filter = 0
		SELECT [Order].OrderId, [Order].EmployeeId,
			   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
			   [Order].OrderDate,
			   Job.Department,
			   [Order].[Status],
			   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
			   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
			   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
		FROM [Order] 
		JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
		JOIN Job ON Employee.JobId = Job.JobId
		WHERE [Order].OrderDate 
		BETWEEN @dateFrom AND @dateTo AND [Order].[Status] = 0 
		AND Job.Department = @department
		ORDER BY OrderDate DESC 
	IF @filter = 1
		SELECT [Order].OrderId, [Order].EmployeeId,
			   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
			   [Order].OrderDate,
			   Job.Department,
			   [Order].[Status],
			   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
			   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
			   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
		FROM [Order] 
		JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
		JOIN Job ON Employee.JobId = Job.JobId
		WHERE [Order].OrderDate 
		BETWEEN @dateFrom AND @dateTo AND [Order].[Status] = 1
		AND Job.Department = @department
		ORDER BY OrderDate DESC
	IF @filter = 2
		SELECT [Order].OrderId, [Order].EmployeeId,
			   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
			   [Order].OrderDate,
			   Job.Department,
			   [Order].[Status],
			   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
			   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
			   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
		FROM [Order] 
		JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
		JOIN Job ON Employee.JobId = Job.JobId
		WHERE [Order].OrderDate 
		BETWEEN @dateFrom AND @dateTo
		AND Job.Department = @department
		ORDER BY OrderDate DESC
	ELSE
		RAISERROR('Invalid filter type.',15,1)
END
GO
/****** Object:  StoredProcedure [dbo].[GetOrdersByDepartment]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns the purchase orders by employee.
-- =============================================
CREATE PROCEDURE [dbo].[GetOrdersByDepartment]
	-- Add the parameters for the stored procedure here
	@employeeId INT,
	@department INT,
	@filter INT,
	@dateFrom DATE = NULL,
	@dateTo DATE = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    BEGIN TRY
    IF @filter = 0
    BEGIN
		IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
		BEGIN
			SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE Job.Department = @department AND [Order].[Status] = 0
			AND [Order].OrderDate BETWEEN @dateFrom AND @dateTo
			AND [Order].EmployeeId <> @employeeId
			ORDER BY [Order].OrderDate ASC
			END
		ELSE
			SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE Job.Department = @department AND [Order].[Status] = 0
			AND [Order].EmployeeId <> @employeeId
			ORDER BY [Order].OrderDate DESC
	END
	ELSE IF @filter = 1
		BEGIN
			IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
				SELECT [Order].OrderId, [Order].EmployeeId,
					   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
					   [Order].OrderDate,
					   [Order].[Status],
					   Job.Department,
					   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
					   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
					   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order] 
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE Job.Department = @department AND [Order].[Status] = 1
				AND [Order].OrderDate BETWEEN @dateFrom AND @dateTo
				ORDER BY [Order].OrderDate DESC
			ELSE
				SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE Job.Department = @department AND [Order].[Status] = 1
			AND [Order].EmployeeId <> @employeeId
			ORDER BY [Order].OrderDate DESC
		END
	ELSE IF @filter = 2
	BEGIN
		IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
			SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE Job.Department = @department
			AND [Order].OrderDate BETWEEN @dateFrom AND @dateTo
			AND [Order].EmployeeId <> @employeeId
			ORDER BY [Order].OrderDate DESC
		ELSE
			SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE Job.Department = @department
			AND [Order].EmployeeId <> @employeeId
			ORDER BY [Order].OrderDate DESC
	END
	ELSE
		RAISERROR('Invalid filter type.',15,1)
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[GetOrdersByEmployeeId]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns the purchase orders by employee.
-- =============================================
CREATE PROCEDURE [dbo].[GetOrdersByEmployeeId]
	-- Add the parameters for the stored procedure here
	@employeeId INT,
	@dateFrom DATE = NULL,
	@dateTo DATE = NULL,
	@filter INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	BEGIN TRY
	IF @filter = 0
    BEGIN
		IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
		BEGIN
			SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE [Order].EmployeeId = @employeeId 
			AND [Order].[Status] = 0
			AND [Order].OrderDate BETWEEN @dateFrom AND @dateTo
			ORDER BY [Order].OrderDate ASC
			END
		ELSE
			SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE [Order].EmployeeId = @employeeId
			AND [Order].[Status] = 0
			ORDER BY [Order].OrderDate ASC
	END
	ELSE IF @filter = 1
		BEGIN
			IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
				SELECT [Order].OrderId, [Order].EmployeeId,
					   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
					   [Order].OrderDate,
					   [Order].[Status],
					   Job.Department,
					   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
					   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
					   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order] 
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE [Order].[Status] = 1
				AND [Order].OrderDate BETWEEN @dateFrom AND @dateTo
				ORDER BY [Order].OrderDate DESC
			ELSE
				SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE [Order].[Status] = 1
			ORDER BY [Order].OrderDate DESC
		END
	ELSE IF @filter = 2
	BEGIN
		IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
			SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE [Order].OrderDate BETWEEN @dateFrom AND @dateTo
			AND [Order].EmployeeId = @employeeId
			ORDER BY [Order].OrderDate DESC
		ELSE
			SELECT [Order].OrderId, [Order].EmployeeId,
				   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
				   [Order].OrderDate,
				   [Order].[Status],
				   Job.Department,
				   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
				   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
				   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
			FROM [Order] 
			JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
			JOIN Job ON Employee.JobId = Job.JobId
			WHERE [Order].EmployeeId = @employeeId
			ORDER BY [Order].OrderDate DESC
	END
	ELSE
		RAISERROR('Invalid filter type.',15,1)
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[GetOrdersByEmployeeName]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: April 30, 2012
-- Description:	This procedure returns the purchase orders by employee.
-- =============================================
CREATE PROCEDURE [dbo].[GetOrdersByEmployeeName]
	-- Add the parameters for the stored procedure here
	@employeeId INT,
	@department INT,
	@filter INT,
	@firstName VARCHAR(25) = NULL,
	@lastName VARCHAR(25) = NULL,
	@dateFrom DATE = NULL,
	@dateTo DATE = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    BEGIN TRY
    IF @firstName IS NULL AND @lastName IS NULL
		RAISERROR('No names were provided to search by.',15,1)
	ELSE
	BEGIN
		IF @filter = 0
		BEGIN
			IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
				SELECT [Order].OrderId, [Order].EmployeeId,
					   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
					   [Order].OrderDate,
					   [Order].[Status],
					   Job.Department,
					   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
					   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
					   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order]
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE Job.Department = @department
				AND [Order].OrderDate BETWEEN @dateFrom AND @dateTo
				AND [Order].[Status] = 0	
				AND [Order].EmployeeId <> @employeeId
				AND	(Employee.FirstName LIKE @firstName 
				OR Employee.LastName LIKE @lastName)				
				ORDER BY [Order].OrderDate ASC
			ELSE
				SELECT [Order].OrderId, [Order].EmployeeId,
					   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
					   [Order].OrderDate,
					   [Order].[Status],
					   Job.Department,
					   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
					   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
					   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order]
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE Job.Department = @department
				AND [Order].[Status] = 0		
				AND [Order].EmployeeId <> @employeeId
				AND	Employee.FirstName LIKE @firstName 
				OR Employee.LastName LIKE @lastName					
				ORDER BY [Order].OrderDate DESC
		END
		ELSE IF @filter = 1
		BEGIN
			IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
				SELECT [Order].OrderId, [Order].EmployeeId,
					   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
					   [Order].OrderDate,
					   [Order].[Status],
				       Job.Department,
				       dbo.GetOrderTotals(OrderId,0) AS SubTotal,
					   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
					   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order]
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE Job.Department = @department
				AND	(Employee.FirstName LIKE @firstName 
				OR Employee.LastName LIKE @lastName)
				AND [Order].OrderDate BETWEEN @dateFrom AND @dateTo
				AND [Order].[Status] = 1	
				AND [Order].EmployeeId <> @employeeId		
				ORDER BY [Order].OrderDate DESC
			ELSE
				SELECT [Order].OrderId, [Order].EmployeeId,
					   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
					   [Order].OrderDate,
					   [Order].[Status],
					   Job.Department,
					   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
					   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
					   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order]
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE Job.Department = @department	
				AND [Order].[Status] = 1
				AND [Order].EmployeeId <> @employeeId		
				AND	(Employee.FirstName LIKE @firstName 
				OR Employee.LastName LIKE @lastName)							
				ORDER BY [Order].OrderDate DESC
		END
		ELSE IF @filter = 2
		BEGIN
			IF @dateFrom IS NOT NULL AND @dateTo IS NOT NULL
				SELECT [Order].OrderId, [Order].EmployeeId,
					   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
					   [Order].OrderDate,
					   [Order].[Status],
					   Job.Department,
					   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
					   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
					   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order]
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE Job.Department = @department
				AND [Order].OrderDate BETWEEN @dateFrom AND @dateTo		
				AND [Order].EmployeeId <> @employeeId
				AND	(Employee.FirstName LIKE @firstName 
				OR Employee.LastName LIKE @lastName)					
				ORDER BY [Order].OrderDate DESC
			ELSE
				SELECT [Order].OrderId, [Order].EmployeeId,
					   Employee.LastName + ', ' + Employee.FirstName AS EmployeeName,			   
					   [Order].OrderDate,
					   [Order].[Status],
					   Job.Department,
					   dbo.GetOrderTotals(OrderId,0) AS SubTotal,
					   dbo.GetOrderTotals(OrderId,1) AS TaxTotal,
					   dbo.GetOrderTotals(OrderId,2) AS GrandTotal
				FROM [Order]
				JOIN Employee ON Employee.EmployeeId = [Order].EmployeeId
				JOIN Job ON Employee.JobId = Job.JobId
				WHERE Job.Department = @department
				AND [Order].EmployeeId <> @employeeId
				AND	(Employee.FirstName LIKE @firstName 
				OR Employee.LastName LIKE @lastName)
				ORDER BY [Order].OrderDate DESC
		END
		ELSE
			RAISERROR('Invalid filter type.',15,1)
	END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[GetPayStubById]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 1, 2012
-- Description:	Get Paystub by PaystubId
-- =============================================
CREATE PROCEDURE [dbo].[GetPayStubById]
	-- Add the parameters for the stored procedure here
	@payStubId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM PayStub
	WHERE PayStubId = @payStubId
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetPayStubsByDateRange]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 11, 2012
-- Description:	Gets back the pay stubs by date range
-- =============================================
CREATE PROCEDURE [dbo].[GetPayStubsByDateRange] 
	-- Add the parameters for the stored procedure here
	@employeeId int,
	@dateFrom date,
	@dateTo date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM PayStub
	WHERE EmployeeId = @employeeId AND
	PayPeriod >= @dateFrom AND PayPeriod <= @dateTo
	ORDER BY PayPeriod DESC
END
GO
/****** Object:  StoredProcedure [dbo].[GetPayStubsByEmpId]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 1, 2012
-- Description:	Gets pay stubs for an employee
-- =============================================
CREATE PROCEDURE [dbo].[GetPayStubsByEmpId]
	-- Add the parameters for the stored procedure here
	@employeeId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM PayStub
	WHERE EmployeeId = @employeeId
	ORDER by PayPeriod DESC
END
GO
/****** Object:  StoredProcedure [dbo].[GetSalaryAdjustmentByEmpId]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 1, 2012
-- Description:	Gets the Salary Adjustments for employeeID
-- =============================================
CREATE PROCEDURE [dbo].[GetSalaryAdjustmentByEmpId] 
	-- Add the parameters for the stored procedure here
	@employeeId int,
	@getType int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    IF @getType = 0 -- PENDING ADJUSTMENTS   
		SELECT * FROM SalaryAdjustment JOIN EmployeeSalaryAdjustment
			ON SalaryAdjustment.SalaryAdjustmentId = EmployeeSalaryAdjustment.SalaryAdjustmentId
			WHERE EmployeeId = @employeeId AND EffectiveDate > dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
			ORDER BY EffectiveDate
	ELSE IF @getType = 1 -- APPLIED ADJUSTMENTS
		SELECT * FROM SalaryAdjustment JOIN EmployeeSalaryAdjustment
			ON SalaryAdjustment.SalaryAdjustmentId = EmployeeSalaryAdjustment.SalaryAdjustmentId
			WHERE EmployeeId = @employeeId AND EffectiveDate < dbo.GET_CURRENT_PAYPERIOD_STARTDATE()
			ORDER BY EffectiveDate
END
GO
/****** Object:  StoredProcedure [dbo].[GetSalaryAdjustmentById]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 1, 2012
-- Description:	Gets a salary adjustment by ID
-- =============================================
CREATE PROCEDURE [dbo].[GetSalaryAdjustmentById] 
	-- Add the parameters for the stored procedure here
	@salaryAdjId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM SalaryAdjustment
	WHERE SalaryAdjustmentId = @salaryAdjId
END
GO
/****** Object:  StoredProcedure [dbo].[GetSalaryAdjustmentEmployeeIds]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: April 30, 2012
-- Description:	Gets the employee Ids of the specified salary adjustment
-- =============================================
CREATE PROCEDURE [dbo].[GetSalaryAdjustmentEmployeeIds]
	-- Add the parameters for the stored procedure here
	@salaryAdjId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT employeeId FROM EmployeeSalaryAdjustment 
	WHERE SalaryAdjustmentId = @salaryAdjId
		
END
GO
/****** Object:  StoredProcedure [dbo].[GetSickDays]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 3, 2012
-- Description:	Gets the sick days for specified employee
-- =============================================
CREATE PROCEDURE [dbo].[GetSickDays] 
	-- Add the parameters for the stored procedure here
	@empId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SickDayId,[Date],IsFullDay FROM SickDay
	WHERE EmployeeId = @empId
END
GO
/****** Object:  StoredProcedure [dbo].[InsertEmployee]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 15, 2012
-- Description:	Inserts an employee Record
-- =============================================
CREATE PROCEDURE [dbo].[InsertEmployee]
	-- Add the parameters for the stored procedure here
	@empId int OUTPUT,
    @fname varchar(25),
	@intial char(1) = null,
    @lname varchar(25),
    @address varchar(50),
    @city varchar(20),
    @prov char(2),
    @pc char(6),
    @phone char(10),
    @cell char(10) = null,
    @email varchar(50),
    @dob date,
    @sin char(9), 
	@jobstartdate date,
    @jobid int,
    @salary decimal,
    @password varchar(12)    
    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRAN
			-- Insert statements for procedure here
			INSERT INTO Employee (FirstName, LastName, MiddleInitial, [Address], City, Province,
						  PostalCode, Phone, CellPhone, Email, DateOfBirth, [SIN], [Status],
						  HireDate, JobStartDate, JobId, Salary, SalaryEffectiveDate, PrevSalary)
				
				VALUES	 (@fname, @lname, @intial, @address, @city, @prov, @pc, @phone, 
						  @cell, @email, @dob, @sin, 1, GETDATE(), @jobstartdate, @jobid,
						  @salary, GETDATE(), 0)
	
			SET @empId = SCOPE_IDENTITY();
			
			INSERT INTO EmployeeLogin
				VALUES (@empId, @password)
		COMMIT
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000)
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		IF @@TRANCOUNT > 0
			BEGIN
			ROLLBACK TRAN
				SELECT
				@ErrorMessage = 'Error number ' + CONVERT(VARCHAR, ERROR_NUMBER(),1)
				+ ' Transaction rolled back - ' + ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			END
			ELSE
			BEGIN
				SELECT
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
			END
	END CATCH
    
	 
END
GO
/****** Object:  StoredProcedure [dbo].[ProcessItem]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shane LeBlanc
-- Create date: May 2, 2012
-- Description:	This procedure verifies if there are more items pending and can safely close the order.
-- =============================================
CREATE PROCEDURE [dbo].[ProcessItem] 
	-- Add the parameters for the stored procedure here
	@result BIT OUTPUT,
	@employeeId INT,
	@itemId INT,
	@orderId INT,
	@status INT,
	@reason VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    BEGIN TRY
		IF EXISTS(SELECT 1 FROM Item WHERE ItemId = @itemId)
		BEGIN
			IF (SELECT [Order].EmployeeId FROM [Order] WHERE [Order].OrderId = @orderId) = @employeeId
				RAISERROR('Unable to change an item''s status on your own order.',15,1)
			IF (SELECT [Status] FROM [Order] WHERE OrderId = @orderId) = 1
				RAISERROR('Unable to change an item''s status on a closed order status.',15,1)			
			ELSE
				UPDATE Item SET 
					[Status] = @status,
					Reason = @reason
				WHERE ItemId = @itemId
				EXEC CheckIfItemsArePending @result OUTPUT, @orderId
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
/****** Object:  StoredProcedure [dbo].[RecordSickDay]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 15, 2012
-- Description:	Records Sick days
-- =============================================
CREATE PROCEDURE [dbo].[RecordSickDay]
	-- Add the parameters for the stored procedure here
	@empId int,
	@date date,
	@isFullDay bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO SickDay (EmployeeId, [Date], IsFullDay)
				VALUES(@empId, @date, @isFullDay)
END
GO
/****** Object:  StoredProcedure [dbo].[RunPayroll]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: Apriil 29, 2012
-- Description:	Creates a payroll stub record
--				then applies any salary adjustments
-- =============================================
CREATE PROCEDURE [dbo].[RunPayroll]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRY
		BEGIN TRAN
			
			-- Declaring variables for paystub inserts
			DECLARE @employeeId INT, @jobId int
			DECLARE @salary money
			DECLARE @grossPay money, @ytdGross money
			DECLARE @bonus money, @ytdBonus money			
			DECLARE @incomeTaxDed money, @ytdIncomeTaxDed money
			DECLARE @cppDed money, @ytdCPPDed money			
			DECLARE @eiDed money, @ytdEIDed money			
			DECLARE @pensionDed money, @ytdPensionDed money			
			DECLARE @netPay money, @ytdNetPay money			
			
			-- Create a temp table of active employees to iterate through
			SELECT * INTO #ActiveEmployees FROM dbo.ActiveEmployees
			WHILE EXISTS(SELECT TOP 1 * FROM #ActiveEmployees)
			BEGIN -- LOOP THROUGH EMPLOYEES
				SELECT TOP 1 @employeeId = EmployeeId FROM #ActiveEmployees
				SELECT TOP 1 @salary = Salary FROM #ActiveEmployees
				SELECT TOP 1 @jobId = JobId FROM #ActiveEmployees				
				
				SET @grossPay = @salary / 26 -- Calculate BiWeekly Pay
				
				-- Calculate any bonus applicable
				IF EXISTS(SELECT * FROM Bonus JOIN EmployeeBonus
							ON Bonus.BonusId = EmployeeBonus.BonusId
							WHERE PayPeriod = DATEADD(d,-15,GETDATE()) AND EmployeeId = @employeeId)
					BEGIN
						SELECT PercentBonus, FixedBonus INTO #Bonus FROM Bonus JOIN EmployeeBonus 
						ON Bonus.BonusId = EmployeeBonus.BonusId
						WHERE PayPeriod = DATEADD(d,-15,GETDATE()) AND EmployeeId = @employeeId					
					
						DECLARE @percent DECIMAL, @fixed DECIMAL				
						WHILE EXISTS(SELECT TOP 1 * FROM #Bonus)
						BEGIN -- LOOP THROUGH BONUSES
							SELECT TOP 1 @percent = PercentBonus FROM #Bonus
							SELECT TOP 1 @fixed = FixedBonus FROM #Bonus
							IF @percent != 0
								SET @bonus = @bonus + (@salary * @percent) -- Percentage Bonus
							ELSE
								SET @bonus = @bonus + @fixed -- Fixed Bonus
						END -- END BONUS LOOP
						
					END -- END IF EXISTS				
				ELSE
					SET @bonus = 0 -- NO BONUSES
					
				-- Calculate income tax
				SET @incomeTaxDed = (@grossPay + @bonus) * .19
								
				-- Calculate CPP deduction
				IF @incomeTaxDed >= 2100
					SET @cppDed = 2100
				ELSE 
					SET @cppDed = @incomeTaxDed
				
				-- Calculate EI deduction
				IF ((@grossPay + @bonus) * .018) >= 720
					SET @eiDed = 720
				ELSE
					SET @eiDed = (@grossPay + @bonus) * .018
					
				-- Calculate Pension deduction based on YTD amounts
				IF @salary <= 42000
					SET @pensionDed = @grossPay * .058
				ELSE
					SET @pensionDed = (42000 * .058) + (@salary - 42000) * .075		
				
				-- Calculate Net Pay
				DECLARE @totalDed money
				SET @totalDed = @incomeTaxDed + @cppDed + @eiDed + @pensionDed
				SET @netPay = (@grossPay + @bonus) - @totalDed
				
				-- Get the YTD amounts from last paystub in this year
				IF EXISTS(SELECT * FROM PayStub WHERE EmployeeId = @employeeId
							AND PayPeriod > (
								-- Last day of last year
								SELECT DATEADD(ms,-3,DATEADD(yy,0,DATEADD(yy,DATEDIFF(yy,0,GETDATE()),0)))
								)
						 )
				BEGIN
					SELECT TOP 1 * INTO #LastPayStub FROM PayStub
					WHERE EmployeeId = @employeeId
					ORDER BY PayPeriod DESC
						SET @ytdGross = @grossPay + (SELECT YTDGrossPay FROM #LastPayStub)
						SET @ytdBonus = @bonus + (SELECT YTDBonusPay FROM #LastPayStub)
						SET @ytdIncomeTaxDed = @incomeTaxDed + (SELECT YTDIncomeTaxDeduction FROM #LastPayStub)
						SET @ytdCPPDed = @cppDed + (SELECT YTDCPPDeduction FROM #LastPayStub)
						SET @ytdEIDed = @eiDed + (SELECT YTDEIDeduction FROM #LastPayStub)
						SET @ytdPensionDed = @pensionDed + (SELECT YTDPensionDeduction FROM #LastPayStub)
						SET @ytdNetPay = @netPay + (SELECT YTDNetPay FROM #LastPayStub)
					DROP TABLE #LastPayStub
				END
				ELSE -- IF NO PREVIOUS PAYSTUB
				BEGIN
					SET @ytdGross = @grossPay
					SET @ytdBonus = @bonus
					SET @ytdIncomeTaxDed = @incomeTaxDed
					SET @ytdCPPDed = @cppDed
					SET @ytdEIDed = @eiDed
					SET @ytdPensionDed = @pensionDed
					SET @ytdNetPay = @netPay
				END	
						
					
				-- Create PayStub record
				INSERT INTO PayStub(EmployeeId, PayPeriod,
									GrossPay,YTDGrossPay,
									BonusPay,YTDBonusPay,
									IncomeTaxDeduction, YTDIncomeTaxDeduction,
									CPPDeduction, YTDCPPDeduction,
									EIDeduction, YTDEIDeduction,
									PensionDeduction, YTDPensionDeduction,
									NetPay, YTDNetPay)
				VALUES(@employeeId, DATEADD(d,-1,GETDATE()),
						@grossPay,@ytdGross,
						@bonus,@ytdBonus,
						@incomeTaxDed,@ytdIncomeTaxDed,
						@cppDed,@ytdCPPDed,
						@eiDed,@ytdEIDed,
						@pensionDed,@ytdPensionDed,
						@netPay,@ytdNetPay)
				
				-- Apply any Salary Adjustments				
				IF EXISTS(SELECT * FROM SalaryAdjustment JOIN EmployeeSalaryAdjustment
							ON SalaryAdjustment.SalaryAdjustmentId = EmployeeSalaryAdjustment.SalaryAdjustmentId
							WHERE EffectiveDate = GETDATE())
				BEGIN
					DECLARE @salaryIncreasePercent decimal(5,4)
					SELECT TOP 1 PercentIncrease INTO #salaryAdj FROM SalaryAdjustment JOIN EmployeeSalaryAdjustment
					ON SalaryAdjustment.SalaryAdjustmentId = EmployeeSalaryAdjustment.SalaryAdjustmentId
					WHERE EffectiveDate = GETDATE() AND EmployeeId = @employeeId
					
					SELECT @salaryIncreasePercent = PercentIncrease FROM #salaryAdj
					
					-- Check for Max Salary for the job assigned
					DECLARE @maxSalary money
					DECLARE @newSalary money
					SET @maxSalary = (SELECT MaxSalary FROM Job WHERE JobId = @jobId)
					
					-- If increase puts salary over the cap the salary is set to the cap
					IF((@salary * @salaryIncreasePercent)>@maxSalary)
						SET @newSalary = @maxSalary
					ELSE
						SET @newSalary = @salary * @salaryIncreasePercent	
					
					UPDATE Employee
					SET Salary = @newSalary,
						SalaryEffectiveDate = GETDATE(),
						PrevSalary = @salary
					WHERE EmployeeId = @employeeId
					
				END -- END SALARY ADJUSTMENTS		
				
				DELETE TOP (1) FROM #ActiveEmployees
			END -- END EMPLOYEE LOOP
			
		COMMIT TRAN
    END TRY
    BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000)
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		
		IF @@TRANCOUNT > 0
			BEGIN
			ROLLBACK TRAN
				SELECT
				@ErrorMessage = 'Error number ' + CONVERT(VARCHAR, ERROR_NUMBER(),1)
				+ ' Transaction rolled back - ' + ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
			END
			ELSE
			BEGIN
				SELECT
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
				RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
			END
    END CATCH
	
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateEmployee]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 14, 2012
-- Description:	Update Employee
-- =============================================
CREATE PROCEDURE [dbo].[UpdateEmployee]
	-- Add the parameters for the stored procedure here
	@empId int,
    @fname varchar(25),
	@intial char(1) = null,
    @lname varchar(25),
    @address varchar(50),
    @city varchar(20),
    @prov char(2),
    @pc char(6),
    @phone char(10),
    @cell char(10) = null,
    @email varchar(50),
    @dob date,
    @sin char(9),
    @status int,
    @hiredate date,
	@jobstartdate date,
    @jobid int,
    @salary decimal,
    @salaryeffectivedate date,
    @prevsalary decimal
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Employee SET FirstName = @fname, LastName = @lname, MiddleInitial = @intial,
		[Address] = @address, City = @city, Province = @prov, PostalCode = @pc,
		Phone = @phone, CellPhone = @cell, Email = @email, DateOfBirth = @dob,
		[SIN] = @sin, [Status] = @status, HireDate = @hiredate, JobStartDate = @jobstartdate,
		JobId = @jobid, Salary = @salary, SalaryEffectiveDate = @salaryeffectivedate,
		PrevSalary = @prevsalary
	WHERE EmployeeId = @empId
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateEmployeeStatus]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joel Romero
-- Create date: May 15, 2012
-- Description:	Updates the employee status
-- =============================================
CREATE PROCEDURE [dbo].[UpdateEmployeeStatus]
	-- Add the parameters for the stored procedure here
	@empId int,
	@status int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Employee SET [Status] = @status
	WHERE EmployeeId = @empId
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateItem]    Script Date: 10/22/2020 1:14:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateItem] 
	-- Add the parameters for the stored procedure here
	@employeeId INT,
	@itemId INT,
	@orderId INT,
	@name VARCHAR(30),
	@description VARCHAR(50),
	@price DECIMAL,
	@quantity INT,
	@justification VARCHAR(100),
	@source VARCHAR(100)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    BEGIN TRY
	IF EXISTS(SELECT 1 FROM Item WHERE ItemId = @itemId)
	BEGIN
		DECLARE @isClosed BIT
		EXEC CheckIfOrderIsClosed @isClosed OUTPUT, @orderId
		
		IF @isClosed = 1
			RAISERROR('Unable to modify an item on a closed order.',15,1)
			
		DECLARE @orderEmpId INT
		SET @orderEmpId = (SELECT EmployeeId FROM [Order] WHERE [Order].OrderId = @orderId)
		
		IF @employeeId <> @orderEmpId
			RAISERROR('Unable to modify an item for an order you did not create.',15,1)
		ELSE
			UPDATE Item SET Name = @name,
							[Description] = @description,
							Price = @price,
							Quantity = @quantity,
							Justification = @justification,
							[Source] = @source
						WHERE ItemId = @itemId
	END
	ELSE
		RAISERROR('The item Id does not exist.',15,1)
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
	END CATCH	
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[18] 4[31] 2[35] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -192
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Employee"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 356
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1740
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ActiveEmployees'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ActiveEmployees'
GO
USE [master]
GO
ALTER DATABASE [Bismark] SET  READ_WRITE 
GO
