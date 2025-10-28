using BusinessLogicLayer.Abstraction;
using DataAccessLayer.Abstraction;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public async Task CreateAsync(Customer customer)
        {
            // 1) Kiểm tra Telephone
            if (string.IsNullOrWhiteSpace(customer.Telephone) || !customer.Telephone.All(char.IsDigit))
            {
                throw new Exception("Telephone must contain only digits.");
            }

            // Kiểm tra độ dài số điện thoại (tuỳ Vietnam thường 10 digits)
            if (customer.Telephone.Length < 9 || customer.Telephone.Length > 11)
            {
                throw new Exception("Telephone length is invalid (must be 9–11 digits).");
            }

            // 2) Kiểm tra Email
            string email = customer.EmailAddress.Trim();
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                throw new Exception("Invalid email format.");
            }

            DateOnly birthday = (DateOnly)customer.CustomerBirthday;
            if (birthday >= DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Birthday must be before today.");
            }
            await _customerRepository.CreateAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            await _customerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync() ?? [];
        }

        public async Task<Customer?> LoginAsync(string email, string password)
        {
            return await _customerRepository.LoginAsync(email, password);
        }

        public async Task UpdateAsync(Customer customer)
        {
            // 1) Kiểm tra Telephone
            if (string.IsNullOrWhiteSpace(customer.Telephone) || !customer.Telephone.All(char.IsDigit))
            {
                throw new Exception("Telephone must contain only digits.");
            }

            // Kiểm tra độ dài số điện thoại (tuỳ Vietnam thường 10 digits)
            if (customer.Telephone.Length < 9 || customer.Telephone.Length > 11)
            {
                throw new Exception("Telephone length is invalid (must be 9–11 digits).");
            }

            // 2) Kiểm tra Email
            string email = customer.EmailAddress.Trim();
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                throw new Exception("Invalid email format.");
            }

            DateOnly birthday = (DateOnly)customer.CustomerBirthday;
            if (birthday >= DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Birthday must be before today.");
            }
           await  _customerRepository.UpdateAsync(customer);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
