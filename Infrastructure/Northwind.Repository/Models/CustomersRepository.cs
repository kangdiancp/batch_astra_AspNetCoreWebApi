using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;
using Northwind.Contracts.Interfaces;
using Northwind.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using Northwind.Entities.RequestFeatures;

namespace Northwind.Repository.Models
{
    public class CustomersRepository : RepositoryBase<Customer>, ICustomersRepository
    {
        public CustomersRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateCustomerAsync(Customer customer)
        {
            Create(customer);
        }

        public void DeleteCustomerAsync(Customer customer)
        {
            Delete(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomerAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                    .OrderBy(c => c.CompanyName)
                    .ToListAsync();
        }

        public async Task<Customer> GetCustomerAsync(string id, bool trackChanges) =>
            await FindByCondition(c => c.CustomerId.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Customer>> GetPaginationCustomerAsync(CustomerParameters customerParameters, bool trackChanges)
        {
            return await FindAll(trackChanges)
                            .OrderBy(c => c.CompanyName)
                            .Skip((customerParameters.PageNumber - 1) * customerParameters.PageSize)
                            .Take(customerParameters.PageSize)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> SearchCustomer(CustomerParameters customerParameters, bool trackChanges)
        {
            if (string.IsNullOrWhiteSpace(customerParameters.SearchCompany))
            {
                return await FindAll(trackChanges).ToListAsync();
            }
            var lowerCaseSearch = customerParameters.SearchCompany.Trim().ToLower();
            
            return await FindAll(trackChanges)
                .Where(c => c.CompanyName.ToLower().Contains(lowerCaseSearch))
                .Include(c => c.Orders)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();
        }

        public void UpdateCustomerAsync(Customer customer)
        {
            Update(customer);
        }
    }
}
