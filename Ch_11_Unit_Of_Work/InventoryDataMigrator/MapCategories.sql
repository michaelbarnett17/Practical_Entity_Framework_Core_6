/**
NOTE: The final version of activity 9-3 requires that this script has been run on the local database
**/

DECLARE @MoviesID INT
DECLARE @BooksID INT
DECLARE @GamesID INT
SET @MoviesID = (SELECT Id FROM Categories WHERE Name = 'Movies')
SET @BooksID = (SELECT Id FROM Categories WHERE Name = 'Books')
SET @GamesID = (SELECT Id FROM Categories WHERE Name = 'Games')

UPDATE ITEMS SET CategoryId = @MoviesID WHERE Name = 'Batman Begins'
UPDATE ITEMS SET CategoryId = @GamesID WHERE Name = 'World of Tanks'
UPDATE ITEMS SET CategoryId = @BooksID WHERE Name = 'The Sword of Shannara'
UPDATE ITEMS SET CategoryId = @BooksID WHERE Name = 'Practical Entity Framework'
UPDATE ITEMS SET CategoryId = @GamesID WHERE Name = 'Battlefield 2142'
UPDATE ITEMS SET CategoryId = @MoviesID WHERE Name = 'Star Wars: The Empire Strikes Back'
UPDATE ITEMS SET CategoryId = @MoviesID WHERE Name = 'Top Gun'
UPDATE ITEMS SET CategoryId = @MoviesID WHERE Name = 'Remember the Titans'
UPDATE ITEMS SET CategoryId = @MoviesID WHERE Name = 'Inception'

SELECT i.*, c.Name
FROM Items i
LEFT JOIN Categories c on i.CategoryId = c.Id
