using AutoMapper;
using DotNetFlicks.Accessors.Interfaces;
using DotNetFlicks.Managers.Interfaces;
using DotNetFlicks.ViewModels.Music;
using System.Collections.Generic;
using System.Linq;

namespace DotNetFlicks.Managers.Managers
{
    public class MusicManager : IMusicManager
    {
        private readonly IAlbumAccessor _albumAccessor;
        private readonly IUserAlbumAccessor _userAlbumAccessor;

        public MusicManager(IAlbumAccessor albumAccessor, IUserAlbumAccessor userAlbumAccessor)
        {
            _albumAccessor = albumAccessor;
            _userAlbumAccessor = userAlbumAccessor;
        }

        public AlbumViewModel Get(int id, string userId)
        {
            var vm = Mapper.Map<AlbumViewModel>(_albumAccessor.Get(id));
            vm.IsInLibrary = _userAlbumAccessor.Exists(id, userId);
            vm.Tracks = vm.Tracks.OrderBy(x => x.TrackNumber).ToList();
            return vm;
        }

        public AlbumsViewModel GetAll(string userId)
        {
            var albums = Mapper.Map<List<AlbumViewModel>>(_albumAccessor.GetAll());
            foreach (var album in albums)
            {
                album.IsInLibrary = _userAlbumAccessor.Exists(album.Id, userId);
            }

            return new AlbumsViewModel { Albums = albums, IsUserLibrary = false };
        }

        public AlbumsViewModel GetAllForUser(string userId)
        {
            var albums = Mapper.Map<List<AlbumViewModel>>(_userAlbumAccessor.GetAllByUser(userId));
            albums.ForEach(x => x.IsInLibrary = true);
            return new AlbumsViewModel { Albums = albums, IsUserLibrary = true };
        }

        public void AddToLibrary(int id, string userId)
        {
            _albumAccessor.Get(id);
            _userAlbumAccessor.Add(id, userId);
        }

        public void RemoveFromLibrary(int id, string userId)
        {
            _userAlbumAccessor.Remove(id, userId);
        }
    }
}
