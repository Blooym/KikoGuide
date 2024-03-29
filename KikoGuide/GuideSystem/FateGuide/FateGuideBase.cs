using System;
using Dalamud.Utility;
using KikoGuide.Common;
using KikoGuide.DataModels;
using Lumina.Excel.GeneratedSheets;
using Sirensong.Game.Enums;
using ContentType = Sirensong.Game.Enums.ContentType;

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
            this.Note = Note.CreateOrLoad($"{this.ContentType}_{this.Fate.Name}");

            Services.Container.GetOrCreateService<FateConductorService>();
        }

        public override GuideConfigurationBase Configuration => FateGuideConfiguration.Instance;
        public override string Name { get; }
        public override ContentType ContentType { get; } = ContentType.Fates;
        public override string Description { get; }
        public override uint Icon { get; }
        public override ContentDifficulty Difficulty { get; } = ContentDifficulty.Normal;
        public override bool IsUnlocked { get; } = true;
        public Fate Fate { get; }
        protected abstract uint FateId { get; }
        public override Note Note { get; }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Services.Container.RemoveService<FateConductorService>();
            }

            base.Dispose(disposing);
        }
    }
}
