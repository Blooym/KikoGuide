namespace KikoGuide.UI.ImGuiFullComponents.DutyList
{
    using KikoGuide.Base;
    using KikoGuide.Types;

    public sealed class DutyListPresenter
    {
        public Duty? GetPlayerDuty() => PluginService.DutyManager.GetPlayerDuty();
        public bool HasDutyData(Duty duty) => duty.Sections != null && duty.Sections.Count > 0;
    }
}