using UrlShortenerApp.Shared;

namespace UrlShortenerAppTest.ServiceTests
{
    [TestClass]
    public class ErrorMessageServiceTest
    {
        private IErrorMessageService errorMessages;

        [TestInitialize]
        public void Initialize()
        {
            errorMessages = new ErrorMessageService();
        }

        [TestMethod]
        public void GetErrorMessage_GivenRegisteredKey_ReturnMessageTemplate()
        {
            var expectedMessage = "{0} format is invalid. {0} = {1}";
            var res = errorMessages.GetErrorMessage(SharedConstants.ERROR_INVALID_FORMAT);

            Assert.AreEqual(expectedMessage, res);
        }

        [TestMethod]
        public void GetErrorMessage_NotRegisteredKey_ReturnNull()
        {
            var res = errorMessages.GetErrorMessage("THIS_IS_NOT_REGISTERED_KEY");

            Assert.IsNull(res);
        }
    }
}
