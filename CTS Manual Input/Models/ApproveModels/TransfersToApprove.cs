using CTS_Models;
using System.Collections.Generic;

namespace CTS_Manual_Input.Models.ApproveModels
{
	public class TransfersToApprove <T> where T : class, ITransfer
	{
		public List<T> CreatedTransfers { get; set; }
		public List<T> EditedTransfers { get; set; }
		public List<T> ObsoleteTransfers { get; set; }
		public List<T> DeletedTransfers { get; set; }
	}
}