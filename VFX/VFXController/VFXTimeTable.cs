using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class VFXTimeTable
{
    public VFXTimeTableItem[] todoArray = Array.Empty<VFXTimeTableItem>();
    [NonSerialized] public VFXTimeTableItem Empty;
    private Dictionary<int, VFXTimeTableItem> @eventPlanner = new Dictionary<int, VFXTimeTableItem>(24);

    public void Init()
    {
        Empty = new VFXTimeTableItem{igoreOn = true};
        @eventPlanner.Clear();
        @eventPlanner = todoArray.ToDictionary(key => key.thresholdsSeconds, value => value);
    }

    public bool TryGetValue(int thresholdSeconds, out VFXTimeTableItem @event)
    {
        bool exist = @eventPlanner.TryGetValue(thresholdSeconds, out VFXTimeTableItem value);

        if (!exist || value.igoreOn)
        {
            @event = Empty;
            return false;
        }

        @event = value;
        return true;
    }
}