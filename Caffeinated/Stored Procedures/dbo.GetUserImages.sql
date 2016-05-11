-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Get user images
-- Cameron Winters, 000299896
-- December 10, 2015
CREATE PROCEDURE [dbo].GetUserImages
	@username VARCHAR(20)
AS
	BEGIN
		SELECT DISTINCT name FROM Images
		JOIN Customers ON Images.customer_id = Customers.customer_id
		WHERE username = @username;
	END
GO