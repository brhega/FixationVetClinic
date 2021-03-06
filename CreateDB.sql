-- Owner definition

-- Drop table

-- DROP TABLE Owner;

CREATE TABLE Owner (
	ID int IDENTITY(1,1) NOT NULL,
	Name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	phoneNumber varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__Owner__3214EC27B6C0D3D6 PRIMARY KEY (ID)
);


-- Pet definition

-- Drop table

-- DROP TABLE Pet;

CREATE TABLE Pet (
	ID int IDENTITY(1,1) NOT NULL,
	OwnerID int NOT NULL,
	Name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK__Pet__3214EC27263E31D9 PRIMARY KEY (ID),
	CONSTRAINT FK__Pet__OwnerID__184C96B4 FOREIGN KEY (OwnerID) REFERENCES Owner(ID)
);


-- Appointment definition

-- Drop table

-- DROP TABLE Appointment;

CREATE TABLE Appointment (
	ID int IDENTITY(1,1) NOT NULL,
	PetID int NOT NULL,
	OwnerID int NOT NULL,
	ApptTime datetime NULL,
	reason text COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__Appointm__3214EC27A9BB4C00 PRIMARY KEY (ID),
	CONSTRAINT FK__Appointme__Owner__16644E42 FOREIGN KEY (OwnerID) REFERENCES Owner(ID),
	CONSTRAINT FK__Appointme__PetID__1758727B FOREIGN KEY (PetID) REFERENCES Pet(ID)
);
