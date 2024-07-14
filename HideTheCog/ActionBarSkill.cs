using FFXIVClientStructs.FFXIV.Component.GUI;

namespace HideTheCog;

public unsafe class ActionBarSkill(AtkComponentNode* iconComponent, uint actionBarIndex)
{
    private AtkResNode* MacroIcon { get; init; } = iconComponent->Component->UldManager.NodeList[17];
    public uint ActionBarIndex { get; init; } = actionBarIndex;

    public void Show() => MacroIcon->ToggleVisibility(true);

    public void Hide() => MacroIcon->ToggleVisibility(false);

    public void Dispose() => Show();
}
