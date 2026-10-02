using DotNetFlicks.Accessors.Models.DTO;
using System.Collections.Generic;

namespace DotNetFlicks.Accessors.Interfaces
{
    public interface IUserAlbumAccessor
    {
        bool Exists(int albumId, string userId);

        List<AlbumDTO> GetAllByUser(string userId);

        void Add(int albumId, string userId);

        void Remove(int albumId, string userId);
    }
}
