using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstraction
{
    public interface IBookingDetailRepository
    {
        Task<bool> IsActiveBooking(int roomId);
        Task<IEnumerable<BookingDetail>> GetAllAsync();
        Task CreateAsync(BookingDetail bookingDetail);
        Task UpdateAsync(BookingDetail bookingDetail);
        Task<IEnumerable<BookingDetail>> GetByReservationIdAsync(int id);
        Task DeleteByReservationIdAsync(int id);
        Task<IEnumerable<BookingDetail>> GetByStartAndEndDate(DateOnly start, DateOnly end);
    }

}
