using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackParticle : MonoBehaviour {
    

    private Vector2[] fourCorners= {new Vector2(0,0), new Vector2(1, 0), new Vector2(1.0f, 1.5f), new Vector2(0, 1.5f) };

    [SerializeField]
    private GameObject fourCornerTarget;

    [SerializeField]
    private int patrol = 0;

    private bool moveFlag = false;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private bool attackFlag;

    [SerializeField]
    private iTween.EaseType easetype;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //巡回
        if (!attackFlag)
        {
            if (!moveFlag)
            {
                moveFlag = true;
                Hashtable table = new Hashtable();
                table.Add("easeType", easetype);
                var targetSpriteSize = fourCornerTarget.GetComponent<SpriteRenderer>().bounds.size / 2;
                print(targetSpriteSize);
                table.Add("x", fourCornerTarget.transform.position.x - targetSpriteSize.x + fourCorners[patrol].x - fourCornerTarget.transform.position.x / 10.0f + 0.1f);
                table.Add("y", fourCornerTarget.transform.position.y + targetSpriteSize.y - fourCorners[patrol].y - fourCornerTarget.transform.position.y / 10.0f - 0.1f);
                table.Add("time", 1.0f);
                table.Add("oncomplete", "ChangePatrol");
                iTween.MoveTo(gameObject,table);
            }
        }

        //攻撃
        if (attackFlag)
        {
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
        Destroy(this.gameObject);
    }
}
