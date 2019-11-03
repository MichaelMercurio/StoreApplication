/* ------------------------------------------------------------
   Description:  Delete proc for table 'Purchase'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.DeletePurchase') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.DeletePurchase
GO

CREATE PROCEDURE dbo.DeletePurchase
(
    @id INT
)
AS

BEGIN
    SET NOCOUNT ON
    
    DELETE [Purchase]
    WHERE [id] = @id
    
END

