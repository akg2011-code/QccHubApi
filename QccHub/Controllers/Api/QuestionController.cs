using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QccHub.Data;
using QccHub.Data.Interfaces;
using QccHub.DTOS;

namespace QccHub.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public QuestionController(IQuestionRepository questionRepository ,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddQustion(QuestionDTO question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("can't add , some info is wrong");
            }
            Question newQuestion = new Question
            {
                CreatedBy = question.UserID,
                UserID = question.UserID,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                Title = question.Title
            };
            _questionRepository.Add(newQuestion);
            await _unitOfWork.SaveChangesAsync();
            return Created("question added",question);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var result = await _questionRepository.GetAllAsync();
            return Ok(result.AsQueryable<Question>().OrderByDescending(q=>q.CreatedDate).Include(q=>q.User));
        }

        [HttpGet("{questionID}")]
        public async Task<IActionResult> GetQuestion(int questionID)
        {
            var result = await _questionRepository.GetByIdAsync(questionID);
            if (result == null)
            {
                return NotFound("No question for this ID");
            }
            return Ok(result);
        }

        [HttpPut("{questionID}")]
        public async Task<IActionResult> EditQuestion(int questionID , Question editedQuestion)
        {
            Question question = await _questionRepository.GetByIdAsync(questionID);
            if (question == null)
            {
                return NotFound("No question for this ID");
            }

            question.Title = editedQuestion.Title;
            question.UserID = editedQuestion.UserID;
            await _unitOfWork.SaveChangesAsync();
            return Ok("question edited");
        }

    }
}