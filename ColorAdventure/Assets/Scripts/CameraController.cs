using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Transform m_playerTransform;
    private int m_isReverseNum = 1;

    public bool isReverse = false;
    // Start is called before the first frame update
    void Start() {
        m_playerTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked) {
            m_isReverseNum = isReverse ? -1 : 1;
            float xRot = Input.GetAxis("Mouse X");
            float yRot = Input.GetAxis("Mouse Y");
            m_playerTransform.Rotate(0, m_isReverseNum*xRot,0);
            this.transform.Rotate(-(m_isReverseNum)*yRot, 0, 0);
        }
    }
}
