﻿namespace P01_StudentSystem.Data.Models;

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set;}
    public string? PhoneNumber { get; set;}
    public DateTime RegisteredOn { get; set; }
    public DateTime? Birthday { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }
    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
}