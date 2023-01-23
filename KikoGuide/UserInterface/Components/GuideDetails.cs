// using FFXIVClientStructs.FFXIV.Common.Math;
// using ImGuiNET;
// using KikoGuide.DataModels;
// using Sirensong.UserInterface;

// namespace KikoGuide.UserInterface.Components
// {
//     internal static class GuideDetails
//     {
//         public static void Draw(Guide guide)
//         {
//             // Guide image, using override if set, otherwise using the duty's image or nothing.
//             switch (guide.ImageOverride?.GetType())
//             {
//                 case var _ when guide.ImageOverride is string image:
//                     SiUI.Image(image, ScalingMode.Contain, new Vector2(ImGui.GetWindowWidth(), 200));
//                     break;
//                 case var _ when guide.ImageOverride is long imageID:
//                     SiUI.Icon((uint)imageID, ScalingMode.Contain, new Vector2(ImGui.GetWindowWidth(), 200));
//                     break;
//                 default:
//                     SiUI.Icon(guide.LinkedDuty.CFCondition.Image, ScalingMode.Contain, new Vector2(ImGui.GetWindowWidth(), 200));
//                     break;
//             }

//             // Guide title.
//             GuideNameText(guide);
//             ImGui.Separator();

//             // Guide description.
//             ImGui.TextWrapped(guide.LinkedDuty?.CFConditionTransient?.Description.ToString());
//         }

//         /// <summary>
//         ///     Draws the guide name text, centered if specified.
//         /// </summary>
//         /// <param name="guide"></param>
//         /// <param name="centered"></param>
//         private static void GuideNameText(Guide guide, bool centered = true)
//         {
//             if (centered)
//             {
//                 ImGui.SetCursorPosX((ImGui.GetWindowWidth() - ImGui.CalcTextSize(guide.Name).X) / 2);
//             }
//             ImGui.TextUnformatted(guide.Name);
//         }
//     }
// }
