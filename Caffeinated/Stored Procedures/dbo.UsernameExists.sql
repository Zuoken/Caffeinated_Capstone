-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Check if the username exists
-- September 15, 2015
-- Cameron Winters
CREATE PROCEDURE [dbo].UsernameExists
	@username VARCHAR(20)
AS
	IF EXISTS (SELECT customer_id FROM Customers WHERE username = @username)
	BEGIN
		SELECT 1;
	END
	ELSE
	BEGIN
		SELECT 0;
	END
GO
