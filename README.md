# Synetec Basic .Net API assessement

This is Synetec's basic API developer assessment.

## The context…

A company (let’s call them ‘ABC ltd’) have decided to implement a company-wide bonus scheme.
The company plans to allocate a proportion of their profits to a bonus pool. Once the size of the bonus pool is agreed, this total amount will be divided amongst their employees. The amount each employee will receive will depend on their annual salary: The proportion of the bonus pool allocated to an employee should be the same as the proportion of the total wage budget that their salary represents.
If an employee's salary is 15% of the total wage budget for the company (i.e. the total of all salaries of all employees), then they will receive 15% of the bonus pool. So, to elaborate, if the bonus pool was £123,456.00; this employee would receive £18,518.40 (i.e. 123,456 x 0.15)

## Project Business Logic

Refactor the application so that, 
1. It adheres to SOLID principles
2. It returns a bad request when SelectedEmployeeId is not specified, or the employee is not found. 
3. The calculations are unit tested

## Technologies / Patterns

.Net 5.0 C#, Dependency Injection, Async await and Github for version control system. NUnit Test
