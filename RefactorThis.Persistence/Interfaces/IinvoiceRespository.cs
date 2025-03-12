using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Persistence.Interfaces
{
    public interface IinvoiceRespository : IGenericRepository<Invoice>
    {
        Task<Invoice> GetInvoice(string reference, CancellationToken cancellationToken);
        Task SaveInvoice(Invoice invoice, CancellationToken cancellationToken);
        Task Add(Invoice invoice, CancellationToken cancellationToken);
    }
}
