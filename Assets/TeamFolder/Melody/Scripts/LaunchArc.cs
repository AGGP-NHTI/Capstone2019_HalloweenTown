using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArc : MonoBehaviour {
    
    public int sideCount;
    LineRenderer lineRenderer;
    //public Vector3 startPoint;
    public Transform startPoint;
    Transform model;
    public float angle;
    public float velocity;
    float grav;
    float angleRadian;
    
    Pawn pawn;
    Mask mask;
    // Use this for initialization


    void Start () {

        pawn = GetComponent<Pawn>();
        lineRenderer = GetComponent<LineRenderer>();

        grav = Mathf.Abs(Physics2D.gravity.y);
        if (sideCount < 1) sideCount = 1;
       
       // startPoint = pawn.barrel.transform;


        lineRenderer.positionCount = sideCount + 1;
     
      
       


    }
	
	// Update is called once per frame
	void Update () {
        model = pawn.myMask.currentModel.transform;
        startPoint = pawn.myMask.currentModel.transform.GetComponent<GetBarrel>().barrel.transform;
        mask = pawn.myMask;
        
        lineRenderer.SetPositions(CalculateArcArray());
        //Debug.Log("model.rotation" + model.rotation.eulerAngles);
        Debug.Log("line renderer position 0: " + lineRenderer.GetPosition(0));
        Debug.Log("start point position: " + startPoint.position);
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
        //float startX = startPoint.position.x;
        float x = t * maxDistance;
        float multiplier = maxDistance* t;
        //Vector3 xz = new Vector3 (startPoint.position.x * multiplier, 1, startPoint.position.z * multiplier);
        float y = startPoint.position.y + x * Mathf.Tan(angleRadian) -((grav*x*x)/(2*velocity*velocity*Mathf.Cos(angleRadian) * Mathf.Cos(angleRadian)) ) ;
        Vector3 velocityY = new Vector3(0, y, 0);

        Vector3 arcPoint = new Vector3( startPoint.position.x + x, y, 0);
        arcPoint = RotatePoint(arcPoint, startPoint.position , Mathf.Deg2Rad * model.rotation.eulerAngles.y);
        return arcPoint;
    }
    Vector3 RotatePoint(Vector3 point, Vector3 pivot, float angle)
    {
        //Debug.Log("angle: " + angle);
        //translate to origin
        float tempX = point.x - pivot.x;
        float tempZ = point.z - pivot.z;
        //rotate
        
        float primeZ = tempZ * Mathf.Cos(angle) - tempX * Mathf.Sin(angle);
        float primeX = tempZ * Mathf.Cos(angle) + tempX * Mathf.Sin(angle);


        //z = 

        //translate back

        Vector3 rotatedPoint = Vector3.zero;

        rotatedPoint.x = primeX + pivot.x;
        rotatedPoint.z = primeZ + pivot.z;
        rotatedPoint.y = point.y;
        //Vector3 direction = point - pivot;
        //direction = Quaternion.Euler(angle) * direction;
        
        //rotatedPoint = direction + pivot;
        return rotatedPoint;
    }
}
