using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenWeb.Models;

namespace CodeGenWeb.Generators
{
    class ServiceGenerator : Generator
    {
        public static string Generate(Model m)
        {
            string keyName = "";
            string keyType = "";

            foreach (Property pr in m.Properties)
            {
                if (pr.IsKey)
                {
                    keyName = pr.Name;
                    keyType = pr.Type;
                }
            }

            string text = "";

            text += "using System;\r\n";
            text += "using System.Collections.Generic;\r\n";
            text += "using " + m.NameProject + ".Repositories;\r\n";
            text += "using " + m.NameProject + ".Models;\r\n\r\n";

            text += "namespace " + m.NameProject + ".Services\r\n";
            text += "{\r\n";

            text += "\tpublic class " + m.Name + "Service\r\n";
            text += "\t{\r\n";

            text += "\t\tprivate " + LowercaseFirst(m.Name) + "Repository _" + m.Name + "Repository;\r\n\r\n";
            text += "\t\tpublic " + m.Name + "Service()\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\t_" + LowercaseFirst(m.Name) + "Repository = new " + m.Name + "Repository();\r\n";
            text += "\t\t}\r\n\r\n";

            //Get
            text += "\t\tpublic " + m.Name + " Get(" + keyType + " " + keyName + ")\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\treturn _" + LowercaseFirst(m.Name) + "Repository.Get(" + keyName + ");\r\n";
            text += "\t\t}\r\n\r\n";

            //List
            text += "\t\tpublic IEnumerable<" + m.Name + "> List()\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\treturn _" + LowercaseFirst(m.Name) + "Repository.List();\r\n";
            text += "\t\t}\r\n\r\n";

            //Post
            text += "\t\tpublic bool Post(" + m.Name + " obj)\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\treturn _" + LowercaseFirst(m.Name) + "Repository.Post(obj);\r\n";
            text += "\t\t}\r\n\r\n";

            //Put
            text += "\t\tpublic bool Put(" + m.Name + " obj)\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\treturn _" + LowercaseFirst(m.Name) + "Repository.Put(obj);\r\n";
            text += "\t\t}\r\n\r\n";

            //Delete
            text += "\t\tpublic bool Delete(" + keyType + " " + keyName + ")\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\treturn _" + LowercaseFirst(m.Name) + "Repository.Delete(" + keyName + ");\r\n";
            text += "\t\t}\r\n";

            text += "\t}\r\n";
            text += "}";

            return text;
        }       
    }
}
