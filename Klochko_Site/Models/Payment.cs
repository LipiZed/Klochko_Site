using System;
using System.Collections.Generic;

namespace Klochko_Site.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int CustomerId { get; set; }

    public int TransactionId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
