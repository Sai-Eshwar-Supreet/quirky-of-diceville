using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DiceVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private int _targetMaterialIndex = 0;
    private MaterialPropertyBlock _mpb;

    private MaterialPropertyBlock MPB
    {
        get
        {
            if( _mpb == null )
            {
                if(_meshRenderer  == null) _meshRenderer = GetComponent<MeshRenderer>();
                _mpb = new MaterialPropertyBlock();

                _meshRenderer.GetPropertyBlock(_mpb, _targetMaterialIndex);
            }

            return _mpb;
        }
    }

    public void SetColor(Color color)
    {
        MPB.SetColor("_BaseColor",  color);
        MPB.SetColor("_EmissionColor", color * 5);
        _meshRenderer.SetPropertyBlock(MPB, _targetMaterialIndex);
    }

    public void SetTexture(Texture2D tex2D)
    {
        MPB.SetTexture("_BaseMap",  tex2D);
        _meshRenderer.SetPropertyBlock(MPB, _targetMaterialIndex);
    }

    public void ResetOverrides()
    {
        MPB.Clear();
        _meshRenderer.SetPropertyBlock(MPB, _targetMaterialIndex);
    }
}
