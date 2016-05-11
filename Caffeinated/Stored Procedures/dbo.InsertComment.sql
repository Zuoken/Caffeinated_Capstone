-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Insert new comment into the database
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].InsertComment
	@username VARCHAR(20),
	@date DATETIME,
	@blog_id INT = NULL,
	@recipe_id INT = NULL,
	@comment TEXT
AS
	DECLARE @customer_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE [username] = @username;

		IF (@blog_id IS NOT NULL)
		BEGIN
			INSERT INTO Comments (comment_date, blog_id, customer_id, comment) 
			VALUES (@date, @blog_id, @customer_id, @comment);
		END

		IF (@recipe_id IS NOT NULL)
		BEGIN
			INSERT INTO Comments (comment_date, recipe_id, customer_id, comment) 
			VALUES (@date, @recipe_id, @customer_id, @comment);
		END 
	END
GO