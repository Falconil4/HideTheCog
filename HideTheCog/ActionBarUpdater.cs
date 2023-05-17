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

    public void OnActionBarUpdate(bool rebuild = false)
    {
        if (HideTheCog.ClientState == null || !HideTheCog.ClientState.IsLoggedIn) return;

        var actionBarSkills = _actionBarBuilder.Build();
        if (!rebuild && actionBarSkills.SequenceEqual(_actionBarSkills)) return;

        var addedSkills = rebuild ? actionBarSkills : actionBarSkills.Except(_actionBarSkills).ToList();
        addedSkills.ForEach(s => s.Hide());

        _actionBarSkills = actionBarSkills;
    }

    public void Dispose()
    {
        _actionBarSkills.ForEach(skill => skill.Dispose());
        _actionBarSkills.Clear();
    }
}
