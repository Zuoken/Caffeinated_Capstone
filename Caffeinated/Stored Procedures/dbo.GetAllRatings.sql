-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Get ratings from the database for a specific recipe or blog
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].GetAllRatings
	@blog_id INT = NULL,
	@recipe_id INT = NULL
AS
	BEGIN
		IF (@blog_id IS NOT NULL)
		BEGIN
			SELECT *
			FROM Thumbs
			WHERE blog_id = @blog_id
		END

		ELSE
		BEGIN
			SELECT *
			FROM Thumbs
			WHERE recipe_id = @recipe_id
		END
	END
GO