using LinqTasks.Extensions;
using LinqTasks.Models;

namespace LinqTasks;

public static partial class Tasks
{
    public static IEnumerable<Emp> Emps { get; set; }
    public static IEnumerable<Dept> Depts { get; set; }

    static Tasks()
    {
        Depts = LoadDepts();
        Emps = LoadEmps();
    }

    /// <summary>
    ///     SELECT * FROM Emps WHERE Job = "Backend programmer";
    /// </summary>
    public static IEnumerable<Emp> Task1()
    {
        return Emps
            .Where(e => e.Job.Equals("Backend programmer"));
    }

    /// <summary>
    ///     SELECT * FROM Emps Job = "Frontend programmer" AND Salary>1000 ORDER BY Ename DESC;
    /// </summary>
    public static IEnumerable<Emp> Task2()
    {
        return Emps
            .Where(e => e.Job.Equals("Frontend programmer") && e.Salary > 1000)
            .OrderByDescending(e => e.Ename);
    }


    /// <summary>
    ///     SELECT MAX(Salary) FROM Emps;
    /// </summary>
    public static int Task3()
    {
        return Emps
            .Max(e => e.Salary);
    }

    /// <summary>
    ///     SELECT * FROM Emps WHERE Salary=(SELECT MAX(Salary) FROM Emps);
    /// </summary>
    public static IEnumerable<Emp> Task4()
    {
        return Emps
            .Where(e => e.Salary == Emps.Max(e1 => e1.Salary));
    }

    /// <summary>
    ///    SELECT ename AS Nazwisko, job AS Praca FROM Emps;
    /// </summary>
    public static IEnumerable<object> Task5()
    {
        return Emps
            .Select(e => new
            {
                Nazwisko = e.Ename,
                Praca = e.Job
            });
    }

    /// <summary>
    ///     SELECT Emps.Ename, Emps.Job, Depts.Dname FROM Emps
    ///     INNER JOIN Depts ON Emps.Deptno=Depts.Deptno
    ///     Rezultat: Złączenie kolekcji Emps i Depts.
    /// </summary>
    public static IEnumerable<object> Task6()
    {
        return Emps
            .Join(Depts,
                e => e.Deptno,
                d => d.Deptno,
                (e, d) => new
                {
                    e.Ename,
                    e.Job,
                    d.Dname
                });
    }

    /// <summary>
    ///     SELECT Job AS Praca, COUNT(1) LiczbaPracownikow FROM Emps GROUP BY Job;
    /// </summary>
    public static IEnumerable<object> Task7()
    {
        return Emps
            .GroupBy(e => e.Job)
            .Select(e1 => new
            {
                Praca = e1.Key,
                LiczbaPracownikow = e1.Count()
            });
    }

    /// <summary>
    ///     Zwróć wartość "true" jeśli choć jeden
    ///     z elementów kolekcji pracuje jako "Backend programmer".
    /// </summary>
    public static bool Task8()
    { 
        return Emps.Any(e => e.Job.Equals("Backend programmer"));
    }

    /// <summary>
    ///     SELECT TOP 1 * FROM Emp WHERE Job="Frontend programmer"
    ///     ORDER BY HireDate DESC;
    /// </summary>
    public static Emp Task9()
    {
        return Emps
            .Where(e => e.Job.Equals("Frontend programmer"))
            .OrderByDescending(e => e.HireDate)
            .First();
    }

    /// <summary>
    ///     SELECT Ename, Job, Hiredate FROM Emps
    ///     UNION
    ///     SELECT "Brak wartości", null, null;
    /// </summary>
    public static IEnumerable<object> Task10()
    {
        return Emps
            .Select(e => new
            {
                e.Ename,
                e.Job,
                e.HireDate
            })
            .Union(new List<object>()
            {
                new
                {
                    Ename = "Brak wartości",
                    Job = (string) null,
                    HireDate = (DateTime?) null
                }
            });
    }

    /// <summary>
    ///     Wykorzystując LINQ pobierz pracowników podzielony na departamenty pamiętając, że:
    ///     1. Interesują nas tylko departamenty z liczbą pracowników powyżej 1
    ///     2. Chcemy zwrócić listę obiektów o następującej srukturze:
    ///     [
    ///     {name: "RESEARCH", numOfEmployees: 3},
    ///     {name: "SALES", numOfEmployees: 5},
    ///     ...
    ///     ]
    ///     3. Wykorzystaj typy anonimowe
    /// </summary>
    public static IEnumerable<object> Task11()
    {
        return Depts.GroupJoin(
                Emps,
                dept => dept.Deptno,
                emp => emp.Deptno,
                (dept, emp) => new
                {
                    Name = dept.Dname,
                    NumOfEmployees = emp.Count()
                })
            .Where(d => d.NumOfEmployees > 0);
    }

    /// <summary>
    ///     Napisz własną metodę rozszerzeń, która pozwoli skompilować się poniższemu fragmentowi kodu.
    ///     Metodę dodaj do klasy CustomExtensionMethods, która zdefiniowana jest poniżej.
    ///     Metoda powinna zwrócić tylko tych pracowników, którzy mają min. 1 bezpośredniego podwładnego.
    ///     Pracownicy powinny w ramach kolekcji być posortowani po nazwisku (rosnąco) i pensji (malejąco).
    /// </summary>
    public static IEnumerable<Emp> Task12()
    {
        IEnumerable<Emp> result = Emps.GetEmpsWithSubordinates();
        
        return result;
    }

    /// <summary>
    ///     Poniższa metoda powinna zwracać pojedyczną liczbę int.
    ///     Na wejściu przyjmujemy listę liczb całkowitych.
    ///     Spróbuj z pomocą LINQ'a odnaleźć tę liczbę, które występuja w tablicy int'ów nieparzystą liczbę razy.
    ///     Zakładamy, że zawsze będzie jedna taka liczba.
    ///     Np: {1,1,1,1,1,1,10,1,1,1,1} => 10
    /// </summary>
    public static int Task13(int[] arr)
    {
        return arr
            .GroupBy(e => e)
            .Where(e => e.Count() % 2 != 0)
            .Select(e => e.Key)
            .First();
    }

    /// <summary>
    ///     Zwróć tylko te departamenty, które mają 5 pracowników lub nie mają pracowników w ogóle.
    ///     Posortuj rezultat po nazwie departament rosnąco.
    /// </summary>
    public static IEnumerable<Dept> Task14()
    {
        return Depts
            .Where(d => Emps.Count(e => e.Deptno == d.Deptno) == 5 || Emps.Count(e => e.Deptno == d.Deptno) == 0)
            .OrderBy(d => d.Dname);
    //     return Depts.GroupJoin(
    //             Emps,
    //             d => d.Deptno,
    //             e => e.Deptno,
    //             (dept, emp) => new
    //             {
    //                 dept,
    //                 numOfEmps = emp.Count()
    //             })
    //         .Where(d => d.numOfEmps.Equals(5) || d.numOfEmps.Equals(0))
    //         .Select(d => d.dept)
    //         .OrderBy(d => d.Dname);
    }
}