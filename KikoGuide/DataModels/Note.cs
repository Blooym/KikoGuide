using System;
using System.IO;
using KikoGuide.Common;
using Newtonsoft.Json;

namespace KikoGuide.DataModels
{
    /// <summary>
    ///     A note that is stored on the filesystem.
    /// </summary>
    internal sealed record Note
    {
        /// <summary>
        ///     The version of the note format, incremented when breaking changes are made.
        /// </summary>
        private const int FormatVersion = 0;

        /// <summary>
        ///     Creates a new note.
        /// </summary>
        /// <param name="name">The name of the note.</param>
        [JsonConstructor]
        private Note(string name) => this.Name = name;

        /// <summary>
        ///     The version of the note format.
        /// </summary>
        public int Version { get; } = FormatVersion;

        /// <summary>
        ///     The name of the note.
        /// </summary>
        [JsonProperty(nameof(Name))] public string Name { get; private set; }

        /// <summary>
        ///     The content of the note.
        /// </summary>
        [JsonProperty(nameof(Content))] public string Content { get; private set; } = string.Empty;

        /// <summary>
        ///     Creates a new note.
        /// </summary>
        /// <param name="name">The name of the note.</param>
        /// <returns></returns>
        private static Note Create(string name) => new(name);

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
        private static string GetPath(string name) => Path.Combine(Constants.Directory.Notes, name + ".json");

        /// <summary>
        ///     Creates a new note or loads an existing one.
        /// </summary>
        /// <param name="name">The name of the note.</param>
        /// <returns></returns>
        internal static Note CreateOrLoad(string name)
        {
            name = string.Join("_", name.Split(Path.GetInvalidFileNameChars()));
            try
            {
                if (File.Exists(GetPath(name)))
                {
                    return Load(GetPath(name));
                }
            }
            catch (Exception e)
            {
                BetterLog.Error($"Failed to load note {name}, creating new one: {e.Message}");
                return Create(name).Save();
            }

            return Create(name);
        }

        /// <summary>
        ///     Saves the note to the filesystem with the current data.
        /// </summary>
        /// <returns></returns>
        public Note Save()
        {
            if (!Directory.Exists(Constants.Directory.Notes))
            {
                Directory.CreateDirectory(Constants.Directory.Notes);
            }
            File.WriteAllText(GetPath(this.Name), JsonConvert.SerializeObject(this, Formatting.Indented));
            return this;
        }

        public void Delete()
        {
            this.Content = string.Empty;
            File.Delete(GetPath(this.Name));
        }

        /// <summary>
        ///     Sets the name of the note.
        /// </summary>
        /// <param name="name">The name to set.</param>
        /// <returns></returns>
        public Note SetName(string name)
        {
            this.Name = name.Trim();
            return this;
        }

        /// <summary>
        ///     Sets the content of the note.
        /// </summary>
        /// <param name="content">The content to set.</param>
        /// <returns></returns>
        public Note SetContent(string content)
        {
            this.Content = content.Trim();
            return this;
        }
    }
}
