using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HideTheCog;

public class ActionBarUpdater
{
    private ActionBarSkillBuilder _actionBarBuilder { get; set; }
    private List<ActionBarSkill> _actionBarSkills { get; set; } = new();
    private DateTime _lastActionBarUpdate { get; set; } = DateTime.MinValue;

    private const int TIME_BETWEEN_ACTION_BAR_UPDATES = 100;
    private const int DELAY_BEFORE_HIDING = 10;

    public ActionBarUpdater()
    {
        _actionBarBuilder = new();
    }

    public void OnActionBarUpdate()
    {
        TimeSpan timeSinceLastUpdate = DateTime.Now - _lastActionBarUpdate;
        if (timeSinceLastUpdate.TotalMilliseconds < TIME_BETWEEN_ACTION_BAR_UPDATES) return;
        _lastActionBarUpdate = DateTime.Now;

        var actionBarSkills = _actionBarBuilder.Build();
        if (actionBarSkills.SequenceEqual(_actionBarSkills)) return;

        Dispose();
        Task.Run(async () =>
        {
            await Task.Delay(DELAY_BEFORE_HIDING);
            _actionBarSkills.AddRange(actionBarSkills);
            _actionBarSkills.ForEach(skill => skill.Hide());
        });
    }

    public void Dispose()
    {
        _actionBarSkills.ForEach(skill => skill.Dispose());
        _actionBarSkills.Clear();
    }
}
