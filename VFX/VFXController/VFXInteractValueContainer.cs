using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[System.Serializable]
public class VFXInteractValueContainer : MonoBehaviour
{
    private const int Capacity = 128;

    public VisualEffect vfx;
#if UNITY_EDITOR
    [SerializeField] public string scnarioKey;
    public bool _loadSuccess;
#endif
    public string[] InteractValueKeys => _interactValueKeys;
    private string[] _interactValueKeys = Array.Empty<string>();
    [SerializeField] public List<VFXInteractValue> interactItems = new List<VFXInteractValue>(Capacity);
    private Dictionary<string , VFXInteractValue> scenarioItemsDict = new Dictionary<string , VFXInteractValue>(Capacity);
    
    // cached Exposed Property Names work flow
    private void OnEnable()
    {
        Init();
    }
    private void OnDestroy()
    {
        scenarioItemsDict.Clear();
    }

    private void Init()
    {
        int length = interactItems.Count;
        _interactValueKeys = new string[length];
        scenarioItemsDict.Clear();
        
        for (int index = 0; index < length; index++)
        {
            VFXInteractValue item = interactItems[index];
            _interactValueKeys[index] = item.displayName;
            scenarioItemsDict.TryAdd(_interactValueKeys[index], item);
        }
    }

    public bool TryGetValue(string propertyName, out VFXInteractValue item)
    {
        if (!scenarioItemsDict.TryGetValue(propertyName, out VFXInteractValue result))
        {
            item = null;
            return false;
        }
        
        item = result;
        return true;
    }
}