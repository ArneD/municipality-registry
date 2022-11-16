﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipalityRegistry.Producer.Snapshot.Oslo.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MunicipalityRegistryProducerSnapshotOslo");

            migrationBuilder.CreateTable(
                name: "ProjectionStates",
                schema: "MunicipalityRegistryProducerSnapshotOslo",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    DesiredState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesiredStateChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionStates", x => x.Name)
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectionStates",
                schema: "MunicipalityRegistryProducerSnapshotOslo");
        }
    }
}
