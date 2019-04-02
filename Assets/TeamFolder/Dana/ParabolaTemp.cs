using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParabolaTemp : MonoBehaviour {

     //object that moves along parabola.
    float objectT = 0; //timer for that object

    public Transform ChildVector, Tb; //transforms that mark the start and end
    public float h; //desired parabola height

    Vector3 a, b; //Vector positions for start and end
    public LineRenderer ln;
    public float angle = 0;
    public float length = 10;
    GameObject barrel;
    GameObject model;
    public Quaternion test;
    public Vector3[] parabolaPoints;
    public GameObject endpoint;
    Pawn pawn;
    float aim;
    private void Start()
    {
        //Tb = Distance(gameObject.transform.position);
        barrel = gameObject.GetComponent<Pawn>().barrel;
        model = gameObject.GetComponent<Pawn>().myMask.currentModel;
        pawn = gameObject.GetComponent<Pawn>();
    }
    void Update()
    {
        //do when endpoint stays at y = 0
        /*test = model.transform.rotation;
        angle = model.transform.localRotation.y;
        if(model.transform.rotation.x <1)
        {
            endpoint.transform.position = new Vector3(endpoint.transform.position.x, endpoint.transform.position.y, endpoint.transform.position.z + model.transform.rotation.x);
        }*/


        //aim = model.transform.rotation.x;
       // endpoint.transform.position = new Vector3(endpoint.transform.position.x, endpoint.transform.position.y, aim);
        //DrawLine();
        //Tb = Distance(gameObject.transform.position);
        ChildVector = barrel.transform;
        
            a = ChildVector.position; //Get vectors from the transforms
            b = Tb.position;

            /*if (someObject)
            {
                //Shows how to animate something following a parabola
                objectT = Time.time % 1; //completes the parabola trip in one second
                someObject.position = SampleParabola(a, b, h, objectT);
            }*/
        
    }


    /*Vector3 Distance(Vector3 origin)
    {
        float x = (float)Mathf.Cos(angle * (Mathf.PI / 180)) * length + origin.x;
        float y = (float)Mathf.Sin(angle * (Mathf.PI / 180)) * length + origin.y;
        float z = (float)Mathf.Tan(angle * (Mathf.PI / 180)) * length + origin.z;
        Vector3 point = new Vector3(x, y,z);
        return point;
    }*/

    //void OnDrawGizmos()
    
    public void DrawLine()
    {

        //Draw the parabola by sample a few times
        // Gizmos.color = Color.red;
        ln.SetPosition(0, a);
        ln.SetPosition(1, b);
        //Gizmos.DrawLine(a, b);
        int count = 20;
        //ln.SetVertexCount(20);
        /*Vector3[]*/ parabolaPoints = new Vector3[count];
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
