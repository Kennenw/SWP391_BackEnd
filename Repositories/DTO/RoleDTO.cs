using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public partial class RoleDTO
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public bool? Status { get; set; }

    }
}
