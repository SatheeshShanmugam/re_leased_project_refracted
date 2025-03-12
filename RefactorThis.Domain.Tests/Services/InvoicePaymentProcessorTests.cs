using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RefactorThis.Domain.Interfaces;
using RefactorThis.Domain.Utility;
using RefactorThis.Persistence;
using RefactorThis.Persistence.Interfaces;
using RefactorThis.Persistence.UnitOfWork;

namespace RefactorThis.Domain.Tests
{
	[TestFixture]
	public class Invoice_invoiceServiceTests
	{

        private Mock<UnitOfWork> _unitOfWork;
        private Mock<InvoiceService> _invoiceService;       
        private IinvoiceRespository _InvoiceRepository;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<UnitOfWork>();
            _invoiceService = new Mock<InvoiceService>(_unitOfWork.Object);
            _InvoiceRepository =  _unitOfWork.Object.GetRepository<Invoice, InvoiceRepository>(() => new InvoiceRepository()).Result;
        }
        public static IEnumerable<TestCaseData> PaymentTestCases()
        {
            yield return new TestCaseData(new Payment(), null, ResponseMessages.No_Invoice_Matching_Current_Payment);
            yield return new TestCaseData(new Payment(), new Invoice { Amount = 0, AmountPaid = 0, Payments = null }, ResponseMessages.No_Payment_Needed);
            yield return new TestCaseData(new Payment(), new Invoice { Amount = 10, AmountPaid = 10, Payments = new List<Payment> { new Payment { Amount = 10 } } }, ResponseMessages.Invoice_Was_Already_Fully_Paid);
            yield return new TestCaseData(new Payment { Amount = 6 }, new Invoice { Amount = 10, AmountPaid = 5, Payments = new List<Payment> { new Payment { Amount = 5 } } }, ResponseMessages.Payment_Is_Greater_Than_Partial_Amount_Remaining);
            yield return new TestCaseData(new Payment { Amount = 6 }, new Invoice { Amount = 5, AmountPaid = 0, Payments = new List<Payment>() }, ResponseMessages.Payment_Is_Greater_Than_Invoice_Amount);
            yield return new TestCaseData(new Payment { Amount = 5 }, new Invoice { Amount = 10, AmountPaid = 5, Payments = new List<Payment> { new Payment { Amount = 5 } } }, ResponseMessages.Final_Partial_Payment_Received);
            yield return new TestCaseData(new Payment { Amount = 10 }, new Invoice { Amount = 10, AmountPaid = 0, Payments = new List<Payment> { new Payment { Amount = 10 } } }, ResponseMessages.Invoice_Was_Already_Fully_Paid);
            yield return new TestCaseData(new Payment { Amount = 1 }, new Invoice { Amount = 10, AmountPaid = 5, Payments = new List<Payment> { new Payment { Amount = 5 } } }, ResponseMessages.Another_Partial_Payment_Received);
            yield return new TestCaseData(new Payment { Amount = 1 }, new Invoice { Amount = 10, AmountPaid = 0, Payments = new List<Payment>() }, ResponseMessages.Invoice_is_partially_paid);

        }

        [Test, TestCaseSource(nameof(PaymentTestCases))]
        public async Task ProcessPayment_Should_ReturnExpectedMessage(Payment payment, Invoice invoice, string expectedMessage)
        {
            if (invoice != null)
                await _InvoiceRepository.Add(invoice, CancellationToken.None);

            var result = await _invoiceService.Object.ProcessPayment(payment);

            Assert.AreEqual(expectedMessage, result.Message);
        }

    }
}