using UnityEngine;
using UnityEngine.InputSystem;
public class PaintSwitchTest : MonoBehaviour

{
    public bool canpaint = true;
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Time.timeScale = 0.5f;
        }
        else if (Keyboard.current.eKey.wasReleasedThisFrame)
        {
            Time.timeScale = 1f;
        }
    }
}
