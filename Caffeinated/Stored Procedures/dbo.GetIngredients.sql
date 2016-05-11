-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Get ingredients from the database
-- Cameron Winters, 000299896
-- November 25, 2015
CREATE PROCEDURE [dbo].GetIngredients
	@recipe_id INT
AS
	BEGIN
		SELECT *
		FROM Ingredients
		JOIN Measurement m ON m.measurement_id = Ingredients.measurement_id
		WHERE recipe_id = @recipe_id
	END
GO