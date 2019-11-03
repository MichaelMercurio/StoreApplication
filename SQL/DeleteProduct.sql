/* ------------------------------------------------------------
   Description:  Delete proc for table 'Product'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.DeleteProduct') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.DeleteProduct
GO

CREATE PROCEDURE dbo.DeleteProduct
(
    @id INT
)
AS

BEGIN
    SET NOCOUNT ON
    
    DELETE [Product]
    WHERE [id] = @id
    
END

