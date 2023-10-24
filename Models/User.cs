using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace School_Web_API.Models;

public partial class User
{
    [Key]   
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}

public partial class Login
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}


