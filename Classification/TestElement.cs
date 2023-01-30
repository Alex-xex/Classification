using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classification
{
    internal class TestElement
    {
        public string classification { get; set; }
        public int score { get; set; }

        public TestElement(string classification)
        {
            this.classification = classification;
            score = 0;
        }

      

    }
}
