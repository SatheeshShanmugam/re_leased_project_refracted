using RefactorThis.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private T _entity ;
        
        public virtual async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            
            if (cancellationToken.IsCancellationRequested)
            {
                return await  Task.FromCanceled<T>(cancellationToken); // Return a canceled task
            }
            
            return await Task.FromResult(_entity);
        }
        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                 await Task.FromCanceled(cancellationToken); // Return a canceled task
            }
            _entity = entity;
        }      

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {          

            if (cancellationToken.IsCancellationRequested)
            {
                await Task.FromCanceled(cancellationToken); // Return a canceled task
            }

            //saves the invoice to the database 
           
        }
    }
}
