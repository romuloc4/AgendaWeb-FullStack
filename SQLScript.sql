﻿CREATE TABLE USUARIO(
	ID					UNIQUEIDENTIFIER		NOT NULL,
	NOME				NVARCHAR(150)			NOT NULL,
	EMAIL				NVARCHAR(150)			NOT NULL UNIQUE,
	SENHA				NVARCHAR(50)			NOT NULL,
	DATAINCLUSAO		DATETIME				NOT NULL,
	PRIMARY KEY(ID))
GO

ALTER TABLE EVENTO ADD
IDUSUARIO	UNIQUEIDENTIFIER
GO

ALTER TABLE EVENTO ADD CONSTRAINT FK_USUARIO
FOREIGN KEY(IDUSUARIO) REFERENCES USUARIO(ID)
GO
