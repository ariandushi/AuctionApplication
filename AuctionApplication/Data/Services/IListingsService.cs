using AuctionApplication.Models;

namespace AuctionApplication.Data.Services
{
    public interface IListingsService
    {
        IQueryable<Listing> getAll();
        Task Add(Listing listing);
        Task<Listing> getById(int? id);
    }
}
