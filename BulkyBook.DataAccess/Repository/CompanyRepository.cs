using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company Company)
        {
            var objFromDb = _db.Companies.FirstOrDefault(s => s.Id == Company.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = Company.Name;
                objFromDb.IsAuthorized = Company.IsAuthorized;
                objFromDb.StreetAddress = Company.StreetAddress;
                objFromDb.City = Company.City;
                objFromDb.State = Company.State;
                objFromDb.PhoneNumber = Company.PhoneNumber;
            }
        }
    }
}
