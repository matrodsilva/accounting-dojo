using Microsoft.AspNetCore.SignalR;

namespace ArquiveStatus.Web;

public class ArchiveHub : Hub
{
    public Task Connect(string user)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, user);
    }
}
