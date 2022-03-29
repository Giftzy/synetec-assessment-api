using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly EmployeeService _employeeService;
        private readonly BonusPoolService _bonusPoolService;
        public BonusPoolController(AppDbContext dbContext, EmployeeService employeeService)
        {
            _dbContext = dbContext;
            _employeeService = employeeService;
            _bonusPoolService = new BonusPoolService(_dbContext, _employeeService);
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        { 
            return Ok(await _employeeService.GetEmployeesAsync());
        }

        [HttpPost(nameof(CalculateBonus))]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        { 
            if(request.SelectedEmployeeId == 0)
            {
                return BadRequest("Bad Request!");
            }
            var result = await _bonusPoolService.CalculateAsync(request.TotalBonusPoolAmount,request.SelectedEmployeeId);
            if (result != null)
                return Ok(result);
            else
                return BadRequest("Record not found!");
        }
    }
}
