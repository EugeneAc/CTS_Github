using CTS_Manual_Input.Models.SkipModels;
using CTS_Models;
using System.Collections.Generic;

namespace CTS_Manual_Input.Models.Dictionary
{
    public class SkipsSkipWeights
	{
        public IList<Skip> Skips { get; set; }
		public PagedList.IPagedList<SkipWeightWithAct> SkipWeightsActs { get; set; }
    }
}