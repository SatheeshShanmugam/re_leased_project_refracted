using RefactorThis.Domain.Models;
using RefactorThis.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Interfaces
{
    public interface IInvoiceService : IBaseService
    {
        Task<ResponseModel> ProcessPayment(Payment payment);
    }
}
