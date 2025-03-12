using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Get a repository for a specific entity type
         Task<TRepository> GetRepository<T, TRepository>(Func<TRepository> repositoryFactory) where TRepository : class where T : class; 

        // "Commit" changes (in-memory)
        Task CommitAsync();
    }
}
