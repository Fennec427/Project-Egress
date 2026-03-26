using UnityEngine;

public class PaintIdentifier : MonoBehaviour
{
    public string paint;
    [SerializeField] private Sprite fallback;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        try
        {
            if (paint == null || paint == ""){
                paint = GetComponent<SpriteRenderer>().sprite.name[..(GetComponent<SpriteRenderer>().sprite.name.Length - 2)];
            }
        }
        catch
        {
            GetComponent<SpriteRenderer>().sprite = fallback;
            paint = fallback.name;
        }
        finally
        {
            print(paint);
        }
        
    }

    public string getPaintId()
    {
        return paint;
    }
}
