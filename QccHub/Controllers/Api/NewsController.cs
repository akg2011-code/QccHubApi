using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QccHub.Data.Interfaces;
using QccHub.Data.Models;
using QccHub.Logic.Helpers;

namespace QccHub.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NewsController : BaseController
    {
        private readonly INewsRepository _newsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NewsController(INewsRepository newsRepository,
            IUnitOfWork unitOfWork,
            CurrentSession currentSession,
            IHttpContextAccessor contextAccessor) : base(currentSession, contextAccessor)
        {
            _newsRepository = newsRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _newsRepository.Add(news);
            await _unitOfWork.SaveChangesAsync();
            return Created("news added", news);
        }

        [HttpPut("{newsID}")]
        public async Task<IActionResult> EditNews(int newsID, News editedNews)
        {
            News news = await _newsRepository.GetByIdAsync(newsID);
            if (news == null && news.IsDeleted == false)
            {
                return NotFound("No news for this ID");
            }
            news.Time = editedNews.Time;
            news.Title = editedNews.Title;
            news.Details = editedNews.Details;
            await _unitOfWork.SaveChangesAsync();
            return Ok("news edited");
        }

        [HttpDelete("{newsID}")]
        public async Task<IActionResult> DeleteNews(int newsID)
        {
            News news = await _newsRepository.GetByIdAsync(newsID);
            if (news == null && news.IsDeleted == false)
            {
                return NotFound("No news for this ID");
            }
            news.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
            return Ok("news is deleted");
        }
    }
}