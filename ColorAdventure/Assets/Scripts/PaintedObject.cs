using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Material))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class PaintedObject : MonoBehaviour {
    #region SerializedProperties
    
    [SerializeField, Tooltip("メインテクスチャ")] private string m_MainTextureName = "_MainTex";
    [SerializeField, Tooltip("テクスチャペイント用マテリアル")] private Material m_paintMaterial = null;
    [SerializeField, Tooltip("ブラシ用テクスチャ")] private Texture2D m_brushTex = null;
    [SerializeField, Tooltip("ブラシサイズ")] private float m_brushScale = 0.1f;
    [SerializeField, Tooltip("ブラシの色")] private Color m_brushColor = default(Color);
    #endregion
    /// <summary>
    /// Shaderの各プロパティをint型の変数で管理
    /// InitPropertyID()で設定する
    /// </summary>
    #region ShaderPropertyID
    
    private int m_mainTexturePropertyID;
    private int m_paintUVProptyID;
    private int m_brushTexturePropertyID;
    private int m_brushScalePropertyID;
    private int m_brushColorPropertyID;

    #endregion

    private RenderTexture m_paintTexture;
    private Material m_material;
    [SerializeField]private Color m_paintablebrushColor;
    
    public float brushScale {
        get => Mathf.Clamp01(m_brushScale);
        set => m_brushScale = Mathf.Clamp01(value);
    }

    public Color brushColor {
        get => m_brushColor;
        set => m_brushColor = value;
    }

    public Texture2D brushTex {
        get => m_brushTex;
        set => m_brushTex = (Texture2D)value;
    }
    public void OnStart() {
        InitPropertyID();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        var player = GameManager.Instance.Player;
        
        m_material = meshRenderer.material;
        int rand = Random.Range(0, player.NumOfInks);
        m_paintablebrushColor = player.Inks[rand].InkColor;
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

    private void InitPropertyID() {
        m_mainTexturePropertyID = Shader.PropertyToID(m_MainTextureName);
        m_paintUVProptyID = Shader.PropertyToID("_PaintUV");
        m_brushColorPropertyID = Shader.PropertyToID("_BrushColor");
        m_brushScalePropertyID = Shader.PropertyToID("_BrushScale");
        m_brushTexturePropertyID = Shader.PropertyToID("_Brush");
        
    }

    public bool Paint(RaycastHit hitInfo, Texture brush, Color brushColor, float brushScale) {
        if (hitInfo.collider != null && hitInfo.collider.gameObject == gameObject && brushColor==m_paintablebrushColor) {
            Debug.Log("call");
            var uv = hitInfo.textureCoord;
            RenderTexture buf = RenderTexture.GetTemporary(m_paintTexture.width, m_paintTexture.height);

            #region ErrorCheck

            if (buf == null)
            {
                Debug.LogError("テンポラリテクスチャが生成できませんでした");
                return false;
            }

            if (brush == null)
            {
                Debug.LogError("ブラシが設定されていません");
                return false;
            }

            if (m_paintMaterial == null) {
                Debug.LogError("ブラシ用のマテリアルが設定されていません");
                return false;
            }
            #endregion

            brushScale = Mathf.Clamp01(brushScale);
            
            m_paintMaterial.SetVector(m_paintUVProptyID, uv);
            m_paintMaterial.SetTexture(m_brushTexturePropertyID, brush);
            m_paintMaterial.SetFloat(m_brushScalePropertyID, brushScale);
            m_paintMaterial.SetVector(m_brushColorPropertyID, brushColor);
            Graphics.Blit(m_paintTexture, buf, m_paintMaterial);
            Graphics.Blit(buf, m_paintTexture);
            
            RenderTexture.ReleaseTemporary(buf);
            return true;
        }
        return false;
    }

    public bool Paint(RaycastHit hitInfo, Texture brush, Color brushColor) {
        return Paint(hitInfo, brush, brushColor, m_brushScale);
    }

    public bool Paint(RaycastHit hitInfo, Texture brush) {
        return Paint(hitInfo, brush, m_brushColor, m_brushScale);
    }

    public bool Paint(RaycastHit hitInfo, Color brushColor) {
        return Paint(hitInfo, m_brushTex, brushColor, m_brushScale);
    }

    public bool Paint(RaycastHit hitInfo) {
        return Paint(hitInfo, m_brushTex, m_brushColor, m_brushScale);
    }
}
