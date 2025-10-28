using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
    public interface IBookingReservationRepository
    {
        Task<BookingReservation?> GetByIdAsync(int id);
        Task<IEnumerable<BookingReservation>> GetAllAsync();
        Task CreateAsync(BookingReservation bookingReservation);
        Task UpdateAsync(BookingReservation bookingReservation);
        Task DeleteAsync(int id);
        
    }
}
