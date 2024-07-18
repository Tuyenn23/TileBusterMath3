using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveFX : MonoBehaviour
{
    public float delayTime = 2f; // Th?i gian ch? ?? k�ch ho?t Particle System

    private void OnEnable()
    {
        Invoke("ActivateParticleSystem", delayTime);
    }

    void ActivateParticleSystem()
    {
        SimplePool.Despawn(gameObject);
    }
}
