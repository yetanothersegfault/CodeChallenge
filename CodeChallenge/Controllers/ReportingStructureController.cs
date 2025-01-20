using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Text.Json;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reportingstructure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Takes a employee id and returns a ReportingStructure object that details the employee and number of direct reports
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     Not found if employee id is not found in db
        ///     The ReportingStructure containing employee and number of direct reports.
        /// </returns>
        [HttpGet("{id}", Name = "GetReportingStructureById")]
        public IActionResult GetReportingStructureById(String id)
        {
            _logger.LogDebug($"Received reporting structure get request for '{id}'");

            // get the requested employee from the db
            var employee = _employeeService.GetById(id);

            // if it is null then the employee does not exist and return not found
            if (employee == null)
                return NotFound();

            var reportingStructure = new ReportingStructure(employee, 0);

            Queue<Employee> queue = new Queue<Employee>();

            AddEmployeesToQueue(queue, employee);

            // iterate over each employee that is a direct report. adding their direct reports to the queue to add as well
            while (queue.Count > 0)
            {
                // dequeue the top employee off and add them to the number of reports
                var emp = queue.Dequeue();
                reportingStructure.NumberOfReports++;
                // add their direct reports to the queue
                AddEmployeesToQueue(queue, emp);
            }

            return Ok(reportingStructure);
        }

        /// <summary>
        /// Adds any employee objects located in the Direct Reports of the employee passed in to the queue to be calculated
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="employee"></param>
        private void AddEmployeesToQueue(Queue<Employee> queue, Employee employee)
        {
            // update the employee to get the next level direct reports.
            employee = _employeeService.GetById(employee.EmployeeId);

            if (employee.DirectReports != null)
            {
                // get employee direct reports and add them to the queue
                foreach (Employee emp in employee.DirectReports)
                {
                    queue.Enqueue(emp);
                }
            }
        }
    }
}
