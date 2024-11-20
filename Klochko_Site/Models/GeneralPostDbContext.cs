using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Klochko_Site.Models;

public partial class GeneralPostDbContext : DbContext
{
    public GeneralPostDbContext()
    {
    }

    public GeneralPostDbContext(DbContextOptions<GeneralPostDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageType> PackageTypes { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<UserCredential> UserCredentials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=GeneralPost;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FA5B416F25F");

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BranchName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B865270861");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.DateRegistered)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<DeliveryStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Delivery__C8EE20435F4B9AAC");

            entity.ToTable("DeliveryStatus");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.DateUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.StatusDescription).HasMaxLength(255);

            entity.HasOne(d => d.Package).WithMany(p => p.DeliveryStatuses)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeliveryS__Packa__5070F446");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF109603B4B");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Position).HasMaxLength(50);

            entity.HasOne(d => d.Branch).WithMany(p => p.Employees)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__Employees__Branc__4222D4EF");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Logs__5E5499A8C3DFAA13");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.LogDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__Packages__322035EC5C1E88FB");

            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.DateDelivered).HasColumnType("datetime");
            entity.Property(e => e.DateSent)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dimensions).HasMaxLength(50);
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("('В пути')");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Branch).WithMany(p => p.Packages)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Packages__Branch__4BAC3F29");

            entity.HasOne(d => d.Receiver).WithMany(p => p.PackageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Packages__Receiv__4AB81AF0");

            entity.HasOne(d => d.Sender).WithMany(p => p.PackageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Packages__Sender__49C3F6B7");

            entity.HasOne(d => d.Type).WithMany(p => p.Packages)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Packages__TypeID__4CA06362");
        });

        modelBuilder.Entity<PackageType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__PackageT__516F03958EF56416");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58C0442A48");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Custom__59FA5E80");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Transa__5AEE82B9");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB0EA40946981");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4BF6CDA3D6");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Transacti__Emplo__5535A963");

            entity.HasOne(d => d.Package).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Packa__5441852A");
        });

        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserCred__3214EC0746042A77");

            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(256);

            entity.HasOne(d => d.Customer).WithMany(p => p.UserCredentials)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_UserCredentials_Customer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
