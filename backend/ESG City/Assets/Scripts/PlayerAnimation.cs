using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    public string[] staticDirections = { "Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    public string[] runDirections = { "Walk N", "Walk NW", "Walk W", "Walk SW", "Walk S", "Walk SE", "Walk E", "Walk NE" };

    int lastDirection;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        string[] directionArray = null;

        if (direction.magnitude < 0.01)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = runDirections;

            lastDirection = DirectionToIndex(direction);
        }

        anim.Play(directionArray[lastDirection]);                       //plays an animation
    }

    private int DirectionToIndex(Vector2 direction)
    // helper function, takes in a Vector2 and returns direction as an index
    {   // 0 is N
        // 1 is NW
        // 2 is W
        // 3 is SW
        // 4 is S
        // 5 is SE
        // 6 is E
        // 7 is NE

        Vector2 norDir = direction.normalized;
        float step = 360 / 8;
        float angle = Vector2.SignedAngle(Vector2.up, norDir);          // returns the difference between Vector2.up (0 degrees) and norDir(x degrees)
        float offset = step / 2;
        angle = angle + offset;
        if (angle < 0)
        {
            angle += 360;
        }
        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);

    }
}