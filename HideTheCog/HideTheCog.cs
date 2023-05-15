using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Hooking;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;

namespace HideTheCog;

public sealed unsafe class HideTheCog : IDalamudPlugin
{
    public string Name => "Hide The Cog";

    private ClientState _clientState { get; init; }
    private ActionBarUpdater _actionBarUpdater { get; init; }

    private delegate byte ActionBarUpdate(AddonActionBarBase* atkUnitBase, NumberArrayData** numberArrayData, StringArrayData** stringArrayData);
    private const string ACTION_BAR_UPDATE_SIGNATURE = "E8 ?? ?? ?? ?? 83 BB ?? ?? ?? ?? ?? 75 09";
    private Hook<ActionBarUpdate> _actionBarHook { get; set; }

    public HideTheCog(ClientState clientState)
    {
        _clientState = clientState;
        _actionBarUpdater = new();

        var scanner = new SigScanner(true);
        var address = scanner.ScanText(ACTION_BAR_UPDATE_SIGNATURE);
        _actionBarHook = Hook<ActionBarUpdate>.FromAddress(address, ActionBarUpdateDetour);
        OnLogin();

        _clientState.Login += OnLogin;
        _clientState.Logout += OnLogout;
    }

    private void OnLogin(object? sender, EventArgs e) => OnLogin();
    private void OnLogin()
    {
        if (_clientState?.IsLoggedIn == true)
        {
            _actionBarHook.Enable();
            _actionBarUpdater.OnActionBarUpdate();
        }
    }

    private void OnLogout(object? sender, EventArgs e) => OnLogout();
    private void OnLogout()
    {
        _actionBarHook.Disable();
        _actionBarUpdater.Dispose();
    }

    public void Dispose() 
    {
        if (_clientState != null)
        {
            _clientState.Login -= OnLogin;
            _clientState.Logout -= OnLogout;
        }

        OnLogout();
    }

    private byte ActionBarUpdateDetour(AddonActionBarBase* atkUnitBase, NumberArrayData** numberArrayData, StringArrayData** stringArrayData)
    {
        if (_clientState?.IsLoggedIn == true)
        {
            _actionBarUpdater.OnActionBarUpdate();
        }
        return _actionBarHook.Original(atkUnitBase, numberArrayData, stringArrayData);
    }
}
