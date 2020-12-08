using QccHub.Data.Interfaces;
using QccHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QccHub.Data.Repository
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        private ApplicationDbContext _context;
        public NewsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}