using MgAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MgAPI.Data.Repositories
{
    public class PostRepository : BaseRepository<Post>
    {
        public PostRepository(Context context) : base(context)
        {
            
        }
    }
}
