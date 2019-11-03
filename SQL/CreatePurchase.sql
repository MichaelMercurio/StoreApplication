/* ------------------------------------------------------------
   Description:  Create proc for table 'Purchase'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.CreatePurchase') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.CreatePurchase
GO

CREATE PROCEDURE dbo.CreatePurchase
(
    @id INT OUTPUT,
    @userid INT,
    @productid INT
)
AS

BEGIN
    SET NOCOUNT ON
    
    INSERT [Purchase]
    (
        [userid],
		[productid]
    )
    VALUES
    (
        @userid,
		@productid
    )
    
    SET @id = @@IDENTITY

END

