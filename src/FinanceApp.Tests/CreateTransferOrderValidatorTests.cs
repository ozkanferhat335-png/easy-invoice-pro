using FinanceApp.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinanceApp.Tests
{
    [TestClass]
    public class CreateTransferOrderValidatorTests
    {
        [TestMethod]
        public void Should_Fail_When_Amount_Is_Zero()
        {
            var validator = new CreateTransferOrderValidator();
            var result = validator.Validate(new CreateTransferOrderCommand { ReferenceNo = "REF-1", SourceIban = "TR000000000000000000000000", DestinationIban = "TR111111111111111111111111", Amount = 0, CurrencyCode = "TRY" });
            Assert.IsFalse(result.IsValid);
        }
    }
}
