using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{

    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }

        IRepository<Video> Videos { get; }

        void Save();
    }
}
