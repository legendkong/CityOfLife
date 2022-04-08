using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Cloud : MonoBehaviour
{
    private float _speed;
    private float _endPosX;
    bool startMoving = false;
    private float _elapsed = 0;
    private float _elapsedTimeBeforeFading;
    private Transform t;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        t = GetComponent<Transform>();
        _speed = Random.Range(2f, 4f);
        _elapsedTimeBeforeFading = Random.Range(0.5f, 2f);
        _endPosX = 10f;
    }
    private void Start()
    {
        t.localScale = new Vector3(0.005f, 0.005f, 1);
        StartCoroutine(LerpSize());
    }   

    // Update is called once per frame
    private void Update()
    {
        if (startMoving)
        {
            _elapsed += Time.deltaTime;
            t.Translate(Vector3.right * (Time.deltaTime * _speed));
            if (t.position.x > _endPosX)
            {
                Destroy(gameObject);
            }
            if (_elapsed >= _elapsedTimeBeforeFading)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - 0.005f);
                if (t.localScale.y > 0.1f && sr.color.a < 0.5f)
                    t.localScale = new Vector3(t.localScale.x, t.localScale.y - 0.005f, 1);
            }
        }
    }
    IEnumerator LerpSize()
    {
        while (t.localScale.x <= 0.5f && t.localScale.x <= 0.5f)
        {
            t.localScale = new Vector3(t.localScale.x + 0.004f, t.localScale.x + 0.004f, 1);
            yield return new WaitForSeconds(0.001f);
        }
        startMoving = true;
        yield return null;
    }
}

