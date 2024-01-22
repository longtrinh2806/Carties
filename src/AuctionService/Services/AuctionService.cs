using AuctionService.Data;
using AuctionService.Dtos;
using AuctionService.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Services
{
    public interface IAuctionService
    {
        Task<ResultModel> Delete(Guid id);
        Task<ResultModel> Get();
        Task<ResultModel> Get(Guid id);
        Task<ResultModel> Post(CreateAuctionDto request);
        Task<ResultModel> Put(Guid id, UpdateAuctionDto request);
    }
    public class AuctionsService : IAuctionService
    {
        private readonly AuctionDbContext _context;

        public AuctionsService(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel> Get()
        {
            var auctions = await _context.Auctions
                .Include(x => x.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();

            List<AuctionDto> result = new();
            
            foreach (var auction in auctions)
            {
                var auctionDto = new AuctionDto
                {
                    Id = auction.Id,
                    CreatedAt = auction.CreatedAt,
                    UpdatedAt = auction.UpdatedAt,
                    AuctionEnd = auction.AuctionEnd,
                    Seller = auction.Seller,
                    Winner = auction.Winner,
                    Make = auction.Item.Make,
                    Model = auction.Item.Model,
                    Year = auction.Item.Year,
                    Color = auction.Item.Color,
                    Mileage = auction.Item.Mileage,
                    ImageUrl = auction.Item.ImageUrl,
                    Status = auction.Status.ToString()
                };
                result.Add(auctionDto);
            }

            return new ResultModel
            {
                Data = result,
                IsSucceed = true,
                Message = "Get Succesfully"
            };
        }

        public async Task<ResultModel> Get(Guid id)
        {
            var auction = await _context.Auctions
                .Include (x => x.Item)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (auction == null)
                return new ResultModel
                {
                    IsSucceed = false,
                    Message = "Data Not Found"
                };
            
            var auctionDto = auction.Adapt<AuctionDto>();

            return new ResultModel
            {
                Data = auctionDto,
                IsSucceed = true,
                Message = "Get Successfully"
            };

        }

        public async Task<ResultModel> Post(CreateAuctionDto request)
        {
            var auction = new Auction
            {
                ReservePrice = request.ReservePrice,
                Seller = "test",
                AuctionEnd = request.AuctionEnd,
                Item = new Item
                {
                    Make = request.Make,
                    Model = request.Model,
                    Year = request.Year,
                    Color = request.Color,
                    Mileage = request.Mileage,
                    ImageUrl = request.ImageUrl
                }
            };

            // TODO: add current user as seller

            _context.Auctions.Add(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return new ResultModel
                {
                    IsSucceed = false,
                    Message = "Could not save changes to the DB"
                };
            return new ResultModel
            {
                IsSucceed = true,
                Message = "Create Successfully"
            };
        }

        public async Task<ResultModel> Put(Guid id, UpdateAuctionDto request)
        {
            var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (auction == null)
                return new ResultModel
                {
                    IsSucceed = false,
                    Message = "Auction not existed"
                };

            auction.Item.Make = request.Make ?? auction.Item.Make;
            auction.Item.Model = request.Model ?? auction.Item.Model;
            auction.Item.Color = request.Color ?? auction.Item.Color;
            auction.Item.Mileage = request.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = request.Year ?? auction.Item.Year;

            _context.Auctions.Update(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return new ResultModel
                {
                    IsSucceed = false,
                    Message = "Can not save changes"
                };
            return new ResultModel
            {
                IsSucceed = true,
                Message = "Updated Successfully"
            };
        }

        public async Task<ResultModel> Delete(Guid id)
        {
            var auction = await _context.Auctions.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (auction == null)
            {
                return new ResultModel
                {
                    IsSucceed = false,
                    Message = "auction not existed"
                };
            }

            _context.Auctions.Remove(auction);

            var result = await _context.SaveChangesAsync() > 0;
            if (!result)
                return new ResultModel
                {
                    IsSucceed = false,
                    Message = "Can not save changes"
                };
            return new ResultModel
            {
                IsSucceed = true,
                Message = "Deleted Successfully"
            };

        }

    }
}
