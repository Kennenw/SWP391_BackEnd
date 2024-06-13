using System;
using System.Collections.Generic;
using Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public partial class BookingBadmintonSystemContext : DbContext
{
    public BookingBadmintonSystemContext()
    {
    }

    public BookingBadmintonSystemContext(DbContextOptions<BookingBadmintonSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<AmenityCourt> AmenityCourts { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingDetail> BookingDetails { get; set; }

    public virtual DbSet<BookingType> BookingTypes { get; set; }

    public virtual DbSet<CheckIn> CheckIns { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Court> Courts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SlotTime> SlotTimes { get; set; }

    public virtual DbSet<SubCourt> SubCourts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=BookingBadmintonSystem;UID=sa;PWD=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Image)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsFixedLength();
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.AmenitiId).HasName("PK_Amenity");

            entity.Property(e => e.AmenitiId).HasColumnName("AmenitiID");
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<AmenityCourt>(entity =>
        {
            entity.ToTable("AmenityCourt");

            entity.Property(e => e.AmenityCourtId).HasColumnName("AmenityCourtID");
            entity.Property(e => e.AmenityId).HasColumnName("AmenityID");
            entity.Property(e => e.CourtId).HasColumnName("CourtID");

            entity.HasOne(d => d.Amenity).WithMany(p => p.AmenityCourts)
                .HasForeignKey(d => d.AmenityId)
                .HasConstraintName("FK_AmenityCourt_Amenities");

            entity.HasOne(d => d.Court).WithMany(p => p.AmenityCourts)
                .HasForeignKey(d => d.CourtId)
                .HasConstraintName("FK_AmenityCourt_Court1");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK_Complexe");

            entity.ToTable("Area");

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.Location).HasMaxLength(50);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingTypeId).HasColumnName("BookingTypeID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.BookingType).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BookingTypeId)
                .HasConstraintName("FK_Booking_BookingType1");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Booking_Account");
        });

        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.ToTable("BookingDetail");

            entity.Property(e => e.BookingDetailId).HasColumnName("BookingDetailID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.SlotId).HasColumnName("SlotID");
            entity.Property(e => e.SubCourtId).HasColumnName("SubCourtID");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_BookingDetail_Booking");

            entity.HasOne(d => d.Slot).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK_BookingDetail_SlotTime");

            entity.HasOne(d => d.SubCourt).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.SubCourtId)
                .HasConstraintName("FK_BookingDetail_SubCourt1");
        });

        modelBuilder.Entity<BookingType>(entity =>
        {
            entity.ToTable("BookingType");

            entity.Property(e => e.BookingTypeId).HasColumnName("BookingTypeID");
            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.BookingDetailId).HasColumnName("BookingDetailID");
            entity.Property(e => e.CheckInId)
                .ValueGeneratedOnAdd()
                .HasColumnName("CheckInID");
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");

            entity.HasOne(d => d.BookingDetail).WithMany()
                .HasForeignKey(d => d.BookingDetailId)
                .HasConstraintName("FK_CheckIns_BookingDetail");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.Context).HasMaxLength(450);
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Court>(entity =>
        {
            entity.ToTable("Court");

            entity.Property(e => e.CloseTime)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CourtName).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.OpenTime)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.Area).WithMany(p => p.Courts)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_Court_Area");

            entity.HasOne(d => d.Manager).WithMany(p => p.Courts)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Court_Account");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_Payment_Booking");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Content).HasMaxLength(450);
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.Images)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Account).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Post_Account");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(5)
                .IsFixedLength();
        });

        modelBuilder.Entity<SlotTime>(entity =>
        {
            entity.HasKey(e => e.SlotId);

            entity.ToTable("SlotTime");

            entity.Property(e => e.EndTime)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StartTime)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Manager).WithMany(p => p.SlotTimes)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_SlotTime_Account");

            entity.HasOne(d => d.SubCourt).WithMany(p => p.SlotTimes)
                .HasForeignKey(d => d.SubCourtId)
                .HasConstraintName("FK_SlotTime_SubCourt");
        });

        modelBuilder.Entity<SubCourt>(entity =>
        {
            entity.HasKey(e => e.SubCourtId).HasName("PK_CourtNumber");

            entity.ToTable("SubCourt");

            entity.HasOne(d => d.Court).WithMany(p => p.SubCourts)
                .HasForeignKey(d => d.CourtId)
                .HasConstraintName("FK_SubCourt_Court");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
