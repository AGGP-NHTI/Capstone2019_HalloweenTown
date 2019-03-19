using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArc : MonoBehaviour {
    
    public int sideCount;
    LineRenderer lineRenderer;
    public Vector3 startPoint;
    public float angle;
    public float velocity;
    float grav;
    float angleRadian;
    
    Pawn pawn;
    // Use this for initialization


    void Start () {

        pawn = GetComponent<Pawn>();
        lineRenderer = GetComponent<LineRenderer>();
        grav = Mathf.Abs(Physics.gravity.y);
        if (sideCount < 1) sideCount = 1;
       
        startPoint = pawn.barrel.transform.position;


        lineRenderer.positionCount = sideCount + 1;
      //Debug.Log(string.Format("sideCount: {0} positionCount: {1}", sideCount , lineRenderer.positionCount));
      
        lineRenderer.SetPosition(0, startPoint);


    }
	
	// Update is called once per frame
	void Update () {
        //startpoint
        startPoint = pawn.barrel.transform.position;
        lineRenderer.SetPositions(CalculateArcArray());

        //end point
       // lineRenderer.SetPosition(lineRenderer.positionCount - 1, startPoint + pawn.myMask.currentModel.transform.forward * 10);
    }

    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[sideCount + 1];
        angleRadian = Mathf.Deg2Rad * angle;
        float vSinAngle = velocity * Mathf.Sin(angleRadian);
        float vCosAngle = velocity * Mathf.Cos(angleRadian);
        float startY = startPoint.y;
        float maxDistance = (vCosAngle/ grav) * (vSinAngle + Mathf.Sqrt(vSinAngle*vSinAngle + 2*grav*startY));

        for (int i = 0; i <= sideCount; i++)
        {
            float t = (float)i / (float)sideCount;
            arcArray[i] = CalculateArcPoint(t, maxDistance);

        }

        return arcArray;
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
       
        float x = startPoint.x + t * maxDistance;
        float y = startPoint.y + x * (Mathf.Tan(angleRadian) -( (grav*x*x)/(2*velocity*velocity*Mathf.Cos(angleRadian) * Mathf.Cos(angleRadian)) ) );
        Vector3 arcPoint = new Vector3(x, y, 0);
        return arcPoint;
    }
}
