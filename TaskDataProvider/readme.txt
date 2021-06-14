Create a database.

Create a table:
CREATE TABLE [dbo].[Tasks] (
[Id]               UNIQUEIDENTIFIER NOT NULL,
[TaskText]         NVARCHAR (50)    NULL,
[Status]           INT              NULL,
[CreationTime]     DATETIME2 (7)    NOT NULL,
[ModificationTime] DATETIME2 (7)    NOT NULL,
[ConsumerId]       UNIQUEIDENTIFIER NULL,
PRIMARY KEY CLUSTERED ([Id] ASC)
);
         
Set connection string in TestDbContext.cs file

Run ProducerApp
Run ConsumerApp