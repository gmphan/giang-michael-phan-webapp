﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmphan.DataAccessLib.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectSummary",
                table: "Projects",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectSummary",
                table: "Projects");
        }
    }
}
