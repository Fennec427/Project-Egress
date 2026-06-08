using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PaintSwitch2 : MonoBehaviour

{
    public bool canpaint = true;
    [SerializeField] GameObject centerPoint;
    [SerializeField] Sprite[] sprites;

    private int paintType = 0;
    public int PaintType => paintType;
    private PaintGun paintGun;

    void Start()
    {
        PaintGun[] components = transform.parent.transform.parent.GetComponentsInChildren<PaintGun>();
        if(components.Length != 0) paintGun = components[0];
    }

    void Update()
    {
        if (Time.timeScale != 0f)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
            {
                Time.timeScale = 0.5f;
                GetComponent<SpriteRenderer>().enabled = true;        
            }
            if (Keyboard.current.eKey.wasReleasedThisFrame || Mouse.current.rightButton.wasReleasedThisFrame)
            {
                Time.timeScale = 1f;
                GetComponent<SpriteRenderer>().enabled = false;
                if(paintGun != null) paintGun.UpdatePaint(paintType);
            }
        }
        if (Keyboard.current.eKey.isPressed || Mouse.current.rightButton.isPressed)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
            Vector2 relativePos = centerPoint.transform.InverseTransformPoint(worldMousePos);

            if(relativePos.x <= 0)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                paintType = 0;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[2];
                paintType = 1;
            }
        }
    }
}
