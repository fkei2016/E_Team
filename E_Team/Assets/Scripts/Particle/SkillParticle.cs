using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticle : MonoBehaviour {

    [SerializeField]
    private GameObject[] particles;

    public void Emission(int useNumber, Vector2 position, float time) {
        var particle = Instantiate(particles[useNumber], position, Quaternion.identity, transform);
        Destroy(particle, time);
    }
}
