/* ------------------------------------------------------------
   Description:  Update proc for table 'Customer'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.UpdateCustomer') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.UpdateCustomer
GO

CREATE PROCEDURE dbo.UpdateCustomer
(
    @id INT,
    @name VARCHAR(255),
	@email VARCHAR(255),
	@password VARCHAR(100)
)
AS

BEGIN
    SET NOCOUNT ON
    
    UPDATE [Customer]
    SET
        [name] = @name,
		[email] = @email,
		[password] = @password
    WHERE [id] = @id
    
END

