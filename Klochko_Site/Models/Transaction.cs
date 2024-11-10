using System;
using System.Collections.Generic;

namespace Klochko_Site.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int PackageId { get; set; }

    public int? EmployeeId { get; set; }

    public string TransactionType { get; set; } = null!;

    public DateTime? TransactionDate { get; set; }

    public decimal? Amount { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Package Package { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
