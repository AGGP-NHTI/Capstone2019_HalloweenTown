using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaper : Projectile {
        
    public float throwForce;
    public Vector3 moveSpeed;

    Vector3 throwAngle;
    float stunTime = 5f;
    public List<Vector3> parabolaPoints = new List<Vector3>();
    float speed = 50f;

    public ParticleSystem tpTrail;

    // Use this for initialization
    void Start () {
        

        base.Start();
        tpTrail.Play();
        StartCoroutine(ThrowToiletPaper());
        
        //doathrow();
        //throwAngle = (transform.forward  + transform.up).normalized * throwForce;
        //throwAngle += moveSpeed;
        // rb.velocity = throwAngle;

        //for(int i =0; i < throwyPoints.Length; i++)
        //{
        //    gameObject.transform.position =  throwyPoints[i];
        //}
    }
	
	// Update is called once per frame
	void Update () {
        
            //SampleParabola(throwyPoints[0], throwyPoints[throwyPoints.Length - 1], 4, Time.time % 1);
        /*if(gameObject.transform.position.y < 0)
        {
            Destroy(gameObject);
        }*/
	}

    protected void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            
            Debug.Log("Collision Toilet Paper");
            if (collision.gameObject != owner)
            {

                GameObject player = collision.gameObject;
                Stun stun = player.GetComponent<Stun>();

                if (stun.stunned == false)
                {
                    stun.StunPlayer(stunTime);
                    //StartCoroutine(stun.suspendMovement(stunTime));
                }
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }*/

    IEnumerator ThrowToiletPaper()
    {
        
        for (int i = 1; i < parabolaPoints.Count; i++)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, parabolaPoints[i], speed * Time.deltaTime);
            //gameObject.transform.position = throwyPoints[i];
            if (gameObject.transform.position == parabolaPoints[parabolaPoints.Count-1])
            {
                Destroy(gameObject);
            }
            yield return null;// new WaitForSeconds(0.2f);
        }
    }

}
