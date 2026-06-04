using UnityEngine;
using UnityEngine.InputSystem;

public class PaintGun : MonoBehaviour
{
    GameObject parent;
    [SerializeField] Sprite[] sprites;
    SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent.gameObject;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Vector2 direction = worldMousePos - parent.transform.position;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, direction);
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, targetRotation, 10f * Time.deltaTime);

        print(parent.transform.eulerAngles.z);
        if(parent.transform.rotation.eulerAngles.z > 0 && parent.transform.rotation.eulerAngles.z < 180) sr.flipY = true;
        else sr.flipY = false;
    }
}
