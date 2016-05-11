-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Based on code presented in Brian Minaji's Web Application ASP.NET course at Mohawk College,
-- Ontario, Canada.  

-- Authenticate the User
-- Cameron Winters, 000299896
-- September 15, 2015
CREATE PROCEDURE [dbo].AuthenticateUser
	@username VARCHAR(20),
	@password VARCHAR(128)
AS
	DECLARE @salt VARBINARY(64)
	DECLARE @hash VARBINARY(128)
BEGIN
	SELECT @salt = user_salt FROM Customers WHERE username = @username;
	SELECT @hash = user_password FROM Customers WHERE username = @username;
	IF HASHBYTES('SHA2_512', CAST(@salt AS VARCHAR) + @password) = @hash
		SELECT 1 AS Result; -- User authenticated
	ELSE
		SELECT 0 AS Result; -- User not authenticated
END