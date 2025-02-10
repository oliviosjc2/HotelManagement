using HM.API.Utils;
using HM.Application.Commands.Hotel;
using HM.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "SYS_ADMIN")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateHotelCommand command, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "HOTEL_ADMIN")]
        [HttpPost("add-employee")]
        public async Task<IActionResult> AddEmployee([FromBody] AddHotelEmployeeCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "HOTEL_ADMIN")]
        [HttpPost("suite-category")]
        public async Task<IActionResult> AddSuiteCategory([FromBody] CreateSuiteCategoryCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "HOTEL_ADMIN, HOTEL_EMPLOYEE")]
        [HttpPost("suite")]
        public async Task<IActionResult> AddSuite([FromBody] CreateSuiteCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "FINAL_CUSTOMER")]
        [HttpPost("reserve")]
        public async Task<IActionResult> CreateReserve([FromBody] CreateReserveCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "HOTEL_ADMIN, SYS_ADMIN")]
        [HttpGet("reserves")]
        public async Task<IActionResult> GetAllReserves([FromQuery] GetAllReservesQuery query, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(query, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "HOTEL_ADMIN")]
        [HttpGet("invoices-detailed")]
        public async Task<IActionResult> GetInvoicesDetailed([FromQuery] GetInvoicesDetailsQuery query, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(query, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}