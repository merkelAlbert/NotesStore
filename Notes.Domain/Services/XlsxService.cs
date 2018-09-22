using System;
using System.Collections.Generic;
using System.IO;
using Notes.Domain.Interfaces;
using Notes.Domain.Models;
using OfficeOpenXml;

namespace Notes.Domain.Services
{
    public class XlsxService : IXlsxService
    {
        public void Save(List<UserViewModel> users, string fileName)
        {
            using (var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), fileName + ".xlsx")).Create())
            {
                using (var package =
                    new ExcelPackage(file))
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
}