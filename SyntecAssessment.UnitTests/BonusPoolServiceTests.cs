using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Persistence;
using SynetecAssessmentApi.Services;
using SyntecAssessment.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks; 

namespace SyntecAssessment.UnitTests
{
    public class BonusPoolServiceTests
    {
        BonusPoolService _bonuspoolService;
        [SetUp]
        public void Setup()
        {
            
        }

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable(); 

            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));

            dbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));

            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);   

            return dbSet.Object;
        }

        [Test]
        
        public async Task CalculateAsync_When_Valid_EmployeeID_Expect_BonusAmountAsync()
        {
            int employeeID = 1;
            int bonusAmountAvailable = 100;
            decimal expectedBonus = 25;

            /**
             * Insert two employees 
             * ID - 1, salary - 1000, 
             * ID - 2, salary - 3000, 
             *  bonus percentage  - salary/totalsalary   * 100 %
             *  ID -  1 -  1000/4000 * 100 =  25 %
             *  Bonus amount = 25/100 * <see cref="bonusAmountAvailable">100</see> = <see cref="expectedBonus">25</see> 
             */

            Mock<AppDbContext> mockContext = new Mock<AppDbContext>();
            var firstEmployee = new Employee(1, "gift onaivwe", jobTitle: "", departmentId: 1, salary: 1000);
            firstEmployee.Department = new Department(1,"First", "My dep");
            var secondEmployee = new Employee(2, "blessing onaivwe", jobTitle: "", departmentId: 1, salary:3000);
            secondEmployee.Department = firstEmployee.Department;

            var employees = new List<Employee>();
            employees.Add(firstEmployee);
            employees.Add(secondEmployee); 
            var employeeDBSet = GetQueryableMockDbSet(employees);
         
            mockContext.Object.Employees = employeeDBSet;  

            _bonuspoolService = new BonusPoolService(mockContext.Object, new EmployeeService(mockContext.Object));

            var bonus = await _bonuspoolService.CalculateAsync(bonusAmountAvailable, employeeID);

            Assert.AreEqual(expectedBonus, bonus.Amount);
        }
        [Test]
        public async Task CalculateAsync_When_BonusAmountIsZero_Expect_ZeroBonusAmount()
        {
            int employeeID = 1;
            int bonusAmountAvailable = 0; 
            /**
             * Insert two employees 
             * ID - 1, salary - 1000, 
             * ID - 2, salary - 3000, 
             *  bonus percentage  - salary/totalsalary   * 100 %
             *  ID -  1 -  1000/4000 * 100 =  25 %
             *  Bonus amount = 25/100 * <see cref="bonusAmountAvailable">100</see> = <see cref="expectedBonus">25</see> 
             */

            Mock<AppDbContext> mockContext = new Mock<AppDbContext>();
            var firstEmployee = new Employee(1, "gift onaivwe", jobTitle: "", departmentId: 1, salary: 1000);
            firstEmployee.Department = new Department(1, "First", "My dep");
            var secondEmployee = new Employee(2, "blessing onaivwe", jobTitle: "", departmentId: 1, salary: 3000);
            secondEmployee.Department = firstEmployee.Department;

            var employees = new List<Employee>();
            employees.Add(firstEmployee);
            employees.Add(secondEmployee);
            var employeeDBSet = GetQueryableMockDbSet(employees);

            mockContext.Object.Employees = employeeDBSet;

            _bonuspoolService = new BonusPoolService(mockContext.Object, new EmployeeService(mockContext.Object));

            var bonus = await _bonuspoolService.CalculateAsync(bonusAmountAvailable, employeeID);
 
            Assert.AreEqual(0, bonus.Amount);
        }
    }
}