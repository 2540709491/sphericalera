using UnityEngine;

public class ImageClick : MonoBehaviour
{
    void Update()
    {
        // ������������
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ��������Ƿ������ͼƬ
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // ����¼������߼�
                    OnClick();
                }
            }
        }
    }

    void OnClick()
    {
        Debug.Log("ͼƬ������ˣ�");
        // �������������Ҫִ�еĵ���¼��߼�
    }
}