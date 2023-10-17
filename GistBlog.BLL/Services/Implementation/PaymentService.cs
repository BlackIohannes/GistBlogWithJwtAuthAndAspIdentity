using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Configurations;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Exceptions;
using GistBlog.DAL.Repository.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PayStack.Net;

namespace GistBlog.BLL.Services.Implementation;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;

    private readonly DataContext _context;

    // private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<ProductTransaction> _transRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly string token;

    public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration, UserManager<AppUser> userManager,
        DataContext context)
    {
        _unitOfWork = unitOfWork;
        _transRepo = _unitOfWork.GetRepository<ProductTransaction>();
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
        // _httpContextAccessor = httpContextAccessor;
        token = _configuration["Payment:PaystackTestKey"];
        PayStack = new PayStackApi(token);
    }

    private PayStackApi PayStack { get; }

    public async Task<TransactionInitializeResponse> InitializePayment(PaymentRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException($"User with id {user} not found");

        TransactionInitializeRequest createRequest = new()
        {
            AmountInKobo = request.Amount * 100,
            Email = user.Email,
            Currency = "NGN",
            Reference = Generate().ToString(),
            CallbackUrl = "https://localhost:7242/api/Payment/verify-payment"
        };
        var response = PayStack.Transactions.Initialize(createRequest);
        if (response.Status)
        {
            var transaction = new ProductTransaction
            {
                Name = user.Fullname,
                Amount = request.Amount,
                TrxnRef = createRequest.Reference,
                Email = user.Email,
                Status = false,
                AppUser = new AppUser()
            };
            // await _transRepo.AddAsync(transaction);
            await _context.ProductTransactions.AddAsync(transaction);
            return response;
        }

        throw new Exception("The payment was unable to go through");
    }

    public async Task<TransactionVerifyResponse> VerifyPayment(VerifyPaymentRequest reference)
    {
        // var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        // if (userId == null)
        //     throw new NotFoundException("Invalid User id please authenticate");

        var response = PayStack.Transactions.Verify(reference.ReferenceCode);
        if (response.Data.Status == "success")
        {
            var transaction = await _transRepo.GetSingleByAsync(x => x.TrxnRef == reference.ReferenceCode);

            if (transaction != null)
            {
                if (!transaction.Status)
                {
                    transaction.Status = true;

                    await _transRepo.UpdateAsync(transaction);

                    return response;
                }

                throw new Exception("You have already verified this payment");
            }
        }

        throw new Exception("Was not able to complete this request");
    }

    public async Task<IEnumerable<ProductTransaction>> GetPayments()
    {
        // var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        // if (userId == null)
        //     throw new NotFoundException("Invalid User id please authenticate");

        var payments = await _transRepo.GetAllAsync();
        if (payments != null) return payments;
        throw new NotFoundException("No transactions");
    }

    public async Task<ProductTransaction> GetPaymentById(string id)
    {
        // var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        // if (userId == null)
        //     throw new NotFoundException("Invalid User id please authenticate");
        var result = await _transRepo.GetByIdAsync(new Guid(id));
        if (result != null) return result;
        throw new NotFoundException("Invalid userid");
    }

    private static int Generate()
    {
        var rand = new Random((int)DateTime.Now.Ticks);
        return rand.Next(100000000, 999999999);
    }
}
