using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private PaintedObject[] m_walls;
    // Start is called before the first frame update
    public void OnStart() {
        var walls = GameObject.FindGameObjectsWithTag("Wall");
        m_walls = new PaintedObject[walls.Length];
        for (int i = 0; i < m_walls.Length; i++) {
            m_walls[i] = walls[i].GetComponent<PaintedObject>();
            m_walls[i].OnStart();
        }
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        
    }
}
