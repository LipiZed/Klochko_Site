using System;
using System.Collections.Generic;

namespace Klochko_Site.Models;

public partial class UserCredential
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
