/* ------------------------------------------------------------
   Description:  Create proc for table 'Product'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.CreateProduct') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.CreateProduct
GO

CREATE PROCEDURE dbo.CreateProduct
(
    @id INT OUTPUT,
    @name VARCHAR(255),
	@price DECIMAL(15,2)
)
AS

BEGIN
    SET NOCOUNT ON
    
    INSERT [Product]
    (
        [name],
		[price]
    )
    VALUES
    (
        @name,
		@price
    )
    
    SET @id = @@IDENTITY

END

