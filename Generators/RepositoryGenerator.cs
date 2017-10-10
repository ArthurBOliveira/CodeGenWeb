using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenWeb.Models;

namespace CodeGenWeb.Generators
{
    class RepositoryGenerator : Generator
    {
        public static string Generate(Model m)
        {            
            string keyName = "";
            string keyType = "";
            string properties = "";
            List<string> prop = new List<string>();
            List<string> propNoKey = new List<string>();

            foreach (Property pr in m.Properties)
            {
                prop.Add(pr.Name);

                if (pr.IsKey)
                {
                    keyName = pr.Name;
                    keyType = pr.Type;
                }
                else
                {
                    propNoKey.Add(pr.Name);
                }             
            }

            properties = String.Join(",", prop);

            string text = "";

            text += "using System;\r\n";
            text += "using Dapper;\r\n";
            text += "using System.Configuration;\r\n";
            text += "using System.Data.SqlClient;\r\n";
            text += "using " + m.NameProject + ".Models;\r\n\r\n";

            text += "namespace " + m.NameProject + ".Repositories\r\n";
            text += "{\r\n";

            text += "\tpublic class " + m.Name + "Repository\r\n";
            text += "\t{\r\n";

            text += "\t\tprivate string connectionStrings = ConfigurationManager.ConnectionStrings[\"ConnectionString\"].ConnectionString;\r\n\r\n";

            //Get
            text += "\t\tpublic " + m.Name + " Get(" + keyType + " " + keyName + ")\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\t" + m.Name + " result;\r\n";
            text += "\t\t\tusing (var conexaoBD = new SqlConnection(connectionStrings))\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\tStringBuilder query = new StringBuilder();\r\n";
            text += "\t\t\t\tquery.Append(\"select top 1 \");\r\n";
            text += "\t\t\t\tquery.Append(\" " + properties + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" from \");\r\n";
            text += "\t\t\t\tquery.Append(\" dbo." + m.Name + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" where \");\r\n";
            text += "\t\t\t\tquery.Append(\" " + keyName + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" = \" + " + keyName + ".ToString() + \"\");\r\n\r\n";

            text += "\t\t\t\tresult = conexaoBD.QuerySingleOrDefault<" + keyName + ">(query.ToString());\r\n";
            text += "\t\t\t}\r\n\r\n";

            text += "\t\t\treturn result;\r\n";
            text += "\t\t}\r\n\r\n";

            //List
            text += "\t\tpublic IEnumerable<" + m.Name + "> List()\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\tIEnumerable<" + m.Name + "> result = new List<" + m.Name + ">();\r\n";
            text += "\t\t\tusing (var conexaoBD = new SqlConnection(connectionStrings))\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\tStringBuilder query = new StringBuilder();\r\n";
            text += "\t\t\t\tquery.Append(\"select \");\r\n";
            text += "\t\t\t\tquery.Append(\" " + properties + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" from \");\r\n";
            text += "\t\t\t\tquery.Append(\" dbo." + m.Name + " \");\r\n\r\n";

            text += "\t\t\t\tresult = conexaoBD.Query<" + m.Name + ">(query.ToString());\r\n";
            text += "\t\t\t}\r\n\r\n";

            text += "\t\t\treturn result;\r\n";
            text += "\t\t}\r\n\r\n";


            //Post
            text += "\t\tpublic bool Post<" + m.Name + ">(" + m.Name + " obj)\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\tint result;\r\n";
            text += "\t\t\tusing (var conexaoBD = new SqlConnection(connectionStrings))\r\n";
            text += "\t\t\t{\r\n";

            text += "\t\t\t\tStringBuilder query = new StringBuilder();\r\n";
            text += "\t\t\t\tquery.Append(\"insert into \");\r\n";
            text += "\t\t\t\tquery.Append(\" dbo." + m.Name + " \");\r\n";
            text += "\t\t\t\tquery.Append(\"(\");\r\n";
            text += "\t\t\t\tquery.Append(\" " + properties + " \");\r\n";
            text += "\t\t\t\tquery.Append(\") values (\");\r\n";
            text += "\t\t\t\tquery.Append(\" @" + String.Join(",@", prop) + " \");\r\n";
            text += "\t\t\t\tquery.Append(\")\");\r\n\r\n";

            text += "\t\t\t\tresult = conexaoBD.Execute(query.ToString(), obj);\r\n";
            text += "\t\t\t}\r\n\r\n";

            text += "\t\t\treturn result > 0;\r\n";
            text += "\t\t}\r\n\r\n";

            //Put
            text += "\t\tpublic bool Put<" + m.Name + ">(" + m.Name + " obj)\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\tint result;\r\n";
            text += "\t\t\tusing (var conexaoBD = new SqlConnection(connectionStrings))\r\n";
            text += "\t\t\t{\r\n";

            text += "\t\t\t\tStringBuilder query = new StringBuilder();\r\n";
            text += "\t\t\t\tquery.Append(\"update \");\r\n";
            text += "\t\t\t\tquery.Append(\" dbo." + m.Name + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" set \");\r\n";
            text += "\t\t\t\tquery.Append(\" @" + String.Join(",@", propNoKey) + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" where \");\r\n";
            text += "\t\t\t\tquery.Append(\" " + keyName + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" = \" + @" + keyName + ".ToString() + \"\");\r\n\r\n";

            text += "\t\t\t\tresult = conexaoBD.Execute(query.ToString(), obj);\r\n";
            text += "\t\t\t}\r\n\r\n";

            text += "\t\t\treturn result > 0;\r\n";
            text += "\t\t}\r\n\r\n";

            //Delete
            text += "\t\tpublic bool Delete(" + keyType + " " + keyName + ")\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\tint result;\r\n";
            text += "\t\t\tusing (var conexaoBD = new SqlConnection(connectionStrings))\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\tStringBuilder query = new StringBuilder();\r\n";
            text += "\t\t\t\tquery.Append(\"delete \");\r\n";
            text += "\t\t\t\tquery.Append(\" dbo." + m.Name + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" where \");\r\n";
            text += "\t\t\t\tquery.Append(\" " + keyName + " \");\r\n";
            text += "\t\t\t\tquery.Append(\" = \" + " + keyName + ".ToString() + \"\");\r\n\r\n";

            text += "\t\t\t\tresult = conexaoBD.Execute(query.ToString(), obj);\r\n";
            text += "\t\t\t}\r\n\r\n";

            text += "\t\t\treturn result > 0;\r\n";
            text += "\t\t}\r\n";

            text += "\t}\r\n";
            text += "}";            

            return text;
        }        
    }
}
