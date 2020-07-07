namespace P01_StudentSystem.Data.Models
{
    using P01_StudentSystem.Data.Models.Enumerations;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Homework
    {
        [Key, Required]
        public int HomeworkId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public ContentType ContentType { get; set; }

        [Required]
        public DateTime SubmissionTime { get; set; }

        [ForeignKey("Student"), Required]
        public int StudentId { get; set; }

        [Required]
        public Student Student { get; set; }

        [ForeignKey("Course"), Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
