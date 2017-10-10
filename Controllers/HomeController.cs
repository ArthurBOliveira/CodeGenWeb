using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using CodeGenWeb.Models;
using CodeGenWeb.Generators;

namespace CodeGenWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public FileResult GetFile()
        {
            Model m = new Model()
            {
                Name = "Person",
                NameProject = "Teste",
                Properties = new List<Property>()
            };

            m.Properties.Add(new Property() { Name = "Id", Type = "Guid", IsKey = true });
            m.Properties.Add(new Property() { Name = "Name", Type = "string", IsKey = false });
            m.Properties.Add(new Property() { Name = "Age", Type = "int", IsKey = false });

            //string fileName = m.Name + "Repository.cs";
            //string text = RepositoryGenerator.Generate(m);

            //string fileName = m.Name + ".sql";
            //string text = TableGenerator.Generate(m, new List<Model>());

            //string fileName = m.Name + "Service.cs";
            //string text = ServiceGenerator.Generate(m);

            string fileName = m.Name + "Controller.cs";
            string text = APIControllerGenerator.Generate(m);

            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            tw.WriteLine(text);

            tw.Flush();
            byte[] bytes = ms.ToArray();
            ms.Dispose();

            return File(bytes, "application/x-msdownload", fileName);
        }

        [HttpPost("UploadFiles")]
        public FileResult Post(List<IFormFile> files)
        {
            List<Model> Models = new List<Model>();
            long size = files.Sum(f => f.Length);

            try
            {
                var filePath = Path.GetTempFileName();

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var reader = new StreamReader(formFile.OpenReadStream()))
                        {
                            string text = reader.ReadToEnd();
                            reader.Dispose();

                            string[] models = text.Split(new string[] { "class" }, StringSplitOptions.None);

                            for (int i = 1; i < models.Length; i++)
                            {
                                Model m = new Model(models[i], "Test");

                                Models.Add(m);
                            }
                        }
                    }
                }

                string fileName = "Project.txt";
                byte[] bytes = new byte[0];

                MemoryStream ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);

                List<string> downloads = new List<string>();
                foreach (Model m in Models)
                {
                    tw.WriteLine(APIControllerGenerator.Generate(m));
                    tw.WriteLine(RepositoryGenerator.Generate(m));
                    tw.WriteLine(ServiceGenerator.Generate(m));
                    tw.WriteLine(TableGenerator.Generate(m, Models));
                }

                tw.Flush();
                bytes = ms.ToArray();
                ms.Dispose();

                return File(bytes, "application/x-msdownload", fileName);
            }
            catch (Exception ex)
            {
                return File("", "");
            }
        }
    }
}
