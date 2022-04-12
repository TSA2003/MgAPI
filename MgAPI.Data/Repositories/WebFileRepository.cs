using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MgAPI.Data.Entities;

namespace MgAPI.Data.Repositories
{
    public class WebFileRepository : BaseRepository<WebFile>
    {
        public WebFileRepository(Context context)
            : base(context)
        {

        }
    }
}
