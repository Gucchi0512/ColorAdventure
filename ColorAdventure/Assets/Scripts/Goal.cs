using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Goal : MonoBehaviour {
    private bool m_isPainted = false;

    private void Start() {
        GetComponent<PaintedObject>().OnStart();
    }

    public bool IsPainted{
        get { return m_isPainted; }
        set { m_isPainted = value; }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") && m_isPainted) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.Instance.SceneChanger.SceneChange("EndScene");
        }
    }
}
