USE BuildATrainDb
GO

-- Insert data into the Attributes table
INSERT INTO Attributes (Id, AttributeName, CarCapacity, FuelUse, FuelAdded, PurchasePrice)
VALUES
  (1, 'Power Express', 200, 150, 50, 1000.00),
  (2, 'Speed Master', 150, 100, 30, 800.00),
  (3, 'Cargo Hauler', 300, 200, 80, 1500.00),
  (4, 'Super Turbo', 250, 180, 60, 1200.00),
  (5, 'Mega Hauler', 350, 250, 100, 1800.00);
  
-- Insert data into the Locomotives table
INSERT INTO Locomotives (Id, AttributeId, LocomotiveSize)
VALUES
  (1, 1, 'Medium'),
  (2, 2, 'Small'),
  (3, 3, 'Large'),
  (4, 4, 'Large'),
  (5, 5, 'Medium');
  
-- Insert data into the Players table
INSERT INTO Players (Id, Username, Email, CurrentWallet)
VALUES
  (1, 'JohnDoe', 'johndoe@example.com', 5000.00),
  (2, 'JaneSmith', 'janesmith@example.com', 7500.00),
  (3, 'MikeWard', 'mikeward@example.com', 3000.00),
  (4, 'EmilyBrown', 'emilybrown@example.com', 6000.00),
  (5, 'DavidWilson', 'davidwilson@example.com', 4500.00);
  
-- Insert data into the PlayerTrains table
INSERT INTO PlayerTrains (TrainId, PlayerId, LocomotiveTypeId, LocomotiveName, NumFuelCars, NumPassengerCars, NumCargoCars)
VALUES
  (1, 1, 1, 'Orient Express', 2, 5, 3),
  (2, 2, 2, 'Indian Pacific',  3, 4, 2),
  (3, 3, 3, 'Glacier Express', 1, 6, 4),
  (4, 1, 2, 'Super Chief', 1, 3, 2),
  (5, 2, 3, 'Flying Scotsman', 2, 6, 4),
  (6, 3, 4, 'Blue Train', 3, 4, 1),
  (7, 4, 1, 'Yellow Train', 2, 5, 3),
  (8, 5, 5, 'Red Train', 1, 4, 2);
