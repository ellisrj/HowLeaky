using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky
{
    public class BatchConfiguration
    {

        public int Index { get; set; }

        public string path { get; set; }

        public List<int> Simulations { get; set; }

        public List<int> TimeSeriesOutputs { get; set; }

        public int FormatIndex { get; set; }

        public BatchConfiguration(int theID, string thePath, int theFormatIndex)
        {
            Index = theID;
            path = thePath;
            FormatIndex = theFormatIndex;
        }
    }
}
