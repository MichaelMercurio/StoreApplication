/* ------------------------------------------------------------
   Description:  Update proc for table 'Purchase'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.UpdatePurchase') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.UpdatePurchase
GO

CREATE PROCEDURE dbo.UpdatePurchase
(
    @id INT,
    @userid INT,
    @productid INT
)
AS

BEGIN
    SET NOCOUNT ON
    
    UPDATE [Purchase]
    SET
        [userid] = @userid,
		[productid] = @productid
    WHERE [id] = @id
    
END

