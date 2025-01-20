using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    public class CompensationControllerTests
    {
        [TestClass]
        public class EmployeeControllerTests
        {
            private static HttpClient _httpClient;
            private static TestServer _testServer;

            [ClassInitialize]
            // Attribute ClassInitialize requires this signature
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
            public static void InitializeClass(TestContext context)
            {
                _testServer = new TestServer();
                _httpClient = _testServer.NewClient();
            }

            [ClassCleanup]
            public static void CleanUpTest()
            {
                _httpClient.Dispose();
                _testServer.Dispose();
            }

            [TestMethod]
            public void CreateCompensation_Returns_Created()
            {
                // Arrange
                var employee = new Employee()
                {
                    EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                    Department = "Engineering",
                    FirstName = "John",
                    LastName = "Lennon",
                    Position = "Development Manager",
                };

                var compensation = new Compensation()
                {
                    Employee = employee,
                    Salary = 100000,
                    EffectiveDate = System.DateTime.Today
                };

                var requestContent = new JsonSerialization().ToJson(compensation);

                // Execute
                var postRequestTask = _httpClient.PostAsync("api/compensation",
                   new StringContent(requestContent, Encoding.UTF8, "application/json"));
                var response = postRequestTask.Result;

                // Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                var newCompensation = response.DeserializeContent<Compensation>();
                Assert.AreEqual(employee.FirstName, newCompensation.Employee.FirstName);
                Assert.AreEqual(employee.LastName, newCompensation.Employee.LastName);
                Assert.AreEqual(employee.Department, newCompensation.Employee.Department);
                Assert.AreEqual(employee.Position, newCompensation.Employee.Position);
                Assert.AreEqual(compensation.Salary, newCompensation.Salary);
                Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            }

            [TestMethod]
            public void GetCompensationById_Returns_Ok()
            {
                // Arrange (need to add to the persistance layer as it is not bootstrapped.)
                var exptectedEmployee = new Employee()
                {
                    EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                    Department = "Engineering",
                    FirstName = "John",
                    LastName = "Lennon",
                    Position = "Development Manager",
                };

                var expectedCompensation = new Compensation()
                {
                    Employee = exptectedEmployee,
                    Salary = 100000,
                    EffectiveDate = System.DateTime.Today
                };

                var requestContent = new JsonSerialization().ToJson(expectedCompensation);
                var postRequestTask = _httpClient.PostAsync("api/compensation",
                   new StringContent(requestContent, Encoding.UTF8, "application/json"));
                var response = postRequestTask.Result;

                // Execute
                var getRequestTask = _httpClient.GetAsync($"api/compensation/{expectedCompensation.Employee.EmployeeId}");
                response = getRequestTask.Result;

                // Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                var actualCompensation = response.DeserializeContent<Compensation>();
                Assert.AreEqual(expectedCompensation.Employee.EmployeeId, actualCompensation.Employee.EmployeeId);
                Assert.AreEqual(expectedCompensation.Salary, actualCompensation.Salary);
                Assert.AreEqual(expectedCompensation.EffectiveDate, actualCompensation.EffectiveDate);
            }

            [TestMethod]
            public void CreateCompensation_Returns_NotFound()
            {
                // Arrange
                var employee = new Employee()
                {
                    EmployeeId = "Invalid_Id",
                    Department = "Music",
                    FirstName = "Sunny",
                    LastName = "Bono",
                    Position = "Singer/Song Writer",
                };

                var compensation = new Compensation()
                {
                    Employee = employee,
                    Salary = 10000,
                    EffectiveDate = System.DateTime.Today
                };

                var requestContent = new JsonSerialization().ToJson(compensation);

                // Execute
                var getRequestTask = _httpClient.GetAsync($"api/compensation/{compensation.Employee.EmployeeId}");
                var response = getRequestTask.Result;

                // Assert
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
