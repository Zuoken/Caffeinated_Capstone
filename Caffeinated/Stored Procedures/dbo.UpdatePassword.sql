-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Based on code presented in Brian Minaji's Web Application ASP.NET course at Mohawk College,
-- Ontario, Canada.  

-- Update the user's password
-- Cameron Winters, 000299896
-- December 10, 2015
CREATE PROCEDURE [dbo].UpdatePassword
	@username VARCHAR(20),
	@password VARCHAR(128)
AS
	DECLARE @customer_id AS INT
	DECLARE @salt VARBINARY(64)
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE username = @username;

		SET @salt = CRYPT_GEN_RANDOM(64);

		UPDATE Customers
		SET user_password = HASHBYTES('SHA2_512', CAST(@salt AS VARCHAR) + @password), user_salt = @salt
		WHERE customer_id = @customer_id;
	END
GO