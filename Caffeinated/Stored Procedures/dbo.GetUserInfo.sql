-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Cameron Winters, 000299896
-- Get user information
-- November 15, 2015
CREATE PROCEDURE [dbo].GetUserInfo
	@username VARCHAR(20)
AS
	BEGIN
		SELECT * FROM Customers
		JOIN Countries ON Countries.country_id = Customers.country_id
		WHERE username = @username;	
	END
GO