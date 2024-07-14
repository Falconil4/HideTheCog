using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace HideTheCog;

public sealed unsafe class HideTheCog : IDalamudPlugin
{
    public static string Name => "Hide The Cog";

    public static IClientState? ClientState { get; private set; }
    private ActionBarSkillUpdater ActionBarSkillUpdater { get; init; }

    public HideTheCog(IClientState clientState, IAddonLifecycle addonLifecycle)
    {
        ClientState = clientState;

        ActionBarSkillUpdater = new(addonLifecycle);

        Init();

        ClientState.Login += OnLogin;
        ClientState.Logout += OnLogout;
    }

    private void Init()
    {
        if (ClientState?.IsLoggedIn != true) return;

        ActionBarSkillUpdater.Init();
    }

    private void Deinit()
    {
        ActionBarSkillUpdater.Dispose();
    }

    public void Dispose() 
    {
        if (ClientState != null)
        {
            ClientState.Login -= OnLogin;
            ClientState.Logout -= OnLogout;
        }

        Deinit();
    }

    private void OnLogin() => Init();
    private void OnLogout() => Deinit();
}
