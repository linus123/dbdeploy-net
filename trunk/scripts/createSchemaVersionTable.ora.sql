CREATE TABLE DatabaseVersion (
  ChangeNumber INTEGER NOT NULL,
  Project VARCHAR2(10) NOT NULL,
  StartDate TIMESTAMP NOT NULL,
  CompletedDate TIMESTAMP NULL,
  AppliedBy VARCHAR2(100) NOT NULL,
  FileName VARCHAR2(500) NOT NULL
);

ALTER TABLE changelog ADD CONSTRAINT Pkchangelog PRIMARY KEY (ChangeNumber, Project)
;