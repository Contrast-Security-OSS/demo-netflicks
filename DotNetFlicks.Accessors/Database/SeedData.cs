using DotNetFlicks.Accessors.Models.EF;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetFlicks.Accessors.Database
{
    public static class SeedData
    {
        public static void SeedAlbums(this ModelBuilder builder)
        {
            builder.Entity<Album>().HasData(
                new Album
                {
                    Id = 1,
                    Title = "Neon Skyline",
                    Artist = "The Midnight Signals",
                    Description = "Synth-driven night music built for open roads and glowing city streets.",
                    ReleaseDate = new DateTime(2024, 3, 15),
                    CoverImageUrl = "/images/music/music-placeholder.svg",
                    Genre = "Electronic"
                },
                new Album
                {
                    Id = 2,
                    Title = "Paper Planets",
                    Artist = "June Atlas",
                    Description = "Warm indie pop songs about distance, memory, and finding your way home.",
                    ReleaseDate = new DateTime(2023, 9, 8),
                    CoverImageUrl = "/images/music/music-placeholder.svg",
                    Genre = "Indie Pop"
                },
                new Album
                {
                    Id = 3,
                    Title = "Blue Hour Sessions",
                    Artist = "Northbound Quartet",
                    Description = "A relaxed collection of modern jazz recorded live in one evening.",
                    ReleaseDate = new DateTime(2022, 11, 18),
                    CoverImageUrl = "/images/music/music-placeholder.svg",
                    Genre = "Jazz"
                },
                new Album
                {
                    Id = 4,
                    Title = "Wildflower Radio",
                    Artist = "Cassidy Reed",
                    Description = "Acoustic stories, close harmonies, and a little dust from the back roads.",
                    ReleaseDate = new DateTime(2024, 6, 21),
                    CoverImageUrl = "/images/music/music-placeholder.svg",
                    Genre = "Folk"
                });
        }

        public static void SeedTracks(this ModelBuilder builder)
        {
            const string previewOne = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3";
            const string previewTwo = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3";

            builder.Entity<Track>().HasData(
                new Track { Id = 1, AlbumId = 1, TrackNumber = 1, Title = "Afterimage", Duration = new TimeSpan(0, 3, 42), PreviewUrl = previewOne },
                new Track { Id = 2, AlbumId = 1, TrackNumber = 2, Title = "Neon Skyline", Duration = new TimeSpan(0, 4, 5), PreviewUrl = previewTwo },
                new Track { Id = 3, AlbumId = 1, TrackNumber = 3, Title = "Last Train Awake", Duration = new TimeSpan(0, 3, 31), PreviewUrl = previewOne },
                new Track { Id = 4, AlbumId = 2, TrackNumber = 1, Title = "Fold the Map", Duration = new TimeSpan(0, 3, 18), PreviewUrl = previewTwo },
                new Track { Id = 5, AlbumId = 2, TrackNumber = 2, Title = "Paper Planets", Duration = new TimeSpan(0, 3, 55), PreviewUrl = previewOne },
                new Track { Id = 6, AlbumId = 2, TrackNumber = 3, Title = "Borrowed Light", Duration = new TimeSpan(0, 4, 2), PreviewUrl = previewTwo },
                new Track { Id = 7, AlbumId = 3, TrackNumber = 1, Title = "Platform Nine", Duration = new TimeSpan(0, 5, 12), PreviewUrl = previewOne },
                new Track { Id = 8, AlbumId = 3, TrackNumber = 2, Title = "Blue Hour", Duration = new TimeSpan(0, 6, 8), PreviewUrl = previewTwo },
                new Track { Id = 9, AlbumId = 3, TrackNumber = 3, Title = "Closing Time", Duration = new TimeSpan(0, 4, 47), PreviewUrl = previewOne },
                new Track { Id = 10, AlbumId = 4, TrackNumber = 1, Title = "County Line", Duration = new TimeSpan(0, 3, 26), PreviewUrl = previewTwo },
                new Track { Id = 11, AlbumId = 4, TrackNumber = 2, Title = "Wildflower Radio", Duration = new TimeSpan(0, 3, 49), PreviewUrl = previewOne },
                new Track { Id = 12, AlbumId = 4, TrackNumber = 3, Title = "Porchlight", Duration = new TimeSpan(0, 4, 11), PreviewUrl = previewTwo });
        }

        public static void SeedGenres(this ModelBuilder builder)
        {
            var genres = JsonConvert.DeserializeObject<List<Genre>>(File.ReadAllText(@"Config" + Path.DirectorySeparatorChar + "SeedData" + Path.DirectorySeparatorChar + "genres.json"));

            foreach (var genre in genres)
            {
                builder.Entity<Genre>().HasData(genre);
            }
        }

        public static void SeedDepartments(this ModelBuilder builder)
        {
            var departments = JsonConvert.DeserializeObject<List<Department>>(File.ReadAllText(@"Config" + Path.DirectorySeparatorChar + "SeedData" + Path.DirectorySeparatorChar + "departments.json"));

            foreach (var department in departments)
            {
                builder.Entity<Department>().HasData(department);
            }
        }

        public static void SeedPeople(this ModelBuilder builder)
        {
            var people = JsonConvert.DeserializeObject<List<Person>>(File.ReadAllText(@"Config" + Path.DirectorySeparatorChar + "SeedData" + Path.DirectorySeparatorChar + "people.json"));

            foreach (var person in people)
            {
                builder.Entity<Person>().HasData(person);
            }
        }

        public static void SeedMovies(this ModelBuilder builder)
        {
            var movies = JsonConvert.DeserializeObject<List<Movie>>(File.ReadAllText(@"Config" + Path.DirectorySeparatorChar + "SeedData" + Path.DirectorySeparatorChar + "movies.json"));

            foreach (var movie in movies)
            {
                builder.Entity<Movie>().HasData(movie);
            }
        }

        public static void SeedMovieGenres(this ModelBuilder builder)
        {
            var movieGenres = JsonConvert.DeserializeObject<List<MovieGenre>>(File.ReadAllText(@"Config" + Path.DirectorySeparatorChar + "SeedData" + Path.DirectorySeparatorChar + "movieGenres.json"));

            foreach (var movieGenre in movieGenres)
            {
                builder.Entity<MovieGenre>().HasData(movieGenre);
            }
        }

        public static void SeedCastMembers(this ModelBuilder builder)
        {
            var castMembers = JsonConvert.DeserializeObject<List<CastMember>>(File.ReadAllText(@"Config" + Path.DirectorySeparatorChar + "SeedData" + Path.DirectorySeparatorChar + "castMembers.json"));

            foreach (var castMember in castMembers)
            {
                builder.Entity<CastMember>().HasData(castMember);
            }
        }

        public static void SeedCrewMembers(this ModelBuilder builder)
        {
            var crewMembers = JsonConvert.DeserializeObject<List<CrewMember>>(File.ReadAllText(@"Config" + Path.DirectorySeparatorChar + "SeedData" + Path.DirectorySeparatorChar + "crewMembers.json"));

            foreach (var crewMember in crewMembers)
            {
                builder.Entity<CrewMember>().HasData(crewMember);
            }
        }
    }
}
