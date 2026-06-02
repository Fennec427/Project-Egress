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
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f));
        Vector2 mouseRelativePos = parent.transform.InverseTransformPoint(worldMousePos);
        
        //set parent rotation to point at mouse
        //Vector2 direction = (mouseRelativePos - (Vector2)parent.transform.position).normalized;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //parent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        parent.transform.LookAt(worldMousePos);
    }
}
