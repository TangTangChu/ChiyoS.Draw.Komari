using System.Collections.Generic;

namespace ChiyoS.Draw.Komari
{
    public class List
    {
        public string text { get; set; }
        public string path { get; set; }
        public string color { get; set; }
    }

    public class Root_bg
    {
        public List<List> list { get; set; }
        public int interval { get; set; }
    }
}
