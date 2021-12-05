using System;
using System.Threading.Tasks;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Infrastructure.Persistence.Dapper;

namespace MyPomodoro.Infrastructure.Persistence.Repositories
{
    public class TestRepo : ITestRepo
    {
        IDapper dapper;

        public TestRepo(IDapper dapper)
        {
            this.dapper = dapper;
        }
        public void addDepart()
        {
            var addDepartmentQuery = $"INSERT INTO Departments(Name,Description) VALUES('salamunalekumbabbaaa','sdfsdfsdfsdf');SELECT CAST(SCOPE_IDENTITY() as int)";
            var departmentId = dapper.Execute(addDepartmentQuery, null);
        }
    }
}