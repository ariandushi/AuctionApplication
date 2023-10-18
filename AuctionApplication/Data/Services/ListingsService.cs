﻿using AuctionApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionApplication.Data.Services
{
    public class ListingsService : IListingsService
    {
        private readonly ApplicationDbContext _context;
        public ListingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Listing listing)
        {
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Listing> getAll()
        {
            var applicationDbContext = _context.Listings.Include(l => l.User);
            return applicationDbContext;
        }
    }
}
