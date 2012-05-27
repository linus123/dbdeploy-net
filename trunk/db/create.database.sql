USE [master]
GO
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'DbDeployTest')
DROP DATABASE [DbDeployTest]
GO

CREATE DATABASE [DbDeployTest] ON  PRIMARY 
( NAME = N'DbDeployTest', FILENAME = N'C:\working\dbdeploy-net\trunk\db\DbDeployTest.mdf' , SIZE = 10240KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DbDeployTest_log', FILENAME = N'C:\working\dbdeploy-net\trunk\db\DbDeployTest_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'DbDeployTest', @new_cmptlevel=90
GO
USE [DbDeployTest]
GO
IF NOT EXISTS (SELECT name FROM sys.filegroups WHERE is_default=1 AND name = N'PRIMARY') 
ALTER DATABASE [DbDeployTest] MODIFY FILEGROUP [PRIMARY] DEFAULT
GO

