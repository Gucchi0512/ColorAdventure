using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBall : MonoBehaviour {
    [SerializeField] private GameObject m_hitEffect;
    private Color m_ballInkColor;

    private Transform m_playerCamTransform;

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
        var contactPoint = other.contacts[0];
        Vector3 dir = this.gameObject.transform.position - m_playerCamTransform.position;
        var ray = new Ray(m_playerCamTransform.position, Vector3.Normalize(dir));
        
        var rot = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
        var eff = Instantiate(m_hitEffect, this.gameObject.transform.position, rot).GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = eff.main;
        main.startColor = m_ballInkColor;
        eff.Play();
        
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo)) {
            var paintObject = hitInfo.collider.gameObject.GetComponent<PaintedObject>();
            if (paintObject!=null) {
                if (other.gameObject.CompareTag("Goal")) other.gameObject.GetComponent<Goal>().IsPainted = true;
                paintObject.Paint(hitInfo, m_ballInkColor);
            }
        }
        Destroy(gameObject);
    }
}
