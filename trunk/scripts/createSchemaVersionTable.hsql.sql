DROP TABLE DatabaseVersion;

CREATE TABLE DatabaseVersion (
  ChangeNumber INTEGER NOT NULL,
  Project VARCHAR(255) NOT NULL,
  StartDate TIMESTAMP NOT NULL,
  CompletedDate TIMESTAMP NULL,
  AppliedBy VARCHAR(100) NOT NULL,
  FileName VARCHAR(500) NOT NULL
);

ALTER TABLE DatabaseVersion ADD CONSTRAINT PkDatabaseVersion PRIMARY KEY (ChangeNumber, Project)
;