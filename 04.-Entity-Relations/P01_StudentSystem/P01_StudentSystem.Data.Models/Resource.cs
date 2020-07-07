﻿namespace P01_StudentSystem.Data.Models
{
    using P01_StudentSystem.Data.Models.Enumerations;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Resource
    {
        [Key, Required]
        public int ResourceId { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public ResourceType ResourceType { get; set; }

        [ForeignKey("Course"), Required]
        public int CourseId { get; set; }
        [Required]
        public Course Course { get; set; }
    }
}
