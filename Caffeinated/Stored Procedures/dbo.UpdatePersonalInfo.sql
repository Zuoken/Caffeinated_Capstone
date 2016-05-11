-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Update the user's personal information in the database
-- This SP may be used to change any values
-- Cameron Winters, 000299896
-- October 13, 2015
CREATE PROCEDURE [dbo].UpdatePersonalInfo
	@username VARCHAR(20),
	@firstname VARCHAR(15) = null,
	@lastname VARCHAR(15) = null,
	@dob DATE = null,
	@location VARCHAR(255) = null,
	@email VARCHAR(255) = null,
	@fname_privacy BIT = null,
	@lname_privacy BIT = null,
	@birth_privacy BIT = null,
	@location_privacy BIT = null,
	@email_privacy BIT = null
AS
	DECLARE @customer_id AS INT
	DECLARE @country_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE username = @username;

		IF (@firstname IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET first_name = @firstname
			WHERE customer_id = @customer_id;
		END

		IF (@lastname IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET last_name = @lastname
			WHERE customer_id = @customer_id;
		END

		IF (@dob IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET birth_date = @dob
			WHERE customer_id = @customer_id;
		END

		IF (@location IS NOT NULL)
		BEGIN
			SELECT @country_id = [country_id] FROM Countries
			WHERE name = @location;

			UPDATE Customers
			SET country_id = @country_id
			WHERE customer_id = @customer_id;
		END

		IF (@email IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET email = @email
			WHERE customer_id = @customer_id;
		END

		IF (@fname_privacy IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET fname_private = @fname_privacy
			WHERE customer_id = @customer_id;
		END

		IF (@lname_privacy IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET lname_private = @lname_privacy
			WHERE customer_id = @customer_id;
		END

		IF (@birth_privacy IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET birth_private = @birth_privacy
			WHERE customer_id = @customer_id;
		END

		IF (@location_privacy IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET location_private = @location_privacy
			WHERE customer_id = @customer_id;
		END

		IF (@email_privacy IS NOT NULL)
		BEGIN
			UPDATE Customers
			SET email_private = @email_privacy
			WHERE customer_id = @customer_id;
		END
	END
GO