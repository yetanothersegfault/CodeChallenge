using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _logger = logger;
            _compensationContext = compensationContext;
        }

        public Compensation Add(Compensation compensation)
        {
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetCompensation(string id)
        {
            // there could be multiple compensations due to effective date. Return the most recent one.
            return _compensationContext.Compensations.Include(x => x.Employee).OrderBy(x => x.EffectiveDate).FirstOrDefault(x => x.Employee.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
