using System;
using System.Collections.Generic;

namespace Klochko_Site.Models;

public partial class DeliveryStatus
{
    public int StatusId { get; set; }

    public int PackageId { get; set; }

    public DateTime? DateUpdated { get; set; }

    public string? Location { get; set; }

    public string? StatusDescription { get; set; }

    public virtual Package Package { get; set; } = null!;
}
