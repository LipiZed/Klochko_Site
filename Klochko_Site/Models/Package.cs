using System;
using System.Collections.Generic;

namespace Klochko_Site.Models;

public partial class Package
{
    public int PackageId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public int BranchId { get; set; }

    public int TypeId { get; set; }

    public decimal? Weight { get; set; }

    public string? Dimensions { get; set; }

    public string? Status { get; set; }

    public DateTime? DateSent { get; set; }

    public DateTime? DateDelivered { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual ICollection<DeliveryStatus> DeliveryStatuses { get; set; } = new List<DeliveryStatus>();

    public virtual Customer Receiver { get; set; } = null!;

    public virtual Customer Sender { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual PackageType Type { get; set; } = null!;
}
