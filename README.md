#  Currency Conversion API (.NET 8)

A .NET 8 Web API project to manage currency exchange rates, perform conversions, and maintain conversion history. Data is stored in a SQL Server database using Dapper ORM.

---

##  Features

- Upsert and retrieve currency exchange rates
- Convert currencies and log the conversion history
- SQL Server database integration with Dapper
- Swagger UI with JWT Bearer Token Authentication using Azure AD
- Unit tests with xUnit and Moq

---

##  Technologies Used

- .NET 8
- ASP.NET Core Web API
- SQL Server
- Dapper
- xUnit & Moq
- Swagger/OpenAPI
- JWT Authentication

---

##  Local Setup

###  Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup) (optional)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or VS Code

---

##  Manual Database Setup

### 1. Create Database

Open SSMS or your SQL client and run:

```sql
CREATE DATABASE CURRENCYCONVERSION;

USE [CURRENCYCONVERSION];

CREATE TABLE [dbo].[CurrencyRates](
	  NOT NULL,
	[Rate] [decimal](18, 6) NULL,
	[RetrievedDate] [datetime] NULL,
	PRIMARY KEY CLUSTERED ([CurrencyCode])
);

CREATE TABLE [dbo].[ConversionHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	  NULL,
	[FromAmount] [decimal](18, 2) NULL,
	[ConvertedAmount] [decimal](18, 2) NULL,
	[ConversionDate] [datetime] NULL,
	PRIMARY KEY CLUSTERED ([Id])
);

###Stored Procedures

sp_upsertcurrencyrate
----------------------------------------------------

USE [CURRENCYCONVERSION]
GO
/****** Object:  StoredProcedure [dbo].[sp_UpsertCurrencyRate]    Script Date: 27-05-2025 17:24:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_UpsertCurrencyRate]
    @CurrencyCode VARCHAR(10),
    @Rate DECIMAL(18,6),
    @RetrievedDate DATETIME
AS
BEGIN
    IF EXISTS (SELECT 1 FROM CurrencyRates WHERE CurrencyCode = @CurrencyCode)
        UPDATE CurrencyRates SET Rate = @Rate, RetrievedDate = @RetrievedDate WHERE CurrencyCode = @CurrencyCode
    ELSE
        INSERT INTO CurrencyRates (CurrencyCode, Rate, RetrievedDate) VALUES (@CurrencyCode, @Rate, @RetrievedDate)
END


##InsertConversionHistory
-----------------------------------------------

USE [CURRENCYCONVERSION]
GO
/****** Object:  StoredProcedure [dbo].[InsertConversionHistory]    Script Date: 27-05-2025 17:24:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[InsertConversionHistory]
    @FromCurrency NVARCHAR(10),
    @FromAmount DECIMAL(18, 2),
    @ConvertedAmount DECIMAL(18, 2),
    @ConversionDate DATETIME
AS
BEGIN
    INSERT INTO ConversionHistory (FromCurrency, FromAmount, ConvertedAmount, ConversionDate)
    VALUES (@FromCurrency, @FromAmount, @ConvertedAmount, @ConversionDate)
END

##GetRateByCurrencyCode
----------------------------------------------

USE [CURRENCYCONVERSION]
GO
/****** Object:  StoredProcedure [dbo].[GetRateByCurrencyCode]    Script Date: 27-05-2025 17:24:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetRateByCurrencyCode]
    @CurrencyCode NVARCHAR(10)
AS
BEGIN
    SELECT * FROM CurrencyRates WHERE CurrencyCode = @CurrencyCode
END





 ##GetAllCurrencyRates
 ------------------------------------------------

USE [CURRENCYCONVERSION]
GO
/****** Object:  StoredProcedure [dbo].[GetAllCurrencyRates]    Script Date: 27-05-2025 17:24:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetAllCurrencyRates]
AS
BEGIN
    SELECT * FROM CurrencyRates
END
