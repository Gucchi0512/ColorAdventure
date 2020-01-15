using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class Ink{

    public readonly float MAX_INK_AMOUNT = 100;
    public readonly float MIN_INK_AMOUNT = 0;
    public readonly float INK_CONSUMPTION = 2;

    private Color m_inkColor;
    ReactiveProperty<float> m_inkAmount = new ReactiveProperty<float>(0f);

    public Color InkColor => m_inkColor;

    public IReadOnlyReactiveProperty<float> InkAmount => m_inkAmount;

    /// <summary>
    /// 引数なしなら黒で初期化
    /// </summary>
    public Ink(){
        m_inkColor = Color.black;
        m_inkAmount.Value = MAX_INK_AMOUNT;
    }
    public Ink(Color color){
        m_inkColor = color;
        m_inkAmount.Value = MAX_INK_AMOUNT;
    }
    public bool UseInk(){
        if(m_inkAmount.Value>=MIN_INK_AMOUNT){
            m_inkAmount.Value -= INK_CONSUMPTION;
            return true;
        }else{
            return false;
        }
    }
    public bool ChargeInk(float chargeAmount){
        if(m_inkAmount.Value<=MAX_INK_AMOUNT){
            m_inkAmount.Value += chargeAmount;
            return true;
        }else{
            return false;
        }
    }
}
