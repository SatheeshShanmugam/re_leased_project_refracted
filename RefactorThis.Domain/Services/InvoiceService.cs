using System;
using System.Linq;
using System.Threading.Tasks;
using RefactorThis.Domain.Interfaces;
using RefactorThis.Domain.Models;
using RefactorThis.Domain.Services;
using RefactorThis.Domain.Utility;
using RefactorThis.Persistence;
using RefactorThis.Persistence.Interfaces;

namespace RefactorThis.Domain
{
	public class InvoiceService : BaseService, IInvoiceService
	{
		public InvoiceService(IUnitOfWork unitOfWork) : base(unitOfWork)
		{			
		}


		/// <summary>
		/// This method is used to process the current payment against invoice amount
		/// </summary>
		/// <param name="payment"></param>
		/// <returns>ResponseModel which contains the process/exception message</returns>

		public async Task<ResponseModel> ProcessPayment( Payment payment )
		{
			var responseMessage = string.Empty;
			try
            {
				IinvoiceRespository _invoiceRepository = _unitOfWork.GetRepository<Invoice,InvoiceRepository>(()=>new InvoiceRepository());

				if (_invoiceRepository == null) 
					return new ResponseModel(ResponseMessages.Contact_System_Admin);

				Invoice invoice = await _invoiceRepository.GetInvoice(payment.Reference, new System.Threading.CancellationToken());

				if (invoice is null) 
					return new ResponseModel(ResponseMessages.No_Invoice_Matching_Current_Payment);

				bool hasPayments = !(invoice.Payments==null || !invoice.Payments.Any());

				if (!hasPayments && invoice.Amount == 0)
					return new ResponseModel(ResponseMessages.No_Payment_Needed);

				if (hasPayments && invoice.Amount == 0)
					return new ResponseModel(ResponseMessages.Invalid_Invoice_With_Payments);

				if (hasPayments && invoice.Amount == invoice.Payments.Sum(x => x.Amount))
						return new ResponseModel(ResponseMessages.Invoice_Was_Already_Fully_Paid);

				

				if (payment.Amount > (invoice.Amount - invoice.AmountPaid))
				{
					responseMessage = hasPayments
					? ResponseMessages.Payment_Is_Greater_Than_Partial_Amount_Remaining
					: ResponseMessages.Payment_Is_Greater_Than_Invoice_Amount;
					return new ResponseModel(responseMessage);
				}

				 if (payment.Amount==(invoice.Amount - invoice.AmountPaid))  
                {
					responseMessage = hasPayments
					? ResponseMessages.Final_Partial_Payment_Received
					: ResponseMessages.Invoice_Is_Now_Fully_Paid;
				}
                else
                {	
					responseMessage = hasPayments
						? ResponseMessages.Another_Partial_Payment_Received
						: ResponseMessages.Invoice_is_partially_paid;
				}


				switch (invoice.Type)
				{
					case InvoiceType.Standard:
						invoice.TaxAmount += hasPayments ? 0 : payment.Amount * 0.14m;
						break;
					case InvoiceType.Commercial:
						invoice.TaxAmount += payment.Amount * 0.14m;
						break;
					default:
						return new ResponseModel(ResponseMessages.Invalid_Invoice_Type);
				}

				invoice.AmountPaid += payment.Amount;
				invoice.Payments.Add(payment);

				await _invoiceRepository.SaveInvoice(invoice, new System.Threading.CancellationToken());
				await _unitOfWork.CommitAsync();
				return new ResponseModel(responseMessage); 
			}
			catch(Exception ex)
            {
				return new ResponseModel(ResponseMessages.Contact_System_Admin);
			}
		}

		

		
	}
}