using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Texture2D[] textures;
    [SerializeField] private float pixelsPerUnit;
    [SerializeField] public float lerpSpeed;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 goalPos;
    public float currY, target;
    public bool isWater;
    private Sprite sprite;
    private SpriteRenderer sr;
    public Cell(bool isWater)
    {
        this.isWater = isWater;
    }
    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        if (isWater)
        {
            lerpSpeed = 0.8f;
            target = 1f;
            startPos = transform.localPosition;
            goalPos = transform.localPosition + new Vector3(0, -0.1f, 0);
        }
    }
    void Update()
    {
        if (isWater)
        {
            //Lerp();
        }
        SetZPos();
    }
    public void Lerp()
    {
        if (transform.localPosition.y == goalPos.y || transform.localPosition.y == startPos.y) //swap the start and end pos once the water tile reached end pos
        {
            target = target == 1 ? 0 : 1;
        }
        currY = Mathf.MoveTowards(currY, target, lerpSpeed * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(startPos, goalPos, currY);
        Debug.Log(currY);
        Debug.Log(Time.deltaTime);
    }   
    public void ChangeSprite(int s)
    {
        Texture2D t = textures[s];
        sr.sprite = Sprite.Create(t, new Rect(0.0f, 0.0f, t.width, t.height), new Vector2(transform.position.x, transform.position.y), pixelsPerUnit);
    }

    public void SetZPos()   //Ensure all sprites are rendered in the correct order relative to y position
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.y / 100);
    }

}
