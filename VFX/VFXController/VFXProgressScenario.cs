using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VFXProgressScenario : MonoBehaviour
{
   public VFXTraits vfxTraits;
   public int currentIndex;
   public bool isStart;
   public bool isEnd;
   public List<VFXProgressItem> progressItems = new List<VFXProgressItem>(128);

   public void Apply(int current)
   {
      if (progressItems.Count == 0)
      {
         Debug.Log($"progressItems Length is 0");
         return;
      }
      progressItems[current].ApplyValeus(vfxTraits);
   }

   private void Update()
   {
      if (!isStart || isEnd) return;

      Apply(currentIndex);
   }
}