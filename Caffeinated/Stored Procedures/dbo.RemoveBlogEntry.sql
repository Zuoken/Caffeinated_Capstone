-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Delete blog entry from the database
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].RemoveBlogEntry
	@blog_id INT
AS
	BEGIN
		DELETE FROM Blogs
		WHERE blog_id = @blog_id;
	END
GO