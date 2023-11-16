using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer rangeSpriteRen;

    private void Start()
    {
        // Reference to sprite renderer
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        // Range is a child of Hover so we get a child
        this.rangeSpriteRen = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FollowMouse();
    }

    /// <summary>
    /// Hover icon follow mouse pointer
    /// </summary>
    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            // Translate the mouse on screen position to a world position
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Reseting z to 0
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    /// <summary>
    /// Activate hover icon
    /// </summary>
    /// <param name="sprite"></param>
    public void Activate(Sprite sprite)
    {
        // Set the sprite
        this.spriteRenderer.sprite = sprite;
        // Enable sprite renderer
        spriteRenderer.enabled = true;
        rangeSpriteRen.enabled = true;
    }

    /// <summary>
    /// Deactivate hover icon
    /// </summary>
    public void Deactivate()
    {
        // Disable sprite renderer
        spriteRenderer.enabled = false;
        rangeSpriteRen.enabled = false;
        // Unclick the button
        GameManager.Instance.ClickedBtn = null;
    }
}
