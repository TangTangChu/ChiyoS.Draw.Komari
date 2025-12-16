using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiyoS.Draw
{
    public class Students
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// b
        /// </summary>
        public string s { get; set; }
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
