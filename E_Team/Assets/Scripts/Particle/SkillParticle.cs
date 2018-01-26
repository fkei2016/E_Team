using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticle : MonoBehaviour {

    [SerializeField]
    private GameObject[] particles;

    public GameObject Emission(int useNumber, Vector2 position, float time) {
        var original = particles[useNumber];
        var particle = Instantiate(original, position, original.transform.rotation, transform);
        Destroy(particle, time);
        return particle;
    }
}
