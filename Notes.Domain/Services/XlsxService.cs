using System.Collections.Generic;
using System.IO;
using Notes.Domain.Models;
using OfficeOpenXml;

namespace Notes.Domain.Services
{
    public class XlsxService
    {
        public void Save(List<UserViewModel> users, string name)
        {
            using (var package = new ExcelPackage(new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), name)).Create()))
            {
                package.Workbook.Worksheets.Add("Users");
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                for (int i = 0; i < users.Count; i++)
                {
                    worksheet.Cells[i + 1, 1].Value = users[i].UserName;
                    worksheet.Cells[i + 1, 2].Value = users[i].NotesAmount;
                }

                package.Save();
            }
        }
    }
}