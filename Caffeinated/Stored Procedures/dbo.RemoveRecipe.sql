-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Delete recipe from the database
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].RemoveRecipe
	@recipe_id INT,
	@username VARCHAR(20)
AS
	DECLARE @username_check AS VARCHAR(20)
	BEGIN
		-- Check to make sure that the user requesting to delete the recipe is the one that owns the recipe
		SELECT @username_check = [username] FROM Customers c
		JOIN Recipes r ON c.customer_id = r.customer_id
		WHERE r.recipe_id = @recipe_id;

		IF (@username = @username_check)
		BEGIN
			-- Clear ingredients first to avoid FK conflicts
			DELETE FROM Ingredients
			WHERE recipe_id = @recipe_id;
			-- Remove ratings
			DELETE FROM Thumbs
			WHERE recipe_id = @recipe_id;
			-- Clear recipe
			DELETE FROM Recipes
			WHERE recipe_id = @recipe_id;
		END
	END
GO