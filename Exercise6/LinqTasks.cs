using System;
using System.Collections.Generic;
using System.Linq;
using Exercise6.Models;

namespace Exercise6
{
    public static class LinqTasks
    {
        public static IEnumerable<Emp> Emps { get; set; }
        public static IEnumerable<Dept> Depts { get; set; }

        // Task 1: SELECT * FROM Emps WHERE Job = "Backend programmer";
        public static IEnumerable<Emp> Task1()
        {
            return Emps.Where(emp => emp.Job == "Backend programmer");
        }

        // Task 2: SELECT * FROM Emps Job = "Frontend programmer" AND Salary > 1000 ORDER BY Ename DESC;
        public static IEnumerable<Emp> Task2()
        {
            return Emps.Where(emp => emp.Job == "Frontend programmer" && emp.Salary > 1000)
                       .OrderByDescending(emp => emp.Ename);
        }

        // Task 3: SELECT MAX(Salary) FROM Emps;
        public static int Task3()
        {
            return Emps.Max(emp => emp.Salary);
        }

        // Task 4: SELECT * FROM Emps WHERE Salary = (SELECT MAX(Salary) FROM Emps);
        public static IEnumerable<Emp> Task4()
        {
            int maxSalary = Emps.Max(emp => emp.Salary);
            return Emps.Where(emp => emp.Salary == maxSalary);
        }

        // Task 5: SELECT ename AS Nazwisko, job AS Praca FROM Emps;
        public static IEnumerable<dynamic> Task5()
        {
            return Emps.Select(emp => new { Nazwisko = emp.Ename, Praca = emp.Job });
        }

        // Task 6: SELECT Emps.Ename, Emps.Job, Depts.Dname FROM Emps INNER JOIN Depts ON Emps.Deptno = Depts.Deptno;
        public static IEnumerable<dynamic> Task6()
        {
            return Emps.Join(Depts, emp => emp.Deptno, dept => dept.Deptno,
                             (emp, dept) => new { emp.Ename, emp.Job, dept.Dname });
        }

        // Task 7: SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps GROUP BY Job;
        public static IEnumerable<dynamic> Task7()
        {
            return Emps.GroupBy(emp => emp.Job)
                       .Select(g => new { Praca = g.Key, LiczbaPracownikow = g.Count() });
        }

        // Task 8: Return true if any employee works as "Backend programmer"
        public static bool Task8()
        {
            return Emps.Any(emp => emp.Job == "Backend programmer");
        }

        // Task 9: SELECT TOP 1 * FROM Emp WHERE Job = "Frontend programmer" ORDER BY HireDate DESC;
        public static Emp Task9()
        {
            return Emps.Where(emp => emp.Job == "Frontend programmer")
                       .OrderByDescending(emp => emp.HireDate)
                       .FirstOrDefault();
        }

        // Task 10: SELECT Ename, Job, Hiredate FROM Emps UNION SELECT "Brak wartości", null, null;
        public static IEnumerable<dynamic> Task10()
        {
            var employees = Emps.Select(emp => new { emp.Ename, emp.Job, emp.HireDate });
            var brakWartosci = new[] { new { Ename = "Brak wartości", Job = (string)null, HireDate = (DateTime?)null } };
            return employees.Union(brakWartosci);
        }

        // Task 11: Return list of departments with more than 1 employee
        public static IEnumerable<dynamic> Task11()
        {
            return Emps.GroupBy(emp => emp.Deptno)
                       .Where(g => g.Count() > 1)
                       .Select(g => new { Name = Depts.FirstOrDefault(dept => dept.Deptno == g.Key)?.Dname, NumOfEmployees = g.Count() });
        }

        // Task 12: Return employees with at least 1 direct subordinate, sorted by name and salary
        public static IEnumerable<Emp> Task12()
        {
            return Emps.Where(emp => Emps.Any(e => e.Mgr?.Empno == emp.Empno))
                       .OrderBy(emp => emp.Ename)
                       .ThenByDescending(emp => emp.Salary);
        }

        // Task 13: Return number appearing an odd number of times in array
        public static int Task13(int[] array)
        {
            return array.GroupBy(x => x)
                        .Where(g => g.Count() % 2 != 0)
                        .Select(g => g.Key)
                        .First();
        }

        // Task 14: Return departments with 5 or no employees, sorted by name
        public static IEnumerable<Dept> Task14()
        {
            var deptWithEmployeeCount = Emps.GroupBy(emp => emp.Deptno)
                                            .Select(g => new { Deptno = g.Key, Count = g.Count() });

            return Depts.Where(dept => deptWithEmployeeCount.Any(d => d.Deptno == dept.Deptno && d.Count == 5) ||
                                       !deptWithEmployeeCount.Any(d => d.Deptno == dept.Deptno))
                        .OrderBy(dept => dept.Dname);
        }
    }
}
