using UnityEngine;
using UnityEngine.InputSystem;

public class PaintGun : MonoBehaviour
{
    GameObject parent;
    [SerializeField] Sprite[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Vector2 direction = worldMousePos - parent.transform.position;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, direction);
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, targetRotation, 10f * Time.deltaTime);
    }
}
