using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynamicDriving.TripManagement.Infrastructure.Migrations.TripManagement
{
    public partial class AddCostToTrips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditsCost",
                table: "Trips",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditsCost",
                table: "Trips");
        }
    }
}
