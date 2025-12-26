namespace AssetGuard.WebUI.Models
{
    public class UserThemeSettings
    {
        public string SidebarColor { get; set; } = "default";
        public bool IsDarkMode { get; set; } = false;
        public string ChartType { get; set; } = "doughnut";
    }
}