using System;
using System.Collections.Generic;

namespace BOs;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string Role { get; set; } = null!;

    public string Department { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
