using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GsmsRazor.Pages
{
    public class Note
    {
        public string id { get; set; }
        public string senderId { get; set; }
        public string content { get; set; }

        public Note(string id, string senderId, string content)
        {
            this.id = id;
            this.senderId = senderId;
            this.content = content;
        }

        public Note() { }


    }
    [Authorize(Roles = "Customer")]
    public class DashboardModel : PageModel
    {
        public IEnumerable<Note> notes;
        private SignalRHub _hub;
        public DashboardModel(IHubContext<SignalRHub> contextR)
        {
            _hub = new SignalRHub(contextR);
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "nlruRTC3Ji4w3bdoPU4UDbqA1tzy7OOAhCfRLgGI",
            BasePath = "https://gsms-prn-default-rtdb.firebaseio.com"
        };
        IFirebaseClient firebaseClient;

        public async Task<IEnumerable<Note>> getNotesAsync()
        {
            firebaseClient = new FirebaseClient(config);
            var res = await firebaseClient.GetAsync("notes");
            if (res.Body.Length > 0)
            {
                List<Note> jArray = JsonConvert.DeserializeObject<List<Note>>(res.Body);
                jArray.RemoveAt(1);
                var list = new List<Note>();
                foreach (var note in jArray)
                {
                    if (note != null)
                    {
                        list.Add(note);
                    }
                }
                return list;
            }
            return null;
        }

        public async Task<FireSharp.Response.FirebaseResponse> addNotesAsync(Note note)
        {
            firebaseClient = new FirebaseClient(config);
            var res = await firebaseClient.GetAsync("notes");
            List<Note> jArray = JsonConvert.DeserializeObject<List<Note>>(res.Body);
            return await firebaseClient.SetAsync<Note>("notes/" + jArray.Count, note);
        }
        public async Task<FireSharp.Response.FirebaseResponse> removeNotesAsync(string noteId)
        {
            firebaseClient = new FirebaseClient(config);
            var res = await firebaseClient.GetAsync("notes");
            List<Note> jArray = JsonConvert.DeserializeObject<List<Note>>(res.Body);
            int delIndex = jArray.FindIndex(n => n != null && n.id.Equals(noteId));
            return await firebaseClient.DeleteAsync("notes/" + delIndex);
        }

        public async Task<IActionResult> OnGet()
        {
            notes = await getNotesAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetLoadNotes()
        {
            notes = await getNotesAsync();
            return new JsonResult(notes);
        }

        public async Task<IActionResult> OnPostAddNote(string content)
        {
            if (content == null)
            {
                await _hub.ReloadNotes();
                return new JsonResult("");
            }
            if (content.Trim().Length == 0)
            {
                await _hub.ReloadNotes();
                return new JsonResult("");
            }
            Note noteToBeAdded = new Note();
            noteToBeAdded.id = Guid.NewGuid().ToString();
            noteToBeAdded.senderId = "asdasd";
            noteToBeAdded.content = content;
            await addNotesAsync(noteToBeAdded);
            await _hub.ReloadNotes();
            return new JsonResult("");
        }

        public async void OnPostRemoveNote(string noteId)
        {
            await removeNotesAsync(noteId);
            await _hub.ReloadNotes();
        }
    }
}
