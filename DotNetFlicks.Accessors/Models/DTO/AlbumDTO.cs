using DotNetFlicks.Accessors.Models.DTO.Base;
using System;
using System.Collections.Generic;

namespace DotNetFlicks.Accessors.Models.DTO
{
    public class AlbumDTO : EntityDTO
    {
        public string Title { get; set; }

        public string Artist { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string CoverImageUrl { get; set; }

        public string Genre { get; set; }

        public ICollection<TrackDTO> Tracks { get; set; }
    }
}
