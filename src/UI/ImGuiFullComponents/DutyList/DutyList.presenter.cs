namespace KikoGuide.UI.ImGuiFullComponents.DutyList
{
    using KikoGuide.Managers;
    using KikoGuide.Types;

    public sealed class DutyListPresenter
    {
        public Duty? GetPlayerDuty() => DutyManager.GetPlayerDuty();
        public bool HasDutyData(Duty duty) => duty.Sections != null && duty.Sections.Count > 0;
    }
}