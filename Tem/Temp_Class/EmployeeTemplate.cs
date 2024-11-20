using Klochko_Site.Models;
using Microsoft.EntityFrameworkCore;
using NGS.Templater;

internal class EmployeeTemplate
{
    public void DoWork()
    {
        // Копируем файл шаблона
        File.Copy(@"EmployeeTemplate.docx", "EmployeeReport.docx", true);

        // Создаем экземпляр контекста базы данных
        using (var context = new GeneralPostDbContext())
        {
            var employees = context.Employees
                                   .Select(e => new
                                   {
                                       e.EmployeeId,
                                       e.FullName,
                                       e.Position,
                                       e.Phone,
                                       e.Email,
                                       Transactions = e.Transactions.Select(t => new
                                       {
                                           t.TransactionType,
                                           t.TransactionDate,
                                           t.Amount
                                       }).ToList()
                                   })
                                   .ToList();

            var data = new Dictionary<string, object>
            {
                ["Employee"] = employees
            };

            var factory = Configuration.Builder.Build();
            using (var doc = factory.Open("EmployeeReport.docx"))
            {
                doc.Process(data);
            }
        }
    }
}
