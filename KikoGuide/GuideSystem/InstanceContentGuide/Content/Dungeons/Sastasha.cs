namespace KikoGuide.GuideSystem.InstanceContentGuide.Content.Dungeons
{
    internal sealed class Sastasha : InstanceContentGuide
    {
        public override string[] Authors => new[] { "Kiko" };
        public override DutyGuideContent Content { get; } = new();
        public override uint DutyId { get; } = 4;
        public override uint UnlockQuestId { get; } = 66211;
    }
}
