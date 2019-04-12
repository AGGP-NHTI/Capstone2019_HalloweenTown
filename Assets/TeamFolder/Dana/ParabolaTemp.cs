using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParabolaTemp : MonoBehaviour {

    public LineRenderer ln;
    GameObject model;
    public GameObject parabarrel;
    public Quaternion test;
    //public List<Vector3> parabolaPoints = new List<Vector3>();
    public GameObject endpoint;
    Pawn pawn;
    float aim;

    public float parabolaMagnitude = 10f;
    public float maxArcLength = 20f;
    public float parabolaStepSize = 0.1f;
    LayerMask notHittable;
    private void Start()
    {
       
        
        notHittable = 1 << ~LayerMask.NameToLayer("NotHittable");

        model = gameObject.GetComponent<Pawn>().myMask.currentModel;
       
        pawn = gameObject.GetComponent<Pawn>();
        //parabarrel.transform.position = pawn.barrel.transform.position;
        //parabarrel.transform.rotation = pawn.barrel.transform.rotation;
        //parabarrel.transform.localScale = pawn.barrel.transform.localScale;
    }
    void Update()
    {
        
    }


    /*Vector3 Distance(Vector3 origin)
    {
        float x = (float)Mathf.Cos(angle * (Mathf.PI / 180)) * length + origin.x;
        float y = (float)Mathf.Sin(angle * (Mathf.PI / 180)) * length + origin.y;
        float z = (float)Mathf.Tan(angle * (Mathf.PI / 180)) * length + origin.z;
        Vector3 point = new Vector3(x, y,z);
        return point;
    }*/
    

    /* public void DrawLine()
     {

         //Draw the parabola by sample a few times
         // Gizmos.color = Color.red;
         ln.SetPosition(0, a);
         ln.SetPosition(1, b);
         //Gizmos.DrawLine(a, b);
         int count = 20;
         //ln.SetVertexCount(20);
         /*Vector3[] parabolaPoints = new Vector3[count];
         ln.SetVertexCount(count);
         Vector3 lastP = a;
         for (int i = 0; i < count; i++)
         {
             Vector3 p = SampleParabola(a, b, h, i / (float)count);
             //Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
             //ln.SetPosition(i, p);
             parabolaPoints[i] = p;
             //Gizmos.DrawLine(lastP, p);
             lastP = p;
         }
         ln.SetPositions(parabolaPoints);
     }*/

    public List<Vector3> DrawLine()
    {
        parabarrel = new GameObject("programaticallyCreatedParabollaBarrel");
        model = gameObject.GetComponent<Pawn>().myMask.currentModel;
        parabarrel.transform.SetParent(model.transform);
        parabarrel.transform.position = pawn.barrel.transform.position;
        parabarrel.transform.localScale = pawn.barrel.transform.localScale;
        parabarrel.transform.rotation = model.transform.rotation;
        //set layer here 
        //parabarrel.layer = pawn.myMask;
        List<Vector3> parabolaPoints = new List<Vector3>();
        Vector3 point = parabarrel.transform.position;
        Vector3 pointVelocity = parabarrel.transform.forward * parabolaMagnitude;

        parabolaPoints.Add(point);

        RaycastHit hit = new RaycastHit();
        Ray parabolaArcRay;
        bool parabolaCollided = false;

        for (float distanceArced = 0.0f; distanceArced < maxArcLength && !parabolaCollided; distanceArced += parabolaStepSize)
        {
            parabolaArcRay = new Ray(point, pointVelocity);
            //Debug.DrawRay(parabolaArcRay.origin, parabolaArcRay.direction);
            if (Physics.Raycast(parabolaArcRay, out hit, parabolaStepSize, notHittable))
            {                
                parabolaCollided = true;
                parabolaPoints.Add(hit.point);                
            }
            else
            {
                point += pointVelocity.normalized * parabolaStepSize;
                parabolaPoints.Add(point);
            }

            pointVelocity += Physics.gravity * parabolaStepSize;
        }

        ln.positionCount = parabolaPoints.Count;
        ln.SetPositions(parabolaPoints.ToArray());
        GameObject.Destroy(parabarrel);
        return parabolaPoints;
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
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
    }
}
