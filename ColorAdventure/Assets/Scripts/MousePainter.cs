using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class MousePainter : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // クリックされたスクリーンの位置からRayを飛ばす
            var ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;

            // Rayとの当たり判定
            if (Physics.Raycast(ray, out hitInfo)) {
                var paintObject = hitInfo.collider.gameObject.GetComponent<PaintedObject>();
                if (paintObject!=null) {
                    paintObject.Paint(hitInfo);
                }
            }
        }
    }
}