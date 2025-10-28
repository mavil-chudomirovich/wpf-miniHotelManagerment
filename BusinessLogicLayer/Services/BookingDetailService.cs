using BusinessLogicLayer.Abstraction;
using DataAccessLayer.Abstraction;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly IBookingDetailRepository _bookingDetailRepository;
        public BookingDetailService()
        {
            _bookingDetailRepository = new BookingDetailRepository();

        }
        public async Task<IEnumerable<BookingDetail>> GetAllAsync()
        {
            return await _bookingDetailRepository.GetAllAsync() ?? [];
        }
        public async Task CreateAsync(BookingDetail bookingDetail)
        {
            await _bookingDetailRepository.CreateAsync(bookingDetail);
        }

        public async Task<IEnumerable<BookingDetail>> GetByBookingReservationIdAsync(int id)
        {
            return (await _bookingDetailRepository.GetByReservationIdAsync(id)) ?? [];
        }

        public async Task<IEnumerable<BookingDetail>> GetByStartAndEndDate(DateOnly start, DateOnly end)
        {
            return (await _bookingDetailRepository.GetByStartAndEndDate(start, end)) ?? [];
        }
    }
}
