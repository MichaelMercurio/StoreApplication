/* ------------------------------------------------------------
   Description:  Retrieve proc for table 'Purchase'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.RetrievePurchase') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.RetrievePurchase
GO

CREATE PROCEDURE dbo.RetrievePurchase
(
    @id INT,
    @userid INT OUTPUT,
    @productid INT OUTPUT
)
AS

BEGIN
    SET NOCOUNT ON
    
    SELECT
        @userid = [userid],
        @productid = [productid]
    FROM Purchase
    WHERE [id] = @id
    
END

