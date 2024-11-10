using System;
using System.Collections.Generic;

namespace Klochko_Site.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Position { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? BranchId { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
