using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.AppDbContext
{
    public interface IFuminiHotelManagementContext
    {
       DbSet<BookingDetail> BookingDetails { get; set; }

         DbSet<BookingReservation> BookingReservations { get; set; }

        DbSet<Customer> Customers { get; set; }

        DbSet<RoomInformation> RoomInformations { get; set; }

        DbSet<RoomType> RoomTypes { get; set; }
        Task SaveChangesAsync();
    }
}
