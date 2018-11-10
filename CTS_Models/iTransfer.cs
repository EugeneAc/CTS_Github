using System;

namespace CTS_Models
{
	public interface ITransfer
	{
		string ID { get; set; }
		string Comment { get; set; }
		string OperatorName { get; set; }
		DateTime LasEditDateTime { get; set; }
		DateTime TransferTimeStamp { get; set; }
		bool IsValid { get; set; }
		int Status { get; set; }
		string ApprovedBy { get; set; }
		string InheritedFrom { get; set; }
		int? EquipID { get; set; }
	}
}