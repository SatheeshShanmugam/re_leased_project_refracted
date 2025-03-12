using Moq;
using NUnit.Framework;
using RefactorThis.Persistence;
using RefactorThis.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Tests.Repositories
{
    class UnitOfWorkRepositoriesTests
    {
        private Mock<UnitOfWork> _mockUnitOfWork;

        [SetUp]
        public void SetUp()
        {
            // setup
            _mockUnitOfWork = new Mock<UnitOfWork>();
        }
        [Test]
        public async Task GetRepository_method_test()
        {
            var InvoiceRepository= await _mockUnitOfWork.Object.GetRepository<Invoice,InvoiceRepository>(() => new InvoiceRepository());          

            Assert.AreEqual(typeof(InvoiceRepository), InvoiceRepository.GetType());            

        }


    }
}
