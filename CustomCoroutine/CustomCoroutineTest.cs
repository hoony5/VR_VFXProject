using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class CustomCoroutineTest : MonoBehaviour
{
    public bool keepGoining;
    private CustomCoroutine _coroutine = new CustomCoroutine();
    [Button]
    void TsetStartCoroutine()
    {
        _coroutine.StartCoroutine(HypeBoy(), new CustomCoroutineToken());
    }

    [Button]
    void SetToken()
    {
        _coroutine.SetToken(0, new CustomCoroutineToken(){keepGoing = keepGoining});
    }

    private void Update()
    {
        _coroutine.Update();
    }

    IEnumerator HypeBoy()
    {
        while (true)
        {
            Debug.Log("1");
            yield return Ditto();
            Debug.Log("4");
        }
    }

    IEnumerator Ditto()
    {
        yield return 1;
        Debug.Log("2");
        yield return null;
        Debug.Log("3");
    }
}