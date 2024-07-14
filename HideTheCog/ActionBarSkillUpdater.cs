using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.RaptureHotbarModule;

namespace HideTheCog;

public unsafe class ActionBarSkillUpdater(IAddonLifecycle addonLifecycle)
{
    private List<string> _actionBarNames { get; set; } = [
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
    ];

    private const int ACTION_BAR_SLOTS_COUNT = 12;

    private readonly TimeSpan updateFrequency = TimeSpan.FromSeconds(2);

    private List<ActionBarSkill> actionBarSkills = [];

    private Dictionary<int, DateTimeOffset> lastUpdates = [];

    public void Init()
    {
        addonLifecycle.RegisterListener(AddonEvent.PreRequestedUpdate, _actionBarNames, ActionBarUpdate);

        actionBarSkills = CreateActionBarSkills();
    }

    private void ActionBarUpdate(AddonEvent type, AddonArgs args)
    {
        var actionBar = (AddonActionBarBase*)args.Addon;

        if (DateTimeOffset.Now - lastUpdates.GetValueOrDefault(actionBar->RaptureHotbarId) < updateFrequency) return;
        lastUpdates[actionBar->RaptureHotbarId] = DateTimeOffset.Now;

        actionBarSkills = [.. actionBarSkills.Where(skill => skill.ActionBarIndex != actionBar->RaptureHotbarId), .. CreateSkillsForActionBar(actionBar)];
    }

    public void Dispose()
    {
        addonLifecycle.UnregisterListener(ActionBarUpdate);

        actionBarSkills.ForEach(skill => skill.Dispose());
    }

    private List<ActionBarSkill> CreateActionBarSkills()
    {
        var manager = AtkStage.Instance()->RaptureAtkUnitManager;

        return _actionBarNames
            .SelectMany(actionBarName => CreateSkillsForActionBar((AddonActionBarBase*)manager->GetAddonByName(actionBarName)))
            .ToList();
    }

    private List<ActionBarSkill> CreateSkillsForActionBar(AddonActionBarBase* actionBar)
    {
        List<ActionBarSkill> skills = [];
        var actionBarSlots = actionBar->ActionBarSlotVector;
        var hotbarModule = Framework.Instance()->GetUIModule()->GetRaptureHotbarModule();

        for (uint slotIndex = 0; slotIndex < ACTION_BAR_SLOTS_COUNT; slotIndex++)
        {
            var actionBarSlot = actionBarSlots[slotIndex];
            var hotBarSlot = hotbarModule->GetSlotById(actionBar->RaptureHotbarId, slotIndex);

            if (hotBarSlot->CommandType == HotbarSlotType.Macro)
            {
                ActionBarSkill skill = new(actionBarSlot.Icon, actionBar->RaptureHotbarId);
                skill.Hide();

                skills.Add(skill);
            }
        }

        return skills;
    }
}
