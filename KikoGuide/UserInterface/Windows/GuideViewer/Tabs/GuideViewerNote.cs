using Dalamud.Interface.Internal.Notifications;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using KikoGuide.DataModels;
using KikoGuide.Resources.Localization;
using Sirensong.UserInterface;

namespace KikoGuide.UserInterface.Windows.GuideViewer.Tabs
{
    internal static class GuideViewerNote
    {
        private static Vector2 NoteContentSize => new(-1, -30);

        /// <summary>
        /// Draws the note tab.
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="note"></param>
        public static void Draw(GuideViewerLogic logic, Note note)
        {
            SiGui.Heading(Strings.Note_Name);
            switch (logic.NoteState)
            {
                case GuideViewerLogic.NoteTabState.Viewing:
                    DrawNoteViewing(logic, note);
                    break;
                case GuideViewerLogic.NoteTabState.Editing:
                    DrawNoteEditing(logic, note);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Draws the note viewing state.
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="note"></param>
        private static void DrawNoteViewing(GuideViewerLogic logic, Note note)
        {
            if (string.IsNullOrEmpty(note.Content))
            {
                if (ImGui.Selectable(Strings.Note_Create))
                {
                    logic.NoteState = GuideViewerLogic.NoteTabState.Editing;
                }
            }
            else
            {
                if (ImGui.BeginChild("NoteContent", NoteContentSize))
                {
                    SiGui.TextWrapped(note.Content);
                }
                ImGui.EndChild();
                if (ImGui.Selectable(Strings.Note_Edit))
                {
                    logic.NoteState = GuideViewerLogic.NoteTabState.Editing;
                }
            }
        }

        /// <summary>
        /// Draws the note editing state.
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="note"></param>
        private static void DrawNoteEditing(GuideViewerLogic logic, Note note)
        {
            var content = note.Content;

            if (SiGui.InputTextMultiline("##NoteInput", ref content, 2048, NoteContentSize, true))
            {
                SaveNoteWithContent(note, content);
                logic.NoteState = GuideViewerLogic.NoteTabState.Viewing;
            }

            if (ImGui.Selectable(Strings.Note_Save))
            {
                SaveNoteWithContent(note, content);
                logic.NoteState = GuideViewerLogic.NoteTabState.Viewing;
            }
        }

        private static void SaveNoteWithContent(Note note, string content)
        {
            note.SetContent(content).Save();
            SiGui.ShowToast("Note saved!", NotificationType.Success);
        }
    }
}
