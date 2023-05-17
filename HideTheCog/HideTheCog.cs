using System;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Hooking;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace HideTheCog;

public sealed unsafe class HideTheCog : IDalamudPlugin
{
    public string Name => "Hide The Cog";

    public static ClientState? ClientState { get; private set; }
    private EventDebouncer _eventDebouncer { get; init; }

    private delegate byte ActionBarUpdate(AddonActionBarBase* atkUnitBase, NumberArrayData** numberArrayData, StringArrayData** stringArrayData);
    private const string ACTION_BAR_UPDATE_SIGNATURE = "E8 ?? ?? ?? ?? 83 BB ?? ?? ?? ?? ?? 75 09";
    private Hook<ActionBarUpdate> _actionBarHook { get; set; }

    public HideTheCog(ClientState clientState)
    {
        ClientState = clientState;
        _eventDebouncer = new();

        _actionBarHook = CreateHook();
        Init();

        ClientState.Login += OnLogin;
        ClientState.Logout += OnLogout;
    }

    private Hook<ActionBarUpdate> CreateHook()
    {
        var scanner = new SigScanner(true);
        var address = scanner.ScanText(ACTION_BAR_UPDATE_SIGNATURE);

        return Hook<ActionBarUpdate>.FromAddress(address, ActionBarUpdateDetour);
    }


    private void Init()
    {
        if (ClientState?.IsLoggedIn == true)
        {
            _actionBarHook.Enable();
            _eventDebouncer.Init();
        }
    }

    private void Deinit()
    {
        _actionBarHook.Disable();
        _eventDebouncer.Dispose();
    }

    public void Dispose() 
    {
        if (ClientState != null)
        {
            ClientState.Login -= OnLogin;
            ClientState.Logout -= OnLogout;
            ClientState = null;
        }

        Deinit();
    }

    private void OnLogin(object? sender, EventArgs e) => Init();
    private void OnLogout(object? sender, EventArgs e) => Deinit();

    private byte ActionBarUpdateDetour(AddonActionBarBase* atkUnitBase, NumberArrayData** numberArrayData, StringArrayData** stringArrayData)
    {
        if (ClientState?.IsLoggedIn == true)
        {
            _eventDebouncer.Next(*atkUnitBase);
        }
        return _actionBarHook.Original(atkUnitBase, numberArrayData, stringArrayData);
    }
}
