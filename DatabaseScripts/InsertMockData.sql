USE BuildATrainDb
GO

-- Insert data into the Attributes table
INSERT INTO Attributes (Id, AttributeName, CarCapacity, FuelUse, FuelAdded, PurchasePrice, IncomeMinRange, IncomeMaxRange)
VALUES
  (1, 'Small', 200, 150, 50, 10000.00, 0, 0),
  (2, 'Medium', 150, 100, 30, 20000.00, 0, 0),
  (3, 'Large', 300, 200, 80, 30000.00, 0, 0),
  (4, 'CargoCar', 250, 180, 60, 5000.00, 0, 1000),
  (5, 'PassengerCar', 350, 250, 1500, 1800.00, 50, 200),
  (6, 'FuelCar', 0, 250, 100, 4000.00, 0, 0);
  
-- Insert data into the Locomotives table
INSERT INTO Locomotives (Id, AttributeId, LocomotiveSize)
VALUES
  (1, 1, 'Small'),
  (2, 2, 'Medium'),
  (3, 3, 'Large');
  
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
  (6, 3, 2, 'Blue Train', 3, 4, 1),
  (7, 4, 1, 'Yellow Train', 2, 5, 3),
  (8, 5, 2, 'Red Train', 1, 4, 2);
