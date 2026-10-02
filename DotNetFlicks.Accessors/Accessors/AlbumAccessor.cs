using AutoMapper;
using DotNetFlicks.Accessors.Accessors.Base;
using DotNetFlicks.Accessors.Database;
using DotNetFlicks.Accessors.Interfaces;
using DotNetFlicks.Accessors.Models.DTO;
using DotNetFlicks.Accessors.Models.EF.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DotNetFlicks.Accessors.Accessors
{
    public class AlbumAccessor : EntityAccessor<Entity>, IAlbumAccessor
    {
        public AlbumAccessor(DotNetFlicksContext db) : base(db)
        {
        }

        public AlbumDTO Get(int id)
        {
            var entity = _db.Albums
                .AsNoTracking()
                .Include(x => x.Tracks)
                .Single(x => x.Id == id);

            return Mapper.Map<AlbumDTO>(entity);
        }

        public List<AlbumDTO> GetAll()
        {
            var entities = _db.Albums
                .AsNoTracking()
                .Include(x => x.Tracks)
                .OrderBy(x => x.Artist)
                .ThenBy(x => x.Title)
                .ToList();

            return Mapper.Map<List<AlbumDTO>>(entities);
        }
    }
}
