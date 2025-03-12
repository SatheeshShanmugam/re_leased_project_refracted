using RefactorThis.Persistence.Interfaces;
using RefactorThis.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public TRepository GetRepository<T, TRepository>(Func<TRepository> repositoryFactory) where T : class where TRepository : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                
                _repositories.Add(typeof(T), repositoryFactory());                
            }
            return _repositories[typeof(T)] as TRepository;            
        }       

        public Task CommitAsync()
        {
            // Perform any actions needed to "commit" changes (e.g., logging, validation)
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Clean up resources if needed
        }
    }
}
