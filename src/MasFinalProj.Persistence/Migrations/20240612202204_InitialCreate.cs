using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MasFinalProj.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlacklistedEmails",
                columns: table => new
                {
                    BlacklistedEmailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(52)", maxLength: 52, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedEmails", x => x.BlacklistedEmailId);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageFormat = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Base64Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Checksum = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(52)", maxLength: 52, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CampaignImageId = table.Column<long>(type: "bigint", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    GameCurrency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.CampaignId);
                    table.ForeignKey(
                        name: "FK_Campaigns_Images_CampaignImageId",
                        column: x => x.CampaignImageId,
                        principalTable: "Images",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(52)", maxLength: 52, nullable: false),
                    DiscordId = table.Column<long>(type: "bigint", nullable: true),
                    DiscordUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    ProfileImageId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    StaffSinceUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSuperUser = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Images_ProfileImageId",
                        column: x => x.ProfileImageId,
                        principalTable: "Images",
                        principalColumn: "ImageId");
                });

            migrationBuilder.CreateTable(
                name: "CampaignUsers",
                columns: table => new
                {
                    CampaignUserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignNickname = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CampaignBio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CampaignUserType = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Storyline = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignUsers", x => x.CampaignUserId);
                    table.ForeignKey(
                        name: "FK_CampaignUsers_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExpiryDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(52)", maxLength: 52, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    CharacterImageId = table.Column<long>(type: "bigint", nullable: true),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    PlayerOwnerId = table.Column<long>(type: "bigint", nullable: true),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_Characters_CampaignUsers_PlayerOwnerId",
                        column: x => x.PlayerOwnerId,
                        principalTable: "CampaignUsers",
                        principalColumn: "CampaignUserId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Characters_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId");
                    table.ForeignKey(
                        name: "FK_Characters_Images_CharacterImageId",
                        column: x => x.CharacterImageId,
                        principalTable: "Images",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAttribute",
                columns: table => new
                {
                    CharacterAttributeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    MoneyValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAttribute", x => x.CharacterAttributeId);
                    table.ForeignKey(
                        name: "FK_CharacterAttribute_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterRelationsWith",
                columns: table => new
                {
                    CharacterRelationWithId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCharacterId = table.Column<long>(type: "bigint", nullable: false),
                    ToCharacterId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RelationValue = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRelationsWith", x => x.CharacterRelationWithId);
                    table.ForeignKey(
                        name: "FK_CharacterRelationsWith_Characters_FromCharacterId",
                        column: x => x.FromCharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CharacterRelationsWith_Characters_ToCharacterId",
                        column: x => x.ToCharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterAuthorId = table.Column<long>(type: "bigint", nullable: true),
                    AuthorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_CampaignUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "CampaignUsers",
                        principalColumn: "CampaignUserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Characters_CharacterAuthorId",
                        column: x => x.CharacterAuthorId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CampaignUserId = table.Column<long>(type: "bigint", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: true),
                    CharacterAttributeId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Notes_CampaignUsers_CampaignUserId",
                        column: x => x.CampaignUserId,
                        principalTable: "CampaignUsers",
                        principalColumn: "CampaignUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_CharacterAttribute_CharacterAttributeId",
                        column: x => x.CharacterAttributeId,
                        principalTable: "CharacterAttribute",
                        principalColumn: "CharacterAttributeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notes_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Description", "DiscordId", "DiscordUsername", "Discriminator", "Email", "IsActive", "PasswordHash", "PasswordSalt", "ProfileImageId", "Username" },
                values: new object[] { new Guid("4d247e62-5966-42f3-be78-40ff86a18304"), "Testowy użytkownik", null, null, "User", "user@s24759masfinal.com", false, "6a+XneCh6rTjrASzNv6eqxJ5ESMUdH4z/l0VGEieU2s=", "VSRGlNT2+p8PrYKNSnNOKw==", null, "BaseUser" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Description", "DiscordId", "DiscordUsername", "Discriminator", "Email", "IsActive", "IsSuperUser", "PasswordHash", "PasswordSalt", "ProfileImageId", "StaffSinceUtc", "Username" },
                values: new object[,]
                {
                    { new Guid("c9143eb1-2c7b-4211-b070-b6948e55b707"), "Moje konto do testowania logowania z Discord OAuth", null, null, "Admin", "cypkowski@gmail.com", false, true, "GRJjOOXAq+VsXGcMkd7WuM/UVoT++Rq06+iSq/KqQD4=", "TZEWQPpM9B/0oauAX0qnrA==", null, new DateTime(2024, 6, 12, 20, 22, 4, 213, DateTimeKind.Utc).AddTicks(1129), "SirCypkowskyy" },
                    { new Guid("edd21e0a-8193-445e-8cac-4c90f71f6549"), "Base admin account", null, null, "Admin", "b.admin@s24759masfinal.com", false, false, "GRJjOOXAq+VsXGcMkd7WuM/UVoT++Rq06+iSq/KqQD4=", "TZEWQPpM9B/0oauAX0qnrA==", null, new DateTime(2024, 6, 12, 20, 22, 4, 213, DateTimeKind.Utc).AddTicks(1094), "BaseAdmin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedEmails_Email",
                table: "BlacklistedEmails",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CampaignImageId",
                table: "Campaigns",
                column: "CampaignImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_Name",
                table: "Campaigns",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CampaignUsers_CampaignId",
                table: "CampaignUsers",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignUsers_UserId",
                table: "CampaignUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAttribute_CharacterId",
                table: "CharacterAttribute",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationsWith_FromCharacterId_ToCharacterId",
                table: "CharacterRelationsWith",
                columns: new[] { "FromCharacterId", "ToCharacterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRelationsWith_ToCharacterId",
                table: "CharacterRelationsWith",
                column: "ToCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CampaignId",
                table: "Characters",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterImageId",
                table: "Characters",
                column: "CharacterImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_PlayerOwnerId",
                table: "Characters",
                column: "PlayerOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CampaignId",
                table: "Messages",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CharacterAuthorId",
                table: "Messages",
                column: "CharacterAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CampaignUserId",
                table: "Notes",
                column: "CampaignUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CharacterAttributeId",
                table: "Notes",
                column: "CharacterAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CharacterId",
                table: "Notes",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Value",
                table: "RefreshTokens",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileImageId",
                table: "Users",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedEmails");

            migrationBuilder.DropTable(
                name: "CharacterRelationsWith");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "CharacterAttribute");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "CampaignUsers");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
