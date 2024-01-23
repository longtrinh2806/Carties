using AuctionService.Data;
using AuctionService.Dtos;
using AuctionService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Controllers
{
    [Route("api/auctions")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuctions([FromQuery] string? date)
        {
            var result = await _auctionService.Get(date);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuctionById(Guid id)
        {
            var result = await _auctionService.Get(id);

            if (!result.IsSucceed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuction(CreateAuctionDto request)
        {
            var result = await _auctionService.Post(request);

            if (!result.IsSucceed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuctionById(Guid id, UpdateAuctionDto request)
        {
            var result = await _auctionService.Put(id, request);

            if (!result.IsSucceed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(Guid id)
        {
            var result = await _auctionService.Delete(id);

            if (!result.IsSucceed)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
