using Microsoft.AspNetCore.SignalR;

namespace BOL.Models.Hubs
{
    public class CustomHubs : Hub
    {
        public static Dictionary<string, string> _userConnections = new Dictionary<string, string>();

        public async Task AttenOtProcessTask(string taskId)
        {
            if (!string.IsNullOrEmpty(taskId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, taskId);
            }          
        }
        public async Task TextUploadProcessTask(string taskId)
        {
            if (!string.IsNullOrEmpty(taskId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, taskId);
            }           
        }
        public async Task SalProcessBulkProcessTask(string taskId)
        {
            if (!string.IsNullOrEmpty(taskId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, taskId);
            }           
        }
    }
}
