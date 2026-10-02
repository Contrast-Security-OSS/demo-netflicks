using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetFlicks.Accessors.Migrations
{
    public partial class AddMusicLibrary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Artist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    TrackNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    PreviewUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAlbums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAlbums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAlbums_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAlbums_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "Artist", "CoverImageUrl", "Description", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, "The Midnight Signals", "/images/music/music-placeholder.svg", "Synth-driven night music built for open roads and glowing city streets.", "Electronic", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Neon Skyline" },
                    { 2, "June Atlas", "/images/music/music-placeholder.svg", "Warm indie pop songs about distance, memory, and finding your way home.", "Indie Pop", new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paper Planets" },
                    { 3, "Northbound Quartet", "/images/music/music-placeholder.svg", "A relaxed collection of modern jazz recorded live in one evening.", "Jazz", new DateTime(2022, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Blue Hour Sessions" },
                    { 4, "Cassidy Reed", "/images/music/music-placeholder.svg", "Acoustic stories, close harmonies, and a little dust from the back roads.", "Folk", new DateTime(2024, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wildflower Radio" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "AlbumId", "Duration", "PreviewUrl", "Title", "TrackNumber" },
                values: new object[,]
                {
                    { 1, 1, new TimeSpan(0, 0, 3, 42, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3", "Afterimage", 1 },
                    { 2, 1, new TimeSpan(0, 0, 4, 5, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3", "Neon Skyline", 2 },
                    { 3, 1, new TimeSpan(0, 0, 3, 31, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3", "Last Train Awake", 3 },
                    { 4, 2, new TimeSpan(0, 0, 3, 18, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3", "Fold the Map", 1 },
                    { 5, 2, new TimeSpan(0, 0, 3, 55, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3", "Paper Planets", 2 },
                    { 6, 2, new TimeSpan(0, 0, 4, 2, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3", "Borrowed Light", 3 },
                    { 7, 3, new TimeSpan(0, 0, 5, 12, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3", "Platform Nine", 1 },
                    { 8, 3, new TimeSpan(0, 0, 6, 8, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3", "Blue Hour", 2 },
                    { 9, 3, new TimeSpan(0, 0, 4, 47, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3", "Closing Time", 3 },
                    { 10, 4, new TimeSpan(0, 0, 3, 26, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3", "County Line", 1 },
                    { 11, 4, new TimeSpan(0, 0, 3, 49, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3", "Wildflower Radio", 2 },
                    { 12, 4, new TimeSpan(0, 0, 4, 11, 0), "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3", "Porchlight", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_AlbumId",
                table: "Tracks",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAlbums_AlbumId",
                table: "UserAlbums",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAlbums_UserId_AlbumId",
                table: "UserAlbums",
                columns: new[] { "UserId", "AlbumId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "UserAlbums");

            migrationBuilder.DropTable(
                name: "Albums");
        }
    }
}
