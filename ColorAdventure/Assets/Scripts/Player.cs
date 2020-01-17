using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    
    private PlayerController m_playerController;
    private Ink m_currentInk;
    private List<Ink> m_inkList = new List<Ink>(){
        new Ink(Color.black),
        new Ink(Color.red),
        new Ink(Color.green),
        new Ink(Color.blue),
        new Ink(Color.yellow)
    };
    private int m_currentInkIndex = 0;
    public Ink CurrentInk => m_currentInk;
    public List<Ink> Inks{
        get{ return m_inkList; }
        set{ m_inkList = value; }
    }

    public void OnStart(){
        m_currentInk = m_inkList[m_currentInkIndex];
    }
    public void OnUpdate(){
    }

    public void ChangeNextInk(){
        if(m_currentInkIndex<m_inkList.Count-1) m_currentInkIndex++;
        else m_currentInkIndex=0;
        m_currentInk = m_inkList[m_currentInkIndex];
        GameManager.Instance.UIManager.InkChanged(m_currentInk);
    }

    public void ChangePrebInk(){
        if(m_currentInkIndex>0) m_currentInkIndex--;
        else m_currentInkIndex=m_inkList.Count-1;
        m_currentInk = m_inkList[m_currentInkIndex];
        GameManager.Instance.UIManager.InkChanged(m_currentInk);
    }
}
