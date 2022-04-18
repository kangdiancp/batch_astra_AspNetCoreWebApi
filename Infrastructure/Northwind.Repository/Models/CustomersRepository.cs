using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Entities.Models;
using Northwind.Contracts.Interfaces;
using Northwind.Entities.Contexts;


namespace Northwind.Repository.Models
{
    public class CustomersRepository : RepositoryBase<Customer>, ICustomersRepository
    {
        public CustomersRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
