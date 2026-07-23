using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFlowProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SkillId",
                table: "JobPosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "AttachmentType",
                table: "AttachmentsFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "AttachmentsFiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Skills_AspNetUsers_DeleterId",
                        column: x => x.DeleterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Skills_AspNetUsers_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Skills_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobPosts_SkillId",
                table: "JobPosts",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentsFiles_CompanyId",
                table: "AttachmentsFiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CategoryId",
                table: "Skills",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CreatorId",
                table: "Skills",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_DeleterId",
                table: "Skills",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ModifierId",
                table: "Skills",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                table: "Skills",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentsFiles_Companies_CompanyId",
                table: "AttachmentsFiles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosts_Skills_SkillId",
                table: "JobPosts",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentsFiles_Companies_CompanyId",
                table: "AttachmentsFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosts_Skills_SkillId",
                table: "JobPosts");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_JobPosts_SkillId",
                table: "JobPosts");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentsFiles_CompanyId",
                table: "AttachmentsFiles");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "JobPosts");

            migrationBuilder.DropColumn(
                name: "AttachmentType",
                table: "AttachmentsFiles");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AttachmentsFiles");
        }
    }
}
