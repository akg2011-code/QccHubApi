using Microsoft.EntityFrameworkCore;
using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QccHub.Data.Repository
{
    public class QuestionRepository : GenericRepository<Question> , IQuestionRepository
    {
        private ApplicationDbContext _context;
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public override Task<List<Question>> GetAllAsync()
        {
            return _context.Question.OrderByDescending(q => q.CreatedDate).Include(q => q.User).ToListAsync();
        }
    }
}
