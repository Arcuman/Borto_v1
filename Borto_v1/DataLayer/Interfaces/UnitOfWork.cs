using System;

namespace Borto_v1
{

    public interface IUnitOfWork : IDisposable
    {
        UserRepository Users { get; }

        VideoRepository Videos { get; }

        void Save();
    }
}
