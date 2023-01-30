using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classification
{
    internal class ClassElement
    {
        public List<string> classlist;
        public int classnumber { get; set; }
        public ClassElement(int classnumber)
        {
            classlist = new List<string>();
            this.classnumber = classnumber;
        }
    }
}
