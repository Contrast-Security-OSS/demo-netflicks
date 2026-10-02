using AutoMapper;
using DotNetFlicks.Accessors.Accessors.Base;
using DotNetFlicks.Accessors.Database;
using DotNetFlicks.Accessors.Interfaces;
using DotNetFlicks.Accessors.Models.DTO;
using DotNetFlicks.Accessors.Models.EF;
using DotNetFlicks.Accessors.Models.EF.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetFlicks.Accessors.Accessors
{
    public class UserAlbumAccessor : EntityAccessor<Entity>, IUserAlbumAccessor
    {
        public UserAlbumAccessor(DotNetFlicksContext db) : base(db)
        {
        }

        public bool Exists(int albumId, string userId)
        {
            return _db.UserAlbums.Any(x => x.AlbumId == albumId && x.UserId == userId);
        }

        public List<AlbumDTO> GetAllByUser(string userId)
        {
            var albums = _db.UserAlbums
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.Album).ThenInclude(x => x.Tracks)
                .OrderByDescending(x => x.AddedDate)
                .Select(x => x.Album)
                .ToList();

            return Mapper.Map<List<AlbumDTO>>(albums);
        }

        public void Add(int albumId, string userId)
        {
            if (Exists(albumId, userId))
            {
                return;
            }

            _db.UserAlbums.Add(new UserAlbum
            {
                AlbumId = albumId,
                UserId = userId,
                AddedDate = DateTime.UtcNow
            });
            _db.SaveChanges();
        }

        public void Remove(int albumId, string userId)
        {
            var entity = _db.UserAlbums.SingleOrDefault(x => x.AlbumId == albumId && x.UserId == userId);
            if (entity == null)
            {
                return;
            }

            _db.UserAlbums.Remove(entity);
            _db.SaveChanges();
        }
    }
}
