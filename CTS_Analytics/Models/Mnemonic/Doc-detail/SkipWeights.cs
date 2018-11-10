using CTS_Models;

namespace CTS_Analytics.Models.Mnemonic.Doc_detail
{
    public class SkipWeights
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Weight { get; set; }
        public SkipWeight SkipWeight { get; set; }
        public string ActFilePath { get; set; }
    }
}