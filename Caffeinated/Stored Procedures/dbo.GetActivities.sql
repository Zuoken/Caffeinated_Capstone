-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Get activity log for a user
-- Cameron Winters, 000299896
-- December 5, 2015
CREATE PROCEDURE [dbo].GetActivities
	@username VARCHAR(20)
AS
	DECLARE @customer_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE [username] = @username;

		-- Return activities ordered by most recent activity first
		SELECT * FROM Activities
		WHERE customer_id = @customer_id
		ORDER BY log_date DESC; 
	END
GO