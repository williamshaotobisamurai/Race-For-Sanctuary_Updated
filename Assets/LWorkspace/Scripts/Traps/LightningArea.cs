using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningArea : MonoBehaviour
{
    [SerializeField] private ParticleSystem hintParticle;
    [SerializeField] private ParticleSystem strikeParticle;

    public void ShowHintParticle()
    {
        hintParticle.Play();
    }

    public void HitSkully(Skully skully)
    {
        if (hintParticle.isPlaying)
        {
            strikeParticle.transform.position = skully.transform.position;
            strikeParticle.Play();
            skully.HitByLightningStrike();
        }
    }
}
