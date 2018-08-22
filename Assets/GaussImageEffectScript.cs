using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GaussImageEffectScript : MonoBehaviour
{
    private Material material = null;

    [SerializeField, HideInInspector]
    private Shader shader = null;

#if UNITY_EDITOR
#if false
    private void calcGaussianParam(float dev, int tap)
    {
        var gs = "";
        var gx = "";
        var id2_2 = -1 / (dev * dev * 2.0f);
        float[] tmp = new float[tap + 1];
        var s = 0.0f;
        for (int i = 0; i < tap; i++)
        {
            var v = i - tap / 2;
            var g = Mathf.Exp(v * v * id2_2);
            tmp[i] = g;
            s += g;
        }
        for (int i = 0; i < tap; i += 2)
        {
            var g0 = tmp[i + 0] / s;
            var g1 = tmp[i + 1] / s;
            gs = gs + (g0 + g1).ToString() + " ";
            gx = gx + (i - 3 + (g1 / (g0 + g1))).ToString() + " ";
        }
        Debug.Log(gs);
        Debug.Log(gx);
    }
    private void Awake()
    {
        calcGaussianParam(0.5f, 5);
    }
#endif
#endif
    private void OnValidate()
    {
        shader = Shader.Find("Hidden/GaussImageEffectShader");
    }
    private void OnEnable()
    {
        material = new Material(shader);
    }
    private void OnDisable()
    {
        Destroy(material);
        material = null;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var filterModeOld = source.filterMode;
        source.filterMode = FilterMode.Bilinear;
        Graphics.Blit(source, destination, material);
        source.filterMode = filterModeOld;
    }
}
