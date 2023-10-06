using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Collections.Generic;

namespace HideTheCog;

public unsafe class ActionBarSkillBuilder
{
    private List<string> _actionBarNames { get; set; } = new() {
        "_ActionBar",
        "_ActionBar01",
        "_ActionBar02",
        "_ActionBar03",
        "_ActionBar04",
        "_ActionBar05",
        "_ActionBar06",
        "_ActionBar07",
        "_ActionBar08",
        "_ActionBar09",
    };

    private const int ACTION_BAR_SLOTS_COUNT = 12;

    public List<ActionBarSkill> Build()
    {
        List<ActionBarSkill> actionBarSkills = new();

        for (uint actionBarIndex = 0; actionBarIndex < _actionBarNames.Count; actionBarIndex++)
        {
            var actionBar = GetActionBar(actionBarIndex)->ActionBarSlots;

            for (uint slotIndex = 0; slotIndex < ACTION_BAR_SLOTS_COUNT; slotIndex++)
            {
                var actionBarSlot = &actionBar[slotIndex];
                var hotBarSlot = GetHotBarModule()->GetSlotById(actionBarIndex, slotIndex);

                if (hotBarSlot->CommandType == HotbarSlotType.Macro)
                {
                    ActionBarSkill skill = new(actionBarSlot->Icon, hotBarSlot->CommandId, actionBarIndex, slotIndex);
                    actionBarSkills.Add(skill);
                }
            }
        }

        return actionBarSkills;
    }

    private AddonActionBarBase* GetActionBar(uint actionBarIndex)
    {
        return (AddonActionBarBase*)AtkStage.GetSingleton()->RaptureAtkUnitManager->
            GetAddonByName(_actionBarNames[(int)actionBarIndex]);
    }

    private static RaptureHotbarModule* GetHotBarModule()
    {
        return Framework.Instance()->GetUiModule()->GetRaptureHotbarModule();
    }
}
