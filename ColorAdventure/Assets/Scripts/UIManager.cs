using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class UIManager : MonoBehaviour{
    [SerializeField] Image m_sightImage;
    [SerializeField] Image m_inkAmountImage;

    [SerializeField] Slider m_inkGaugeSlider;

    [SerializeField] private Text m_cautionText;
    public void OnStart() {
        var player = GameManager.Instance.Player;
        foreach (var ink in player.Inks) {
            ink.InkAmount.Subscribe(x => {
                if (ink == player.CurrentInk) UpdateInkGauge(x);
            }).AddTo(this);
        }
        InkChanged(player.CurrentInk);
        UpdateInkGauge(player.CurrentInk.InkAmount.Value);
    }

    public void OnUpdate(){
        
    }

    void UpdateInkGauge(float amount) {
        m_inkGaugeSlider.value = amount/GameManager.Instance.Player.CurrentInk.MAX_INK_AMOUNT;
    }

    public void InkChanged(Ink currentInk) {
        m_sightImage.color = currentInk.InkColor;
        m_inkAmountImage.color = currentInk.InkColor;
        UpdateInkGauge(currentInk.InkAmount.Value);
    }

    public void ShowCaution() {
        m_cautionText.GetComponent<Animation>().Play();
    }
}
