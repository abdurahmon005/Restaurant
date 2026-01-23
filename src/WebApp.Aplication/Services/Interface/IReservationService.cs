using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Aplication.Models.Reservation;

namespace WebApp.Aplication.Services.Interface
{
    public interface IReservationService
    {
            Task<ReservationResponceDTO> CreateAsync(CreateReservationDTO dto);
            Task<ReservationResponceDTO> UpdateAsync(UpdateResrvationDTO dto);
            Task<ReservationResponceDTO> GetByIdAsync(int id);
            Task<string> DeleteAsync(int id);
    }
}