using DataAccessLayer.AppDbContext;

namespace TestConnection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new FuminiHotelManagementContext();
            foreach (var item in db.Customers.ToList())
            {
                Console.WriteLine(item.Telephone);
            }
            Console.WriteLine("Hello, World!");
        }
    }
}
