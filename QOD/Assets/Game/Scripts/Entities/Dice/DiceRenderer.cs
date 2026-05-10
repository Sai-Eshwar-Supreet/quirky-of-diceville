using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DiceVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _mpb;

    private MaterialPropertyBlock MPB
    {
        get
        {
            if( _mpb == null )
            {
                if(_meshRenderer  == null) _meshRenderer = GetComponent<MeshRenderer>();
                _mpb = new MaterialPropertyBlock();

                _meshRenderer.GetPropertyBlock(_mpb);
            }

            return _mpb;
        }
    }

    public void SetColor(Color color)
    {
        MPB.SetColor("_BaseColor",  color);
        _meshRenderer.SetPropertyBlock(MPB);
    }

    public void SetTexture(Texture2D tex2D)
    {
        MPB.SetTexture("_BaseMap",  tex2D);
        _meshRenderer.SetPropertyBlock(MPB);
    }

    public void ResetOverrides()
    {
        MPB.Clear();
        _meshRenderer.SetPropertyBlock(MPB);
    }
}
