using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBusiness.DAL;
using EBusiness.Models;
using EBusiness.DTOs.CardDTOs;
using EBusiness.Extention;

namespace EBusiness.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public readonly IWebHostEnvironment _env;

        public CardsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.Cards.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cards == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CardPostDto cardPostDto)
        {
                await _context.AddAsync(new Card
                {
                    Name = cardPostDto.Name,
                    JobTitle = cardPostDto.JobTitle,
                    Image = cardPostDto.File.CreateFile(_env.WebRootPath, "assets/img")
                });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cards == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            CardUpdateDto cardUpdateDto = new CardUpdateDto()
            {
                cardGetDto = new CardGetDto
                {
                    Id = card.Id,
                    Name = card.Name,
                    JobTitle = card.JobTitle,
                    Image = card.Image
                }
            };
            return View(cardUpdateDto);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CardUpdateDto cardUpdateDto)
        {
            Card? card = _context.Cards.Find(cardUpdateDto.cardGetDto.Id);
            card.Name = cardUpdateDto.cardPostDto.Name;
            card.JobTitle = cardUpdateDto.cardPostDto.JobTitle;
            if (cardUpdateDto.cardPostDto.File != null)
            {
                card.Image = cardUpdateDto.cardPostDto.File.CreateFile(_env.WebRootPath, "assets/img");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cards == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cards == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cards'  is null.");
            }
            var card = await _context.Cards.FindAsync(id);
            if (card != null)
            {
                _context.Cards.Remove(card);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
          return _context.Cards.Any(e => e.Id == id);
        }
    }
}
