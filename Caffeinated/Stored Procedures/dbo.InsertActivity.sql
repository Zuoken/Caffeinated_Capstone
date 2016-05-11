-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Insert new activity log into the database
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].InsertActivity
	@username VARCHAR(20),
	@date DATETIME,
	@activity VARCHAR(255)
AS
	DECLARE @customer_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE [username] = @username;
		INSERT INTO Activities (log_date, activity, customer_id) 
		VALUES (@date, @activity, @customer_id); 
	END
GO