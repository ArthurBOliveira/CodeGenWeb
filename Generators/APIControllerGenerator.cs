using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenWeb.Models;

namespace CodeGenWeb.Generators
{
    class APIControllerGenerator : Generator
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
            text += "using System.Linq;\r\n";
            text += "using System.Web.Http;\r\n";
            text += "using System.Web.Http.Description;\r\n";
            text += "using " + m.NameProject + ".Models;\r\n";
            text += "using " + m.NameProject + ".Services;\r\n\r\n";

            text += "namespace " + m.NameProject + ".Controllers\r\n";
            text += "{\r\n";

            //Documentation
            text += "\t///<summary>\r\n";
            text += "\t/// Controller for the " + m.Name + "\r\n";
            text += "\t///</summary>\r\n";

            text += "\t[RoutePrefix(\"api/" + m.Name + "\")]\r\n";
            text += "\tpublic class " + m.Name + "Controller\r\n";
            text += "\t{\r\n";

            text += "\t\tprotected " + m.Name + "Service GetService()\r\n";
            text += "\t\t{\r\n";
            text += "\t\t\treturn new " + m.Name + "Service();\r\n";
            text += "\t\t}\r\n\r\n";


            //Documentation
            text += "\t\t///<summary>\r\n";
            text += "\t\t///GET a specific " + m.Name + "\r\n";
            text += "\t\t///</summary>\r\n";
            text += "\t\t///<param name=\"" + keyName + "\">" + m.Name + " " + keyName + "</param>\r\n";
            text += "\t\t///<returns>200 - List of " + m.Name + "</returns>\r\n";

            //Get
            text += "\t\t[HttpGet, ResponseType(typeof(" + m.Name + "))]\r\n";
            text += "\t\tpublic IHttpActionResult Get([FromUri]" + keyType + " " + keyName + ")\r\n";
            text += "\t\t{\r\n";

            text += "\t\t\ttry\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\t" + m.Name + " obj = GetService().Get(" + keyName + ");\r\n";
            text += "\t\t\t\tif (obj != null)\r\n";
            text += "\t\t\t\t\treturn Ok(obj);\r\n";
            text += "\t\t\t\telse\r\n";
            text += "\t\t\t\t\treturn NotFound();\r\n";
            text += "\t\t\t}\r\n";
            text += "\t\t\tcatch (Exception ex)\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\treturn InternalServerError(ex);\r\n";
            text += "\t\t\t}\r\n";

            text += "\t\t}\r\n\r\n";


            //Documentation
            text += "\t\t///<summary>\r\n";
            text += "\t\t///GET all " + m.Name + "s on Database\r\n";
            text += "\t\t///</summary>\r\n";
            text += "\t\t///<returns>200 - List of " + m.Name + "</returns>\r\n";

            //List
            text += "\t\t[HttpGet, ResponseType(typeof(IEnumerable<" + m.Name + ">))]\r\n";
            text += "\t\tpublic IHttpActionResult Get()\r\n";
            text += "\t\t{\r\n";

            text += "\t\t\ttry\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\tIEnumerable<" + m.Name + "> objs = GetService().List();\r\n";
            text += "\t\t\t\tif (objs != null || objs.Count() != 0)\r\n";
            text += "\t\t\t\t\treturn Ok(obj);\r\n";
            text += "\t\t\t\telse\r\n";
            text += "\t\t\t\t\treturn NotFound();\r\n";
            text += "\t\t\t}\r\n";
            text += "\t\t\tcatch (Exception ex)\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\treturn InternalServerError(ex);\r\n";
            text += "\t\t\t}\r\n";

            text += "\t\t}\r\n\r\n";


            //Documentation
            text += "\t\t///<summary>\r\n";
            text += "\t\t///POST a " + m.Name + "\r\n";
            text += "\t\t///</summary>\r\n";
            text += "\t\t///<param name=\"value\">" + m.Name + " to Post</param>\r\n";
            text += "\t\t///<returns>201</returns>\r\n";

            //Post
            text += "\t\t[HttpPost]\r\n";
            text += "\t\tpublic IHttpActionResult Post([FromBody]" + m.Name + " value)\r\n";
            text += "\t\t{\r\n";

            text += "\t\t\ttry\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\tif (GetService().Post(value))\r\n";
            text += "\t\t\t\t\treturn Created(\"Database\");\r\n";
            text += "\t\t\t\telse\r\n";
            text += "\t\t\t\t\treturn BadRequest();\r\n";
            text += "\t\t\t}\r\n";
            text += "\t\t\tcatch (Exception ex)\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\treturn InternalServerError(ex);\r\n";
            text += "\t\t\t}\r\n";

            text += "\t\t}\r\n\r\n";


            //Documentation
            text += "\t\t///<summary>\r\n";
            text += "\t\t///PUT a " + m.Name + "\r\n";
            text += "\t\t///</summary>\r\n";
            text += "\t\t///<param name=\"value\">" + m.Name + " to Update</param>\r\n";
            text += "\t\t///<returns>200</returns>\r\n";

            //Put
            text += "\t\t[HttpPut]\r\n";
            text += "\t\tpublic IHttpActionResult Put([FromBody]" + m.Name + " value)\r\n";
            text += "\t\t{\r\n";

            text += "\t\t\ttry\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\tif (GetService().Put(value))\r\n";
            text += "\t\t\t\t\treturn Ok();\r\n";
            text += "\t\t\t\telse\r\n";
            text += "\t\t\t\t\treturn BadRequest();\r\n";
            text += "\t\t\t}\r\n";
            text += "\t\t\tcatch (Exception ex)\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\treturn InternalServerError(ex);\r\n";
            text += "\t\t\t}\r\n";

            text += "\t\t}\r\n\r\n";



            //Documentation
            text += "\t\t///<summary>\r\n";
            text += "\t\t///Delete a " + m.Name + "\r\n";
            text += "\t\t///</summary>\r\n";
            text += "\t\t///<param name=\"" + keyName + "\">" + m.Name + " to Delete</param>\r\n";
            text += "\t\t///<returns>200</returns>\r\n";

            //Delete
            text += "\t\t[HttpDelete]\r\n";
            text += "\t\tpublic IHttpActionResult Delete([FromUri]" + keyType + " " + keyName + ")\r\n";
            text += "\t\t{\r\n";

            text += "\t\t\ttry\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\tif (GetService().Delete(" + keyName + "))\r\n";
            text += "\t\t\t\t\treturn Ok();\r\n";
            text += "\t\t\t\telse\r\n";
            text += "\t\t\t\t\treturn BadRequest();\r\n";
            text += "\t\t\t}\r\n";
            text += "\t\t\tcatch (Exception ex)\r\n";
            text += "\t\t\t{\r\n";
            text += "\t\t\t\treturn InternalServerError(ex);\r\n";
            text += "\t\t\t}\r\n";

            text += "\t\t}\r\n";


            text += "\t}\r\n";
            text += "}";

            return text;
        }
    }
}