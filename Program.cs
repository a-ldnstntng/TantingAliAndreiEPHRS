using System;
using System.Collections.Generic;
using System.Linq;

public class Employee
{
    public int Id { get; set; }                  // Col 1: Unique ID (starts at 1001)
    public string FirstName { get; set; }        // Col 2: First name
    public string LastName { get; set; }         // Col 3: Last name
    public string Department { get; set; }       // Col 4: Department (IT, HR, Sales, Finance)
    public string JobTitle { get; set; }         // Col 5: Position title
    public decimal MonthlySalary { get; set; }   // Col 6: Base monthly salary
    public double TaxRate { get; set; }          // Col 7: Tax deduction rate (e.g., 0.12)
    public DateTime HireDate { get; set; }       // Col 8: Date of employment joining
    public bool IsActive { get; set; }           // Col 9: Active status (True/False)
    public string Email { get; set; }            // Col 10: Company email address
}

class Program
{
    // Starts with an empty list (0 pre-defined records)
    static List<Employee> employees = new List<Employee>();
    static int nextId = 1001;

    static void Main(string[] args)
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== EMPLOYEE PAYROLL & HR SYSTEM [TantingAliAndrei Branch] ===");
            Console.WriteLine("1. Create (Add Employee)");
            Console.WriteLine("2. Read (View All Employees)");
            Console.WriteLine("3. Search / Filter Employees");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit");
            Console.Write("\nSelect an option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1": CreateEmployee(); break;
                case "2": ReadEmployees(employees); break;
                case "3": SearchEmployees(); break;
                case "4": UpdateEmployee(); break;
                case "5": DeleteEmployee(); break;
                case "6": running = false; break;
                default: Console.WriteLine("Invalid choice."); break;
            }
            if (running) { Console.WriteLine("\nPress Enter to continue..."); Console.ReadLine(); }
        }
    }

    static string Prompt(string label, string current = null)
    {
        Console.Write(current == null ? $"{label}: " : $"{label} (current: {current}): ");
        string input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? current : input.Trim();
    }

    static void CreateEmployee()
    {
        Console.WriteLine("--- Add New Employee ---");
        var emp = new Employee
        {
            Id = nextId++,
            FirstName = Prompt("First Name") ?? "",
            LastName = Prompt("Last Name") ?? "",
            Department = Prompt("Department (IT, HR, Sales, Finance)") ?? "",
            JobTitle = Prompt("Job Title") ?? "",
            MonthlySalary = decimal.TryParse(Prompt("Monthly Salary"), out decimal sal) ? sal : 0m,
            TaxRate = double.TryParse(Prompt("Tax Rate (e.g., 0.12)"), out double tax) ? tax : 0.0,
            HireDate = DateTime.TryParse(Prompt("Hire Date (yyyy-MM-dd)"), out DateTime dt) ? dt : DateTime.Today,
            IsActive = bool.TryParse(Prompt("Is Active (true/false)"), out bool act) ? act : true,
            Email = Prompt("Email") ?? ""
        };
        employees.Add(emp);
        Console.WriteLine($"\nEmployee added successfully (ID: {emp.Id})!");
    }

    static void ReadEmployees(IEnumerable<Employee> list)
    {
        Console.WriteLine("--- Employee Roster ---");
        var rows = list.ToList();
        if (rows.Count == 0)
        {
            Console.WriteLine("No employee records found.");
            return;
        }

        Console.WriteLine(
            $"{"ID",-5} | {"First",-10} | {"Last",-10} | {"Dept",-8} | {"Job Title",-17} | " +
            $"{"Salary",-10} | {"Tax",-5} | {"Hire Date",-10} | {"Active",-6} | {"Email"}");
        Console.WriteLine(new string('-', 122));
        foreach (var e in rows)
        {
            Console.WriteLine(
                $"{e.Id,-5} | {e.FirstName,-10} | {e.LastName,-10} | {e.Department,-8} | {e.JobTitle,-17} | " +
                $"${e.MonthlySalary,-9:F2} | {e.TaxRate,-5:P0} | {e.HireDate:yyyy-MM-dd} | {e.IsActive,-6} | {e.Email}");
        }
    }

    static void SearchEmployees()
    {
        Console.WriteLine("--- Search / Filter Employees (LINQ) ---");
        string keyword = (Prompt("Enter search keyword (Name, Dept, Title, or Email)") ?? "").ToLower();
        var results = employees.Where(e =>
            e.FirstName.ToLower().Contains(keyword) ||
            e.LastName.ToLower().Contains(keyword) ||
            e.Department.ToLower().Contains(keyword) ||
            e.JobTitle.ToLower().Contains(keyword) ||
            e.Email.ToLower().Contains(keyword)).ToList();

        Console.WriteLine($"\nFound {results.Count} matching employee(s):");
        ReadEmployees(results);
    }

    static void UpdateEmployee()
    {
        ReadEmployees(employees);
        if (!int.TryParse(Prompt("\nEnter Employee ID to update"), out int id))
        {
            Console.WriteLine("\nInvalid ID format.");
            return;
        }

        Employee emp = employees.FirstOrDefault(e => e.Id == id);
        if (emp == null)
        {
            Console.WriteLine("\nEmployee not found.");
            return;
        }

        emp.FirstName = Prompt("First Name", emp.FirstName);
        emp.LastName = Prompt("Last Name", emp.LastName);
        emp.Department = Prompt("Department", emp.Department);
        emp.JobTitle = Prompt("Job Title", emp.JobTitle);
        if (decimal.TryParse(Prompt("Monthly Salary", emp.MonthlySalary.ToString("F2")), out decimal sal)) emp.MonthlySalary = sal;
        if (double.TryParse(Prompt("Tax Rate", emp.TaxRate.ToString()), out double tax)) emp.TaxRate = tax;
        if (DateTime.TryParse(Prompt("Hire Date", emp.HireDate.ToString("yyyy-MM-dd")), out DateTime dt)) emp.HireDate = dt;
        if (bool.TryParse(Prompt("Is Active", emp.IsActive.ToString()), out bool act)) emp.IsActive = act;
        emp.Email = Prompt("Email", emp.Email);

        Console.WriteLine("\nEmployee updated successfully!");
    }

    static void DeleteEmployee()
    {
        ReadEmployees(employees);
        if (!int.TryParse(Prompt("\nEnter Employee ID to delete"), out int id))
        {
            Console.WriteLine("\nInvalid ID format.");
            return;
        }

        int removed = employees.RemoveAll(e => e.Id == id);
        Console.WriteLine(removed > 0 ? "\nEmployee deleted successfully!" : "\nEmployee ID not found.");
    }
}
