using CTS_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models.SkipModels
{
	public class SkipWeightWithAct
	{
		public SkipWeight SkipWeight { get; set; }
		public string ActFilePath { get; set; }

		public SkipWeightWithAct(SkipWeight skipWeight)
		{
			this.SkipWeight = skipWeight;
			try
			{
				this.ActFilePath = Directory.EnumerateFiles(ProjectConstants.SkipActFolderPath, this.SkipWeight.ID + ".*").FirstOrDefault();
			}
			catch
			{
				//Directoty doen't exist :'(
			}
		}
	}
}