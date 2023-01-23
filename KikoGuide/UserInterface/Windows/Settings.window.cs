using Dalamud.Interface.Windowing;
using ImGuiNET;
using KikoGuide.Common;

namespace KikoGuide.UserInterface.Windows
{
    public sealed class SettingsWindow : Window
    {
        public SettingsWindow() : base(Constants.Windows.SettingsTitle)
        {
            this.Size = new(400, 400);
            this.SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void Draw()
        {
            var guides = Services.GuideManager.Guides;
            ImGui.Text("Guide");
            ImGui.Separator();
            ImGui.Text("Current Guide: " + Services.GuideManager.CurrentGuide?.Name);

            foreach (var guide in guides)
            {
                ImGui.Text(guide.Name);
                ImGui.Separator();

                if (guide.Content.Sections is not null)
                {
                    foreach (var section in guide.Content.Sections)
                    {
                        ImGui.Text(section.Title.UICurrent);
                        ImGui.Separator();

                        if (section.SubSections is not null)
                        {
                            foreach (var subsection in section.SubSections)
                            {
                                ImGui.Text(subsection.Content.UICurrent);
                                ImGui.Separator();

                                if (subsection.Mechanics is not null)
                                {
                                    foreach (var mechanic in subsection.Mechanics)
                                    {
                                        ImGui.Text(mechanic.Name.UICurrent);
                                        ImGui.Text(mechanic.Description.UICurrent);
                                        ImGui.Separator();
                                    }
                                }

                                if (subsection.Tips is not null)
                                {
                                    foreach (var tip in subsection.Tips)
                                    {
                                        ImGui.BulletText(tip.Content.UICurrent);
                                        ImGui.Separator();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
