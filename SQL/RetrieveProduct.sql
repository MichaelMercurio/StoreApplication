/* ------------------------------------------------------------
   Description:  Retrieve proc for table 'Product'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.RetrieveProduct') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.RetrieveProduct
GO

CREATE PROCEDURE dbo.RetrieveProduct
(
    @id INT,
    @name VARCHAR(255) OUTPUT,
	@price DECIMAL(15,2) OUTPUT
)
AS

BEGIN
    SET NOCOUNT ON
    
    SELECT
        @name = [name],
		@price = [price]
    FROM Product
    WHERE [id] = @id
    
END

