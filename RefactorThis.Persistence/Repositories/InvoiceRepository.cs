using RefactorThis.Persistence.Interfaces;
using RefactorThis.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace RefactorThis.Persistence {
	public class InvoiceRepository : GenericRepository<Invoice>, IinvoiceRespository
	{		
		public async Task<Invoice> GetInvoice( string reference, CancellationToken cancellationToken)
		{
			return await GetByIdAsync(reference, cancellationToken);
			
		}
		public async Task SaveInvoice( Invoice invoice, CancellationToken cancellationToken)
		{
			 await UpdateAsync(invoice, cancellationToken);			
		}

		public async Task Add( Invoice invoice, CancellationToken cancellationToken)
		{	
			await AddAsync(invoice, cancellationToken);
		}
	}
}