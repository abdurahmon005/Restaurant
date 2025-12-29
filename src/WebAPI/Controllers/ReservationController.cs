using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;
using WebApp.Domain.Enums;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? date = null)
        {
            var query = _context.Reservations
                .Include(r => r.Table)
                .AsQueryable();

            if (date.HasValue)
            {
                var startDate = date.Value.Date;
                var endDate = startDate.AddDays(1);
                query = query.Where(r => r.ReservationTime >= startDate && r.ReservationTime < endDate);
            }

            var reservations = await query
                .OrderBy(r => r.ReservationTime)
                .Select(r => new
                {
                    r.Id,
                    r.CustomerName,
                    r.CustomerPhone,
                    r.ReservationTime,
                    r.GuestCount,
                    r.Notes,
                    r.IsConfirmed,
                    r.TableId,
                    tableNumber = r.Table.TableNumber,
                    r.CreatedAt
                })
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Table)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.CustomerName,
                    r.CustomerPhone,
                    r.ReservationTime,
                    r.GuestCount,
                    r.Notes,
                    r.IsConfirmed,
                    r.TableId,
                    tableNumber = r.Table.TableNumber,
                    r.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            var table = await _context.Tables.FindAsync(dto.TableId);
            if (table == null)
                return BadRequest("Table not found");

            var reservation = new Reservation
            {
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                ReservationTime = dto.ReservationTime,
                GuestCount = dto.GuestCount,
                Notes = dto.Notes,
                TableId = dto.TableId,
                IsConfirmed = false
            };

            _context.Reservations.Add(reservation);

            // Update table status to Reserved
            table.Status = TableStatus.Reserved;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                reservation.Id,
                reservation.CustomerName,
                reservation.ReservationTime,
                reservation.TableId,
                message = "Reservation created successfully"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            if (!string.IsNullOrEmpty(dto.CustomerName))
                reservation.CustomerName = dto.CustomerName;

            if (!string.IsNullOrEmpty(dto.CustomerPhone))
                reservation.CustomerPhone = dto.CustomerPhone;

            if (dto.ReservationTime.HasValue)
                reservation.ReservationTime = dto.ReservationTime.Value;

            if (dto.GuestCount.HasValue)
                reservation.GuestCount = dto.GuestCount.Value;

            if (dto.Notes != null)
                reservation.Notes = dto.Notes;

            if (dto.IsConfirmed.HasValue)
                reservation.IsConfirmed = dto.IsConfirmed.Value;

            if (dto.TableId.HasValue && dto.TableId.Value != reservation.TableId)
            {
                // Release old table
                var oldTable = await _context.Tables.FindAsync(reservation.TableId);
                if (oldTable != null)
                    oldTable.Status = TableStatus.Available;

                // Reserve new table
                var newTable = await _context.Tables.FindAsync(dto.TableId.Value);
                if (newTable != null)
                {
                    newTable.Status = TableStatus.Reserved;
                    reservation.TableId = dto.TableId.Value;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Reservation updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            // Release the table
            var table = await _context.Tables.FindAsync(reservation.TableId);
            if (table != null && table.Status == TableStatus.Reserved)
                table.Status = TableStatus.Available;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reservation deleted successfully" });
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

    public class CreateReservationDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public DateTime ReservationTime { get; set; }
        public int GuestCount { get; set; }
        public string? Notes { get; set; }
        public int TableId { get; set; }
    }

    public class UpdateReservationDto
    {
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public DateTime? ReservationTime { get; set; }
        public int? GuestCount { get; set; }
        public string? Notes { get; set; }
        public bool? IsConfirmed { get; set; }
        public int? TableId { get; set; }
    }
}
