using UnityEngine;
using DG.Tweening;

public class WindowMover : MonoBehaviour
{
    public enum MoveType { Linear, Jumping } // 运动类型
    public MoveType moveType = MoveType.Jumping;

    public float moveSpeed = 100f; // 线性移动速度
    public float jumpInterval = 1.5f; // 跳跃时间
    public float jumpDistance = 200f; // 跳跃距离
    public float jumpHeight = 150f; // 抛物线高度

    private RectTransform windowRect;
    private RectTransform canvasRect;
    private Vector2 moveDirection;

    void Start()
    {
        windowRect = GetComponent<RectTransform>();
        canvasRect = transform.parent.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        moveDirection = Random.insideUnitCircle.normalized; // 随机方向

        if (moveType == MoveType.Linear)
        {
            MoveLinear();
        }
        else if (moveType == MoveType.Jumping)
        {
            MoveJumping();
        }
    }

    /// <summary>
    /// 线性移动
    /// </summary>
    private void MoveLinear()
    {
        float duration = 3f;
        Vector2 targetPosition = windowRect.anchoredPosition + moveDirection * moveSpeed * duration;
        targetPosition = KeepInsideScreen(targetPosition);

        windowRect.DOAnchorPos(targetPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                moveDirection = GetNewDirection();
                MoveLinear();
            });
    }

    /// <summary>
    /// 抛物线跳跃
     /// <summary>
    /// 抛物线跳跃
    /// </summary>
    private void MoveJumping()
    {
        Vector2 startPos = windowRect.anchoredPosition;
        Vector2 targetPos = startPos + moveDirection * jumpDistance;

        // 确保目标点在屏幕内
        targetPos = KeepInsideScreen(targetPos);

        // 计算抛物线控制点（最高点）
        Vector2 controlPoint = (startPos + targetPos) / 2 + new Vector2(0, jumpHeight);

        // 重新计算 controlPoint 确保它不会超出屏幕
        controlPoint = KeepInsideScreen(controlPoint);

        // 使用 DoTween 生成抛物线路径
        Vector3[] path = new Vector3[] { startPos, controlPoint, targetPos };

        windowRect.DOPath(path, jumpInterval, PathType.CatmullRom, PathMode.Sidescroller2D)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                moveDirection = GetNewDirection();
                MoveJumping();
            });
    }

    /// <summary>
    /// 保持窗口在屏幕内，并在撞到边缘时反弹
    /// </summary>
    private Vector2 KeepInsideScreen(Vector2 newPos)
    {
        Vector2 screenSize = canvasRect.sizeDelta;
        Vector2 windowSize = windowRect.sizeDelta;
        bool hitWall = false;

        if (newPos.x - windowSize.x / 2 < -screenSize.x / 2)
        {
            newPos.x = -screenSize.x / 2 + windowSize.x / 2;
            moveDirection.x *= -1;
            hitWall = true;
        }
        else if (newPos.x + windowSize.x / 2 > screenSize.x / 2)
        {
            newPos.x = screenSize.x / 2 - windowSize.x / 2;
            moveDirection.x *= -1;
            hitWall = true;
        }

        if (newPos.y - windowSize.y / 2 < -screenSize.y / 2)
        {
            newPos.y = -screenSize.y / 2 + windowSize.y / 2;
            moveDirection.y *= -1;
            hitWall = true;
        }
        else if (newPos.y + windowSize.y / 2 > screenSize.y / 2)
        {
            newPos.y = screenSize.y / 2 - windowSize.y / 2;
            moveDirection.y *= -1;
            hitWall = true;
        }

        if (hitWall)
        {
            moveDirection = GetNewDirection();
        }

        return newPos;
    }

    /// <summary>
    /// 计算新的随机方向（防止窗口停滞）
    /// </summary>
    private Vector2 GetNewDirection()
    {
        return Random.insideUnitCircle.normalized;
    }
}
