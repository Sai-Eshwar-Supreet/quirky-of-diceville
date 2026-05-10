using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DoorVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _lockedColor = Color.grey;
    [SerializeField] private Color _unlockedColor = Color.white;

    private MaterialPropertyBlock _mpb;

    private MaterialPropertyBlock MPB
    {
        get
        {
            if( _mpb == null)
            {
                _mpb = new();
                if( _meshRenderer == null ) _meshRenderer = GetComponent<MeshRenderer>();
                _meshRenderer.GetPropertyBlock(_mpb);
            }
            return _mpb;
        }
    }

    public void SetLockState(bool unlocked)
    {
        var targetColor = unlocked ? _unlockedColor : _lockedColor;

        MPB.SetColor("_BaseColor",  targetColor);
        _meshRenderer.SetPropertyBlock(MPB);
    }
}
