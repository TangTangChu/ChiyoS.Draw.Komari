using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiyoS.Draw.Komari
{
    public class Students
    {
        private string _name;
        private string _s;
        /// <summary>
        /// 
        /// </summary>
        public string name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }
            
        public string s
        {
            get => _s;
            set { _s = value; }
        }

    }

    public class Root
    {
        public string title { get; set; }
        /// <summary>
        /// Students
        /// </summary>
        public List<Students> students { get; set; }
    }
}
