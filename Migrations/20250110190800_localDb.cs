using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Esti_bus_project.Migrations
{
    /// <inheritdoc />
    public partial class localDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    route_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    agency_id = table.Column<int>(type: "int", nullable: true),
                    route_short_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    route_long_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    route_type = table.Column<int>(type: "int", nullable: true),
                    route_color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    competent_authority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    route_desc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routes", x => x.route_id);
                });

            migrationBuilder.CreateTable(
                name: "stops",
                columns: table => new
                {
                    stop_id = table.Column<int>(type: "int", nullable: false),
                    stop_code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    stop_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    stop_lat = table.Column<double>(type: "float", nullable: true),
                    stop_lon = table.Column<double>(type: "float", nullable: true),
                    zone_id = table.Column<int>(type: "int", nullable: true),
                    alias = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    stop_area = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    stop_desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lest_x = table.Column<double>(type: "float", nullable: true),
                    lest_y = table.Column<double>(type: "float", nullable: true),
                    zone_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    authority = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stops", x => x.stop_id);
                });

            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    trip_id = table.Column<int>(type: "int", nullable: false),
                    route_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    service_id = table.Column<int>(type: "int", nullable: true),
                    trip_headsign = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    trip_long_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    direction_code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    shape_id = table.Column<int>(type: "int", nullable: true),
                    wheelchair_accessible = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trips", x => x.trip_id);
                    table.ForeignKey(
                        name: "FK_trips_route",
                        column: x => x.route_id,
                        principalTable: "routes",
                        principalColumn: "route_id");
                });

            migrationBuilder.CreateTable(
                name: "stop_times",
                columns: table => new
                {
                    trip_id = table.Column<int>(type: "int", nullable: false),
                    stop_id = table.Column<int>(type: "int", nullable: false),
                    stop_sequence = table.Column<int>(type: "int", nullable: false),
                    arrival_time = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    departure_time = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    pickup_type = table.Column<int>(type: "int", nullable: true),
                    drop_off_type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stops_times", x => new { x.trip_id, x.stop_id, x.stop_sequence });
                    table.ForeignKey(
                        name: "FK_stops_times_stop",
                        column: x => x.stop_id,
                        principalTable: "stops",
                        principalColumn: "stop_id");
                    table.ForeignKey(
                        name: "FK_stops_times_trip",
                        column: x => x.trip_id,
                        principalTable: "trips",
                        principalColumn: "trip_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_stop_times_stop_id",
                table: "stop_times",
                column: "stop_id");

            migrationBuilder.CreateIndex(
                name: "IX_trips_route_id",
                table: "trips",
                column: "route_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stop_times");

            migrationBuilder.DropTable(
                name: "stops");

            migrationBuilder.DropTable(
                name: "trips");

            migrationBuilder.DropTable(
                name: "routes");
        }
    }
}
