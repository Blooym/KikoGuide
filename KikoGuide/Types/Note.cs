using System;
using System.IO;
using System.Linq;
using KikoGuide.Base;
using Newtonsoft.Json;

namespace KikoGuide.Types
{
    /// <summary>
    ///     Represents a note.
    /// </summary>
    public class Note
    {
        /// <summary>
        ///     The default directory to store notes in.
        /// </summary>
        public static readonly string DefaultLocation = Path.Combine(PluginService.PluginInterface.GetPluginConfigDirectory(), "Notes");

        /// <summary>
        ///     The naming template for storing notes.
        /// </summary>
        private static string StorageNameTemplate => "{0}.kikonote.json";

        /// <summary>
        ///     The storage directory of the note
        /// </summary>
        public string StorageDirectory { get; private set; } = DefaultLocation;

        /// <summary>
        ///     The name of the note
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     The contents of the note
        /// </summary>
        public string Contents { get; private set; } = string.Empty;

        /// <summary>
        ///     The old storage directory of the note
        /// </summary>
        [JsonIgnore]
        public string? OldLocationToDelete { get; private set; }

        /// <summary>
        ///     The version of the note
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        ///     The last time the note was edited
        /// </summary>
        public DateTime LastEdited { get; private set; }

        /// <summary>
        ///     The JSON Constructor.
        /// </summary>
        [JsonConstructor]
        private Note(string name, string contents, string storageDirectory, int version = 0)
        {
            name = SanitizeName(name);
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.Contents = SanitizeContents(contents);
            this.Version = version;
            this.StorageDirectory = storageDirectory;
        }

        /// <summary>
        ///     Sets the name of the note.
        /// </summary>
        /// <param name="name"> The new name of the note. </param>
        public Note SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = SanitizeName(name);
            this.LastEdited = DateTime.Now;
            return this;
        }

        /// <summary>
        ///     Sets the contents of the note.
        /// </summary>
        /// <param name="contents"> The new contents of the note. </param>
        public Note SetContents(string contents)
        {
            this.Contents = SanitizeContents(contents);
            this.LastEdited = DateTime.Now;
            return this;
        }

        /// <summary>
        ///     Sets the version of the note.
        /// </summary>
        /// <param name="version"> The new version of the note. </param>
        public Note SetVersion(int version)
        {
            this.Version = version;
            this.LastEdited = DateTime.Now;
            return this;
        }

        /// <summary>
        ///     Sets the note storage directory.
        /// </summary>
        /// <param name="directory"> The new note storage directory. </param>
        public Note SetDirectory(string directory)
        {
            directory = SanitizeDirectory(directory);

            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.OldLocationToDelete = this.GetLocation();
            this.StorageDirectory = directory;
            this.LastEdited = DateTime.Now;
            return this;
        }

        /// <summary>
        ///     Sets the note storage directory to a sub-directory.
        /// </summary>
        /// <param name="subDirectory"> The new note storage sub-directory. </param>
        public Note AddDirectorySubdir(string subDirectory)
        {
            subDirectory = SanitizeDirectory(subDirectory);

            if (string.IsNullOrWhiteSpace(subDirectory))
            {
                throw new ArgumentNullException(nameof(subDirectory));
            }

            this.OldLocationToDelete = this.GetLocation();
            this.StorageDirectory = Path.Combine(this.StorageDirectory, subDirectory);
            this.LastEdited = DateTime.Now;
            return this;
        }

