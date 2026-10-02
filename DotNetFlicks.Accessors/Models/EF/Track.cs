using DotNetFlicks.Accessors.Models.EF.Base;
using System;

namespace DotNetFlicks.Accessors.Models.EF
{
    public class Track : Entity
    {
        public int AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public int TrackNumber { get; set; }

        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public string PreviewUrl { get; set; }
    }
}
