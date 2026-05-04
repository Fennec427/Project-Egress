using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PaintSwitch3 : MonoBehaviour

{
    public bool canpaint = true;
    [SerializeField] GameObject centerPoint;
    [SerializeField] Sprite[] sprites;
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Time.timeScale = 0.5f;
            GetComponent<SpriteRenderer>().enabled = true;        
        }
        if (Keyboard.current.eKey.wasReleasedThisFrame)
        {
            Time.timeScale = 1f;
            GetComponent<SpriteRenderer>().enabled = false; 
        }
        if (Keyboard.current.eKey.isPressed)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
            Vector2 relativePos = centerPoint.transform.InverseTransformPoint(worldMousePos);

            if(relativePos.y <= 40)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
            else if(relativePos.x<=0)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
        }
    }
}
