using System;
using System.Collections.Generic;

namespace Esti_bus_project.Models;

public partial class StopTime
{
    public int TripId { get; set; }

    public string? ArrivalTime { get; set; }

    public string? DepartureTime { get; set; }

    public int StopId { get; set; }

    public int StopSequence { get; set; }

    public int? PickupType { get; set; }

    public int? DropOffType { get; set; }

    public virtual Stop Stop { get; set; } = null!;

    public virtual Trip Trip { get; set; } = null!;
}
