using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {
    
    public ParticleSystem booParticles;

    public ParticleSystem doorParticles;

    public ParticleSystem dropCandyParticles;

    public ParticleSystem batParticles;

    public ParticleSystem hitParticles;

    public ParticleSystem tornadoParticles;


    public void booPart()
    {
        booParticles.Play();
    }

    public void doorPart()
    {
        doorParticles.Play();
    }

    public void dropPart()
    {
        dropCandyParticles.Play();
    }

    public void batPart()
    {
        batParticles.Play();
    }

    public void hitPart()
    {
        hitParticles.Play();
    }

}
