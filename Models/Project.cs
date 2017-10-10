using System.Collections.Generic;

namespace CodeGenWeb.Models
{
    public class Project
    {
        private string name;
        private List<Model> models;

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public List<Model> Models
        {
            get
            {
                return models;
            }

            set
            {
                models = value;
            }
        }
        #endregion

        #region Constructors
        public Project()
        {
            models = new List<Model>();
        }
        #endregion
    }
}
