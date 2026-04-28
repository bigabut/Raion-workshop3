using System.Collections;
using UnityEngine;

public class knife : MonoBehaviour, EffectScripts
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
        if(gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}
