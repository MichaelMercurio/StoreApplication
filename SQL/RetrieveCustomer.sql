/* ------------------------------------------------------------
   Description:  Retrieve proc for table 'Customer'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.RetrieveCustomer') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.RetrieveCustomer
GO

CREATE PROCEDURE dbo.RetrieveCustomer
(
    @id INT,
    @name VARCHAR(255) OUTPUT,
    @email VARCHAR(255) OUTPUT,
    @password VARCHAR(100) OUTPUT
)
AS

BEGIN
    SET NOCOUNT ON
    
    SELECT
        @name = [name],
        @email = [email],
        @password = [password]
    FROM Customer
    WHERE [id] = @id
    
END

