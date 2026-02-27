using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MyNotes.ViewModels
{
    internal partial class NoteViewModel : ObservableObject, IQueryAttributable
    {
        private Note note;

        public NoteViewModel(Note note)
        {
            this.note = note;
        }

        public NoteViewModel() : this(new Models.Note())
        {
        }

        public string Text
        {
            get => note.Text;
            set
            {
                if (note.Text != value)
                {
                    note.Text = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => note.Date;
        }

        public string Identifier => note.FileName;

        public void Reload()
        {
            note = Models.Note.Load(note.FileName);
            RefreshProperties();
        }

        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(Date));

        }

        [RelayCommand]
        private async Task Save()
        {
            note.Date = DateTime.Now;
            note.Save();
            await Shell.Current.GoToAsync($"..?saved={note.FileName}");
        }

        [RelayCommand]
        private async Task Delete()
        {
            note.Delete();
            await Shell.Current.GoToAsync($"..?deleted={note.FileName}");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("load"))
            {
                note = Models.Note.Load(query["load"].ToString());
                RefreshProperties();
            }
        }
    }
}
