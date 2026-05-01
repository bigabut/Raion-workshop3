
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CamerasShakeCM : MonoBehaviour
{
    public CinemachineCamera vcam;

    CinemachineBasicMultiChannelPerlin noise;
    Coroutine shakeRoutine;

    void Awake()
    {
        noise = vcam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        if (noise == null) return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        // 💥 set amplitude
        noise.AmplitudeGain = 3f;

        yield return new WaitForSeconds(0.5f);

        // 🔄 balik normal
        noise.AmplitudeGain = 0f;

        shakeRoutine = null;
    }
}