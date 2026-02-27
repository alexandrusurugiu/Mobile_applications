using System;
using System.Collections.Generic;
using System.Text;

namespace MyNotes.Models
{
    public class Note
    {
        public string FileName { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public Note() 
        {
            FileName = $"{Path.GetRandomFileName()}.note.txt";
            Date = DateTime.Now;
            Text = string.Empty;
        }

        public static Note Load(string fileName)
        {
            fileName = Path.Combine(FileSystem.AppDataDirectory, fileName);
            
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"The note file '{fileName}' was not found.");
            }

            return new Note()
            {
                FileName = Path.GetFileName(fileName),
                Text = File.ReadAllText(fileName),
                Date = File.GetLastWriteTime(fileName)
            };
        }

        public static IEnumerable<Note> LoadAll()
        {
            string appDataPath = FileSystem.AppDataDirectory;

            return Directory.EnumerateFiles(appDataPath, "*.note.txt").Select(fileName => Load(Path.GetFileName(fileName))).OrderByDescending(note => note.Date);
        }

        public void Save()
        {
            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, FileName), Text);
        }

        public void Delete()
        {
            File.Delete(Path.Combine(FileSystem.AppDataDirectory, FileName));
        }
    }
}
