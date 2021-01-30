using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HighlightCell : MonoBehaviour
{
    [SerializeField] private Material _highlightBadMaterial;
    [SerializeField] private Material _highlightGoodMaterial;

    private Renderer _renderer;
    private Material _originalMaterial;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.material;
    }

    public void HighlightGood()
    {
        _renderer.material = _highlightGoodMaterial;
    }

    public void HighlightBad()
    {
        _renderer.material = _highlightBadMaterial;
    }

    public void ResetHighlight()
    {
        _renderer.material = _originalMaterial;
    }
}
