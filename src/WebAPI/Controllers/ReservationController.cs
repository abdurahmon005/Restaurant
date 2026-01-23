using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Aplication.Models.Reservation;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;
using WebApp.Domain.Enums;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly AppDbContext _context;
        public ReservationController(IReservationService reservationService, AppDbContext appDbContext)
        {
            reservationService = _reservationService;
            _context = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateReservationDTO dto)
        {
            var result = await _reservationService.CreateAsync(dto);

            if (result == null)
            {
                return NotFound("Reservatsiya yaratilmadi");
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reserv = await _reservationService.GetByIdAsync(id);

            if (reserv == null)
                return NotFound();

            return Ok(reserv);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateResrvationDTO dto)
        {
            var reserv = await _reservationService.UpdateAsync(dto);
            if(reserv == null)
            {
                return BadRequest("Ne obnovleno");
            }
            return Ok("Obnovleno");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reserv = await _reservationService.DeleteAsync(id);

            if (reserv == null) {
                return NotFound();
            }

            return Ok("Uspeshno udaleno");
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.IsConfirmed = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reservation confirmed" });
        }
    }
}
