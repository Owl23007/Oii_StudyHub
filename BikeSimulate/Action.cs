using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Action : MonoBehaviour
{

    public BicycleState Simulate(BicycleState currentState, ControlStrategy control)
    {

        
        float newLeanAngle;
        if (currentState.LeanAngle > 0)
        {
            // 如果当前是右倾，那么 LeanAngle 应该增加
            newLeanAngle = currentState.LeanAngle + 0.5f * (1 - currentState.Speed / maxSpeed);
        }
        else if (currentState.LeanAngle < 0)
        {
            // 如果当前是左倾，那么 LeanAngle 应该减少
            newLeanAngle = currentState.LeanAngle - 0.5f * (1 - currentState.Speed / maxSpeed);
        }
        else
        {
            float randomFactor = (Random.value - 0.5f) * 2; // 生成一个在 -1 到 1 之间的随机数
            // 如果当前是中间位置，那么 LeanAngle 应该根据随机数增加或减少
            newLeanAngle = currentState.LeanAngle + randomFactor * (1 - currentState.Speed / maxSpeed);
        }

        var nextState = new BicycleState
        {
            Position = currentState.Position +
            currentState.Speed * new Vector3
            (Mathf.Cos(currentState.Direction * Mathf.Deg2Rad), 0, -Mathf.Sin(currentState.Direction * Mathf.Deg2Rad)),
            TargetPosition = currentState.TargetPosition,
            Speed = currentState.Speed * speedDecay, // 假设速度保持不变
            Direction = currentState.Direction,
            LeanAngle = newLeanAngle// 添加随机噪声
        };
        return nextState;
    }


    public class BicycleState
    {
        public Vector3 Position { get; set; } // 当前位置
        public Vector3 TargetPosition { get; set; } // 目标位置
        public float Speed { get; set; } // 自行车速度
        public float Direction { get; set; } // 运行方向
        public float LeanAngle { get; set; } // 侧偏角度
    }


    public class ControlStrategy
    {
        public float AdjustDirection { get; set; } // 调整方向
    }

    private BicycleState currentState;
    private ControlStrategy control;
    private float inputInterval = 0.2f; // 输入频次限制，单位为秒
    private float lastInputTime; // 上次输入的时间
    private float lastPositionX = 0; // 用于存储物体上一次的X坐标
    public float score = 0f; // 用于存储得分

    public float maxSpeed = 10f;//speedmax
    public float speedDecay = 0.99f; // 速度衰减因子
    public float turnSpeed = 40f;
    public float leanSpeed = 0.5f;
    public float acceleration = 3f;
    public bool isTrue = false;

    IEnumerator StartAfterDelay()
    {
        // 等待5秒
        yield return new WaitForSeconds(5);
        enabled = true;
        // 在这里添加你想在等待5秒后执行的代码
    }

    void Start()
    {

        // 在这里，你可以初始化自行车的状态和控制策略。
        currentState = new BicycleState
        {

            Position = Vector3.zero,
            TargetPosition = new Vector3(270, 0, 0),
            Speed = 1f,
            Direction = 0.0f,
            LeanAngle = 0.0f
        };
        StartCoroutine(StartAfterDelay());
        enabled = false;
        control = new ControlStrategy
        {
            AdjustDirection = 0.1f
        };

    }

    public float GetScore()
    {
        // 返回得分
        return score;
    }



    void Update()
    {
        score += Time.deltaTime;
        if (transform.position.x > lastPositionX)
        {
            // 如果物体在X轴正方向上移动，那么增加得分
            score += transform.position.x - lastPositionX;
        }

        // 更新上一次的X坐标
        lastPositionX = transform.position.x;

        // 检查是否已经过了足够的时间
        if (Time.time - lastInputTime < inputInterval)
        {
            return;
        }


        // 检查按键并更新自行车的状态或控制策略
        if (Input.GetKey(KeyCode.W))
        {
            // W键加速，添加一个加速度变量
            currentState.Speed = Mathf.Min(currentState.Speed + acceleration * Time.deltaTime, maxSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            // A键左转，添加一个转向角速度变量，并将转向角度与自行车的速度关联起来
            currentState.Direction = (currentState.Direction - turnSpeed * Time.deltaTime + 360) % 360;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // D键右转，添加一个转向角速度变量，并将转向角度与自行车的速度关联起来
            currentState.Direction = (currentState.Direction + turnSpeed * Time.deltaTime + 360) % 360;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            // Q键左倾，添加一个倾斜角速度变量，并将倾斜角度与自行车的转向速度关联起来
            currentState.LeanAngle += leanSpeed * Time.deltaTime * Mathf.Abs(turnSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            // E键右倾，添加一个倾斜角速度变量，并将倾斜角度与自行车的转向速度关联起来
            currentState.LeanAngle -= leanSpeed * Time.deltaTime * Mathf.Abs(turnSpeed);
        }



        // 在这里，你可以添加代码来实时画出场地和自行车状态。
        // 例如，你可以更新自行车的位置和旋转：
        // 更新状态
        currentState = Simulate(currentState, control);

        // 更新自行车的位置和旋转
        transform.position = currentState.Position;
        Vector3 eulerAngle = new Vector3(currentState.LeanAngle, currentState.Direction, 0);
        transform.rotation = Quaternion.Euler(eulerAngle);

        // 如果仿真已经结束，我们可以停止更新。
        if (IsFinished(currentState))
        {
            isTrue = true;
            enabled = false;
        }
    }

    bool IsFinished(BicycleState state)
    {
        // 在这里，你需要检查自行车是否已经到达目标位置，或者侧偏角度是否已经超过45度。
        // 这是一个示例实现，你需要根据实际情况进行修改：
        return Mathf.Abs(state.LeanAngle) > 45;
    }
   

}