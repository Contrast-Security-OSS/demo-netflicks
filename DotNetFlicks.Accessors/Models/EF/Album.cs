using DotNetFlicks.Accessors.Models.EF.Base;
using System;
using System.Collections.Generic;

namespace DotNetFlicks.Accessors.Models.EF
{
    public class Album : Entity
    {
        public string Title { get; set; }

        public string Artist { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string CoverImageUrl { get; set; }

        public string Genre { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }

        public virtual ICollection<UserAlbum> UserAlbums { get; set; }
    }
}
