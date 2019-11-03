/* ------------------------------------------------------------
   Description:  Update proc for table 'Product'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.UpdateProduct') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.UpdateProduct
GO

CREATE PROCEDURE dbo.UpdateProduct
(
    @id INT,
    @name VARCHAR(255),
	@price DECIMAL(15,2)
)
AS

BEGIN
    SET NOCOUNT ON
    
    UPDATE [Product]
    SET
        [name] = @name,
		[price] = @price
    WHERE [id] = @id
    
END

