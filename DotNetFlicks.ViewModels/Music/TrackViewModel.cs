using System;

namespace DotNetFlicks.ViewModels.Music
{
    public class TrackViewModel
    {
        public int TrackNumber { get; set; }

        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public string PreviewUrl { get; set; }
    }
}
