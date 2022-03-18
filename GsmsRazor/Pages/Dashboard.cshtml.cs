using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using GSMS.API.PRM;
using GSMS.API.PRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace GsmsRazor.Pages
{
    public class Note
    {
        public string id { get; set; }
        public string senderId { get; set; }
        public string senderName { get; set; }

        public string content { get; set; }

        public Note(string id, string senderId, string senderName, string content)
        {
            this.id = id;
            this.senderId = senderId;
            this.senderName = senderName;
            this.content = content;
        }

        public Note() { }


    }
    [Authorize(Roles = "Customer, Cashier, Store Owner")]
    public class DashboardModel : PageModel
    {
        public IEnumerable<Note> notes;
        private SignalRHub _hub;
        private readonly INotificationService _notificationService;
        public DashboardModel(IHubContext<SignalRHub> contextR, INotificationService notificationService)
        {
            _hub = new SignalRHub(contextR);
            _notificationService = notificationService;

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
            Debug.WriteLine(res.Body);
            if (res.Body.Length > 0)
            {
                List<Note> jArray = JsonConvert.DeserializeObject<List<Note>>(res.Body);
                jArray.RemoveAt(0);
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
            var newArray = jArray.ToArray().Where(n => n != null && !n.id.Equals(noteId)).ToArray();
            return await firebaseClient.SetAsync("notes", newArray);
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
            noteToBeAdded.senderId = HttpContext.Session.GetString("UID");
            noteToBeAdded.senderName = HttpContext.Session.GetString("NAME");
            noteToBeAdded.content = content;
            NotificationModel noti = new NotificationModel();
            noti.DeviceId = "crY1ZWBBTFad7iE5BSPnOY:APA91bHWbVkQxBMN182wqZP8tB7opaZHt1DxyDX9CSEhcDQRyK1jNKrGk9KPnhjoQ6DYI-wNhj4aGZwVnk5ghWwV33ywB8FvFD4lhcMewk9clQ0yTEHo0b6E52LWBiTKOaEej6WGq77g";
            noti.Title = "Message";
            noti.Body = noteToBeAdded.senderName + " said: " + noteToBeAdded.content;

            noti.IsAndroiodDevice = true;
            ResponseModel res = await _notificationService.SendNotification(noti);
            Debug.WriteLine(res.ToString());

            await addNotesAsync(noteToBeAdded);
            await _hub.ReloadNotes();
            return new JsonResult(res.Message);
        }

        public async void OnPostRemoveNote(string noteId)
        {
            await removeNotesAsync(noteId);
            await _hub.ReloadNotes();
        }
    }
}
