﻿using LinqTasks.Models;

namespace LinqTasks.Extensions;

public static class CustomExtensionMethods
{
    //Put your extension methods here
    public static IEnumerable<Emp> GetEmpsWithSubordinates(this IEnumerable<Emp> emps)
    {
        return emps
            .Where(mgr => emps.Any(emp => emp.Mgr != null && mgr.Empno == emp.Mgr.Empno))
            .OrderBy(emp => emp.Ename)
            .ThenByDescending(emp => emp.Salary);
    }
}