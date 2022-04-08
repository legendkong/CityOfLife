using UnityEngine;

public class Patroller : MonoBehaviour
{
    private Animator anim;
    public Vector3 lastPos;
    public Transform[] moveSpots;
    public Transform[] moveSpotsB;
    public int speed;
    public bool forward;
    public int moveSpotsIndex;
    private float dist;
    public int pattern;

    // Start is called before the first frame update
    public void Start()
    {
        forward = true;
        lastPos = transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {   
        if (pattern == 1)
        {
            walk(moveSpots);
        }
        else
        {
            walk(moveSpotsB);
        }
    }

    void walk(Transform[] moveSpots)
    {
        Vector2 velocity = transform.position - lastPos;
        lastPos = transform.position;
        if (velocity.y > 0)
        {
            if (velocity.x > 0)       //is moving top right
            {
                //Debug.Log("Top Right");
                anim.Play("Walk NE");
            }
            else if (velocity.x < 0)      // is moving top left
            {
                //Debug.Log("Top Left");
                anim.Play("Walk NW");
            }
        }
        else if (velocity.y < 0)
        {
            if (velocity.x > 0)       //is moving bottom right
            {
                //Debug.Log("Bottom Right");
                anim.Play("Walk SE");
            }
            else if (velocity.x < 0)      // is moving bottom left
            {
                //Debug.Log("Bottom Left");
                anim.Play("Walk SW");
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[moveSpotsIndex].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[moveSpotsIndex].position) < 0.1f)
        {
            if (forward == true)
            {
                IncreaseIndex(moveSpots);
            }
            else
            {
                DecreaseIndex(moveSpots);
            }
        }
    }

    void IncreaseIndex(Transform[] moveSpots)
    {
        moveSpotsIndex++;
        if (moveSpotsIndex >= moveSpots.Length)
        {
            forward = false;
            moveSpotsIndex--;
        }
    }

    void DecreaseIndex(Transform[] moveSpots)
    {
        moveSpotsIndex--;
        if (moveSpotsIndex == 0)
        {
            forward = true;
        }
    }

}
