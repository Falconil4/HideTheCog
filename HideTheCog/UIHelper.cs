using FFXIVClientStructs.FFXIV.Component.GUI;

namespace HideTheCog;

public unsafe static class UIHelper
{
    public static void Show(AtkResNode* node)
    {
        node->Flags |= 0x10;
        node->Flags_2 |= 0x1;
    }

    public static void Hide(AtkResNode* node)
    {
        node->Flags &= ~0x10;
        node->Flags_2 |= 0x1;
    }
}
