using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Sprite spriteOn = null;
    public Sprite spriteOff = null;
    public PolygonCollider2D spriteOnCollider = null;
    public PolygonCollider2D spriteOffCollider = null;

    private bool _isStateOn = true;

    [HideInInspector]
    public bool isStateOn   
    {
        get
        {
            return _isStateOn;
        }
        set
        {
            _isStateOn = value;
        }
    }

    private SpriteRenderer _spriteRender = null;

    void Awake()
    {
        _spriteRender = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_spriteRender != null)
        {
            _spriteRender.sprite = (_isStateOn ? spriteOn : spriteOff);
            if (spriteOnCollider != null)
                spriteOnCollider.enabled = _isStateOn;

            if (spriteOffCollider != null)
                spriteOffCollider.enabled = !_isStateOn;

        }
    }
}
