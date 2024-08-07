﻿using System;
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

    public virtual DbSet<Payments> Payments { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<RatingCourt> RatingCourts { get; set; }

    public virtual DbSet<RatingPost> RatingPosts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SlotTime> SlotTimes { get; set; }

    public virtual DbSet<SubCourt> SubCourts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(local);Database=BookingBadmintonSystem;UID=sa;PWD=123456;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountName).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(15)
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

            entity.HasIndex(e => e.Description, "unique_description").IsUnique();

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

            entity.HasIndex(e => e.Location, "unique_location").IsUnique();

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.Location).HasMaxLength(250);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingTypeId).HasColumnName("BookingTypeID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.BookingType).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BookingTypeId)
                .HasConstraintName("FK_Booking_BookingType1");

            entity.HasOne(d => d.Court).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CourtId)
                .HasConstraintName("FK_Booking_Court");

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
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.Property(e => e.CheckInId).HasColumnName("CheckInID");
            entity.Property(e => e.BookingDetailId).HasColumnName("BookingDetailID");
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");

            entity.HasOne(d => d.BookingDetail).WithMany(p => p.CheckIns)
                .HasForeignKey(d => d.BookingDetailId)
                .HasConstraintName("FK_CheckIns_BookingDetail");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.Context).HasMaxLength(450);
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Court>(entity =>
        {
            entity.ToTable("Court");

            entity.Property(e => e.CloseTime)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.CourtName).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.OpenTime)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Area).WithMany(p => p.Courts)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_Court_Area1");

            entity.HasOne(d => d.Manager).WithMany(p => p.Courts)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Court_Account");
        });

        modelBuilder.Entity<Payments>(entity =>
        {
            entity.ToTable("Payments");

            entity.HasKey(e => e.PaymentId).HasName("PK_Payments");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_Payments_Booking");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Payments_Account");
        });


        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .IsFixedLength();

            entity.HasOne(d => d.Account).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Post_Account");
        });

        modelBuilder.Entity<RatingCourt>(entity =>
        {
            entity.ToTable("RatingCourt");

            entity.HasOne(d => d.Court).WithMany(p => p.RatingCourts)
                .HasForeignKey(d => d.CourtId)
                .HasConstraintName("FK_RatingCourt_Court");

            entity.HasOne(d => d.User).WithMany(p => p.RatingCourts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RatingCourt_Account");
        });

        modelBuilder.Entity<RatingPost>(entity =>
        {
            entity.HasKey(e => e.RatingId);

            entity.ToTable("RatingPost");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");

            entity.HasOne(d => d.Post).WithMany(p => p.RatingPosts)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_RatingPost_Post");

            entity.HasOne(d => d.User).WithMany(p => p.RatingPosts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RatingPost_Account");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(7)
                .IsFixedLength();
        });

        modelBuilder.Entity<SlotTime>(entity =>
        {
            entity.HasKey(e => e.SlotId);

            entity.ToTable("SlotTime");

            entity.Property(e => e.EndTime)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.StartTime)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.WeekdayPrice).HasColumnName("weekdayPrice");
            entity.Property(e => e.WeekendPrice).HasColumnName("weekendPrice");

            entity.HasOne(d => d.Court).WithMany(p => p.SlotTimes)
                .HasForeignKey(d => d.CourtId)
                .HasConstraintName("FK_SlotTime_Court");

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

            entity.Property(e => e.Number).HasMaxLength(50);

            entity.HasOne(d => d.Court).WithMany(p => p.SubCourts)
                .HasForeignKey(d => d.CourtId)
                .HasConstraintName("FK_SubCourt_Court");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
