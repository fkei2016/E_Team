using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticle : MonoBehaviour {

    [SerializeField]
    private GameObject[] particles;

    public void Emission(int useNumber, Vector2 screenPosition) {
        var fx = Instantiate(particles[useNumber], transform);
        fx.transform.position = screenPosition;
    }
}
