using DotNetFlicks.ViewModels.Base;
using System;
using System.Collections.Generic;

namespace DotNetFlicks.ViewModels.Music
{
    public class AlbumViewModel : EntityViewModel
    {
        public string Title { get; set; }

        public string Artist { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string CoverImageUrl { get; set; }

        public string Genre { get; set; }

        public bool IsInLibrary { get; set; }

        public List<TrackViewModel> Tracks { get; set; }
    }
}
