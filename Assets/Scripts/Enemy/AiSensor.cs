using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSensor : MonoBehaviour
{
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public float heightOffsetFromOrigin = 1.0f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    public List<GameObject> Objects
    {
        get
        {
            //removes all null objects in list.
            objects.RemoveAll(obj => !obj);
            return objects;
        }
    }
    private List<GameObject> objects = new List<GameObject>();

    Collider[] colliders = new Collider[50];
    Mesh mesh;

    int count;
    float scanInterval;
    float scanTimer;
    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    public void Scan()
    {
        //adds all the physical objects scanned in the cone distance to the count.
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        objects.Clear();
        //adds objects found in the layer to the object array.
        for(int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInsight(obj.transform.position))
            {
                objects.Add(obj);
            }
        }
    }

    public bool IsInsight(Vector3 obj)
    {
        Vector3 origin = transform.position;
        origin.y *= 0.5f;
        Vector3 dest = obj;
        Vector3 direction = (dest - origin).normalized;
        //checks if an object is within the height of the sensor
        if(direction.y < 0 || direction.y > height)
        {
            return false;
        }
        //checks if an object is within the horizontal of the sensor
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }
        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            //Debug.Log("Entered raycast line, " + hit.collider.tag);
            //Debug.DrawLine(origin, dest, Color.green, 1f);
            if (hit.collider.tag == "Player")
            {
                return true;
            }

        }

        return false;
    }


    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        //amount of triangles that make up the sensor wedge
        int numTriangles = (segments * 4) + 2 + 2;
        //amount of vertices, each triangle has 3
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        //defines the starting point of the wedge at characters origin.
        Vector3 bottomCenter = new Vector3(0, -heightOffsetFromOrigin, 0);
        //defines bottom left with a vector at the negative angle and multiplying it by the agents forward vector and the distance scalar
        //same but with positive to the right
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        //takes other three vectors and places them in the same spot, moving them up by the height scalar.
        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;


        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;
        //loops on the left side to set the triangle from bottom center back to the bottom center
        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;
        //loops on the Right side to set the triangle from bottom center back to the bottom center
        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        //subdivides current wedges into other wedges so its a cone rather than a triangular prism
        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            //redefines bottom left with a vector at the negative angle and multiplying it by the agents forward vector and the distance scalar
            //same but with positive to the right
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            //redefines left and right vectors
            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;
            //loops on the far side, going from bottom left vertex, to bottom right, to top right, top left, then back to bottom left.
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;


            //top of wedge
            //only loops through once, as only one triangle is needed to create top side.
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom of wedge
            //only loops through once, as only one triangle is needed to create bottom side.
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
            currentAngle += deltaAngle;
        }
        


        for(int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }
        //sets all the vertices and triangles of the mesh in order that is listed above, then recalculates the norms to the updated triangles.
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            //draws the sight mesh on the agent transform and position.
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }
}
