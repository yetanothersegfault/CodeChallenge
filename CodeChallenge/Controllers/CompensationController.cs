using CodeChallenge.Models;
using CodeChallenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Text.Json;
using CodeChallenge.Repositories;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICompensationService _compensationService; 

        public CompensationController(ILogger<EmployeeController> logger, ICompensationService compensationService, IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

            // check to see if the employee is in the db already
            var emp = _employeeService.GetById(compensation.Employee.EmployeeId);

            if (emp == null)
            {
                _logger.LogDebug($"Employee: '{compensation.Employee.FirstName} {compensation.Employee.LastName}' not found.");
                return BadRequest("Employee Not Found");
            }

            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationById", new { id = compensation.Employee.EmployeeId }, compensation);
        }

        /// <summary>
        /// Takes a employee id and returns a compensation object that details the employee, salary, and effective date
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     Not found if employee id is not found in compensation db
        ///     The compensation containing employee, salary, and effective date.
        /// </returns>
        [HttpGet("{id}", Name = "GetCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");

            // get the requested employee from the db
            var compensation = _compensationService.GetById(id);

            // if it is null then the employee does not exist and return not found
            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }

    }
}

