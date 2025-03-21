
--------------------------------------------------------------------- 
-- Purpose : Create tables and database for Conway Game of Life 
-- Author  : Sneha Sanghavi
-- Date    : 3/19/2025 - Initial version
--------------------------------------------------------------------- 

CREATE DATABASE GameOfLifeDB;
GO

CREATE LOGIN gameoflifesvc WITH PASSWORD = 'AddYourPassword';
GO
CREATE USER gameoflifesvc FOR LOGIN gameoflifesvc;
GO
GRANT SELECT, INSERT, UPDATE, DELETE ON Boards TO gameoflifesvc;
GRANT SELECT, INSERT, UPDATE, DELETE ON Cells TO gameoflifesvc;
GO


USE GameOfLifeDB;
GO


CREATE TABLE Boards (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),  
    Name NVARCHAR(255) NOT NULL,                      
    CreatedAt DATETIME DEFAULT SYSUTCDATETIME(),            
    LastModifiedTime DATETIME DEFAULT SYSUTCDATETIME()       
);
GO


CREATE TABLE Cells (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), 
    BoardId UNIQUEIDENTIFIER NOT NULL,                
    RowPosition INT NOT NULL,                           
    ColumnPosition INT NOT NULL,                        
    IsAlive BIT NOT NULL,                             
    CreatedAt DATETIME DEFAULT SYSUTCDATETIME(),            
    LastModifiedTime DATETIME DEFAULT SYSUTCDATETIME(),     
    FOREIGN KEY (BoardId) REFERENCES Boards(Id) ON DELETE CASCADE  
);
GO
