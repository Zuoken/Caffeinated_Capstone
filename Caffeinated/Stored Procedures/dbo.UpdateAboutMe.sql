-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Update the "About Me" section for the user
-- Cameron Winters, 000299896
-- October 13, 2015
CREATE PROCEDURE [dbo].UpdateAboutMe
	@username VARCHAR(20),
	@aboutme TEXT
AS
	DECLARE @customer_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE username = @username;

		UPDATE Customers
		SET about_me = @aboutme
		WHERE customer_id = @customer_id;
	END
GO