using Microsoft.AspNetCore.SignalR;

namespace ArquiveStatus.Web;

public class GetArchivesStatusService: IHostedService, IDisposable
{
    private Timer _timer;
    public IServiceProvider _services { get; }

    private readonly List<User> _solicitations;

    public GetArchivesStatusService(IServiceProvider services)
    {
        _services = services;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(GetStatus, null, 0, 2000);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void GetStatus(object state)
    {
        using (var scope = _services.CreateScope())
        {
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ArchiveHub>>();

            //consumer , service.
            var archiveReady = GetRandomNumber(0, 100);
            hubContext.Clients.Group("user1").SendAsync("ArchiveStatus", $"arquivo {archiveReady}");

            archiveReady = GetRandomNumber(0, 100);
            hubContext.Clients.Group("user2").SendAsync("ArchiveStatus", $"arquivo {archiveReady}");
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private int GetRandomNumber(int minimum, int maximum)
    {
        var random = new Random();

        return random.Next(minimum, maximum);
    }
}

public class User
{
    public string Name { get; set; }
    public List<string> Archives { get; set; }
}
