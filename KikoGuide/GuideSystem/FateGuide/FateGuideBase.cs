using System;
using Dalamud.Utility;
using KikoGuide.Common;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Game.Enums;
using Enums = Sirensong.Game.Enums;

namespace KikoGuide.GuideSystem.FateGuide
{
    internal abstract class FateGuideBase : GuideBase
    {
        public FateGuideBase()
        {
            this.Fate = Services.FateCache.GetRow(this.FateId)!;
            if (this.Fate == null)
            {
                throw new InvalidOperationException("Invalid fate data.");
            }

            this.Name = this.Fate.Name.ToDalamudString().ToString();
            this.Description = this.Fate.Description.ToDalamudString().ToString();
            this.Icon = this.Fate.IconMap;

            Services.RegisterService<FateConductorService>();
        }

        public override GuideConfigurationBase Configuration => FateConfiguration.Instance;
        public override string Name { get; }
        public override Enums.ContentType ContentType { get; } = Enums.ContentType.Fates;
        public override string Description { get; }
        public override uint Icon { get; }
        public override ContentDifficulty Difficulty { get; } = ContentDifficulty.Normal;
        public override bool IsUnlocked { get; } = true;
        public Fate Fate { get; private set; }
        protected abstract uint FateId { get; }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Services.UnregisterService<FateConductorService>();
            }

            base.Dispose(disposing);
        }
    }
}