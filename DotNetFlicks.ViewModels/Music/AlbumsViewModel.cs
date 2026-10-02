using System.Collections.Generic;

namespace DotNetFlicks.ViewModels.Music
{
    public class AlbumsViewModel
    {
        public List<AlbumViewModel> Albums { get; set; }

        public bool IsUserLibrary { get; set; }
    }
}
