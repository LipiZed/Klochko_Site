using Klochko_Site.Models;
using NGS.Templater;
using System.Collections.Generic;
using System.IO;
using System.Linq;

internal class PackageTemplate
{
    public void DoWork()
    {
        // Копируем файл шаблона
        File.Copy(@"PackageTemplate.docx", "PackageReport.docx", true);

        // Создаем экземпляр контекста базы данных
        using (var context = new GeneralPostDbContext())
        {
            var packages = context.Packages
                .Select(p => new
                {
                    p.PackageId,
                    SenderName = p.Sender.FullName,
                    ReceiverName = p.Receiver.FullName,
                    BranchName = p.Branch.BranchName,
                    TypeName = p.Type.TypeName,
                    p.Weight,
                    p.Dimensions,
                    p.Status,
                    p.DateSent,
                    p.DateDelivered,
                })
                .ToList();

            var data = new Dictionary<string, object>
            {
                ["Package"] = packages
            };

            var factory = Configuration.Builder.Build();
            using (var doc = factory.Open("PackageReport.docx"))
            {
                doc.Process(data);
            }
        }
    }
}
