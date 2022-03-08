use [University]

create table Student (
	Id int identity(1,1) constraint PK_Student primary key,
	FirstName nvarchar(100),
	LastName nvarchar(100)
)

go

create table [Group] (
	Id int identity(1,1) constraint PK_Group primary key,
	Name nvarchar(100)
)

go

create table StudentGroup (
	Id int identity(1,1) constraint PK_StudentGroup primary key,
	GroupId int constraint FK_StudentGroup_Group references [Group](Id),
	StudentId int constraint FK_StudentGroup_Student references [Student](Id)
)

go

insert into dbo.Student
values
('Vasya', 'Vasilyev'),
('Kolya', 'Kopitin'),
('Vova', 'Roshin'),
('Maksim', 'Maksimov')

go

insert into dbo.[Group]
values
('Math'),
('History'),
('English'),
('Russian'),
('Computer science')

go

insert into dbo.StudentGroup
values
(1, 1),
(1, 2),
(1, 3),
(2, 1),
(2, 4),
(3, 2),
(3, 3),
(4, 1),
(4, 4)

go

select *
from dbo.Student

select *
from dbo.[Group]

select *
from dbo.StudentGroup

go

SELECT [Id], [FirstName], [LastName] FROM [dbo].[Student]