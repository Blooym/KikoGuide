using System;
using System.IO;
using KikoGuide.Common;
using Newtonsoft.Json;

namespace KikoGuide.DataModels
{
    /// <summary>
    ///     A note that is stored on the filesystem.
    /// </summary>
    [Serializable]
    public sealed record Note
    {
        /// <summary>
        ///     The version of the note format, incremented when breaking changes are made.
        /// </summary>
        [JsonIgnore]
        internal const int FormatVersion = 0;

        /// <summary>
        ///     The version of the note format.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        ///     The name of the note.
        /// </summary>
        public string Name { get; private set; } = null!;

        /// <summary>
        ///     The content of the note.
        /// </summary>
        public string Content { get; private set; } = string.Empty;

        /// <summary>
        ///     The date and time the note was created.
        /// </summary>
        public DateTime CreatedAt { get; } = DateTime.Now;

        /// <summary>
        ///     The date and time the note was last modified.
        /// </summary>
        public DateTime? LastModifiedAt { get; private set; }

        /// <summary>
        ///     Creates a new note.
        /// </summary>
        /// <param name="name">The name of the note.</param>
        private Note(string name) => this.Name = name;

        /// <inheritdoc cref="Note(string)"/>
        [JsonConstructor]
        private Note() { }

        /// <summary>
        ///     Creates a new note.
        /// </summary>
        /// <param name="name">The name of the note.</param>
        /// <returns></returns>
        private static Note Create(string name)
        {
            var note = new Note(name);
            return note;
        }

        /// <summary>
        ///     Loads a note from the filesystem.
        /// </summary>
        /// <param name="path">The absolute path to the note.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Thrown if the note does not exist.</exception>
        private static Note Load(string path)
        {
            var note = JsonConvert.DeserializeObject<Note>(File.ReadAllText(path));
            return note ?? throw new FileNotFoundException("No note found for path " + path);
        }

        /// <summary>
        ///     Gets the absolute path to the note.
        /// </summary>
        /// <param name="name">The name of the note.</param>
        /// <returns>The absolute path to the note.</returns>
        private static string GetPath(string name) => $@"{Constants.NotesDirectory}\{name}.json";

        /// <summary>
        ///     Creates a new note or loads an existing one.
        /// </summary>
        /// <param name="path">The absolute path to the note.</param>
        /// <returns></returns>
        internal static Note CreateOrLoad(string name)
        {
            if (File.Exists(GetPath(name)))
            {
                return Load(GetPath(name));
            }
            else
            {
                return Create(name);
            }
        }

        /// <summary>
        ///     Saves the note to the filesystem with the current data.
        /// </summary>
        /// <returns></returns>
        public Note Save()
        {
            this.LastModifiedAt = DateTime.Now;
            if (!Directory.Exists(Constants.NotesDirectory))
            {
                Directory.CreateDirectory(Constants.NotesDirectory);
            }
            File.WriteAllText(GetPath(this.Name), JsonConvert.SerializeObject(this, Formatting.Indented));
            return this;
        }

        public void Delete() => File.Delete(GetPath(this.Name));

        /// <summary>
        ///     Sets the title of the note.
        /// </summary>
        /// <param name="title">The title to set.</param>
        /// <returns></returns>
        public Note SetTitle(string title)
        {
            this.Name = title;
            this.LastModifiedAt = DateTime.Now;
            return this;
        }

        /// <summary>
        ///     Sets the content of the note.
        /// </summary>
        /// <param name="content">The content to set.</param>
        /// <returns></returns>
        public Note SetContent(string content)
        {
            this.Content = content;
            this.LastModifiedAt = DateTime.Now;
            return this;
        }
    }
}
