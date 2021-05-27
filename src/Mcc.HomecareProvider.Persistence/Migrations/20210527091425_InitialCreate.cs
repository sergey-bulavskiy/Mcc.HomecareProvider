using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mcc.HomecareProvider.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Devices",
                table => new
                {
                    Id = table.Column<Guid>("uuid", nullable: false),
                    SerialNumber = table.Column<string>("text", nullable: true),
                    CurrentBindingId = table.Column<Guid>("uuid", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Devices", x => x.Id); });

            migrationBuilder.CreateTable(
                "Patients",
                table => new
                {
                    Id = table.Column<Guid>("uuid", nullable: false),
                    FirstName = table.Column<string>("text", nullable: false),
                    LastName = table.Column<string>("text", nullable: false),
                    Email = table.Column<string>("text", nullable: false),
                    DateOfBirth = table.Column<DateTime>("timestamp without time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>("timestamp with time zone", nullable: false),
                    CurrentBindingId = table.Column<Guid>("uuid", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Patients", x => x.Id); });

            migrationBuilder.CreateTable(
                "DeviceBindings",
                table => new
                {
                    Id = table.Column<Guid>("uuid", nullable: false),
                    PatientId = table.Column<Guid>("uuid", nullable: true),
                    DeviceId = table.Column<Guid>("uuid", nullable: false),
                    AssignedToPatientAt = table.Column<DateTimeOffset>("timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceBindings", x => x.Id);
                    table.ForeignKey(
                        "FK_DeviceBindings_Devices_DeviceId",
                        x => x.DeviceId,
                        "Devices",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_DeviceBindings_Patients_PatientId",
                        x => x.PatientId,
                        "Patients",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_DeviceBindings_DeviceId",
                "DeviceBindings",
                "DeviceId");

            migrationBuilder.CreateIndex(
                "IX_DeviceBindings_PatientId",
                "DeviceBindings",
                "PatientId");

            migrationBuilder.CreateIndex(
                "IX_Devices_CurrentBindingId",
                "Devices",
                "CurrentBindingId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Devices_SerialNumber",
                "Devices",
                "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Patients_CurrentBindingId",
                "Patients",
                "CurrentBindingId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Patients_Email",
                "Patients",
                "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                "FK_Devices_DeviceBindings_CurrentBindingId",
                "Devices",
                "CurrentBindingId",
                "DeviceBindings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Patients_DeviceBindings_CurrentBindingId",
                "Patients",
                "CurrentBindingId",
                "DeviceBindings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_DeviceBindings_Devices_DeviceId",
                "DeviceBindings");

            migrationBuilder.DropForeignKey(
                "FK_DeviceBindings_Patients_PatientId",
                "DeviceBindings");

            migrationBuilder.DropTable(
                "Devices");

            migrationBuilder.DropTable(
                "Patients");

            migrationBuilder.DropTable(
                "DeviceBindings");
        }
    }
}