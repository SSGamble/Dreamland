using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机跟随
/// </summary>
public class CameraFollow : MonoBehaviour
{

    private Transform target; // 目标（Player）位置
    private Vector3 offset; // 目标位置和当前位置的偏移
    private Vector2 velocity;

    private void Update()
    {
        if (target == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            offset = target.position - transform.position;
        }
    }

    private void FixedUpdate()
    {
        // 同步位置
        if (target != null)
        {
            // public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = Mathf.Infinity, float deltaTime = Time.deltaTime);
            // 随着时间的推移，逐渐地将一个值改变为一个期望的目标。
            float posX = Mathf.SmoothDamp(transform.position.x, target.position.x - offset.x, ref velocity.x, 0.05f); // 摄像机的目标值
            float posY = Mathf.SmoothDamp(transform.position.y, target.position.y - offset.y, ref velocity.y, 0.05f); // 摄像机的目标值

            // 只有当目标位置比当前位置的 y 大时，才改变位置。防止摄像机的上下抖动，只保留上移动
            if (posY > transform.position.y)
            {
                transform.position = new Vector3(posX, posY, transform.position.z); // 改变摄像机位置
            }
        }
    }
}
