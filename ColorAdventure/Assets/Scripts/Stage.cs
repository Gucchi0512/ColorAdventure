using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {
    [SerializeField] private GameObject m_start;
    [SerializeField] private Goal m_goal;
    [SerializeField] private FeltArea m_feltArea;
    [SerializeField]private PaintedObject[] m_walls;
    
    public Goal Goal => m_goal;
    public FeltArea FeltArea => m_feltArea;
    public GameObject Start => m_start;
    
    // Start is called before the first frame update
    public void OnStart() {
        var walls = GameObject.FindGameObjectsWithTag("Stage");
        m_walls = new PaintedObject[walls.Length];
        for (int i = 0; i < m_walls.Length; i++) {
            m_walls[i] = walls[i].GetComponent<PaintedObject>();
            m_walls[i].OnStart();
        }
    }

    // Update is called once per frame
    public void OnUpdate() {
        
    }
}
