
using Employment_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System.Data;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(DbHelper db) : ControllerBase
{
    private readonly DbHelper _db = db;

    [HttpGet]
    public IActionResult Get()
    {
        var dt = _db.ExecuteQuery("select *from Employee");
        var list = new List<Dictionary<string, object>>();

        foreach (DataRow row in dt.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }

        return Ok(list);

    }

    [HttpPost]
    public IActionResult Create(Employee emp)
    {
        string query = $@"INSERT INTO Employee 
            (Name, DOB, DOJ, Designation, Salary, Gender, State) 
            VALUES ('{emp.Name}', '{emp.DOB:yyyy-MM-dd}', '{emp.DOJ:yyyy-MM-dd}', '{emp.Designation}', {emp.Salary}, '{emp.Gender}', '{emp.State}')";
        _db.ExecuteQuery(query);
        return Ok("Inserted");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Employee emp)
    {
        string query = $@"UPDATE Employee SET 
            Name='{emp.Name}', DOB='{emp.DOB:yyyy-MM-dd}', DOJ='{emp.DOJ:yyyy-MM-dd}', 
            Designation='{emp.Designation}', Salary={emp.Salary}, 
            Gender='{emp.Gender}', State='{emp.State}'
            WHERE Id={id}";
        _db.ExecuteQuery(query);
        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _db.ExecuteQuery($"DELETE FROM Employee WHERE Id = {id}");
        return Ok("Deleted");
    }
    [HttpGet("report")]
    public IActionResult GetEmployeeReport()
    {
        var employees = _db.ExecuteQuery("SELECT * FROM Employee"); // Fetch data explicitly
        var list = new List<Dictionary<string, object>>();

        foreach (DataRow row in employees.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in employees.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }

        LocalReport report = new LocalReport();
        report.ReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "EmployeeReport.rdlc");
        report.DataSources.Add(new ReportDataSource("EmployeeDataSet", list));

        var result = report.Render("PDF");

        return File(result, "application/pdf", "EmployeeReport.pdf");
    }
}

