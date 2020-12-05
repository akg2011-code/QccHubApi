using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QccHub.Data.Repository
{
    public class QuestionRepository : GenericRepository<Question> , IQuestionRepository
    {
        private ApplicationDbContext _context;
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
