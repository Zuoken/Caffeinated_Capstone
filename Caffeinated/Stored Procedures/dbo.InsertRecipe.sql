-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Insert new recipe into the database
-- Cameron Winters, 000299896
-- November 15, 2015
CREATE PROCEDURE [dbo].InsertRecipe
	@username VARCHAR(20),
	@date DATETIME,
	@category VARCHAR(255),
	@title VARCHAR(255),
	@description TEXT,
	@directions TEXT,
	@privacy BIT
AS
	DECLARE @customer_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE [username] = @username;
		INSERT INTO Recipes (customer_id, recipe_date, category, title, recipe_description, directions, recipe_privacy) 
		VALUES (@customer_id, @date, @category, @title, @description, @directions, @privacy);
		SELECT SCOPE_IDENTITY() AS [Identity]; 
	END
GO