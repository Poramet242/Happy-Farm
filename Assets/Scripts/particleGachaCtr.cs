using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleGachaCtr : MonoBehaviour
{
    public static particleGachaCtr instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public ParticleSystem particale_plante;
    public ParticleSystem particale_aura;
    public ParticleSystem particale_aura2;
}
