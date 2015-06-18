INSERT INTO [OES].[dbo].[Users]
           ([UserId]
           ,[UserName]
           ,[Name]
           ,[Password]
           ,[Email]
           ,[Role]
           ,[BirthDate]
           ,[Discriminator])
     VALUES
           (NEWID()
           ,'admin'
           ,'Admin'
           ,'admin'
           ,'admin@admin.com'
           ,0
           ,NULL
           ,'Admin')
GO

