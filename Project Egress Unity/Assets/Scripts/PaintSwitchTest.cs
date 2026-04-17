using UnityEngine;
using UnityEngine.InputSystem;
public class PaintSwitchTest : MonoBehaviour

{
    public bool canpaint = true;
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.IsPressed())
        {
            Debug.Log("I AM THE PAINT SWITCH FUNCTION RAHHHHHHHHH");
        }
    }
}
