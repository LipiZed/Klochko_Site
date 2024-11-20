using System;
using System.Collections.Generic;

namespace Klochko_Site.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateTime? DateRegistered { get; set; }

    public virtual ICollection<Package> PackageReceivers { get; set; } = new List<Package>();

    public virtual ICollection<Package> PackageSenders { get; set; } = new List<Package>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<UserCredential> UserCredentials { get; set; } = new List<UserCredential>();
}
