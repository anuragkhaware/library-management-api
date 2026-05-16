using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpPost]
        public IActionResult Borrow([FromBody] BorrowDto borrowDto)
        {
            var record = _borrowService.Borrow(borrowDto);
            if (record == null)
                return BadRequest("Book is not available or does not exist.");
            return Ok(record);
        }

        [HttpPut("{borrowId}/return")]
        public IActionResult Return(int borrowId)
        {
            var record = _borrowService.Return(borrowId);
            if (record == null)
                return BadRequest("Borrow record not found or already returned.");
            return Ok(record);
        }

        [HttpGet("overdue")]
        public IActionResult GetOverdue()
        {
            return Ok(_borrowService.GetOverdue());
        }

        [HttpGet("member/{memberId}")]
        public IActionResult GetByMember(int memberId)
        {
            return Ok(_borrowService.GetByMember(memberId));
        }
    }
}