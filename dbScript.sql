USE [test]
GO
/****** Object:  Table [dbo].[Product2]    Script Date: 03/13/2015 09:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product2](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Unit] [int] SPARSE  NULL,
	[Name] [varchar](20) SPARSE  NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Product2] ON
INSERT [dbo].[Product2] ([id], [Unit], [Name]) VALUES (1, 10, N'BMW')
INSERT [dbo].[Product2] ([id], [Unit], [Name]) VALUES (2, 20, N'Farari')
INSERT [dbo].[Product2] ([id], [Unit], [Name]) VALUES (3, 10, N'TATA')
INSERT [dbo].[Product2] ([id], [Unit], [Name]) VALUES (4, 20, N'Maruti')
SET IDENTITY_INSERT [dbo].[Product2] OFF
/****** Object:  Table [dbo].[Product]    Script Date: 03/13/2015 09:11:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](50) NULL,
	[UnitPrice] [int] NULL,
	[UnitsInStock] [int] NULL,
	[Discontinued] [bit] NULL,
	[Category] [varchar](50) NULL,
	[CreatedDateTime] [datetime] NULL,
	[UniqueCode] [varchar](50) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (3, N'Levis Jeans', 10, 10, 0, N'Men', CAST(0x0000A40100476279 AS DateTime), NULL)
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (6, N'Shirt', 3, 2, 1, N'Casual', CAST(0x0000A37200DB9C4C AS DateTime), NULL)
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (7, N'T-Shirt', 2, 5, 0, N'Casual', CAST(0x0000A37200DBA380 AS DateTime), NULL)
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (16, N'jersy', 2, 3, 1, N'Casual', CAST(0x0000A36F00D2C585 AS DateTime), N'16-jer')
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (17, N'Shorts', 1, 2, 1, N'Men', CAST(0x0000A36F00D864BF AS DateTime), N'17-Sho')
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (18, N'Winter Cap', 1, 2, 1, N'Casual', CAST(0x0000A36F00D89F29 AS DateTime), N'18-Win')
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (19, N'Summer Hat', 2, 2, 0, N'Casual', CAST(0x0000A36F00D9FE80 AS DateTime), N'19-Sum')
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (20, N'Shirt11', 10, 10, 1, N'Men', CAST(0x0000A37200DBEE01 AS DateTime), N'20-Shi')
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (33, N'Shirts-XL', 2, 1, 0, N'Men', CAST(0x0000A37200C4F61E AS DateTime), N'33-Shi')
INSERT [dbo].[Product] ([ProductID], [ProductName], [UnitPrice], [UnitsInStock], [Discontinued], [Category], [CreatedDateTime], [UniqueCode]) VALUES (34, N'Shirt-XXL', 1, 2, 0, N'Men', CAST(0x0000A37200C49C5C AS DateTime), N'34-Shi')
SET IDENTITY_INSERT [dbo].[Product] OFF
/****** Object:  StoredProcedure [dbo].[SelectByIdList]    Script Date: 03/13/2015 09:11:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectByIdList](@productIds xml) AS

DECLARE @Products TABLE (ID int) 
begin
INSERT INTO @Products (ID) SELECT ParamValues.ID.value('.','VARCHAR(20)')
FROM @productIds.nodes('/Products/id') as ParamValues(ID) 

SELECT * FROM 
    @Products
    
    end
GO
/****** Object:  StoredProcedure [dbo].[spUpdateInsertProduct]    Script Date: 03/13/2015 09:11:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateInsertProduct]
	@ID int,
	@ProductName varchar(50),
	@UnitPrice int,
	@UnitsInStock int,
	@Discontinued bit,
	@Category varchar(50),
	@RetID int output
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM Product WHERE ProductID=@ID)
	BEGIN
		UPDATE Product
		SET ProductName=@ProductName ,UnitPrice=@UnitPrice,UnitsInStock=@UnitsInStock,Discontinued=@Discontinued,Category=@Category,CreatedDateTime=GETDATE()
		WHERE ProductID=@ID

		SET @RetID=@ID
		RETURN @RetID
	END

	INSERT INTO Product (ProductName, UnitPrice, UnitsInStock , Discontinued,Category,CreatedDateTime)
	VALUES(@ProductName, @UnitPrice, @UnitsInStock , @Discontinued,@Category,GETDATE())
	
	IF @@ERROR<>0
	BEGIN
		SET @RetID=-1
	END

	SET @RetID=SCOPE_IDENTITY()

	if SCOPE_IDENTITY()>0
	BEGIN
		UPDATE Product
		SET UniqueCode=cast(SCOPE_IDENTITY() as varchar)+'-'+Left(@ProductName,3) WHERE ProductID=SCOPE_IDENTITY()
	END
	--CONVERT(varchar(255), NEWID()) --this will generate random sting in 2008

	RETURN @RetID
END
GO
/****** Object:  StoredProcedure [dbo].[InsertXMLDataIntoProduct2]    Script Date: 03/13/2015 09:11:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertXMLDataIntoProduct2](@Input XML)
AS
BEGIN
    INSERT INTO Product2(Unit, Name)
        SELECT
            XCol.value('(Unit)[1]', 'int'),
            XCol.value('(Name)[1]', 'VARCHAR(20)')
        FROM 
            @input.nodes('/Products/items') AS XTbl(XCol)
            
    --SELECT * FROM  Product2
END
GO
