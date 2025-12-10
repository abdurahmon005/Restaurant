using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Models
{
    public class PaginationResult<T>
    {
        public List<T> Values { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public bool HasPrevious
            => PageNumber > 1;

        public bool HasNext
            => PageSize * PageNumber < TotalCount;
    }
}
