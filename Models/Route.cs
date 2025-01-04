using System;
using System.Collections.Generic;

namespace Esti_bus_project.Models;

public partial class Route
{
    public string RouteId { get; set; } = null!;

    public int? AgencyId { get; set; }

    public string? RouteShortName { get; set; }

    public string? RouteLongName { get; set; }

    public int? RouteType { get; set; }

    public string? RouteColor { get; set; }

    public string? CompetentAuthority { get; set; }

    public string? RouteDesc { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
