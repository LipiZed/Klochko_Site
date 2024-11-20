using Klochko_Site.Models;
using NGS.Templater;

internal class TransactionTemplate
{
    public void DoWork()
    {
        // Копируем файл шаблона
        File.Copy(@"TransactionTemplate.docx", "TransactionReport.docx", true);

        // Создаем экземпляр контекста базы данных
        using (var context = new GeneralPostDbContext())
        {
            var transactions = context.Transactions
                                      .Select(t => new
                                      {
                                          t.TransactionId,
                                          PackageId = t.Package.PackageId,
                                          EmployeeName = t.Employee.FullName,
                                          t.TransactionType,
                                          t.TransactionDate,
                                          t.Amount,
                                          
                                      })
                                      .ToList();

            var data = new Dictionary<string, object>
            {
                ["Transaction"] = transactions
            };

            var factory = Configuration.Builder.Build();
            using (var doc = factory.Open("TransactionReport.docx"))
            {
                doc.Process(data);
            }
        }
    }
}
