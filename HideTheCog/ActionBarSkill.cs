using System;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace HideTheCog;

public unsafe class ActionBarSkill
{
    private AtkResNode* _macroIcon { get; init; }
    public uint CommandId { get; init; }
    public int ActionBarIndex { get; init; }
    public int SlotIndex { get; init; }

    public ActionBarSkill(AtkComponentNode* iconComponent, uint commandId, int actionBarIndex, int slotIndex)
    {
        _macroIcon = iconComponent->Component->UldManager.NodeList[15];
        CommandId = commandId;
        ActionBarIndex = actionBarIndex;
        SlotIndex = slotIndex;
    }

    public void Show() => UIHelper.Show(_macroIcon);

    public void Hide() => UIHelper.Hide(_macroIcon);

    public void Dispose()
    {
        Show();
    }

    public override bool Equals(object? obj) => obj?.GetHashCode() == GetHashCode();

    public override int GetHashCode() => HashCode.Combine(CommandId, ActionBarIndex, SlotIndex);
}
