USE [Northwind];
GO
IF NOT EXISTS
(
    SELECT 1
    FROM sys.tables
    WHERE object_id = OBJECT_ID(N'dbo.Users')
)
    CREATE TABLE [dbo].[Users]
    ([Id]       [INT] NOT NULL,
     [UserName] [NVARCHAR](50) NOT NULL,
     [Password] [NVARCHAR](100) NOT NULL,
     CONSTRAINT [PK_auth.Users] PRIMARY KEY CLUSTERED([Id] ASC)
     WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    )
    ON [PRIMARY];
GO
IF NOT EXISTS
(
    SELECT 1
    FROM sys.tables
    WHERE object_id = OBJECT_ID(N'dbo.Roles')
)
    CREATE TABLE [dbo].[Roles]
    ([Id]       [INT] NOT NULL,
     [RoleName] [NVARCHAR](100) NOT NULL,
     PRIMARY KEY CLUSTERED([Id] ASC)
     WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    )
    ON [PRIMARY];
GO
IF NOT EXISTS
(
    SELECT 1
    FROM sys.tables
    WHERE object_id = OBJECT_ID(N'dbo.UserInRoles')
)
    BEGIN
        CREATE TABLE [dbo].[UserInRoles]
        ([UserId] [INT] NOT NULL,
         [RoleId] [INT] NOT NULL,
         CONSTRAINT [UQ_UIR] UNIQUE NONCLUSTERED([UserId] ASC, [RoleId] ASC)
         WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        )
        ON [PRIMARY];
        ALTER TABLE [dbo].[UserInRoles]
        WITH CHECK
        ADD CONSTRAINT [FK_UIR_Roles] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles]([Id]);
        ALTER TABLE [dbo].[UserInRoles] CHECK CONSTRAINT [FK_UIR_Roles];
        ALTER TABLE [dbo].[UserInRoles]
        WITH CHECK
        ADD CONSTRAINT [FK_UIR_Users] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users]([Id]);
        ALTER TABLE [dbo].[UserInRoles] CHECK CONSTRAINT [FK_UIR_Users];
    END;
GO

MERGE dbo.Users AS Users
USING (VALUES (1, N'Operator1', N'MTIzNDU2'),
			  (2, N'Manager1', N'MTIzNDU2'),
			  (3, N'SuperUser', N'MTIzNDU2'),
			  (4, N'Guest', N'')) AS src (Id, UserName, UserPassword)
ON Users.Id = src.Id
WHEN NOT MATCHED BY TARGET
THEN INSERT (Id, UserName, [Password])
VALUES (src.Id, src.UserName, src.UserPassword)
WHEN MATCHED
THEN UPDATE SET UserName = src.UserName,
			 [Password] = src.[UserPassword];
GO

MERGE dbo.Roles AS Roles
USING (VALUES (1, N'Operator'),
			  (2, N'Manager')) AS src (Id, RoleName)
ON Roles.Id = src.Id
WHEN NOT MATCHED BY TARGET
THEN INSERT (Id, RoleName)
VALUES (src.Id, src.RoleName)
WHEN MATCHED
THEN UPDATE SET RoleName = src.RoleName;
GO

MERGE dbo.UserInRoles AS UserInRoles
USING (VALUES (1, 1),
			  (2, 2),
			  (3, 1),
			  (3, 2)) AS src (UserId, RoleId)
ON  UserInRoles.UserId = src.UserId
    AND UserInRoles.RoleId = src.RoleId
WHEN NOT MATCHED BY TARGET
THEN INSERT (UserId, RoleId)
VALUES (src.UserId, src.RoleId);
GO
