using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    
    private PlayerController m_playerController;
    private Ink m_currentInk;
    private Ink[] m_inkArray = new Ink[]{
        new Ink(Color.black),
        new Ink(Color.red),
        new Ink(Color.green),
        new Ink(Color.blue),
        new Ink(Color.yellow)
    };
    private int m_currentInkIndex = 0;
    public Ink CurrentInk => m_currentInk;
    public Ink[] Inks{
        get{ return m_inkArray; }
        set{ m_inkArray = value; }
    }

    public void OnStart(){
        m_currentInk = m_inkArray[m_currentInkIndex];
    }
    public void OnUpdate(){
    }

    public void ChangeNextInk(){
        if(m_currentInkIndex<m_inkArray.Length-1) m_currentInkIndex++;
        else m_currentInkIndex=0;
        m_currentInk = m_inkArray[m_currentInkIndex];
    }

    public void ChangePrebInk(){
        if(m_currentInkIndex>0) m_currentInkIndex--;
        else m_currentInkIndex=m_inkArray.Length-1;
        m_currentInk = m_inkArray[m_currentInkIndex];
    }
}
