using System;
using System.Linq;
using System.Threading;
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
		public async Task<ResponseModel> ProcessPayment(Payment payment)
		{
			try
			{
				// Retrieve the invoice repository
				var invoiceRepository = await _unitOfWork.GetRepository<Invoice, InvoiceRepository>(() => new InvoiceRepository());
				if (invoiceRepository == null)
					return new ResponseModel(ResponseMessages.Contact_System_Admin);

				// Fetch the invoice using the payment reference
				var invoice = await invoiceRepository.GetInvoice(payment.Reference, CancellationToken.None);
				if (invoice == null)
					return new ResponseModel(ResponseMessages.No_Invoice_Matching_Current_Payment);

				// Validate invoice and payment conditions
				bool isValid;
				var responseMessage = ValidateInvoiceAndPayment(invoice, payment, out isValid);
				if (!isValid)
					return new ResponseModel(responseMessage);

				// Update invoice based on payment
				UpdateInvoice(invoice, payment);

				// Save the updated invoice
				await invoiceRepository.SaveInvoice(invoice, CancellationToken.None);
				await _unitOfWork.CommitAsync();

				return new ResponseModel(responseMessage);
			}
			catch (Exception ex)
			{
				// Log the exception (consider using a logging framework)
				return new ResponseModel(ResponseMessages.Contact_System_Admin);
			}
		}

		private string ValidateInvoiceAndPayment(Invoice invoice, Payment payment, out bool isValid)
		{
			bool hasPayments = invoice.Payments != null && invoice.Payments.Any();

			isValid = false;

			//Invalid conditions
			if (!hasPayments && invoice.Amount == 0)
				return ResponseMessages.No_Payment_Needed;

			if (hasPayments && invoice.Amount == 0)
				return ResponseMessages.Invalid_Invoice_With_Payments;

			if (hasPayments && invoice.Amount == invoice.Payments.Sum(x => x.Amount))
				return ResponseMessages.Invoice_Was_Already_Fully_Paid;

			if (payment.Amount > (invoice.Amount - invoice.AmountPaid))
			{
				return hasPayments
					? ResponseMessages.Payment_Is_Greater_Than_Partial_Amount_Remaining
					: ResponseMessages.Payment_Is_Greater_Than_Invoice_Amount;
			}

			//valid conditions
			isValid = true;
			if (payment.Amount < (invoice.Amount - invoice.AmountPaid))
			{
				
				return hasPayments
				? ResponseMessages.Another_Partial_Payment_Received
				: ResponseMessages.Invoice_is_partially_paid;
				
			}

			return hasPayments
					? ResponseMessages.Final_Partial_Payment_Received
					: ResponseMessages.Invoice_Is_Now_Fully_Paid;


		}

		private void UpdateInvoice(Invoice invoice, Payment payment)
		{
			bool hasPayments = invoice.Payments != null && invoice.Payments.Any();

			switch (invoice.Type)
			{
				case InvoiceType.Standard:
					invoice.TaxAmount += hasPayments ? 0 : payment.Amount * 0.14m;
					break;
				case InvoiceType.Commercial:
					invoice.TaxAmount += payment.Amount * 0.14m;
					break;
				default:
					throw new InvalidOperationException(ResponseMessages.Invalid_Invoice_Type);
			}

			invoice.AmountPaid += payment.Amount;
			invoice.Payments.Add(payment);
		}
	}
}