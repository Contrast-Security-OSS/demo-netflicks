using DotNetFlicks.ViewModels.Music;

namespace DotNetFlicks.Managers.Interfaces
{
    public interface IMusicManager
    {
        AlbumViewModel Get(int id, string userId);

        AlbumsViewModel GetAll(string userId);

        AlbumsViewModel GetAllForUser(string userId);

        void AddToLibrary(int id, string userId);

        void RemoveFromLibrary(int id, string userId);
    }
}
