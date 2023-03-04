using UnityEngine;

public static class CoroutineYield
{
    public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    public static readonly WaitForSeconds waitForAPointSeconds = new WaitForSeconds(0.1f);
}