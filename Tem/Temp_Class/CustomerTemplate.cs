using Klochko_Site.Models;
using NGS.Templater;

internal class CustomerTemplate
{
    public void DoWork()
    {
        // Копируем файл шаблона
        File.Copy(@"CustomerTemplate.docx", "CustomerReport.docx", true);

        // Создаем экземпляр контекста базы данных
        using (var context = new GeneralPostDbContext())
        {
            var customers = context.Customers
                .Select(c => new
                {
                    c.CustomerId,
                    c.FullName,
                    c.Phone,
                    c.Email,
                    c.Address,
                    c.DateRegistered,
                })
                .ToList();

            var data = new Dictionary<string, object>
            {
                ["Customer"] = customers
            };

            var factory = Configuration.Builder.Build();
            using (var doc = factory.Open("CustomerReport.docx"))
            {
                doc.Process(data);
            }
        }
    }
}
