-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Insert new rating into the database
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].InsertRating
	@username VARCHAR(20),
	@date DATETIME,
	@blog_id INT = NULL,
	@recipe_id INT = NULL,
	@thumbs BIT
AS
	DECLARE @customer_id AS INT
	DECLARE @insert AS BIT = 0
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE [username] = @username;

		-- Check to see if a rating already exists for either blog_id or rating_id
		IF (@blog_id IS NOT NULL)
		BEGIN
			IF NOT EXISTS (SELECT * FROM Thumbs WHERE blog_id = @blog_id AND customer_id = @customer_id)
			BEGIN
				SET @insert = 1
			END
		END

		IF (@recipe_id IS NOT NULL)
		BEGIN
			IF NOT EXISTS (SELECT * FROM Thumbs WHERE recipe_id = @recipe_id AND customer_id = @customer_id)
			BEGIN
				SET @insert = 1
			END
		END

		-- If no rating yet exists insert the new rating
		IF (@insert = 1)
		BEGIN
			INSERT INTO Thumbs (thumbs_date, blog_id, recipe_id, customer_id, thumbs_up) 
			VALUES (@date, @blog_id, @recipe_id, @customer_id, @thumbs);
		END

		-- If rating already exists update it
		ELSE
		BEGIN
			IF (@blog_id IS NOT NULL)
			BEGIN
				UPDATE Thumbs 
				SET thumbs_date = @date, blog_id = @blog_id, recipe_id = @recipe_id, thumbs_up = @thumbs
				WHERE blog_id = @blog_id
				AND customer_id = @customer_id
			END
			ELSE
			BEGIN
				UPDATE Thumbs 
				SET thumbs_date = @date, blog_id = @blog_id, recipe_id = @recipe_id, thumbs_up = @thumbs
				WHERE recipe_id = @recipe_id
				AND customer_id = @customer_id
			END
		END
	END
GO
