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

    public void Dispose() => Show();

    public override bool Equals(object? obj)
    {
        if (obj is ActionBarSkill abs)
        {
            return CommandId == abs.CommandId && ActionBarIndex == abs.ActionBarIndex && SlotIndex == abs.SlotIndex;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode() => base.GetHashCode();
}
