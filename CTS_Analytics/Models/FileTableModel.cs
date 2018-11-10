using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models
{
    public class FileTableModel
    {
        public List<file> FileList { get; set; }
    }
    public class file
    {
        public string Location { get; set; }
        public string Equipment { get; set; }
        public string RportPeriod { get; set; }
        public string Author { get; set; }
        public string FileName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}