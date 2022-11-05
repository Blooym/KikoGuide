using KikoGuide.Base;
using KikoGuide.Types;

namespace KikoGuide.UI.ImGuiFullComponents.DutyList
{
    public sealed class DutyListPresenter
    {
        public static Duty? GetPlayerDuty() => PluginService.DutyManager.GetPlayerDuty();

        public static bool HasDutyData(Duty duty) => duty.Sections != null && duty.Sections.Count > 0;
    }
}