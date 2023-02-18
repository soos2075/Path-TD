using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineRender : MonoBehaviour
{
    public GameObject particle_Line;

    public LineRenderer myLine;

    void Start()
    {
        myLine = Instantiate(particle_Line).GetComponent<LineRenderer>();
    }

    //public void DrawLine(List<A_star_Radiant.Pos> pos)
    //{

    //    //myLine.material
    //    myLine.positionCount = pos.Count;

    //    for (int i = 0; i < pos.Count; i++)
    //    {
    //        myLine.SetPosition(i, new Vector3(pos[i].posX, pos[i].posY, 0));
    //    }
    //}
}
