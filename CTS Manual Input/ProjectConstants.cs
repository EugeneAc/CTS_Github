using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input
{
    public static class ProjectConstants
    {
        public static readonly string EditUserRoleName = "CTSmi_EditUser";
        public static readonly string DeleteUserRoleName = "CTSmi_DeleteUser";
        public static readonly string AddUserRoleName = "CTSmi_AddUser";
        public static readonly string ApproveUserRoleName = "CTSmi_ApproveUser";

        public static readonly string SkipUserRoleName = "CTSmi_Skip";
        public static readonly string BeltUserRoleName = "CTSmi_Belt";
        public static readonly string WagonUserRoleName = "CTSmi_Wagon";
        public static readonly string LabUserRoleName = "CTSmi_Lab";
        public static readonly string VehiUserRoleName = "CTSmi_Vehicle";
		public static readonly string DictUserRoleName = "CTSmi_Dict";
		public static readonly string RockUserRoleName = "CTSmi_Rock";
		public static readonly string WarehouseUserRoleName = "CTSmi_Warehouse";
		public static readonly string WarehouseSetUserRoleName = "CTSmi_WarehouseSet";

		public static readonly Dictionary<int, string> ApproveStatus;

		static ProjectConstants ()
		{
			ApproveStatus = new Dictionary<int, string>
			{
				{ 0, "С оборудования" },
				{ 1, "Добавлено, ожидает" },
				{ 2, "отредактирована, ожидает" },
				{ 3, "Отредактировано (исходн.), ожидает" },
				{ 4, "На удалении, ожидает" },
				{ 5, "Добавление отклонено, запись удалена" },
				{ 6, "редактирование отклонено, восстановлено значение" },
				{ 7, "Редактирование отклонено, запись восстановлена" },
				{ 8, "Удаление отклонено, запись восстановлена" },
				{ 9, "Добавление подтверждено" },
				{ 10, "отредактирована, подтверждено" },
				{ 11, "Редактирование подтвеждено, данные значения устарели" },
				{ 12, "Удалено, подтверждено" },
			};
		}
		public static string SkipActFolderPath
		{
			get
			{
#if DEBUG
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"/App_data/SkipData");
#endif
				return ConfigurationManager.AppSettings["SkipActPath"];
			}
		}


	}
}