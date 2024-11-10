using System;
using System.Collections.Generic;

namespace Klochko_Site.Models;

public partial class Log
{
    public int LogId { get; set; }

    public DateTime? LogDate { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? Details { get; set; }

    public string? Ipaddress { get; set; }
}
