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
    
    #endregion
    /// <summary>
    /// Shaderの各プロパティをint型の変数で管理
    /// InitPropertyID()で設定する
    /// </summary>
    #region ShaderPropertyID
    
    private int mainTexturePropertyID;
    private int paintUVProptyID;
    private int blushTexturePropertyID;
    private int blushScalePropertyID;
    private int blushColorPropertyID;

    #endregion

    private RenderTexture paintTexture;
    private Material material;

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
        material = meshRenderer.material;
        var mainTex = material.GetTexture(mainTexturePropertyID);

        if (mainTex == null)
        {
            Debug.LogWarning("テクスチャが設定されていません");
            Destroy(this);
            return;
        }
        else
        {
            //テクスチャペイント用のRenderTextureを用意
            paintTexture = new RenderTexture(mainTex.width, mainTex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            //テクスチャのコピー
            Graphics.Blit(mainTex, paintTexture);
            //マテリアルのテクスチャをRenderTextureに変更
            material.SetTexture(mainTexturePropertyID, paintTexture);
        }
        
        
    }

    private void InitPropertyID()
    {
        mainTexturePropertyID = Shader.PropertyToID(m_MainTextureName);
        paintUVProptyID = Shader.PropertyToID("_PaintUV");
        blushColorPropertyID = Shader.PropertyToID("_BlushColor");
        blushScalePropertyID = Shader.PropertyToID("_BlushScale");
        blushTexturePropertyID = Shader.PropertyToID("_Blush");
        
    }

    public bool Paint(RaycastHit hitInfo, Texture blush, Color blushColor, float blushScale)
    {
        if (hitInfo.collider != null && hitInfo.collider.gameObject == gameObject)
        {
            var uv = hitInfo.textureCoord;
            RenderTexture buf = RenderTexture.GetTemporary(paintTexture.width, paintTexture.height);

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
            
            m_paintMaterial.SetVector(paintUVProptyID, uv);
            m_paintMaterial.SetTexture(blushTexturePropertyID, blush);
            m_paintMaterial.SetFloat(blushScalePropertyID, blushScale);
            m_paintMaterial.SetVector(blushColorPropertyID, blushColor);
            Graphics.Blit(paintTexture, buf, m_paintMaterial);
            Graphics.Blit(buf, paintTexture);
            
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

    public bool Paint(RaycastHit hitInfo) {
        return Paint(hitInfo, m_blushTex, m_blushColor, m_blushScale);
    }
}
