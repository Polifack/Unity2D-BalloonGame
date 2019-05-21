using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    [Header("Follow Settings")]
    public Player target;
    public bool canMove = false;
    public float offset = 2f;


    [Header("Fade-In Settings")]
    public Shader shader;

    [Range(0,1.0f)]
    public float maskValue;
    public Color maskColor = Color.black;
    public Texture2D maskTexture;
    public bool maskInvert;

    private Material m_Material;
    private bool m_maskInvert;

    Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }
    }

    public static CameraManager singleton;

    private void Awake()
    {
        singleton = this;
    }
    void Start()
    {
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        shader = Shader.Find("Hidden/ScreenTransitionImageEffect");

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (shader == null || !shader.isSupported)
            enabled = false;
    }
    void OnDisable()
    {
        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!enabled)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetColor("_MaskColor", maskColor);
        material.SetFloat("_MaskValue", maskValue);
        material.SetTexture("_MainTex", source);
        material.SetTexture("_MaskTex", maskTexture);

        if (material.IsKeywordEnabled("INVERT_MASK") != maskInvert)
        {
            if (maskInvert)
                material.EnableKeyword("INVERT_MASK");
            else
                material.DisableKeyword("INVERT_MASK");
        }

        Graphics.Blit(source, destination, material);
    }
    public void setBlack()
    {
        maskValue = 1.0f;
    }
    public IEnumerator fadeIn(float delta)
    {
        maskValue = 1.0f;
        while (maskValue>0f)
        {
            maskValue -= delta;
            yield return null;
        }
    }

    public void FocusOnPlayer()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y+ offset, transform.position.z);
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            transform.position += new Vector3(0, target.velocity * Time.deltaTime, 0);
        }
    }
}
