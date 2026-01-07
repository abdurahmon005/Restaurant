using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Products;
using WebApp.Aplication.Models.Reservation;
using WebApp.Aplication.Services.Interface;
using WebApp.DataAccess.Persistence;
using WebApp.Domain.Entities;
using WebApp.Domain.Enums;

namespace WebApp.Aplication.Services.Impl
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;
        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationResponceDTO> CreateAsync(CreateReservationDTO dto)
        {
            var table = await _context.Tables.FindAsync(dto.TableId);

            if (table == null)
            {
                throw new Exception("Table Not Found");
            }
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
            table.Status = TableStatus.Reserved;

            await _context.SaveChangesAsync();

            return new ReservationResponceDTO
            {
                Id = reservation.Id,
                CustomerName = dto.CustomerName,
                ReservationTime = reservation.ReservationTime,
                TableId = table.Id,
                Message = "Reservatsiya yaratildi"
            };
        }

        
        public async Task<ReservationResponceDTO> UpdateAsync(UpdateResrvationDTO dto)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Table)
                .Where(r => r.Id == dto.TableId)
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

            await _context.SaveChangesAsync();

            return new ReservationResponceDTO
            {
                Id = reservation.Id,
                CustomerName = reservation.CustomerName,
                ReservationTime = reservation.ReservationTime,
                TableId = reservation.TableId,
                Message = "Reservatsiya sozdana uspeshno"
            };
        }

        public async Task<ReservationResponceDTO> GetByIdAsync(int id)
        {
            var reserv = await _context.Reservations
            .FirstOrDefaultAsync(p => p.Id == id);


            if (reserv == null)
            {
                throw new InvalidOperationException("Netu reservatsii");
            }

            return new ReservationResponceDTO
            {
                Id = reserv.Id,
                CustomerName = reserv.CustomerName,
                ReservationTime = reserv.ReservationTime,
                TableId = reserv.TableId,
                 Message = "Reservatsiya uspeshmo sozdana"
            };
        }

        public async Task<string> DeleteAsync(int id)
        {
            var result = await _context.Reservations.FindAsync(id);

            if (result == null)
            {
                return "Ne udaleno";
            }

            return "Sozdalsya";
        }
    }


}
