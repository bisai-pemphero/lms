using Microsoft.EntityFrameworkCore;
using LandManagement.Api.Entities;

namespace LandManagement.Api.Data;

/// <summary>
/// Application DbContext for the Land Management System.
/// Configures entity mappings to match the legacy database schema.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Core entities
    public DbSet<Client> Clients { get; set; }
    public DbSet<Site> Sites { get; set; }
    public DbSet<Plot> Plots { get; set; }
    public DbSet<PlotAllocation> PlotAllocations { get; set; }
    public DbSet<PlotPayment> PlotPayments { get; set; }
    public DbSet<NextOfKin> NextOfKins { get; set; }

    // Sale agreements
    public DbSet<SaleAgreement> SaleAgreements { get; set; }
    public DbSet<SaleAgreementPlot> SaleAgreementPlots { get; set; }

    // Users and roles
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<CUser> CUsers { get; set; }

    // Creditors
    public DbSet<Creditor> Creditors { get; set; }
    public DbSet<CreditorPayment> CreditorPayments { get; set; }

    // Value sequences
    public DbSet<ValueSequence> ValueSequences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Client entity
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("T_Clients");
            entity.HasKey(e => e.ClientNo);
            entity.Property(e => e.ClientNo).HasColumnName("ClientNo").HasMaxLength(50);
            entity.Property(e => e.ClientName).HasColumnName("ClientName").HasMaxLength(200);
            entity.Property(e => e.IdNo).HasColumnName("IDNo").HasMaxLength(50);
            entity.Property(e => e.PostalAddress).HasColumnName("PostalAddress").HasMaxLength(500);
            entity.Property(e => e.PhysicalAddress).HasColumnName("PhysicalAddress").HasMaxLength(500);
            entity.Property(e => e.Town).HasColumnName("Town").HasMaxLength(100);
            entity.Property(e => e.CellPhone).HasColumnName("CellPhone").HasMaxLength(20);
            entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(100);
            entity.Property(e => e.DateCreated).HasColumnName("DateCreated");
            entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
            entity.Property(e => e.ClientType).HasColumnName("ClientType").HasMaxLength(50);
            entity.Property(e => e.CompanyRegNo).HasColumnName("CompanyRegNo").HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber").HasMaxLength(20);
            entity.Property(e => e.City).HasColumnName("City").HasMaxLength(100);
            entity.Property(e => e.Country).HasColumnName("Country").HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
            entity.Property(e => e.CompanyName).HasColumnName("CompanyName").HasMaxLength(200);
            entity.Property(e => e.Title).HasColumnName("Title").HasMaxLength(50);
            entity.Property(e => e.Comments).HasColumnName("Comments").HasMaxLength(500);
            entity.Property(e => e.Status).HasColumnName("Status").HasMaxLength(50);
        });

        // Configure Site entity
        modelBuilder.Entity<Site>(entity =>
        {
            entity.ToTable("T_Sites");
            entity.HasKey(e => e.SiteCode);
            entity.Property(e => e.SiteCode).HasColumnName("SiteCode").HasMaxLength(50);
            entity.Property(e => e.SiteName).HasColumnName("SiteName").HasMaxLength(100);
            entity.Property(e => e.District).HasColumnName("District").HasMaxLength(100);
            entity.Property(e => e.PhysicalLocation).HasColumnName("PhysicalLocation").HasMaxLength(500);
            entity.Property(e => e.Size).HasColumnName("Size");
            entity.Property(e => e.PreviousOwner).HasColumnName("PreviousOwner").HasMaxLength(200);
            entity.Property(e => e.InitialValue).HasColumnName("InitialValue");
            entity.Property(e => e.AmountPaid).HasColumnName("AmountPaid");
            entity.Property(e => e.Balance).HasColumnName("Balance");
            entity.Property(e => e.DevelopmentsDone).HasColumnName("DevelopmentsDone").HasMaxLength(500);
            entity.Property(e => e.DevelopmentCost).HasColumnName("DevelopmentCost");
            entity.Property(e => e.TA).HasColumnName("TA").HasMaxLength(100);
            entity.Property(e => e.Village).HasColumnName("Village").HasMaxLength(100);
            entity.Property(e => e.Region).HasColumnName("Region").HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(1000);
            entity.Property(e => e.PricePerSqMeter).HasColumnName("PricePerSqMeter");
            entity.Property(e => e.BankName).HasColumnName("BankName").HasMaxLength(200);
            entity.Property(e => e.AccountNumber).HasColumnName("AccountNumber").HasMaxLength(50);
            entity.Property(e => e.RoadSize).HasColumnName("RoadSize");
            entity.Property(e => e.RemainingAcreage).HasColumnName("RemainingAcreage");
            entity.Property(e => e.SiteMap).HasColumnName("SiteMap").HasMaxLength(500);
            entity.Property(e => e.Status).HasColumnName("Status").HasMaxLength(50);
            entity.Property(e => e.DateCreated).HasColumnName("DateCreated");
        });

        // Configure Plot entity
        modelBuilder.Entity<Plot>(entity =>
        {
            entity.ToTable("T_Plots");
            entity.HasKey(e => e.PlotNo);
            entity.Property(e => e.PlotNo).HasColumnName("PlotNo").HasMaxLength(50);
            entity.Property(e => e.SiteNo).HasColumnName("SiteNo").HasMaxLength(50);
            entity.Property(e => e.Block).HasColumnName("Block").HasMaxLength(50);
            entity.Property(e => e.Area).HasColumnName("PlotSize");
            entity.Property(e => e.Value).HasColumnName("PlotValue");
            entity.Property(e => e.NormalPrice).HasColumnName("NormalPrice");
            entity.Property(e => e.PromotionPrice).HasColumnName("PromotionPrice");
            entity.Property(e => e.LandTitle).HasColumnName("LandTitle").HasMaxLength(50);
            entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(500);
            entity.Property(e => e.Status).HasColumnName("PlotStatus").HasMaxLength(50);
            entity.Property(e => e.OfferDate).HasColumnName("OfferDate").HasMaxLength(50);
            entity.Property(e => e.DateCreated).HasColumnName("DateCreated");

            entity.HasOne(e => e.Site)
                .WithMany(s => s.Plots)
                .HasForeignKey(e => e.SiteNo)
                .HasPrincipalKey(s => s.SiteCode);
        });

        // Configure PlotAllocation entity
        modelBuilder.Entity<PlotAllocation>(entity =>
        {
            entity.ToTable("T_PlotAllocations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.ClientNo).HasColumnName("ClientNo").HasMaxLength(50);
            entity.Property(e => e.PlotNo).HasColumnName("PlotNo").HasMaxLength(50);
            entity.Property(e => e.AgreedPrice).HasColumnName("AgreedPrice");
            entity.Property(e => e.AmountPaid).HasColumnName("AmountPaid");
            entity.Property(e => e.Balance).HasColumnName("Balance");
            entity.Property(e => e.MonthlyInstallment).HasColumnName("MonthlyInstallment");
            entity.Property(e => e.AllocationDate).HasColumnName("AllocationDate");
            entity.Property(e => e.PostedBy).HasColumnName("PostedBy").HasMaxLength(100);

            entity.HasOne(e => e.Client)
                .WithMany(c => c.PlotAllocations)
                .HasForeignKey(e => e.ClientNo)
                .HasPrincipalKey(c => c.ClientNo);

            entity.HasOne(e => e.Plot)
                .WithMany(p => p.PlotAllocations)
                .HasForeignKey(e => e.PlotNo)
                .HasPrincipalKey(p => p.PlotNo);
        });

        // Configure PlotPayment entity
        modelBuilder.Entity<PlotPayment>(entity =>
        {
            entity.ToTable("T_PlotPayments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.PlotNo).HasColumnName("PlotNo").HasMaxLength(50);
            entity.Property(e => e.ClientNo).HasColumnName("ClientNo").HasMaxLength(50);
            entity.Property(e => e.AmountPaid).HasColumnName("AmountPaid");
            entity.Property(e => e.Balance).HasColumnName("Balance");
            entity.Property(e => e.DatePaid).HasColumnName("DatePaid");
            entity.Property(e => e.PaymentMode).HasColumnName("PaymentMode").HasMaxLength(50);
            entity.Property(e => e.PaymentRef).HasColumnName("PaymentRef").HasMaxLength(100);
            entity.Property(e => e.ReceiptNo).HasColumnName("ReceiptNo").HasMaxLength(50);
            entity.Property(e => e.PostedBy).HasColumnName("PostedBy").HasMaxLength(100);

            entity.HasOne(e => e.Plot)
                .WithMany(p => p.PlotPayments)
                .HasForeignKey(e => e.PlotNo)
                .HasPrincipalKey(p => p.PlotNo);

            entity.HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientNo)
                .HasPrincipalKey(c => c.ClientNo);
        });

        // Configure NextOfKin entity
        modelBuilder.Entity<NextOfKin>(entity =>
        {
            entity.ToTable("T_Next_of_Kins");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.ClientNo).HasColumnName("ClientNo").HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(200);
            entity.Property(e => e.Relationship).HasColumnName("Relationship").HasMaxLength(100);
            entity.Property(e => e.Contact).HasColumnName("Contact").HasMaxLength(50);
            entity.Property(e => e.Address).HasColumnName("Address").HasMaxLength(500);

            entity.HasOne(e => e.Client)
                .WithMany(c => c.NextOfKins)
                .HasForeignKey(e => e.ClientNo)
                .HasPrincipalKey(c => c.ClientNo);
        });

        // Configure SaleAgreement entity
        modelBuilder.Entity<SaleAgreement>(entity =>
        {
            entity.ToTable("T_SaleAgreements");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.AgreementNo).HasColumnName("AgreementNo").HasMaxLength(50);
            entity.Property(e => e.ClientNo).HasColumnName("ClientNo").HasMaxLength(50);
            entity.Property(e => e.DateCreated).HasColumnName("DateCreated");
            entity.Property(e => e.Status).HasColumnName("Status").HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnName("TotalAmount");
            entity.Property(e => e.AmountPaid).HasColumnName("AmountPaid");
            entity.Property(e => e.Balance).HasColumnName("Balance");

            entity.HasOne(e => e.Client)
                .WithMany(c => c.SaleAgreements)
                .HasForeignKey(e => e.ClientNo)
                .HasPrincipalKey(c => c.ClientNo);
        });

        // Configure SaleAgreementPlot entity
        modelBuilder.Entity<SaleAgreementPlot>(entity =>
        {
            entity.ToTable("T_SaleAgreementPlots");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.SaleAgreementId).HasColumnName("SaleAgreementId");
            entity.Property(e => e.PlotNo).HasColumnName("PlotNo").HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnName("Price");

            entity.HasOne(e => e.SaleAgreement)
                .WithMany(s => s.SaleAgreementPlots)
                .HasForeignKey(e => e.SaleAgreementId);

            entity.HasOne(e => e.Plot)
                .WithMany()
                .HasForeignKey(e => e.PlotNo)
                .HasPrincipalKey(p => p.PlotNo);
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("T_Users");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.Property(e => e.Username).HasColumnName("Username").HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(500);
            entity.Property(e => e.FullName).HasColumnName("FullName").HasMaxLength(200);
            entity.Property(e => e.Email).HasColumnName("Email").HasMaxLength(100);
            entity.Property(e => e.RoleId).HasColumnName("RoleId");
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.DateCreated).HasColumnName("DateCreated");
            entity.Property(e => e.LastLogin).HasColumnName("LastLogin");

            entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId);
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("T_Roles");
            entity.HasKey(e => e.RoleId);
            entity.Property(e => e.RoleId).HasColumnName("RoleId");
            entity.Property(e => e.RoleName).HasColumnName("RoleName").HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(500);
        });

        // Configure Creditor entity
        modelBuilder.Entity<Creditor>(entity =>
        {
            entity.ToTable("T_Creditors");
            entity.HasKey(e => e.CreditorId);
            entity.Property(e => e.CreditorId).HasColumnName("CreditorId");
            entity.Property(e => e.CreditorName).HasColumnName("CreditorName").HasMaxLength(200);
            entity.Property(e => e.LandOwner).HasColumnName("LandOwner").HasMaxLength(200);
            entity.Property(e => e.SiteCode).HasColumnName("SiteCode").HasMaxLength(50);
            entity.Property(e => e.AmountAgreed).HasColumnName("AmountAgreed");
            entity.Property(e => e.AmountPaid).HasColumnName("AmountPaid");
            entity.Property(e => e.Balance).HasColumnName("Balance");
            entity.Property(e => e.RequestedBy).HasColumnName("RequestedBy").HasMaxLength(100);
            entity.Property(e => e.CheckedbyOpm).HasColumnName("CheckedbyOpm").HasMaxLength(100);
            entity.Property(e => e.AuthorisedByCeo).HasColumnName("AuthorisedByCEO").HasMaxLength(100);
            entity.Property(e => e.FinanceLedgeby).HasColumnName("FinanceLedgeby").HasMaxLength(100);
            entity.Property(e => e.Postedby).HasColumnName("Postedby").HasMaxLength(100);
            entity.Property(e => e.DateCreated).HasColumnName("DateCreated");

            entity.HasOne(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteCode)
                .HasPrincipalKey(s => s.SiteCode);
        });

        // Configure CreditorPayment entity
        modelBuilder.Entity<CreditorPayment>(entity =>
        {
            entity.ToTable("T_CreditorPayments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.CreditorId).HasColumnName("CreditorId");
            entity.Property(e => e.AmountPaid).HasColumnName("AmountPaid");
            entity.Property(e => e.DatePaid).HasColumnName("DatePaid");
            entity.Property(e => e.PaymentMode).HasColumnName("PaymentMode").HasMaxLength(50);
            entity.Property(e => e.PaymentRef).HasColumnName("PaymentRef").HasMaxLength(100);
            entity.Property(e => e.PostedBy).HasColumnName("PostedBy").HasMaxLength(100);

            entity.HasOne(e => e.Creditor)
                .WithMany(c => c.CreditorPayments)
                .HasForeignKey(e => e.CreditorId);
        });
    }
}
