using DotNetFlicks.Accessors.Models.DTO.Base;
using System;

namespace DotNetFlicks.Accessors.Models.DTO
{
    public class TrackDTO : EntityDTO
    {
        public int AlbumId { get; set; }

        public int TrackNumber { get; set; }

        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public string PreviewUrl { get; set; }
    }
}
