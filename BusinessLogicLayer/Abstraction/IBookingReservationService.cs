using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Abstraction
{
    public interface IBookingReservationService
    {
        Task<IEnumerable<BookingReservation>> GetAllAsync();
        Task CreateAsync(BookingReservation bookingReservation);
        Task UpdateAsync(int reservationId, int customerId, List<RoomInformation> selectedRooms, int status, string startDateStr, string endDateStr);
        Task DeleteAsync(int id);
        Task CreateBookingAsync(int customerId, List<RoomInformation> selectedRooms, int status, string startDateStr, string endDateStr);
        Task<IEnumerable<BookingReservation>> GetByCustomerId(int customerId);
    }
}
