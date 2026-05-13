using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject Pauseui;
    private bool cont = false;
    
    public void Pause()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        Pauseui.SetActive(true);
        Time.timeScale = 0;
    }

        public void ContinueButton() 
    {
        cont = true;
    }
    public void Continue()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame || cont == true)
        Pauseui.SetActive(false);
        Time.timeScale = 1;
    }
}
