namespace CodeGenWeb.Models
{
    public class Property
    {
        private string type;
        private string name;
        private bool isKey;

        #region Properties
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

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

        public bool IsKey
        {
            get
            {
                return isKey;
            }

            set
            {
                isKey = value;
            }
        }
        #endregion

        #region Constructor
        public Property() { }
        #endregion
    }
}
