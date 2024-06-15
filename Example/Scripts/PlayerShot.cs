using System.Collections;
using System.Collections.Generic;
using BH_Engine;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public BaseEmitter[] emitters;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();

        // 长按左键自动射击
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var emitter in emitters)
            {
                emitter.StartShoot();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            foreach (var emitter in emitters)
            {
                emitter.PauseShoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var emitter in emitters)
            {
                emitter.StopShoot();
            }
        }
    }

    // 2d视角，随鼠标方向转动
    private void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
