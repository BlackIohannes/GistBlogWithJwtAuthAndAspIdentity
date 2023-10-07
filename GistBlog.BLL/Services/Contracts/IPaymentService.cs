using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models;
using PayStack.Net;

namespace GistBlog.BLL.Services.Contracts;

public interface IPaymentService
{
    Task<TransactionInitializeResponse> InitializePayment(PaymentRequest request);
    Task<TransactionVerifyResponse> VerifyPayment(string reference);
    Task<IEnumerable<ProductTransaction>> GetPayments();
    Task<ProductTransaction> GetPaymentById(string id);
}
