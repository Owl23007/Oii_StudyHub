using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAction : MonoBehaviour
{
        
        public Transform target; // 目标物体
        public Vector3 offset; // 相机与目标的偏移量

    // Start is called before the first frame update
    void Start()
    {
        // 计算初始偏移量
        offset = transform.position - target.position;
        //offset.y -= 3;
        //offset.x += 1;
    }

    // Update is called once per frame
    void Update()
    {
        float originalRotationX = transform.rotation.eulerAngles.x;
        // 更新摄像机的位置，使其始终在目标的后方
        transform.position = target.position + offset;
        // 更新摄像机的旋转，使其始终看向目标
        transform.LookAt(target);
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = originalRotationX;
        transform.rotation = Quaternion.Euler(rotation);
    }


}
