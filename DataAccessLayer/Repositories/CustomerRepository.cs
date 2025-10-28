using DataAccessLayer.Abstraction;
using DataAccessLayer.AppDbContext;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IFuminiHotelManagementContext _dbContext;
        public CustomerRepository() 
        {
            _dbContext = new FuminiHotelManagementContext();
        }

        public async Task CreateAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {

            var existingCustomer = await _dbContext.Customers.FindAsync(id)
           ?? throw new DirectoryNotFoundException("Customer not found");
            _dbContext.Customers.Remove(existingCustomer);
            await _dbContext.SaveChangesAsync();

        } 
        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer = await (_dbContext.Customers.FindAsync(customer.CustomerId))
                ?? throw new DirectoryNotFoundException("Customer not found");
            if(customer.CustomerFullName is not null)
                existingCustomer.CustomerFullName = customer.CustomerFullName;
            if(customer.Telephone is not null)
                existingCustomer.Telephone = customer.Telephone;
            if(customer.EmailAddress is not null)
                existingCustomer.EmailAddress = customer.EmailAddress;
            if(customer.CustomerBirthday is not null)
                existingCustomer.CustomerBirthday = customer.CustomerBirthday;
            if(customer.CustomerStatus is not null)
                existingCustomer.CustomerStatus = customer.CustomerStatus;
            if(customer.Password is not null)
                existingCustomer.Password = customer.Password;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<Customer?> LoginAsync(string email, string password)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.EmailAddress == email && c.Password == password);
        }
    }
}
