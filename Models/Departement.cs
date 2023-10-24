using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace School_Web_API.Models;

public partial class Departement
{
    [Key]
    public int DepartementId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
