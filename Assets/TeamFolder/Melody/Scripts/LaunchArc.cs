using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArc : MonoBehaviour {
    
    public int sideCount;
    LineRenderer lineRenderer;
    //public Vector3 startPoint;
    public Transform startPoint;
    public float angle;
    public float velocity;
    float grav;
    float angleRadian;
    
    Pawn pawn;
    // Use this for initialization


    void Start () {

       // pawn = GetComponent<Pawn>();
        lineRenderer = GetComponent<LineRenderer>();
        grav = Mathf.Abs(Physics2D.gravity.y);
        if (sideCount < 1) sideCount = 1;
       
       // startPoint = pawn.barrel.transform;


        lineRenderer.positionCount = sideCount + 1;
      //Debug.Log(string.Format("sideCount: {0} positionCount: {1}", sideCount , lineRenderer.positionCount));
      
        lineRenderer.SetPosition(0, startPoint.position);


    }
	
	// Update is called once per frame
	void Update () {
        //startpoint
        //startPoint = pawn.barrel.transform;
        lineRenderer.SetPositions(CalculateArcArray());
        //lineRenderer.SetPosition(0, startPoint.position);
        Debug.Log("line renderer position 0: " + lineRenderer.GetPosition(0));
        Debug.Log("line renderer position last: " + lineRenderer.GetPosition(lineRenderer.positionCount -1));
        //lineRenderer.SetPosition(1, CalculateArcPoint(.1f, 11.02f));
        //lineRenderer.SetPosition(2, CalculateArcPoint(.2f, 11.02f));
        //lineRenderer.SetPosition(3, CalculateArcPoint(.3f, 11.02f));
        //lineRenderer.SetPositions(CalculateArcArray());

        //end point
        // lineRenderer.SetPosition(lineRenderer.positionCount - 1, startPoint + pawn.myMask.currentModel.transform.forward * 10);
    }

    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[sideCount + 1];
        angleRadian = Mathf.Deg2Rad * angle;
        float vSinAngle = velocity * Mathf.Sin(angleRadian);
        float vCosAngle = velocity * Mathf.Cos(angleRadian);
        float startY = startPoint.position.y;
        float maxDistance = (vCosAngle/ grav) * (vSinAngle + Mathf.Sqrt(vSinAngle*vSinAngle + 2*grav*startY));
        //Debug.Log("Max Distance: " + maxDistance);

        for (int i = 0; i <= sideCount; i++)
        {
            float t = (float)i / (float)sideCount;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
           // Debug.Log("arcArray[0]: " + arcArray[0]);

        }

        return arcArray;
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float startX = startPoint.position.x;
        float x = t * maxDistance;
        float multiplier = maxDistance* t;
        //Vector3 xz = new Vector3 (startPoint.position.x * multiplier, 1, startPoint.position.z * multiplier);
        float y = startPoint.position.y + x * Mathf.Tan(angleRadian) -((grav*x*x)/(2*velocity*velocity*Mathf.Cos(angleRadian) * Mathf.Cos(angleRadian)) ) ;
        Vector3 velocityY = new Vector3(0, y, 0);

        Vector3 arcPoint = new Vector3( x, y, 0);
        arcPoint = RotatePoint(arcPoint, transform.position, transform.rotation);
        return arcPoint;
    }
    Vector3 RotatePoint(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        Vector3 rotatedPoint = Vector3.zero;
        Vector3 direction = point - pivot;
        direction = angle * direction;
        rotatedPoint = direction + pivot;
        return rotatedPoint;
    }
}
