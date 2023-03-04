using System.Runtime.InteropServices;

[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct CustomCoroutineToken
{
    public bool pause;
    public bool stop;
    public bool keepGoing;
    public bool awaitTask;
    public int behaviourIndex;
}