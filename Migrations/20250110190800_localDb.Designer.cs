﻿// <auto-generated />
using System;
using Esti_bus_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Esti_bus_project.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250110190800_localDb")]
    partial class localDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Esti_bus_project.Models.Route", b =>
                {
                    b.Property<string>("RouteId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("route_id");

                    b.Property<int?>("AgencyId")
                        .HasColumnType("int")
                        .HasColumnName("agency_id");

                    b.Property<string>("CompetentAuthority")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("competent_authority");

                    b.Property<string>("RouteColor")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("route_color");

                    b.Property<string>("RouteDesc")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("route_desc");

                    b.Property<string>("RouteLongName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("route_long_name");

                    b.Property<string>("RouteShortName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("route_short_name");

                    b.Property<int?>("RouteType")
                        .HasColumnType("int")
                        .HasColumnName("route_type");

                    b.HasKey("RouteId");

                    b.ToTable("routes", (string)null);
                });

            modelBuilder.Entity("Esti_bus_project.Models.Stop", b =>
                {
                    b.Property<int>("StopId")
                        .HasColumnType("int")
                        .HasColumnName("stop_id");

                    b.Property<string>("Alias")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("alias");

                    b.Property<string>("Authority")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("authority");

                    b.Property<double?>("LestX")
                        .HasColumnType("float")
                        .HasColumnName("lest_x");

                    b.Property<double?>("LestY")
                        .HasColumnType("float")
                        .HasColumnName("lest_y");

                    b.Property<string>("StopArea")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("stop_area");

                    b.Property<string>("StopCode")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("stop_code");

                    b.Property<string>("StopDesc")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("stop_desc");

                    b.Property<double?>("StopLat")
                        .HasColumnType("float")
                        .HasColumnName("stop_lat");

                    b.Property<double?>("StopLon")
                        .HasColumnType("float")
                        .HasColumnName("stop_lon");

                    b.Property<string>("StopName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("stop_name");

                    b.Property<int?>("ZoneId")
                        .HasColumnType("int")
                        .HasColumnName("zone_id");

                    b.Property<string>("ZoneName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("zone_name");

                    b.HasKey("StopId");

                    b.ToTable("stops", (string)null);
                });

            modelBuilder.Entity("Esti_bus_project.Models.StopTime", b =>
                {
                    b.Property<int>("TripId")
                        .HasColumnType("int")
                        .HasColumnName("trip_id");

                    b.Property<int>("StopId")
                        .HasColumnType("int")
                        .HasColumnName("stop_id");

                    b.Property<int>("StopSequence")
                        .HasColumnType("int")
                        .HasColumnName("stop_sequence");

                    b.Property<string>("ArrivalTime")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("arrival_time");

                    b.Property<string>("DepartureTime")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("departure_time");

                    b.Property<int?>("DropOffType")
                        .HasColumnType("int")
                        .HasColumnName("drop_off_type");

                    b.Property<int?>("PickupType")
                        .HasColumnType("int")
                        .HasColumnName("pickup_type");

                    b.HasKey("TripId", "StopId", "StopSequence")
                        .HasName("PK_stops_times");

                    b.HasIndex("StopId");

                    b.ToTable("stop_times", (string)null);
                });

            modelBuilder.Entity("Esti_bus_project.Models.Trip", b =>
                {
                    b.Property<int>("TripId")
                        .HasColumnType("int")
                        .HasColumnName("trip_id");

                    b.Property<string>("DirectionCode")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("direction_code");

                    b.Property<string>("RouteId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("route_id");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int")
                        .HasColumnName("service_id");

                    b.Property<int?>("ShapeId")
                        .HasColumnType("int")
                        .HasColumnName("shape_id");

                    b.Property<string>("TripHeadsign")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("trip_headsign");

                    b.Property<string>("TripLongName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("trip_long_name");

                    b.Property<int?>("WheelchairAccessible")
                        .HasColumnType("int")
                        .HasColumnName("wheelchair_accessible");

                    b.HasKey("TripId");

                    b.HasIndex("RouteId");

                    b.ToTable("trips", (string)null);
                });

            modelBuilder.Entity("Esti_bus_project.Models.StopTime", b =>
                {
                    b.HasOne("Esti_bus_project.Models.Stop", "Stop")
                        .WithMany("StopTimes")
                        .HasForeignKey("StopId")
                        .IsRequired()
                        .HasConstraintName("FK_stops_times_stop");

                    b.HasOne("Esti_bus_project.Models.Trip", "Trip")
                        .WithMany("StopTimes")
                        .HasForeignKey("TripId")
                        .IsRequired()
                        .HasConstraintName("FK_stops_times_trip");

                    b.Navigation("Stop");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Esti_bus_project.Models.Trip", b =>
                {
                    b.HasOne("Esti_bus_project.Models.Route", "Route")
                        .WithMany("Trips")
                        .HasForeignKey("RouteId")
                        .IsRequired()
                        .HasConstraintName("FK_trips_route");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("Esti_bus_project.Models.Route", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("Esti_bus_project.Models.Stop", b =>
                {
                    b.Navigation("StopTimes");
                });

            modelBuilder.Entity("Esti_bus_project.Models.Trip", b =>
                {
                    b.Navigation("StopTimes");
                });
#pragma warning restore 612, 618
        }
    }
}
