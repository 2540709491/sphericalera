using UnityEngine;

public class ImageClick : MonoBehaviour
{
    void Update()
    {
        // 检查鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 检测射线是否击中了图片
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // 点击事件处理逻辑
                    OnClick();
                }
            }
        }
    }

    void OnClick()
    {
        Debug.Log("图片被点击了！");
        // 在这里添加你想要执行的点击事件逻辑
    }
}