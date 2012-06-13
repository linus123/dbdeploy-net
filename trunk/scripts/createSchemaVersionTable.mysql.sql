CREATE TABLE DatabaseVersion (
  ChangeNumber BIGINT NOT NULL,
  Project VARCHAR(10) NOT NULL,
  StartDate TIMESTAMP NOT NULL,
  CompletedDate TIMESTAMP NULL,
  AppliedBy VARCHAR(100) NOT NULL,
  FileName VARCHAR(500) NOT NULL
);

ALTER TABLE DatabaseVersion ADD CONSTRAINT Pkchangelog PRIMARY KEY (ChangeNumber, Project)
;