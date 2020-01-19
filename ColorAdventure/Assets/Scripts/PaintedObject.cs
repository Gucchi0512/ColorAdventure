using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.UI;


[RequireComponent(typeof(Material))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class PaintedObject : MonoBehaviour
{
    #region SerializedProperties
    
    [SerializeField, Tooltip("メインテクスチャ")] private string m_MainTextureName = "_MainTex";
    [SerializeField, Tooltip("テクスチャペイント用マテリアル")] private Material m_paintMaterial = null;
    [SerializeField, Tooltip("ブラシ用テクスチャ")] private Texture2D m_blushTex = null;
    [SerializeField, Tooltip("ブラシサイズ")] private float m_blushScale = 0.1f;
    [SerializeField, Tooltip("ブラシの色")] private Color m_blushColor = default(Color);
    [SerializeField, Tooltip("着色可能な色")] private PaintableColor m_paintableColor = default(PaintableColor);
    #endregion
    /// <summary>
    /// Shaderの各プロパティをint型の変数で管理
    /// InitPropertyID()で設定する
    /// </summary>
    #region ShaderPropertyID
    
    private int m_mainTexturePropertyID;
    private int m_paintUVProptyID;
    private int m_blushTexturePropertyID;
    private int m_blushScalePropertyID;
    private int m_blushColorPropertyID;

    #endregion

    private RenderTexture m_paintTexture;
    private Material m_material;
    private Color m_paintableBlushColor;
    
    public enum PaintableColor {
        BLACK,
        RED,
        GREEN,
        BLUE,
        YELLOW
    }
    public float BlushScale
    {
        get => Mathf.Clamp01(m_blushScale);
        set => m_blushScale = Mathf.Clamp01(value);
    }

    public Color BlushColor
    {
        get => m_blushColor;
        set => m_blushColor = value;
    }

    public Texture2D BlushTex
    {
        get => m_blushTex;
        set => m_blushTex = (Texture2D)value;
    }
    private void Awake()
    {
        InitPropertyID();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        m_material = meshRenderer.material;
        m_paintableBlushColor = GameManager.Instance.Player.Inks[(int)m_paintableColor].InkColor;
        var mainTex = m_material.GetTexture(m_mainTexturePropertyID);

        if (mainTex == null)
        {
            Debug.LogWarning("テクスチャが設定されていません");
            Destroy(this);
            return;
        }
        else
        {
            //テクスチャペイント用のRenderTextureを用意
            m_paintTexture = new RenderTexture(mainTex.width, mainTex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            //テクスチャのコピー
            Graphics.Blit(mainTex, m_paintTexture);
            //マテリアルのテクスチャをRenderTextureに変更
            m_material.SetTexture(m_mainTexturePropertyID, m_paintTexture);
        }
        
        
    }

    private void InitPropertyID()
    {
        m_mainTexturePropertyID = Shader.PropertyToID(m_MainTextureName);
        m_paintUVProptyID = Shader.PropertyToID("_PaintUV");
        m_blushColorPropertyID = Shader.PropertyToID("_BlushColor");
        m_blushScalePropertyID = Shader.PropertyToID("_BlushScale");
        m_blushTexturePropertyID = Shader.PropertyToID("_Blush");
        
    }

    public bool Paint(RaycastHit hitInfo, Texture blush, Color blushColor, float blushScale) {
        if (hitInfo.collider != null && hitInfo.collider.gameObject == gameObject && blushColor==m_paintableBlushColor)
        {
            var uv = hitInfo.textureCoord;
            RenderTexture buf = RenderTexture.GetTemporary(m_paintTexture.width, m_paintTexture.height);

            #region ErrorCheck

            if (buf == null)
            {
                Debug.LogError("テンポラリテクスチャが生成できませんでした");
                return false;
            }

            if (blush == null)
            {
                Debug.LogError("ブラシが設定されていません");
                return false;
            }

            if (m_paintMaterial == null) {
                Debug.LogError("ブラシ用のマテリアルが設定されていません");
                return false;
            }
            #endregion

            blushScale = Mathf.Clamp01(blushScale);
            
            m_paintMaterial.SetVector(m_paintUVProptyID, uv);
            m_paintMaterial.SetTexture(m_blushTexturePropertyID, blush);
            m_paintMaterial.SetFloat(m_blushScalePropertyID, blushScale);
            m_paintMaterial.SetVector(m_blushColorPropertyID, blushColor);
            Graphics.Blit(m_paintTexture, buf, m_paintMaterial);
            Graphics.Blit(buf, m_paintTexture);
            
            RenderTexture.ReleaseTemporary(buf);
            return true;
        }
        return false;
    }

    public bool Paint(RaycastHit hitInfo, Texture blush, Color blushColor) {
        return Paint(hitInfo, blush, blushColor, m_blushScale);
    }

    public bool Paint(RaycastHit hitInfo, Texture blush) {
        return Paint(hitInfo, blush, m_blushColor, m_blushScale);
    }

    public bool Paint(RaycastHit hitInfo, Color blushColor) {
        return Paint(hitInfo, m_blushTex, blushColor, m_blushScale);
    }

    public bool Paint(RaycastHit hitInfo) {
        return Paint(hitInfo, m_blushTex, m_blushColor, m_blushScale);
    }
}
