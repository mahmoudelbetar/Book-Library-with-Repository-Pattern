using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Gallery.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Company company)
        {
            db.Companies.Update(company);
            Save();
        }
    }
}
