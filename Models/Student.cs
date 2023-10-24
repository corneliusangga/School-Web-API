using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace School_Web_API.Models;

public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [ForeignKey("Departement")]
    public int DepartementId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int Semester { get; set; }

    [Required]
    public int Age { get; set; }

    public virtual Departement? Departement { get; set; }
}
