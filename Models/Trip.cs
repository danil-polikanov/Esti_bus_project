using System;
using System.Collections.Generic;

namespace Esti_bus_project.Models;

public partial class Trip
{
    public string RouteId { get; set; } = null!;

    public int? ServiceId { get; set; }

    public int TripId { get; set; }

    public string? TripHeadsign { get; set; }

    public string? TripLongName { get; set; }

    public string? DirectionCode { get; set; }

    public int? ShapeId { get; set; }

    public int? WheelchairAccessible { get; set; }

    public virtual Route Route { get; set; } = null!;

    public virtual ICollection<StopTime> StopTimes { get; set; } = new List<StopTime>();
}
