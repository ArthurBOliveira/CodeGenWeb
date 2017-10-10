using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CodeGenWeb.Models;

namespace CodeGenWeb.Generators
{
    class TableGenerator : Generator
    {
        public static string Generate(Model m, List<Model> models)
        {
            Model parent = null;
            Model grandParent = null;

            if (m.Parent != "")
                parent = models.Find(item => item.Name == m.Parent);

            if (parent != null)
                if (parent.Parent != "")
                    grandParent = models.Find(item => item.Name == parent.Parent);

            string text = "";

            text += "CREATE TABLE [dbo].[" + m.Name + "]\r\n";
            text += "(\r\n";

            for (int i = 0; i < m.Properties.Count; i++)
            {
                if (i == 0)
                {
                    if (ConvertTypeToSQL(m.Properties[i].Type) != "")
                        text += "\t[" + m.Properties[i].Name + "] " + ConvertTypeToSQL(m.Properties[i].Type) + " PRIMARY KEY";
                }
                else
                {
                    if (ConvertTypeToSQL(m.Properties[i].Type) != "")
                    {
                        text += ",\r\n\t[" + m.Properties[i].Name + "] " + ConvertTypeToSQL(m.Properties[i].Type) + " NULL";
                    }
                }
            }

            if (parent != null)
            {
                for (int i = 0; i < parent.Properties.Count; i++)
                {
                    if (ConvertTypeToSQL(parent.Properties[i].Type) != "")
                        text += ",\r\n\t[" + parent.Properties[i].Name + "] " + ConvertTypeToSQL(parent.Properties[i].Type) + " NULL";
                }
            }

            if (grandParent != null)
            {
                for (int i = 0; i < grandParent.Properties.Count; i++)
                {
                    if (ConvertTypeToSQL(grandParent.Properties[i].Type) != "")
                        text += ",\r\n\t[" + grandParent.Properties[i].Name + "] " + ConvertTypeToSQL(grandParent.Properties[i].Type) + " NULL";
                }
            }

            text += "\r\n)";

            return text;
        }
    }
}
