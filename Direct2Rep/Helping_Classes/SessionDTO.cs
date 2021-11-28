using Direct2Rep.BL;
using Direct2Rep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace Direct2Rep.Helping_Classes
{
    public class SessionDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Role { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }


        public string getLogo()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var sdto = identity.Claims.Where(c => c.Type == "Logo")
                  .Select(c => c.Value).SingleOrDefault();
            if (sdto == null)
                return null;

            return sdto;
        }

        public string getName()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var sdto = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                  .Select(c => c.Value).SingleOrDefault();
            if (sdto == null)
                return null;

            return sdto;
        }

        public int getId()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var sdto = identity.Claims.Where(c => c.Type == "Id")
                  .Select(c => c.Value).SingleOrDefault();
            if (sdto == null)
                return -1;

            return Convert.ToInt32(sdto);
        }

        public int getRole()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var sdto = identity.Claims.Where(c => c.Type == "Role")
                  .Select(c => c.Value).SingleOrDefault();
            if (sdto == null)
                return -1;

            return Convert.ToInt32(sdto);
        }

        public string getEmail()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var sdto = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                  .Select(c => c.Value).SingleOrDefault();
            if (sdto == null)
                return null;

            return sdto;
        }

    }
}