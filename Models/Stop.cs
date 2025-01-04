using System;
using System.Collections.Generic;

namespace Esti_bus_project.Models;

public partial class Stop
{
    public int StopId { get; set; }

    public string? StopCode { get; set; }

    public string? StopName { get; set; }

    public double? StopLat { get; set; }

    public double? StopLon { get; set; }

    public int? ZoneId { get; set; }

    public string? Alias { get; set; }

    public string? StopArea { get; set; }

    public string? StopDesc { get; set; }

    public double? LestX { get; set; }

    public double? LestY { get; set; }

    public string? ZoneName { get; set; }

    public string? Authority { get; set; }

    public virtual ICollection<StopTime> StopTimes { get; set; } = new List<StopTime>();
}
