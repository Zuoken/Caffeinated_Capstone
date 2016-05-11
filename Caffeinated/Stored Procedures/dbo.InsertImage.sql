-- I, Cameron Winters, student number 000299896, certify that this material is my
-- original work. No other person's work has been used without due
-- acknowledgement and I have not made my work available to anyone else.

-- Insert new image into the database
-- Cameron Winters, 000299896
-- October 13, 2015
CREATE PROCEDURE [dbo].InsertImage
	@username VARCHAR(20),
	@filename VARCHAR(20),
	@date DATETIME,
	@directory VARCHAR(255)
AS
	DECLARE @customer_id AS INT
	BEGIN
		SELECT @customer_id = [customer_id] FROM Customers
		WHERE username = @username;
		INSERT INTO Images (customer_id, image_date, name, directory) 
		VALUES (@customer_id, @date, @filename, @directory);
	END
GO