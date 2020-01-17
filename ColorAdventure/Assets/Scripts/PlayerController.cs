using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    private Rigidbody m_rb;
    
    private LayerMask m_layerMask;
    
    private bool isGround = true;
        
    [SerializeField] private float m_walkSpeed;
    [SerializeField] private float m_jumpPower;
    [SerializeField] private float m_shotPower;

    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform camera;
    
    [SerializeField] private GameObject inkBallPrefab;

    // Start is called before the first frame update
    public void OnStart() {
        m_rb = GetComponent<Rigidbody>();
        m_layerMask = LayerMask.GetMask("Player");
       
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        #region Input

        if (isGround) {
            #region Move
            if (Input.GetKey(KeyCode.W)) transform.position += transform.forward * m_walkSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) transform.position -= transform.forward * m_walkSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) transform.position += transform.right * m_walkSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) transform.position -= transform.right * m_walkSpeed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space)) {
                isGround = false;
                m_rb.velocity = new Vector3(0, m_jumpPower, 0);
            }
            #endregion Move

            #region Action
            if(Input.GetKeyDown(KeyCode.V)) GameManager.Instance.Player.ChangeNextInk();
            if(Input.GetKeyDown(KeyCode.C)) GameManager.Instance.Player.ChangePrebInk();
            if (Input.GetMouseButtonDown(0)) Shot();
            #endregion Action

        }
        
        #endregion
        //接地判定
        if(Physics.CheckSphere(transform.position, 1.0f, m_layerMask)) {
            if (!isGround) isGround = true;
        }
    }

    void Shot() {
        var currentInk = GameManager.Instance.Player.CurrentInk;
        if (currentInk.UseInk()) {
            var inkBall = Instantiate(inkBallPrefab);
            inkBall.GetComponent<InkBall>().InkColor = currentInk.InkColor;
            inkBall.transform.position = muzzle.position;
            inkBall.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
            var inkRb = inkBall.GetComponent<Rigidbody>();
            inkRb.AddForce(camera.transform.forward*m_shotPower);
        }else {
            GameManager.Instance.UIManager.ShowCaution();   
        }
    }
}
