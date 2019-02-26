using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    

    public ParticleSystem booParticles;

    public ParticleSystem doorParticles;

    public ParticleSystem dropCandyParticles;

    public ParticleSystem batParticles;

    public ParticleSystem hitParticles;

    public void booPart()
    {
        booParticles.Play();
    }

}
