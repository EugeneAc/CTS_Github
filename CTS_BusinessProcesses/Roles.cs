/// Имена здесь должны совпадать с именами в БД 
/// Названия локация должны совпадать с БД в таблице Roles 

namespace CTS_Core
{
	public static class Roles
	{
		public const string EditUserRoleName = "CTSmi_EditUser";
		public const string DeleteUserRoleName = "CTSmi_DeleteUser";
		public const string AddUserRoleName = "CTSmi_AddUser";
		public const string ApproveUserRoleName = "CTSmi_ApproveUser";
		public const string SkipUserRoleName = "CTSmi_Skip";
		public const string BeltUserRoleName = "CTSmi_Belt";
		public const string WagonUserRoleName = "CTSmi_Wagon";
		public const string LabUserRoleName = "CTSmi_Lab";
		public const string VehiUserRoleName = "CTSmi_Vehicle";
		public const string DictUserRoleName = "CTSmi_Dict";
		public const string RockUserRoleName = "CTSmi_Rock";
		public const string WarehouseUserRoleName = "CTSmi_Warehouse";
		public const string WarehouseSetUserRoleName = "CTSmi_WarehouseSet";

		public const string AnalyticsRoleName = "CTS_Analytics";
        public const string AnalyticsDashboardRoleName = "ANL_dashboards";

        public const string RoleAdminRoleName = "CTS_RoleAdmin";

		public const string AdminDebugRoleName = "Administrators, Администраторы";

        // эло должно совпадать с русскими названиями локаций в Locations
        public const string StUglRoleName = "ст. \"Углерудная\"";
        public const string StAbayRoleName = "ст. «Абай»";
        public const string StDubovRoleName = "ст. «Дубовская»";
        public const string StPridolRoleName = "ст. «Придолинская»";
        public const string StRaspRoleName = "ст. «Распорядительная»";
        public const string CofvRoleName = "ЦОФ «Восточная»";
        public const string MineAbayRoleName = "ш. Абайская";
        public const string MineKazRoleName = "ш. Казахстанская";
        public const string MineKostRoleName = "ш. Костенко";
        public const string MineKuzRoleName = "ш. Кузембаева";
        public const string MineLenRoleName = "ш. Ленина";
        public const string MineSarRoleName = "ш. Саранская";
        public const string MineSar1RoleName = "ш. Саранская район 1";
        public const string MineSar3RoleName = "ш. Саранская район 3";
        public const string MineTentRoleName = "ш. Тентекская";
        public const string MineShahRoleName = "ш. Шахтинская";

        // роли отчетов
        public const string OverallProductionReportRole = "REP_OverallProduction";
        public const string AdvancedOverallMineProductionReportRole = "REP_AdvancedOverallMineProduction";
        public const string OverallMineProductionReportRole = "REP_OverallMineProduction";
        public const string OperCoalProductionReportRole = "REP_OperCoalProductionReport";
        public const string AlarmReportRole = "REP_AlarmReport";
        public const string CoalProductionByMineReportRole = "REP_CoalProductionByMine";
        public const string StockpilesMonthReportRole = "REP_StockpilesMonth";
        public const string WagonScalesReportRole = "REP_WagonScalesReport";
        public const string GlobalSafetyReportRole = "REP_GlobalSafety";
        public const string SkipReportRole = "REP_SkipReport";
        public const string SiteSafetyReportRole = "REP_SiteSafety";
        public const string BeltScalesReportRole = "REP_BeltScalesReport";
        public const string ComparisonInfoMiningsReportRole = "REP_ComparisonInfoMiningsReport";
        public const string QualityControlReportRole = "REP_QualityControlReport";
        public const string DynamicReportRole = "REP_DynamicReports";
    }
}