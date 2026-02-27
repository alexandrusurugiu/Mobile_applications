using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyNotes.ViewModels
{
    internal partial class NotesViewModel : IQueryAttributable
    {
        public ObservableCollection<NoteViewModel> AllNotes { get; set; }

        public NotesViewModel() 
        { 
            AllNotes = new ObservableCollection<NoteViewModel>(Models.Note.LoadAll().Select(note => new NoteViewModel(note)));
        }

        [RelayCommand]
        private async Task NewNote()
        {
            await Shell.Current.GoToAsync(nameof(Views.NotePage));
        }

        [RelayCommand]
        private async Task SelectNote(NoteViewModel note)
        {
            if (note != null)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string noteId = query["deleted"].ToString();
                NoteViewModel matchedNote = AllNotes.Where(note => note.Identifier == noteId).FirstOrDefault();
            
                if (matchedNote != null)
                {
                    AllNotes.Remove(matchedNote);
                }
            } 
            else if (query.ContainsKey("saved"))
            {
                string noteId = query["saved"].ToString();
                NoteViewModel matchedNote = AllNotes.Where(note => note.Identifier == noteId).FirstOrDefault();

                if (matchedNote != null)
                {
                    matchedNote.Reload();
                    AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
                }
                else
                {
                    AllNotes.Insert(0, new NoteViewModel(Models.Note.Load(noteId)));
                }
            }
        }
    }
}
