using System;
using System.Collections.Generic;
using System.Linq;

namespace HideTheCog;

public class ActionBarUpdater
{
    private ActionBarSkillBuilder _actionBarBuilder { get; set; }
    private List<ActionBarSkill> _actionBarSkills { get; set; } = new();

    public ActionBarUpdater()
    {
        _actionBarBuilder = new();
    }

    public void OnActionBarUpdate()
    {
        if (HideTheCog.ClientState == null || !HideTheCog.ClientState.IsLoggedIn) return;

        var actionBarSkills = _actionBarBuilder.Build();
        if (actionBarSkills.SequenceEqual(_actionBarSkills)) return;

        var addedSkills = actionBarSkills.Except(_actionBarSkills).ToList();
        addedSkills.ForEach(s => s.Hide());

        _actionBarSkills = actionBarSkills;
    }

    public void Dispose()
    {
        _actionBarSkills.ForEach(skill => skill.Dispose());
        _actionBarSkills.Clear();
    }
}
