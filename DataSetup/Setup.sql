INSERT INTO [OES].[dbo].[Users]
           ([UserId],[UserName], [Name], [Password]
           ,[Email],[Role], [BirthDate], [Discriminator])
     VALUES
			-- Admin
           (NEWID(),'admin', 'Administrator','123','admin@admin.com',0, null, 'Admin'),
           --Instructors
           (NEWID(),'inst', 'Instructor','123','Instructor@Instructor.com',1, null, 'Instructor'),
           (NEWID(),'inst1', 'Instructor1','123','Instructor1@Instructor.com',1, null, 'Instructor'),
           (NEWID(),'inst2', 'Instructor2','123','Instructor2@Instructor.com',1, null, 'Instructor'),
           (NEWID(),'inst3', 'Instructor3','123','Instructor3@Instructor.com',1, null, 'Instructor'),
           -- Student
           (NEWID(),'student1', 'student1','123','student1@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student2', 'student2','123','student2@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student3', 'student3','123','student3@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student4', 'student4','123','student4@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student5', 'student5','123','student5@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student6', 'student6','123','student6@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student7', 'student7','123','student7@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student8', 'student8','123','student8@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student9', 'student9','123','student9@student.com',2, '1993-01-01', 'Student'),
           (NEWID(),'student10', 'student10','123','student10@student.com',2, '1993-01-01', 'Student')
           
GO


INSERT INTO [OES].[dbo].[Semesters]
           ([SemesterId], [SemesterTitle], [StartDate], [EndDate])
     VALUES
           ( NEWID(), 'Spring 2015', '2015-02-01', '2015-07-01'),
           ( NEWID(), 'Summer 2015', '2015-07-01', '2015-10-01')
           
INSERT INTO [OES].[dbo].[Courses]
           ([CourseId], [Code], [Title], [Description])
     VALUES
           (NEWID(),'CS001', 'Modern Programming Practices', ''),
           (NEWID(),'CS002', 'Advanced Software Development', ''),
           (NEWID(),'CS003', 'Computer Networks', ''),
           (NEWID(),'CS004', 'Database Management Systems', ''),
           (NEWID(),'CS005', 'Fundamentals of Algorithms', ''),
           (NEWID(),'CS006', 'Software Engineering', ''),
           (NEWID(),'CS007', 'Web Application Architecture', ''),
           (NEWID(),'CS008', 'Web Application Programming', '')
GO

--DELETE StudentRegistrations
--DELETE FROM Registrations

--DELETE Users
--DELETE Semesters
--DELETE Courses