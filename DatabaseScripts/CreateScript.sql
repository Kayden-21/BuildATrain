USE master;
GO

DROP DATABASE BuildATrainDb
GO

CREATE DATABASE BuildATrainDb
GO

USE BuildATrainDb
GO

CREATE TABLE Attributes (
  Id INT PRIMARY KEY,
  AttributeName VARCHAR(255),
  CarCapacity INT,
  FuelUse INT,
  FuelAdded INT,
  PurchasePrice DECIMAL(18,2)
);
GO

CREATE TABLE Locomotives (
  Id INT PRIMARY KEY,
  AttributeId INT,
  LocomotiveSize VARCHAR(255),
  FOREIGN KEY (AttributeId) REFERENCES Attributes(Id)
);
GO

CREATE TABLE Players (
  Id INT PRIMARY KEY,
  Username VARCHAR(50),
  Email NVARCHAR(255),
  CurrentWallet DECIMAL(18,2)
);
GO

CREATE TABLE PlayerTrains (
  TrainId INT PRIMARY KEY,
  PlayerId INT,
  LocomotiveTypeId INT,
  LocomotiveName VARCHAR(50),
  NumFuelCars INT,
  NumPassengerCars INT,
  NumCargoCars INT,
  FOREIGN KEY (PlayerId) REFERENCES Players(Id),
  FOREIGN KEY (LocomotiveTypeId) REFERENCES Locomotives(Id)
);
GO


/*
Stored Procs
*/

CREATE PROCEDURE InsertPlayerTrain
    @LocomotiveSize VARCHAR(255),
	@LocomotiveName VARCHAR(50),
    @NumFuelCars INT,
    @NumPassengerCars INT,
    @NumCargoCars INT,
    @Username VARCHAR(50)
AS
BEGIN
    DECLARE @LocomotiveId INT
    DECLARE @PlayerId INT

    INSERT INTO Locomotives (AttributeId, LocomotiveSize)
    VALUES (2, @LocomotiveSize);

    SET @LocomotiveId = SCOPE_IDENTITY();

    SELECT @PlayerId = Id FROM Players WHERE Username = @Username;

    INSERT INTO PlayerTrains (TrainId, PlayerId, LocomotiveTypeId, LocomotiveName, NumFuelCars, NumPassengerCars, NumCargoCars)
    VALUES (@LocomotiveId, @PlayerId, @LocomotiveId, @LocomotiveName, @NumFuelCars, @NumPassengerCars, @NumCargoCars);
END;
GO

/* GetPlayerTrainsByEmail takes user email, and returns the player's train setup */
CREATE PROCEDURE GetPlayerTrainsByEmail
  @Email NVARCHAR(255)
AS
BEGIN
  SET NOCOUNT ON;

  SELECT pt.TrainId, pt.NumCargoCars, pt.NumFuelCars, pt.NumPassengerCars, l.LocomotiveSize
  FROM PlayerTrains pt
  JOIN Players p ON p.Id = pt.PlayerId
  JOIN Locomotives l ON l.Id = pt.LocomotiveTypeId
  WHERE p.Email = @Email;
END;
GO

CREATE PROCEDURE GetPlayerTrainsByUsername
  @Username NVARCHAR(50)
AS
BEGIN
  SET NOCOUNT ON;

  SELECT pt.TrainId, pt.NumCargoCars, pt.NumFuelCars, pt.NumPassengerCars, l.LocomotiveSize
  FROM PlayerTrains pt
  JOIN Players p ON p.Id = pt.PlayerId
  JOIN Locomotives l ON l.Id = pt.LocomotiveTypeId
  WHERE p.Username = @Username;
END;
GO

CREATE PROCEDURE GetAndRemovePlayerTrains
  @Username NVARCHAR(50),
  @LocomotiveName VARCHAR(50)
AS
BEGIN
  SET NOCOUNT ON;

  CREATE TABLE #TempPlayerTrainsToRemove (PlayerTrainId INT PRIMARY KEY);

  INSERT INTO #TempPlayerTrainsToRemove (PlayerTrainId)
  SELECT pt.TrainId
  FROM PlayerTrains pt
  JOIN Players p ON p.Id = pt.PlayerId
  WHERE p.Username = @Username AND pt.LocomotiveName = @LocomotiveName;

  DELETE pt
  FROM PlayerTrains pt
  WHERE pt.TrainId IN (SELECT PlayerTrainId FROM #TempPlayerTrainsToRemove);

  SELECT pt.TrainId, pt.NumCargoCars, pt.NumFuelCars, pt.NumPassengerCars, pt.LocomotiveName
  FROM PlayerTrains pt
  JOIN Players p ON p.Id = pt.PlayerId
  WHERE p.Username = @Username;

  DROP TABLE #TempPlayerTrainsToRemove;
END;
GO

/* GetPlayerDetailsByEmail takes user email, and returns the player's username and their current wallet */
CREATE PROCEDURE GetPlayerDetailsByEmail
  @Email NVARCHAR(255)
AS
BEGIN
  SET NOCOUNT ON;

  SELECT Username, CurrentWallet
  FROM Players
  WHERE Email = @Email;
END;
GO

/* GetAttributesById takes attribute id, and returns the details of the attribute */
CREATE PROCEDURE GetAttributesById
  @AttributeId INT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT AttributeName, CarCapacity, FuelUse, FuelAdded, PurchasePrice
  FROM Attributes
  WHERE Id = @AttributeId;
END;
GO

/* PerformPurchaseByAttributeId takes playerId and attributeId, and returns the new wallet, a success bit and a message */
CREATE PROCEDURE PerformPurchaseByAttributeId
  @PlayerId INT,
  @AttributeId INT,
  @CurrentWallet DECIMAL(18,2) OUTPUT,
  @Success BIT OUTPUT,
  @Message NVARCHAR(255) OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  SET @Success = 0;

  BEGIN TRANSACTION;

  BEGIN TRY
    DECLARE @PurchasePrice DECIMAL(18,2);
    SELECT @PurchasePrice = PurchasePrice
    FROM Attributes
    WHERE Id = @AttributeId;

    SELECT @CurrentWallet = CurrentWallet
    FROM Players
    WHERE Id = @PlayerId;

    IF @CurrentWallet >= @PurchasePrice
    BEGIN
      UPDATE Players
      SET CurrentWallet = CurrentWallet - @PurchasePrice
      WHERE Id = @PlayerId;

      SET @CurrentWallet = @CurrentWallet - @PurchasePrice;
      SET @Success = 1;
      SET @Message = 'Purchase successful.';
    END
    ELSE
    BEGIN
      SET @Message = 'Insufficient funds.';
      ROLLBACK TRANSACTION;
    END

    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    SET @Message = ERROR_MESSAGE();
    ROLLBACK TRANSACTION;
  END CATCH;
END;
GO