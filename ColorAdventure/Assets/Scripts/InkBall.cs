using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBall : MonoBehaviour {
    private Color m_ballInkColor;

    private Transform m_playerCamTransform;
    // Start is called before the first frame update

    public Color InkColor {
        get => m_ballInkColor;
        set => m_ballInkColor = value;
    }

    private void Start() {
        GetComponent<MeshRenderer>().material.color = m_ballInkColor;
        m_playerCamTransform = GameObject.FindWithTag("MainCamera").transform;
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision other) {
        Vector3 dir = transform.position - m_playerCamTransform.position;
        var ray = new Ray(m_playerCamTransform.position, Vector3.Normalize(dir));

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo)) {
            Debug.Log(hitInfo.collider.gameObject.name);
            var paintObject = hitInfo.collider.gameObject.GetComponent<PaintedObject>();
            if (paintObject!=null) {
                paintObject.Paint(hitInfo, m_ballInkColor);
            }
        }
        Destroy(gameObject);
    }
}
