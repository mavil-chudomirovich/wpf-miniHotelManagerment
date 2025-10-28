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
    public class BookingReservationService : IBookingReservationService
    {
        private readonly IBookingReservationRepository _bookingReservationRepository;
        private readonly IBookingDetailRepository _bookingDetailRepository = new BookingDetailRepository();
        public BookingReservationService()
        {
            _bookingReservationRepository = new BookingReservationRepository();
            _bookingDetailRepository = new BookingDetailRepository();
        }
        public async Task CreateAsync(BookingReservation bookingReservation)
        {
            await _bookingReservationRepository.CreateAsync(bookingReservation);
        }

        public async Task CreateBookingAsync(int customerId, List<RoomInformation> selectedRooms, int status, string startDateStr, string endDateStr)
        {
            decimal totalPrice = 0m;
            var list = await _bookingReservationRepository.GetAllAsync();
            int bookingReservationsId = list.Any()
            ? list.Max(x => x.BookingReservationId) + 1
            : 1;
            var bookingReservation = new BookingReservation
            {
                BookingReservationId = bookingReservationsId,
                CustomerId = customerId,
                BookingStatus = (byte)status,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                TotalPrice = totalPrice
            };
            await _bookingReservationRepository.CreateAsync(bookingReservation);
            foreach (var room in selectedRooms)
            {
                if (!DateOnly.TryParse(startDateStr, out DateOnly startDate))
                {
                    throw new ArgumentException("Invalid start date format.");
                };
                if (!DateOnly.TryParse(endDateStr, out DateOnly endDate))
                {
                    throw new ArgumentException("Invalid end date format.");
                };
                if (startDate >= endDate)
                {
                    throw new ArgumentException("Start date must be before end date.");
                }
                if (startDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new ArgumentException("Start date must be today or later.");
                }
                if ( await IsRoomOverlappedAsync(room.RoomId, startDate, endDate))
                {
                    throw new InvalidOperationException($"Room {room.RoomNumber} is already booked for the selected dates.");
                }
                var bookingDetail = new BookingDetail
                {
                    BookingReservationId = bookingReservationsId,
                    RoomId = room.RoomId,
                    StartDate = startDate,
                    EndDate = endDate,
                    ActualPrice = room.RoomPricePerDay
                };
                totalPrice += (decimal)(room.RoomPricePerDay * (endDate.DayNumber - startDate.DayNumber))!;
                await _bookingDetailRepository.CreateAsync(bookingDetail);
            }
            bookingReservation.TotalPrice = totalPrice;
            await _bookingReservationRepository.UpdateAsync(bookingReservation);
        }

        private async Task<bool> IsRoomOverlappedAsync(int roomId, DateOnly startDate, DateOnly endDate)
        {
            // lấy dữ liệu booking detail của room đó
            var details = await _bookingDetailRepository.GetAllAsync();

            var overlaps = details.Any(d =>
                d.RoomId == roomId &&
                startDate <= d.EndDate &&
                endDate >= d.StartDate
            );

            return overlaps;
        }

        public async Task DeleteAsync(int id)
        {
            await _bookingReservationRepository.DeleteAsync(id);
            await _bookingDetailRepository.DeleteByReservationIdAsync(id);
        }

        public async Task<IEnumerable<BookingReservation>> GetAllAsync()
        {
            return await _bookingReservationRepository.GetAllAsync() ?? [];
        }

        public async Task UpdateAsync(int reservationId, int customerId, List<RoomInformation> selectedRooms, int status, string startDateStr, string endDateStr)
        {
            decimal totalPrice = 0m;
            await _bookingDetailRepository.DeleteByReservationIdAsync(reservationId);
            foreach (var room in selectedRooms)
            {
                if (!DateOnly.TryParse(startDateStr, out DateOnly startDate))
                {
                    throw new ArgumentException("Invalid start date format.");
                }
                ;
                if (!DateOnly.TryParse(endDateStr, out DateOnly endDate))
                {
                    throw new ArgumentException("Invalid end date format.");
                }
                ;
                if (startDate >= endDate)
                {
                    throw new ArgumentException("Start date must be before end date.");
                }
                if (startDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new ArgumentException("Start date must be today or later.");
                }
                if (await IsRoomOverlappedAsync(room.RoomId, startDate, endDate))
                {
                    throw new InvalidOperationException($"Room {room.RoomNumber} is already booked for the selected dates.");
                }
                await _bookingDetailRepository.CreateAsync(new BookingDetail
                {
                    BookingReservationId = reservationId,
                    RoomId = room.RoomId,
                    StartDate = startDate,
                    EndDate = endDate,
                    ActualPrice = room.RoomPricePerDay
                });
                totalPrice += (decimal)(room.RoomPricePerDay * (endDate.DayNumber - startDate.DayNumber))!;
            }
            await _bookingReservationRepository.UpdateAsync(new BookingReservation
            {
                BookingReservationId = reservationId,
                CustomerId = customerId,
                BookingStatus = (byte)status, 
                TotalPrice = totalPrice
            });
        }

        public async Task<IEnumerable<BookingReservation>> GetByCustomerId(int customerId)
        {
            return (await _bookingReservationRepository.GetAllAsync()).Where(b => b.CustomerId == customerId) ?? [];
        }
    }
}
