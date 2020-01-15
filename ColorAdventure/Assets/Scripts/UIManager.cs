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

    public void OnStart(){
    }

    public void OnUpdate(){
        var currentInk = GameManager.Instance.Player.CurrentInk;
        m_sightImage.color = currentInk.InkColor;
        m_inkAmountImage.color = currentInk.InkColor;
        m_inkGaugeSlider.value = currentInk.InkAmount.Value/currentInk.MAX_INK_AMOUNT;
    }
}
