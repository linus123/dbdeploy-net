USE dbdeploy
GO

CREATE TABLE DatabaseVersion (
  ChangeNumber INTEGER NOT NULL,
  Project VARCHAR(10) NOT NULL,
  StartDate DATETIME NOT NULL,
  CompletedDate DATETIME NULL,
  AppliedBy VARCHAR(100) NOT NULL,
  FileName VARCHAR(500) NOT NULL
)
GO

ALTER TABLE DatabaseVersion ADD CONSTRAINT Pkchangelog PRIMARY KEY (ChangeNumber, Project)
GO