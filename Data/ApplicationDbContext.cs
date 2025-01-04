using System;
using System.Collections.Generic;
using Esti_bus_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Esti_bus_project.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Models.Route> Routes { get; set; }

    public virtual DbSet<Stop> Stops { get; set; }

    public virtual DbSet<StopTime> StopTimes { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:sqlserver-polikanov-dev-001.database.windows.net,1433;Initial Catalog=sqldb-polikanov-001;User=polikanov;Password=Ibragimovich10;TrustServerCertificate=True;Connection Timeout=60;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.Route>(entity =>
        {
            entity.ToTable("routes");

            entity.Property(e => e.RouteId)
                .HasMaxLength(255)
                .HasColumnName("route_id");
            entity.Property(e => e.AgencyId).HasColumnName("agency_id");
            entity.Property(e => e.CompetentAuthority)
                .HasMaxLength(50)
                .HasColumnName("competent_authority");
            entity.Property(e => e.RouteColor)
                .HasMaxLength(10)
                .HasColumnName("route_color");
            entity.Property(e => e.RouteDesc).HasColumnName("route_desc");
            entity.Property(e => e.RouteLongName)
                .HasMaxLength(255)
                .HasColumnName("route_long_name");
            entity.Property(e => e.RouteShortName)
                .HasMaxLength(50)
                .HasColumnName("route_short_name");
            entity.Property(e => e.RouteType).HasColumnName("route_type");
        });

        modelBuilder.Entity<Stop>(entity =>
        {
            entity.ToTable("stops");

            entity.Property(e => e.StopId)
                .ValueGeneratedNever()
                .HasColumnName("stop_id");
            entity.Property(e => e.Alias)
                .HasMaxLength(255)
                .HasColumnName("alias");
            entity.Property(e => e.Authority)
                .HasMaxLength(255)
                .HasColumnName("authority");
            entity.Property(e => e.LestX).HasColumnName("lest_x");
            entity.Property(e => e.LestY).HasColumnName("lest_y");
            entity.Property(e => e.StopArea)
                .HasMaxLength(255)
                .HasColumnName("stop_area");
            entity.Property(e => e.StopCode)
                .HasMaxLength(255)
                .HasColumnName("stop_code");
            entity.Property(e => e.StopDesc).HasColumnName("stop_desc");
            entity.Property(e => e.StopLat).HasColumnName("stop_lat");
            entity.Property(e => e.StopLon).HasColumnName("stop_lon");
            entity.Property(e => e.StopName)
                .HasMaxLength(255)
                .HasColumnName("stop_name");
            entity.Property(e => e.ZoneId).HasColumnName("zone_id");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(255)
                .HasColumnName("zone_name");
        });

        modelBuilder.Entity<StopTime>(entity =>
        {
            entity.HasKey(e => new { e.TripId, e.StopId, e.StopSequence }).HasName("PK_stops_times");

            entity.ToTable("stop_times");

            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.StopId).HasColumnName("stop_id");
            entity.Property(e => e.StopSequence).HasColumnName("stop_sequence");
            entity.Property(e => e.ArrivalTime)
                .HasMaxLength(50)
                .HasColumnName("arrival_time");
            entity.Property(e => e.DepartureTime)
                .HasMaxLength(50)
                .HasColumnName("departure_time");
            entity.Property(e => e.DropOffType).HasColumnName("drop_off_type");
            entity.Property(e => e.PickupType).HasColumnName("pickup_type");

            entity.HasOne(d => d.Stop).WithMany(p => p.StopTimes)
                .HasForeignKey(d => d.StopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_stops_times_stop");

            entity.HasOne(d => d.Trip).WithMany(p => p.StopTimes)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_stops_times_trip");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.ToTable("trips");

            entity.Property(e => e.TripId)
                .ValueGeneratedNever()
                .HasColumnName("trip_id");
            entity.Property(e => e.DirectionCode)
                .HasMaxLength(255)
                .HasColumnName("direction_code");
            entity.Property(e => e.RouteId)
                .HasMaxLength(255)
                .HasColumnName("route_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.ShapeId).HasColumnName("shape_id");
            entity.Property(e => e.TripHeadsign)
                .HasMaxLength(255)
                .HasColumnName("trip_headsign");
            entity.Property(e => e.TripLongName)
                .HasMaxLength(255)
                .HasColumnName("trip_long_name");
            entity.Property(e => e.WheelchairAccessible).HasColumnName("wheelchair_accessible");

            entity.HasOne(d => d.Route).WithMany(p => p.Trips)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_trips_route");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
