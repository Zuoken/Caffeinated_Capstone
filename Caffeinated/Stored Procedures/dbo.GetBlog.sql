-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Get blog info from the database
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].GetBlog
	@blog_id INT
AS
	BEGIN
		SELECT username, b.customer_id, blog_date, title, content FROM Blogs b
		JOIN Customers c ON b.customer_id = c.customer_id
		WHERE blog_id = @blog_id;
	END
GO