using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Models
{
    public class File
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public string OriginalPath { get; set; } 

    }
}
