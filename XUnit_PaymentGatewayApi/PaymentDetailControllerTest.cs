using System;
using Xunit;
using Moq;
using paymentGatewayApi.Controllers;
using paymentGatewayApi.Models;
using paymentGatewayApi.Functions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XUnit_PaymentGatewayApi
{
    public class PaymentDetailControlleTest
    {
        private PaymentDetail BuildPaymentSample(string pm, string cn, string ct, string expD, string cvv, string mid, decimal tm, string crcy, string ps, bool isEncrypted)
        {
            PaymentDetail aPayment = new PaymentDetail
            {
                Id = new Guid(),
                paymentMethod = pm,
                cardType = ct,
                expDate = expD,
                cvv = cvv,
                merchantIdentifier = mid,
                totalAmount = tm,
                currency = crcy,
                paymentState = ps
            };
            if (isEncrypted)
                aPayment.cardNumber = new ControllerFunctions().encryptCard(cn);
            else
                aPayment.cardNumber = cn;

            return aPayment;
        }

        [Fact]
        public void GetpaymentDetailsMid_test()
        {
            var options = new DbContextOptionsBuilder<PaymentDetailContext>()
            .UseInMemoryDatabase(databaseName: "PaymentDetailListDatabase")
            .Options;
            using (var context = new PaymentDetailContext(options))
            {
                context.paymentDetails.Add(BuildPaymentSample("credit-card", "4567042172880411", "visa", "12/2023", "766", "appleInc12345", 200, "EUR", "Successful", true));
                context.paymentDetails.Add(BuildPaymentSample("credit-card", "4567042172880412", "visa", "07/2024", "123", "fbInc12345", 300, "EUR", "Successful", true));
                context.SaveChanges();
            }

            using (var context = new PaymentDetailContext(options))
            {
                var logger = Mock.Of<ILogger<PaymentDetailController>>();
                logger.LogInformation("*********Controller Test: GetpaymentDetailsMid BEGIN*********");
                PaymentDetailController _controller = new PaymentDetailController(logger, context);
                var response = _controller.GetpaymentDetailsMid("appleInc12345");
                logger.LogInformation("Response received: " + response);
                Assert.IsType<Task<ActionResult<IEnumerable<PaymentDetail>>>>(response);
                List<PaymentDetail> paymentList = (List<PaymentDetail>) response.Result.Value;
                logger.LogInformation("Payment list size is: " + paymentList.Count);
                Assert.True(paymentList.Count > 0);
                logger.LogInformation("Assertion passed. Test OK");
                logger.LogInformation("*********Controller Test: GetpaymentDetailsMid END*********");
            }
        }

        [Fact]
        public void GetpaymentDetailsMid_NoContent_test()
        {
            var options = new DbContextOptionsBuilder<PaymentDetailContext>()
            .UseInMemoryDatabase(databaseName: "PaymentDetailListDatabase")
            .Options;

            using (var context = new PaymentDetailContext(options))
            {
                var logger = Mock.Of<ILogger<PaymentDetailController>>();
                logger.LogInformation("*********Controller Test: GetpaymentDetailsMid_NoContent BEGIN*********");
                PaymentDetailController _controller = new PaymentDetailController(logger, context);
                var response = _controller.GetpaymentDetailsMid("appleInc12345");
                logger.LogInformation("Response received: " + response);
                Assert.Equal(204, (response.Result.Result as NoContentResult).StatusCode);
                logger.LogInformation("Assertion passed. Test OK");
                logger.LogInformation("*********Controller Test: GetpaymentDetailsMid_NoContent END*********");
            }
        }

        [Fact]
        public void PostPaymentDetail_test()
        {
            var options = new DbContextOptionsBuilder<PaymentDetailContext>()
            .UseInMemoryDatabase(databaseName: "PaymentDetailListDatabase")
            .Options;

            using (var context = new PaymentDetailContext(options))
            {
                var logger = Mock.Of<ILogger<PaymentDetailController>>();
                logger.LogInformation("*********Controller Test: PostPaymentDetail BEGIN*********");
                PaymentDetailController _controller = new PaymentDetailController(logger, context);
                logger.LogInformation("Building payment sample to pass...");
                var response = _controller.PostPaymentDetail(BuildPaymentSample("credit-card", "4512042172880411", "visa", "12/2023", "766", "appleInc12345", 200, "EUR", "", false));
                logger.LogInformation("Build successful, reponse received from PostPaymentDetail: " + response);
                var result = response.Result.Result as OkObjectResult;
                Assert.Equal(200, result.StatusCode); 
                Assert.Equal("Payment successful", result.Value);
                logger.LogInformation("Assertion passed. Test OK");
                logger.LogInformation("*********Controller Test: PostPaymentDetail END*********");
            }
        }

        [Fact]
        public void PostPaymentDetail_InvalidBankResponse_test()
        {
            var options = new DbContextOptionsBuilder<PaymentDetailContext>()
            .UseInMemoryDatabase(databaseName: "PaymentDetailListDatabase")
            .Options;

            using (var context = new PaymentDetailContext(options))
            {
                var logger = Mock.Of<ILogger<PaymentDetailController>>();
                logger.LogInformation("*********Controller Test: PostPaymentDetail_InvalidBankResponse BEGIN*********");

                PaymentDetailController _controller = new PaymentDetailController(logger, context);

                logger.LogInformation("Building payment sample to pass for invalid payment method...");
                var response = _controller.PostPaymentDetail(BuildPaymentSample("debit-card", "4567042172880411", "visa", "12/2023", "766", "appleInc12345", 200, "EUR", "", false));
                logger.LogInformation("Build successful, reponse received from PostPaymentDetail: " + response);
                var result = response.Result.Result as BadRequestObjectResult;
                Assert.Equal(400, result.StatusCode);
                Assert.Equal("Invalid payment method.", result.Value);
                logger.LogInformation("Assertion passed. Test OK");

                logger.LogInformation("*********Controller Test: PostPaymentDetail_InvalidBankResponse END*********");
            }
        }
    }
}
