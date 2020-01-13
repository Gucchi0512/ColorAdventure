using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Transform m_playerTransform;
    // Start is called before the first frame update
    void Start() {
        m_playerTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        float xRot = Input.GetAxis("Mouse X");
        float yRot = Input.GetAxis("Mouse Y");
        m_playerTransform.Rotate(0, -xRot,0);
        this.transform.Rotate(yRot, 0, 0);
    }
}
