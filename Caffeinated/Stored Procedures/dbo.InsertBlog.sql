-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Insert new blog post into the database
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].InsertBlog
	@username VARCHAR(20),
	@date DATETIME,
	@title VARCHAR(255),
	@content TEXT
AS
	DECLARE @customer_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE [username] = @username;
		INSERT INTO Blogs (customer_id, blog_date, title, content) 
		VALUES (@customer_id, @date, @title, @content); 
	END
GO