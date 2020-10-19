using DotNetFlicks.Accessors.Identity;
using DotNetFlicks.Accessors.Models.EF.Base;
using System;

namespace DotNetFlicks.Accessors.Models.EF
{
    public class UserSearch : Entity
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string SearchTerm { get; set; }
    }
}