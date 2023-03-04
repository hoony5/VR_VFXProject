using UnityEngine;

public class VFXGizmo : MonoBehaviour
{
#if UNITY_EDITOR
    public bool turnOn;
    //cache
    private Transform _transformCache;
    // size
    [Space(5)] public bool useScalar;
    [Range(1, 100)] public float scalar;
    // design
    [Space(15)] public Color gizmoColor;
    [Space(5),Range(0, 1)] public float sidesAlpha;
    private void OnDrawGizmos()
    {
        if (!turnOn) return;
        
        _transformCache = transform;
        
        if(useScalar)
            _transformCache.localScale = Vector3.one * scalar;

        Vector3 position = _transformCache.position;
        Vector3 localScale = _transformCache.localScale;
        
        Gizmos.color = new Color(gizmoColor.r,gizmoColor.g,gizmoColor.b,sidesAlpha);
        Gizmos.DrawCube(position, localScale);
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(position, localScale);
    }
#endif
}   
