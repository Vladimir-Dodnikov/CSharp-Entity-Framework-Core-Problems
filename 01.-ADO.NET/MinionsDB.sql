USE MinionsDB

--Problem 2
  SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
    FROM Villains AS v 
    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
GROUP BY v.Id, v.Name 
  HAVING COUNT(mv.VillainId) > 3 
ORDER BY COUNT(mv.VillainId)

--Problem 3
SELECT Name FROM Villains WHERE Id = @Id

SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum, m.[Name], m.Age 
FROM MinionsVillains AS mv
JOIN Minions As m ON mv.MinionId = m.Id
WHERE mv.VillainId = @Id
ORDER BY m.Name

--Problem 4
SELECT Id FROM Villains WHERE Name = @Name

SELECT Id FROM Minions WHERE Name = @Name

INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)

INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)

INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)

INSERT INTO Towns (Name) VALUES (@townName)

SELECT Id FROM Towns WHERE Name = @townName

--Problem 6
SELECT Name FROM Villains WHERE Id = @villainId

DELETE FROM MinionsVillains WHERE VillainId = @villainId

DELETE FROM Villains WHERE Id = @villainId

--Problem 9
GO
CREATE PROC usp_GetOlder @id INT
AS
UPDATE Minions
   SET Age += 1
 WHERE Id = @id

SELECT Name, Age FROM Minions WHERE Id = @Id