        /// <summary>
        ///     Moves the note storage directory back a given number of levels.
        /// </summary>
        /// <param name="levels"> The number of levels to move back. </param>
        public Note BackDirectoryDir(int levels = 1)
        {
            this.OldLocationToDelete = this.GetLocation();
            this.StorageDirectory = this.StorageDirectory.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)[^levels];
            this.LastEdited = DateTime.Now;
            return this;
        }

        /// <summary>
        ///     Sets the note storage directory back to the default.
        /// </summary>
        public Note SetDefaultDirectory()
        {
            this.OldLocationToDelete = this.GetLocation();
            this.StorageDirectory = DefaultLocation;
            this.LastEdited = DateTime.Now;
            return this;
        }

        /// <summary>
        ///     Returns the note location.
        /// </summary>
        /// <returns> The note location. </returns>
        public string GetLocation() => Path.Combine(this.StorageDirectory, string.Format(StorageNameTemplate, this.Name));

        /// <summary>
        ///     Returns the storage directory of the note.
        /// </summary>
        /// <returns> The storage directory of the note. </returns>
        public string GetDirectory() => this.StorageDirectory;

        /// <summary>
        ///     Returns the name of the note.
        /// </summary>
        /// <returns> The name of the note. </returns>
        public string GetName() => this.Name;

        /// <summary>
        ///     Returns the contents of the note.
        /// </summary>
        /// <returns> The contents of the note. </returns>
        public string GetContents() => this.Contents;

        /// <summary>
        ///     Returns the version of the note.
        /// </summary>
        /// <returns> The version of the note. </returns>
        public int GetVersion() => this.Version;

        /// <summary>
        ///     Saves the note to the storage directory.
        /// </summary>
        public Note Save()
        {
            var serializedNote = JsonConvert.SerializeObject(this, Formatting.Indented);

            if (!Directory.Exists(this.StorageDirectory))
            {
                Directory.CreateDirectory(this.StorageDirectory);
            }

            File.WriteAllText(this.GetLocation(), serializedNote);

            if (this.OldLocationToDelete != null && this.GetLocation() != this.OldLocationToDelete && File.Exists(this.OldLocationToDelete))
            {
                File.Delete(this.OldLocationToDelete);
                this.OldLocationToDelete = null;
            }

            return this;
        }

        /// <summary>
        ///     Deletes the note from the storage directory if it exists.
        /// </summary>
        public void Delete()
        {
            if (File.Exists(this.GetLocation()))
            {
                File.Delete(this.GetLocation());
            }
        }

        /// <summary>
        ///     Creates or loads a note from the default storage directory.
        /// </summary>
        /// <param name="name"> The name of the note. </param>
        /// <returns> The note. </returns>
        public static Note CreateOrLoad(string name)
        {
            name = SanitizeName(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!File.Exists(Path.Combine(DefaultLocation, string.Format(StorageNameTemplate, name))))
            {
                return new Note(name, string.Empty, DefaultLocation);
            }

            var note = JsonConvert.DeserializeObject<Note>(File.ReadAllText(Path.Combine(DefaultLocation, string.Format(StorageNameTemplate, name))));
            return note ?? new Note(name, string.Empty, DefaultLocation);
        }

        /// <summary>
        ///     Creates or loads a note from a given directory.
        /// </summary>
        /// <param name="name"> The name of the note. </param>
        /// <param name="directory"> The directory of the note. </param>
        /// <returns> The note. </returns>
        public static Note CreateOrLoad(string name, string directory)
        {
            name = SanitizeName(name);
            directory = SanitizeDirectory(directory);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (!File.Exists(Path.Combine(directory, string.Format(StorageNameTemplate, name))))
            {
                return new Note(name, string.Empty, directory);
            }

            var note = JsonConvert.DeserializeObject<Note>(File.ReadAllText(Path.Combine(directory, string.Format(StorageNameTemplate, name))));
            return note ?? new Note(name, string.Empty, directory);
        }

        /// <summary>
        ///    Checks to see if the given note exists.
        /// </summary>
        /// <param name="name"> The name of the note. </param>
        /// <returns> True if the note exists, false otherwise. </returns>
        public static bool Exists(string name)
        {
            name = SanitizeName(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return File.Exists(Path.Combine(DefaultLocation, string.Format(StorageNameTemplate, name)));
        }

        /// <summary>
        ///    Checks to see if the given note exists in the given directory.
        /// </summary>
        /// <param name="name"> The name of the note. </param>
        /// <param name="directory"> The directory of the note. </param>
        /// <returns> True if the note exists, false otherwise. </returns>
        public static bool Exists(string name, string directory)
        {
            name = SanitizeName(name);
            directory = SanitizeDirectory(directory);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            return File.Exists(Path.Combine(directory, string.Format(StorageNameTemplate, name)));
        }

        /// <summary>
        ///     Sanitizes a given string for use as a file name.
        /// </summary>
        /// <param name="name"> The string to sanitize. </param>
        /// <returns> The sanitized string. </returns>
        private static string SanitizeName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return new string(name.Where(c => !invalidChars.Contains(c)).ToArray());
        }

        /// <summary>
        ///     Sanitizes a given string for use as a directory name.
        /// </summary>
        /// <param name="name"> The string to sanitize. </param>
        /// <returns> The sanitized string. </returns>
        private static string SanitizeDirectory(string name)
        {
            var invalidChars = Path.GetInvalidPathChars();
            return new string(name.Where(c => !invalidChars.Contains(c)).ToArray());
        }

        /// <summary>
        ///     Sanitizes a given string for storing as contents.
        /// </summary>
        /// <param name="contents"> The string to sanitize. </param>
        /// <returns> The sanitized string. </returns>
        private static string SanitizeContents(string contents) => contents.Trim();
    }
}
