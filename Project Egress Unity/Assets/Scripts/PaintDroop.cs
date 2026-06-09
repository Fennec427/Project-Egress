using System;
using Unity.Mathematics;
using UnityEngine;

public class PaintDroop : MonoBehaviour
{
    [SerializeField] private Sprite[] redPaint;
    [SerializeField] private Sprite[] orangePaint;
    [SerializeField] private Sprite[] greenPaint;
    private object[][] paintIndex = new object[3][];
    private int _currentPaint = 0;
    public int CurrentPaint
    {
        get {return _currentPaint;}
        set
        {
            if(value >= 0 && value < paintIndex.Length)
                _currentPaint = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value), $"Sprite value must be at least 0 and less than {paintIndex.Length}");
        }
    }

    private SpriteRenderer sr;
    private BoxCollider2D col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Initialize(int paintType, Vector3 rotation, Vector3 goTo)
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        sr.enabled = false;
        col.enabled = false;
        
        paintIndex[0] = new object[] {redPaint};
        paintIndex[1] = new object[] {orangePaint};
        paintIndex[2] = new object[] {greenPaint};

        gameObject.transform.rotation = Quaternion.Euler(rotation);
        gameObject.transform.position = goTo;
        CurrentPaint = paintType;

        Sprite[] paintColors = (Sprite[])paintIndex[_currentPaint][0];
        sr.sprite = paintColors[UnityEngine.Random.Range(minInclusive: 0, maxExclusive: paintColors.Length)];
        sr.enabled = true;
        col.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
