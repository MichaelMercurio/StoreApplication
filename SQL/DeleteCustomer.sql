/* ------------------------------------------------------------
   Description:  Delete proc for table 'Customer'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.DeleteCustomer') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.DeleteCustomer
GO

CREATE PROCEDURE dbo.DeleteCustomer
(
    @id INT
)
AS

BEGIN
    SET NOCOUNT ON
    
    DELETE [Customer]
    WHERE [id] = @id
    
END

