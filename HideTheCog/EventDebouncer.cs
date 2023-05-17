using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace HideTheCog;

public class EventDebouncer {
    private Subject<AddonActionBarBase?> _subject { get; init; }
    private ActionBarUpdater _actionBarUpdater { get; init; }

    private const int THROTTLE_MILLISECONDS = 50;

    public EventDebouncer()
    {
        _subject = new();
        _actionBarUpdater = new();

        _subject.Throttle(TimeSpan.FromMilliseconds(THROTTLE_MILLISECONDS)).Subscribe(_ => _actionBarUpdater.OnActionBarUpdate());
    }

    public void Init()
    {
        _actionBarUpdater.OnActionBarUpdate(true);
    }

    public void Next(AddonActionBarBase? eventData) => _subject.OnNext(eventData);

    public void Dispose()
    {
        _actionBarUpdater.Dispose();
    }
}
