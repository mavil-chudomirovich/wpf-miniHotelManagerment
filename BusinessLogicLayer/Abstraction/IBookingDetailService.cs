using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Abstraction
{
    public interface IBookingDetailService
    {
        Task<IEnumerable<BookingDetail>> GetAllAsync();
        Task CreateAsync(BookingDetail bookingDetail);
        Task<IEnumerable<BookingDetail>> GetByBookingReservationIdAsync(int id);
        Task<IEnumerable<BookingDetail>> GetByStartAndEndDate(DateOnly start, DateOnly end);
    }
}
