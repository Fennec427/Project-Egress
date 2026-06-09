using UnityEngine;
using UnityEngine.InputSystem;

public class PaintGun : MonoBehaviour
{
    GameObject parent;
    [SerializeField] Sprite[] sprites;
    SpriteRenderer sr;
    private int paintType = 0;
    [SerializeField] private Paintball paintballObj;
    public bool switchingPaint = false;

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
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, targetRotation, 50f * Time.deltaTime);

        if(parent.transform.rotation.eulerAngles.z > 0 && parent.transform.rotation.eulerAngles.z < 180) sr.flipY = true;
        else sr.flipY = false;

        if (Mouse.current.leftButton.wasPressedThisFrame && !switchingPaint)
        {
            Paintball paintball = Instantiate(paintballObj);
            paintball.SpriteValue = paintType;
            paintball.Initialize(direction, 10f, transform.position);
            GameManager.activePaintballs.Add(paintball.gameObject);
        }
    }

    public void UpdatePaint(int newPaint)
    {
        if(newPaint > sprites.Length - 1)
        {
            Debug.LogError("Invalid paint number");
            return;
        }
        paintType = newPaint;
        sr.sprite = sprites[newPaint];
    }
}
