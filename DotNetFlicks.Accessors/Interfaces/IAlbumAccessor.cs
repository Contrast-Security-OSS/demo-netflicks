using DotNetFlicks.Accessors.Models.DTO;
using System.Collections.Generic;

namespace DotNetFlicks.Accessors.Interfaces
{
    public interface IAlbumAccessor
    {
        AlbumDTO Get(int id);

        List<AlbumDTO> GetAll();
    }
}
