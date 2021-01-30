using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HighlightCell : MonoBehaviour
{
    [SerializeField] private Material _highlightMaterial;

    private Renderer _renderer;
    private Material _originalMaterial;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.material;
    }

    public void Highlight()
    {
        _renderer.material = _highlightMaterial;
    }

    public void ResetHighlight()
    {
        _renderer.material = _originalMaterial;
    }
}
