-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Get recipe from the database
-- Cameron Winters, 000299896
-- November 25, 2015
CREATE PROCEDURE [dbo].GetRecipe
	@recipe_id INT
AS
	BEGIN
		SELECT r.recipe_date, r.category, r.title, r.recipe_description, r.directions, r.recipe_image, r.recipe_privacy, i.name, i.amount, m.short_name, m.long_name, c.username
		FROM Recipes r
		JOIN Ingredients i ON r.recipe_id = i.recipe_id
		JOIN Measurement m ON i.measurement_id = m.measurement_id
		JOIN Customers c ON r.customer_id = c.customer_id
		WHERE r.recipe_id = @recipe_id;
	END
GO