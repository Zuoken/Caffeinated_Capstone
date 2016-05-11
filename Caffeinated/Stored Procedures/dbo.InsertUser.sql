-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Insert new user into the database
-- User will, by default, have their personal information made public.  This is changeable from
-- the user profile page.
-- Cameron Winters, 000299896
-- September 15, 2015
CREATE PROCEDURE [dbo].InsertUser
	@username VARCHAR(20),
	@firstname VARCHAR(15),
	@lastname VARCHAR(15),
	@email VARCHAR(255),
	@birth_date DATE,
	@sec_question TEXT,
	@sec_answer TEXT,
	@password VARCHAR(128),
	@country VARCHAR(255)
AS
	IF EXISTS (SELECT customer_id FROM Customers WHERE username = @username) 
		SELECT 0 AS Result; -- User already exists, cancel
	ELSE
		DECLARE @country_id AS INT
		DECLARE @salt VARBINARY(64)
		BEGIN
			SET @salt = CRYPT_GEN_RANDOM(64);
			SELECT @country_id = [country_id] FROM Countries
			WHERE [name] = @country;
			INSERT INTO Customers (username, first_name, last_name, email, birth_date, sec_question, sec_answer, user_password, user_salt, country_id, fname_private, lname_private, birth_private, location_private, email_private) 
			VALUES (@username, @firstname, @lastname, @email, @birth_date, @sec_question, @sec_answer, HASHBYTES('SHA2_512', CAST(@salt AS VARCHAR) + @password), @salt, @country_id, '1', '1', '1', '1', '1');
		END
		SELECT 1 AS Result; -- New user inserted into db
GO