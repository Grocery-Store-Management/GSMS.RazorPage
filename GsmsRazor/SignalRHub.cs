using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GsmsRazor
{
    public class SignalRHub : Hub
    {
        protected IHubContext<SignalRHub> _context;

        public SignalRHub(IHubContext<SignalRHub> context)
        {
            this._context = context;
        }
        public async Task ReloadNotes()
        {
            await _context.Clients.All.SendAsync("reloadNotes");
        }
        public async Task ReloadPage()
        {
            await _context.Clients.All.SendAsync("reloadPage");
        }
    }
}
