CREATE DATABASE ECM

GO

USE [ECM]
GO
/****** Object:  Table [dbo].[BankEmployee]    Script Date: 22/8/2021 3:54:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankEmployee](
	[BankEmpID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[BankAccount] [varchar](50) NOT NULL,
	[BankBranch] [varchar](50) NOT NULL,
	[BankAccName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_BankEmployee] PRIMARY KEY CLUSTERED 
(
	[BankEmpID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 22/8/2021 3:54:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeID] [int] NOT NULL,
	[FirstName] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NULL,
	[City] [varchar](255) NULL,
 CONSTRAINT [PK_Employee_1] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpenseDetails]    Script Date: 22/8/2021 3:54:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseDetails](
	[DetailID] [int] IDENTITY(1,1) NOT NULL,
	[ExpenseID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[CostCenter] [int] NOT NULL,
	[GLCode] [int] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[Currency] [varchar](3) NULL,
	[ExhangeRate] [decimal](18, 6) NULL,
	[Gst] [decimal](18, 2) NULL,
	[ClaimCurrAmount] [decimal](18, 2) NULL,
	[ClaimAmount] [decimal](18, 2) NOT NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_ExpenseDetails] PRIMARY KEY CLUSTERED 
(
	[DetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpenseMaster]    Script Date: 22/8/2021 3:54:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseMaster](
	[ExpenseID] [int] IDENTITY(1,1) NOT NULL,
	[RequesterID] [int] NOT NULL,
	[ApproverID] [int] NULL,
	[ApprovedDate] [datetime] NULL,
	[Reference] [nvarchar](150) NOT NULL,
	[Date] [datetime] NOT NULL,
	[BranchCode] [varchar](50) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[BankAccount] [varchar](50) NULL,
	[BankBranch] [varchar](50) NULL,
	[BankAccName] [varchar](50) NULL,
 CONSTRAINT [PK_ExpenseMaster] PRIMARY KEY CLUSTERED 
(
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[uvwExpenseDetails]    Script Date: 22/8/2021 3:54:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[uvwExpenseDetails]
AS

	SELECT [DetailID]
		  ,[ExpenseID]
		  ,CONVERT(VARCHAR, [Date], 103) Date
		  ,[CostCenter]
		  ,[GLCode]
		  ,[Description]
		  ,[Currency]
		  ,[ExhangeRate]
		  ,[Gst]
		  ,[ClaimCurrAmount]
		  ,[ClaimAmount]
		  ,[Status]
		  ,CASE WHEN CostCenter = 1 THEN 'IT'
			WHEN CostCenter = 2 THEN 'Finance'
			WHEN CostCenter = 3 THEN 'HR'
			WHEN CostCenter = 4 THEN 'Sales' 
			END CostCenterDesc
		  ,CASE WHEN GLCode = 4165 THEN 'Staff Reimbursement Mobile'
            WHEN GLCode = 4190 THEN 'Postage'
            WHEN GLCode = 4191 THEN 'Couriers'
            WHEN GLCode = 4200 THEN 'Stationary'
            WHEN GLCode = 4311 THEN 'International Fares'
            WHEN GLCode = 4312 THEN 'International Accomondation'
            WHEN GLCode = 4313 THEN 'International Expenses'
            WHEN GLCode = 4321 THEN 'Local Fares'
			END GLCodeDesc
		  ,CASE WHEN Status = 1 THEN 'Pending Approval' WHEN Status = 2 THEN 'Approved' ELSE 'Rejected' END StatusDesc
	  FROM [dbo].[ExpenseDetails]
GO
/****** Object:  View [dbo].[uvwExpenseMaster]    Script Date: 22/8/2021 3:54:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[uvwExpenseMaster]
AS
	SELECT [ExpenseID]
		  ,[RequesterID]
		  ,[ApproverID]
		  ,CONVERT(VARCHAR, [ApprovedDate], 103) ApprovedDate
		  ,[Reference]
		  ,CONVERT(VARCHAR, [Date], 103) Date
		  ,[BranchCode]
		  ,[Amount]
		  ,[Status]
		  ,[BankAccount]
		  ,[BankBranch]
		  ,[BankAccName]
		  ,E.[FirstName] + ' ' + E.[LastName] AS RequesterName
		  ,A.[FirstName] + ' ' + A.[LastName] AS ApproverName
		  ,CASE WHEN Status = 1 THEN 'Pending Approval' WHEN Status = 2 THEN 'Approved' ELSE 'Rejected' END StatusDesc
	  FROM [dbo].[ExpenseMaster] M 
	  LEFT JOIN [dbo].[Employee] E ON E.EmployeeID = M.RequesterID
	  LEFT JOIN [dbo].[Employee] A ON A.EmployeeID = M.ApproverID
GO
SET IDENTITY_INSERT [dbo].[BankEmployee] ON 
GO
INSERT [dbo].[BankEmployee] ([BankEmpID], [EmployeeID], [BankAccount], [BankBranch], [BankAccName]) VALUES (1, 1234, N'87654321', N'0012', N'Jeff Johnson')
GO
INSERT [dbo].[BankEmployee] ([BankEmpID], [EmployeeID], [BankAccount], [BankBranch], [BankAccName]) VALUES (2, 1236, N'76753452', N'0103', N'Susan Connor')
GO
SET IDENTITY_INSERT [dbo].[BankEmployee] OFF
GO
INSERT [dbo].[Employee] ([EmployeeID], [FirstName], [LastName], [City]) VALUES (1234, N'Jeff', N'Johnson', NULL)
GO
INSERT [dbo].[Employee] ([EmployeeID], [FirstName], [LastName], [City]) VALUES (1235, N'Melvin', N'Forbis', NULL)
GO
INSERT [dbo].[Employee] ([EmployeeID], [FirstName], [LastName], [City]) VALUES (1236, N'Susan', N'Connor', NULL)
GO
INSERT [dbo].[Employee] ([EmployeeID], [FirstName], [LastName], [City]) VALUES (1237, N'Margaret', N'Adelman', NULL)
GO
SET IDENTITY_INSERT [dbo].[ExpenseDetails] ON 
GO
INSERT [dbo].[ExpenseDetails] ([DetailID], [ExpenseID], [Date], [CostCenter], [GLCode], [Description], [Currency], [ExhangeRate], [Gst], [ClaimCurrAmount], [ClaimAmount], [Status]) VALUES (2, 2, CAST(N'2021-08-01T00:00:00.000' AS DateTime), 1, 4191, N'Taxi fare', N'SGD', CAST(3.012300 AS Decimal(18, 6)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(30.12 AS Decimal(18, 2)), 1)
GO
SET IDENTITY_INSERT [dbo].[ExpenseDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[ExpenseMaster] ON 
GO
INSERT [dbo].[ExpenseMaster] ([ExpenseID], [RequesterID], [ApproverID], [ApprovedDate], [Reference], [Date], [BranchCode], [Amount], [Status], [BankAccount], [BankBranch], [BankAccName]) VALUES (2, 0, NULL, NULL, N'Outstation Jan', CAST(N'2021-08-22T04:48:51.260' AS DateTime), N'123', CAST(30.12 AS Decimal(18, 2)), 1, N'87654321', N'0012', N'Jeff Johnson')
GO
SET IDENTITY_INSERT [dbo].[ExpenseMaster] OFF
GO
ALTER TABLE [dbo].[BankEmployee]  WITH CHECK ADD  CONSTRAINT [FK_BankEmployee_Employee] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[BankEmployee] CHECK CONSTRAINT [FK_BankEmployee_Employee]
GO
ALTER TABLE [dbo].[ExpenseDetails]  WITH CHECK ADD  CONSTRAINT [FK_ExpenseDetails_ExpenseMaster] FOREIGN KEY([ExpenseID])
REFERENCES [dbo].[ExpenseMaster] ([ExpenseID])
GO
ALTER TABLE [dbo].[ExpenseDetails] CHECK CONSTRAINT [FK_ExpenseDetails_ExpenseMaster]
GO
