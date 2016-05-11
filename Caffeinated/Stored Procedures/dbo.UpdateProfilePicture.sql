-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Update the user's profile picture 
-- Cameron Winters, 000299896
-- December 10, 2015
CREATE PROCEDURE [dbo].UpdateProfilePicture
	@username VARCHAR(20),
	@picture VARCHAR(255)
AS
	DECLARE @customer_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE username = @username;

		UPDATE Customers
		SET profile_picture = @picture
		WHERE customer_id = @customer_id;
	END
GO