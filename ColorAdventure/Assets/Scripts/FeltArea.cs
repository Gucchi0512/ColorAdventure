using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeltArea : MonoBehaviour{
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            var startPos = GameManager.Instance.Stage.Start.transform.position;
            var player = GameManager.Instance.Player;
            
            player.gameObject.transform.position = new Vector3(startPos.x, startPos.y+10f, startPos.z);
        }
    }
}
