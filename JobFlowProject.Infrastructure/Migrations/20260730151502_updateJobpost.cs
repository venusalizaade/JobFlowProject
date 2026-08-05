using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFlowProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateJobpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FeaturedUntil",
                table: "JobPosts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "JobPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeaturedUntil",
                table: "JobPosts");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "JobPosts");
        }
    }
}
