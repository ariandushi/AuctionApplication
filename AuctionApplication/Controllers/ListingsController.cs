﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionApplication.Data;
using AuctionApplication.Models;
using AuctionApplication.Data.Services;

namespace AuctionApplication.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingsService _listingsService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ListingsController(IListingsService listingsService, IWebHostEnvironment webHostEnvironment)
        {
            _listingsService = listingsService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Listings
        public async Task<IActionResult> Index(int? pageNumber, string searchStr)
        {
            var applicationDbContext = _listingsService.getAll();

            int pageSize = 3;
            if (!string.IsNullOrEmpty(searchStr))
            {
                applicationDbContext = applicationDbContext.Where(a => a.Title.Contains(searchStr));
            }
            return View(await ListPaginator<Listing>.CreateAsync(applicationDbContext.Where(l => l.isSold == false).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingViewModel listing)
        {

           if(listing.Image != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string fileName = listing.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    listing.Image.CopyTo(fileStream);
                }

                var listObj = new Listing
                {
                    Title = listing.Title,
                    Description = listing.Description,
                    Price = listing.Price,
                    IdentityUserId = listing.IdentityUserId,
                    imgPath = "/Images/"+fileName,
                };
                await _listingsService.Add(listObj);
                return RedirectToAction("Index");
            }
           return View(listing);
        }

          public async Task<IActionResult> Details(int? id)
          {
              if (id == null)
              {
                  return NotFound();
              }

              var listing = await _listingsService.getById(id);
              if (listing == null)
              {
                  return NotFound();
              }

              return View(listing);
          }




        /*      // GET: Listings/Edit/5
              public async Task<IActionResult> Edit(int? id)
              {
                  if (id == null || _context.Listings == null)
                  {
                      return NotFound();
                  }

                  var listing = await _context.Listings.FindAsync(id);
                  if (listing == null)
                  {
                      return NotFound();
                  }
                  ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
                  return View(listing);
              }

              // POST: Listings/Edit/5
              // To protect from overposting attacks, enable the specific properties you want to bind to.
              // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
              [HttpPost]
              [ValidateAntiForgeryToken]
              public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,imgPath,isSold,IdentityUserId")] Listing listing)
              {
                  if (id != listing.Id)
                  {
                      return NotFound();
                  }

                  if (ModelState.IsValid)
                  {
                      try
                      {
                          _context.Update(listing);
                          await _context.SaveChangesAsync();
                      }
                      catch (DbUpdateConcurrencyException)
                      {
                          if (!ListingExists(listing.Id))
                          {
                              return NotFound();
                          }
                          else
                          {
                              throw;
                          }
                      }
                      return RedirectToAction(nameof(Index));
                  }
                  ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
                  return View(listing);
              }

              // GET: Listings/Delete/5
              public async Task<IActionResult> Delete(int? id)
              {
                  if (id == null || _context.Listings == null)
                  {
                      return NotFound();
                  }

                  var listing = await _context.Listings
                      .Include(l => l.User)
                      .FirstOrDefaultAsync(m => m.Id == id);
                  if (listing == null)
                  {
                      return NotFound();
                  }

                  return View(listing);
              }

              // POST: Listings/Delete/5
              [HttpPost, ActionName("Delete")]
              [ValidateAntiForgeryToken]
              public async Task<IActionResult> DeleteConfirmed(int id)
              {
                  if (_context.Listings == null)
                  {
                      return Problem("Entity set 'ApplicationDbContext.Listings'  is null.");
                  }
                  var listing = await _context.Listings.FindAsync(id);
                  if (listing != null)
                  {
                      _context.Listings.Remove(listing);
                  }

                  await _context.SaveChangesAsync();
                  return RedirectToAction(nameof(Index));
              }
      */
        //private bool ListingExists(int id)
        //{
        //  return (_context.Listings?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
