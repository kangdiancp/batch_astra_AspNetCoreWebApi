using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Contracts;
using Northwind.Contracts.Interfaces;
using Northwind.Entities.Contexts;
using Northwind.Repository.Models;

namespace Northwind.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private ICategoryRepository _categoryRepository;
        private ICustomersRepository _customersRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ICategoryRepository Category
        {
            get { 
                if(_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_repositoryContext);
                }
                return _categoryRepository; 
            }
        }

        public ICustomersRepository Customers => throw new NotImplementedException();

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
