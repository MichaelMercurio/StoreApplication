/* ------------------------------------------------------------
   Description:  Create proc for table 'Customer'
   ------------------------------------------------------------ */
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID('dbo.CreateCustomer') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE dbo.CreateCustomer
GO

CREATE PROCEDURE dbo.CreateCustomer
(
    @id INT OUTPUT,
    @name VARCHAR(255),
	@email VARCHAR(255),
	@password VARCHAR(100)
)
AS

BEGIN
    SET NOCOUNT ON
    
    INSERT [Customer]
    (
        [name],
		[email],
		[password]
    )
    VALUES
    (
        @name,
		@email,
		@password
    )
    
    SET @id = @@IDENTITY

END

