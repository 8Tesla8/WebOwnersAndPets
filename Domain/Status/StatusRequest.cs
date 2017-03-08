using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Status
{
    public enum StatusRequest : int
    {
        Null,
        Ok,
        Created,
        Found,
        NotFound,
        Updated,
        Deleted,
        NotChanged,
        BadRequest,
    }
}
