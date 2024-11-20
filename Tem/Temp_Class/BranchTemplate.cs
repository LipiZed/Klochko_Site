using Klochko_Site.Models;
using NGS.Templater;

internal class BranchTemplate
{
    public void DoWork()
    {
        // Копируем файл шаблона
        File.Copy(@"BranchTemplate.docx", "BranchReport.docx", true);

        // Создаем экземпляр контекста базы данных
        using (var context = new GeneralPostDbContext())
        {
            var branches = context.Branches
                                  .Select(b => new
                                  {
                                      b.BranchId,
                                      b.BranchName,
                                      b.Address,
                                      b.Phone,
                                      Employees = b.Employees.Select(e => new
                                      {
                                          e.FullName,
                                          e.Position
                                      }).ToList(),
                                      Packages = b.Packages.Select(p => new
                                      {
                                          p.PackageId,
                                          p.Status
                                      }).ToList()
                                  })
                                  .ToList();

            var data = new Dictionary<string, object>
            {
                ["Branch"] = branches
            };

            var factory = Configuration.Builder.Build();
            using (var doc = factory.Open("BranchReport.docx"))
            {
                doc.Process(data);
            }
        }
    }
}
