using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Get an entity by ID
        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);

        // Add a new entity
        Task AddAsync(T entity, CancellationToken cancellationToken);

        // Update an entity
        Task UpdateAsync(T entity, CancellationToken cancellationToken);

    }
}
