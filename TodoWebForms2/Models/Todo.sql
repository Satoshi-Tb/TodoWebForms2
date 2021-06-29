CREATE TABLE [dbo].[Todo] (
    [Id]          INT           NOT NULL,
    [Title]       NVARCHAR (50) NOT NULL,
    [DueDate]     DATE          NULL,
    [IsCompleted] TINYINT       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);