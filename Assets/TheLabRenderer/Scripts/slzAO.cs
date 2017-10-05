using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slzAO : MonoBehaviour {

    [NonSerialized]
   // [HideInInspector]
    public static List<slzAO> s_allAOSpheres = new List<slzAO>();

    public enum AOShape { Point, Sphere };

    public AOShape TypeOfShape = AOShape.Point;

    public float SphereRadius = 1;

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {

        if (UnityEditor.Selection.Contains(gameObject))
        {


            if (TypeOfShape == AOShape.Point)
            {

                Gizmos.color = Color.grey;
                Gizmos.DrawWireSphere(transform.position, SphereRadius);
                Gizmos.DrawIcon(transform.position, "pointshadow.png");            }

            if (TypeOfShape == AOShape.Sphere)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(transform.position, SphereRadius);
            }

        }

    }
#endif

    void OnEnable()
    {
        if (!s_allAOSpheres.Contains(this))
        {
            s_allAOSpheres.Add(this);
          //  m_cachedLight = GetComponent<Light>();
        }
    }



    void OnDisable()
    {
        s_allAOSpheres.Remove(this);
    }


    // Update is called once per frame
    void Update()
    {

        if (TypeOfShape == AOShape.Sphere)
        {
     //       Shader.SetGlobalVector("_SphereAO", new Vector4(transform.position.x, transform.position.y, transform.position.z, SphereRadius));


       //     Shader.SetGlobalVector("_PointAO", new Vector4(transform.position.x, transform.position.y, transform.position.z, 0));

        }

        if (TypeOfShape == AOShape.Point)
        {
      //      Shader.SetGlobalVector("_PointAO", new Vector4(transform.position.x, transform.position.y, transform.position.z, SphereRadius));



      //      Shader.SetGlobalVector("_SphereAO", new Vector4(transform.position.x, transform.position.y, transform.position.z, 0));

        }

    }
}
