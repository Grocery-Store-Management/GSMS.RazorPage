using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

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


    }
    public class DashboardModel : PageModel
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "nlruRTC3Ji4w3bdoPU4UDbqA1tzy7OOAhCfRLgGI",
            BasePath = "https://gsms-prn-default-rtdb.firebaseio.com"
        };
        IFirebaseClient firebaseClient;

        public IEnumerable<Note> getNotes()
        {
            firebaseClient = new FirebaseClient(config);
            var res = firebaseClient.Get("notes");
            List<Note> jArray = JsonConvert.DeserializeObject<List<Note>>(res.Body);
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
        public IActionResult OnGet()
        {
            getNotes();
            return Page();
        }
    }
}
