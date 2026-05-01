using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sicoklat:MonoBehaviour, EffectScripts
{

    public int priority = 10;
    public int Priority => priority;

    public IEnumerator playEffects()
    {
        Debug.Log("ilang");
        depleted();
        yield break;
    }

    private void depleted()
    {
        gameObject.SetActive(false);
    }
}