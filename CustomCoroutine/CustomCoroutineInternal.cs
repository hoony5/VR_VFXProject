using System.Collections;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
[System.Serializable]
public struct CustomCoroutineInternal
{
    public IEnumerator Routine { get; }
    public CustomCoroutineToken token;

    public void SetToken(CustomCoroutineToken token)
    {
        this.token = token;
    }
    public CustomCoroutineInternal(IEnumerator routine, CustomCoroutineToken token)
    {
        Routine = routine;
        this.token = token;
    }
}