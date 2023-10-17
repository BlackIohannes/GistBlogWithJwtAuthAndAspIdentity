using System.Security.Claims;
using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("/api/v1/payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    // private readonly string _userId;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
        // _userId = HttpContext.User.FindFirstValue("Id");
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create Payment", Description = "Requires authorization")]
    [SwaggerResponse(StatusCodes.Status201Created, "Return a Transaction Initialize Response")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
    public async Task<IActionResult> MakePayment(PaymentRequest request)
    {
        var newPayment = await _paymentService.InitializePayment(request);
        if (newPayment is null)
            return BadRequest();

        return Ok(newPayment);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all payment made on the application", Description = "Requires authorization")]
    [SwaggerResponse(StatusCodes.Status200OK, "Return a list of payments")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
    public async Task<IActionResult> GetAllTransaction()
    {
        var result = await _paymentService.GetPayments();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get Payment by id", Description = "Requires authorization")]
    [SwaggerResponse(StatusCodes.Status200OK, "Return a single payment")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
    public async Task<IActionResult> GetTransactionById(string id)
    {
        var result = await _paymentService.GetPaymentById(id);
        if (result == null) return NotFound($"Transaction with id: {id} not found");
        return Ok(result);
    }

    [HttpPut("verify-payment")]
    [SwaggerOperation(Summary = "Verify Payment by id", Description = "Requires authorization")]
    [SwaggerResponse(StatusCodes.Status201Created, "Return a Transaction Verify Response")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
    public async Task<IActionResult> VerifyPayment(VerifyPaymentRequest reference)
    {
        var result = await _paymentService.VerifyPayment(reference);
        if (result == null) return BadRequest("Payment verification was unable to be completed");
        return Ok(result);
    }
}
