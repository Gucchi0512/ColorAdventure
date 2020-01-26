using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InkTanks : MonoBehaviour {
    [SerializeField]private float m_chargeInkAmount;
    
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            var player = GameManager.Instance.Player;
            var inkChange = player.Inks.Select(ink => ink.ChargeInk(m_chargeInkAmount));
            foreach(var item in inkChange){}
            Destroy(gameObject);
        }
    }
}
