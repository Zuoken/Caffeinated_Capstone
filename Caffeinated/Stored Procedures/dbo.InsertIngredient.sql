-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Insert new ingredient into the database
-- Cameron Winters, 000299896
-- November 15, 2015
CREATE PROCEDURE [dbo].InsertIngredient
	@recipe_id INT,
	@measurement_id INT,
	@name VARCHAR(255),
	@amount INT
AS
	BEGIN
		INSERT INTO Ingredients (recipe_id, measurement_id, name, amount) 
		VALUES (@recipe_id, @measurement_id, @name, @amount);
	END
GO