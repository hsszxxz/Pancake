using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DetectPlayerInput : MonoSingleton<DetectPlayerInput>
{
    [HideInInspector]
    public List<Vector2> singleClick = new List<Vector2>();
    [HideInInspector]
    public List<Vector2> touch  = new List<Vector2>();
    [HideInInspector]
    public List<Vector2> flick = new List<Vector2>();
    [HideInInspector]
    public List<Vector2>lastTouch = new List<Vector2>();
    private void Update()
    {
        lastTouch.Clear();
        foreach (Vector2 pos in touch)
        {
            lastTouch.Add(pos);
        }
        singleClick.Clear();
        touch.Clear();
        flick.Clear();
       for (int i =0;i <Input.touchCount;i++)
        {
            Touch finger = Input.GetTouch(i);
            Vector2 pos = Camera.main.ScreenToWorldPoint(finger.position);
            if (finger.phase == TouchPhase.Began)
            {
                singleClick.Add(pos);
                Debug.Log("began");
            }
            if (finger.phase == TouchPhase.Moved)
            {
                touch.Add(pos);
                if (lastTouch.Count > i)
                Debug.Log(pos - lastTouch[i]);
            }
        }
    }
}
