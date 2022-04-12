﻿// <auto-generated />
using System;
using DynamicDriving.TripManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DynamicDriving.TripManagement.Infrastructure.Migrations
{
    [DbContext(typeof(TripManagementContext))]
    partial class TripManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DynamicDriving.TripManagement.Domain.DriversAggregate.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CarType")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SoftDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("DynamicDriving.TripManagement.Domain.DriversAggregate.Driver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SoftDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("DynamicDriving.TripManagement.Domain.LocationsAggregate.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SoftDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("City");
                });

            modelBuilder.Entity("DynamicDriving.TripManagement.Domain.LocationsAggregate.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SoftDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("DynamicDriving.TripManagement.Domain.TripsAggregate.Trip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DestinationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DriverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Kilometers")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("OriginId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PickUp")
                        .HasColumnType("datetime2");

                    b.Property<bool>("SoftDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("TripStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DestinationId");

                    b.HasIndex("DriverId");

                    b.HasIndex("OriginId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("DynamicDriving.TripManagement.Domain.DriversAggregate.Driver", b =>
                {
                    b.HasOne("DynamicDriving.TripManagement.Domain.DriversAggregate.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("DynamicDriving.TripManagement.Domain.LocationsAggregate.Location", b =>
                {
                    b.HasOne("DynamicDriving.TripManagement.Domain.LocationsAggregate.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("DynamicDriving.TripManagement.Domain.Common.Coordinates", "Coordinates", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Latitude")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Latitude");

                            b1.Property<decimal>("Longitude")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Longitude");

                            b1.HasKey("LocationId");

                            b1.ToTable("Locations");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.Navigation("City");

                    b.Navigation("Coordinates")
                        .IsRequired();
                });

            modelBuilder.Entity("DynamicDriving.TripManagement.Domain.TripsAggregate.Trip", b =>
                {
                    b.HasOne("DynamicDriving.TripManagement.Domain.LocationsAggregate.Location", "Destination")
                        .WithMany()
                        .HasForeignKey("DestinationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DynamicDriving.TripManagement.Domain.DriversAggregate.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DynamicDriving.TripManagement.Domain.LocationsAggregate.Location", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("DynamicDriving.TripManagement.Domain.TripsAggregate.UserId", "UserId", b1 =>
                        {
                            b1.Property<Guid>("TripId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uniqueidentifier")
                                .HasColumnName("UserId");

                            b1.HasKey("TripId");

                            b1.ToTable("Trips");

                            b1.WithOwner()
                                .HasForeignKey("TripId");
                        });

                    b.Navigation("Destination");

                    b.Navigation("Driver");

                    b.Navigation("Origin");

                    b.Navigation("UserId")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
