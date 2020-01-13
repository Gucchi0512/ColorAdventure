using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBall : MonoBehaviour {
    private Color m_inkColor;

    private Transform m_playerCamTransform;
    // Start is called before the first frame update

    public Color InkColor {
        get => m_inkColor;
        set => m_inkColor = value;
    }

    private void Start() {
        GetComponent<MeshRenderer>().material.color = m_inkColor;
        m_playerCamTransform = GameObject.FindWithTag("MainCamera").transform;
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision other) {
        Vector3 dir = transform.position - m_playerCamTransform.position;
        var ray = new Ray(m_playerCamTransform.position, Vector3.Normalize(dir));
        Debug.DrawRay(ray.origin, dir, Color.red, 1f);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo)) {
            Debug.Log(hitInfo.collider.gameObject.name);
            var paintObject = hitInfo.collider.gameObject.GetComponent<PaintedObject>();
            if (paintObject!=null) {
                paintObject.Paint(hitInfo);
            }
        }
        Destroy(gameObject);
    }
}
