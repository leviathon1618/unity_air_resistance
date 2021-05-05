using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class my_air_script : MonoBehaviour
{
    [Range(2,12)]
    public float dot_density = 10f;
    private Vector3 m_Center;
    private Vector3 m_Size, m_Min, m_Max;  
    private Collider Collider;
    private MeshCollider mesh_collider;
    private GameObject blob;
    private Rigidbody rb;
    public bool hide_dots = true;
    public List<GameObject> push_points;
    
    void Start()
    {
        Collider = GetComponent<Collider>();
        mesh_collider = GetComponent<MeshCollider>();
        blob = GameObject.Find("blob").gameObject;
        rb = GetComponent<Rigidbody>();
        rb.drag = 1;
        if (mesh_collider != null)
        {
            if (mesh_collider.convex == false)
            {
                mesh_collider.convex = true;
            }
        }

    }

    public void redo_surface()
    {
        m_Min = Collider.bounds.min;
        m_Max = Collider.bounds.max;
        float spread_x = m_Max.x - m_Min.x;
        float spread_z = m_Max.z - m_Min.z;
        spread_x /= dot_density;
        spread_z /= dot_density;
        for (float i = 0; i <= m_Max.x - m_Min.x; i += spread_x)
        {
            for (float j = 0; j <= m_Max.z - m_Min.z; j += spread_z)
            {
                GameObject force_point = Instantiate(blob, new Vector3(m_Min.x + i, m_Min.y - 2, m_Min.z + j), Quaternion.Euler(0, 0, 0));
                RaycastHit hit;
                if (Physics.Raycast(force_point.transform.position, Vector3.up, out hit, 10f))
                {
                    force_point.transform.name = i + " " + j;
                    force_point.transform.position = hit.point;
                    force_point.transform.parent = hit.transform;
                    push_points.Add(force_point);
                }
                else
                {
                    Destroy(force_point);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        
        redo_surface();
        push_points = push_points.OrderBy(platform => platform.transform.position.y).ToList();
        float lowest_point = push_points[0].transform.position.y - transform.position.y;
        float highest_point = push_points[push_points.Count - 1].transform.position.y - transform.position.y;
        float total_dist = highest_point - lowest_point;
        foreach (var item in push_points)
        {
            rb.AddForceAtPosition(new Vector3(0, total_dist + (transform.position.y - item.transform.position.y), 0), item.transform.position, ForceMode.Force);
        }
        if (hide_dots == true)
        {
            foreach (var item in push_points)
            {
                Destroy(item);
            }
            push_points.Clear();
        }
        //if (rb.velocity.y >0)
        //{
        //    rb.velocity =new Vector3(rb.velocity.x, rb.velocity.y * -1, rb.velocity.z);
        //}
        //print(rb.velocity.y);
    }
}
