using DotNetFlicks.Accessors.Identity;
using DotNetFlicks.Accessors.Models.EF.Base;
using System;

namespace DotNetFlicks.Accessors.Models.EF
{
    public class UserAlbum : Entity
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
