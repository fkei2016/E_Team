using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackParticle : MonoBehaviour {


    private Vector2[] fourCorners = { new Vector2(-1.0f, 1.0f), new Vector2(1.0f, 1.0f), new Vector2(1.0f, -1.0f), new Vector2(-1.0f, -1.0f) };

    
    public GameObject fourCornerTarget;

    [SerializeField]
    private int patrol = 0;

    private bool moveFlag = false;

    public GameObject target;

    public bool attackFlag;

    [SerializeField]
    private iTween.EaseType easetype;

    [SerializeField]
    private ParticleSystem wildfire;

    [SerializeField]
    private ParticleSystem explosion;

    private Vector2 targetSize;
    public Vector2 TargetSize
    {
        set
        {
            targetSize = value;
        }
    }

    //パーティクルのサイズ
    [SerializeField]
    private float particleSize = 1.0f;

    public float ParticleSize
    {
        set
        {
            particleSize = value;
            GetComponent<ParticleSystem>().startSize = particleSize;
        }
        get
        {
            return particleSize;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        GetComponent<ParticleSystem>().startSize = particleSize;

        //巡回
        if (!attackFlag)
        {
            if (!moveFlag)
            {
                moveFlag = true;
                Hashtable table = new Hashtable();
                table.Add("easeType", easetype);
                var targetSpriteSize = (targetSize / 2) / 10;
                table.Add("x", fourCornerTarget.transform.position.x - targetSpriteSize.x * fourCorners[patrol].x);
                table.Add("y", fourCornerTarget.transform.position.y + targetSpriteSize.y * fourCorners[patrol].y);
                table.Add("time", 1.0f);
                table.Add("oncomplete", "ChangePatrol");
                iTween.MoveTo(gameObject,table);
            }
        }

        //攻撃
        if (attackFlag)
        {
            wildfire.gameObject.SetActive(true);
            Hashtable table = new Hashtable();
            table.Add("easeType", easetype);
            table.Add("x", target.transform.position.x);
            table.Add("y", target.transform.position.y);
            table.Add("time", 1.0f);
            table.Add("oncomplete", "attackDestroy");
            iTween.MoveTo(gameObject, table);
        }
    }

    void ChangePatrol()
    {
        patrol++;
        if (patrol == 4)
            patrol = 0;
        moveFlag = false;
    }

    void attackDestroy()
    {
        explosion.gameObject.SetActive(true);
        Destroy(this.gameObject,1.0f);
    }
}
