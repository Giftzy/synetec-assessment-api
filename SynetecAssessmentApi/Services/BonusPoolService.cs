using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public class BonusPoolService
    {
        private readonly AppDbContext _dbContext;
        private readonly EmployeeService _employeeService;

        public BonusPoolService(AppDbContext dbContext, EmployeeService employeeService)
        {
            //var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            //dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "HrDb");

            //_dbContext = new AppDbContext(dbContextOptionBuilder.Options);
            _dbContext = dbContext;
            _employeeService = employeeService;
        }

     
        public async Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId)
        {
            //load the details of the selected employee using the Id
            EmployeeDto employee = await _employeeService.GetEmployeeAsync(selectedEmployeeId);
            if(employee == null) { return null; }

            //get the total salary budget for the company
            int totalSalary = _employeeService.GetTotalEmployeeSalary();

            //calculate the bonus allocation for the employee
            var bonusAllocation = GetEmployeeBonus(employee.Salary, totalSalary, bonusPoolAmount);

            return new BonusPoolCalculatorResultDto
            {
                Employee = new EmployeeDto
                {
                    Fullname = employee.Fullname,
                    JobTitle = employee.JobTitle,
                    Salary = employee.Salary,
                    Department = new DepartmentDto
                    {
                        Title = employee.Department.Title,
                        Description = employee.Department.Description
                    }
                },

                Amount = bonusAllocation
            };
        }
   
        private int GetEmployeeBonus(int employeeSalary, int totalSalary, int bonusPoolAmount)
        {
            decimal bonusPercentage = (decimal)employeeSalary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);
            return bonusAllocation;
        }
    }
}
