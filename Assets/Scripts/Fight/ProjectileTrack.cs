using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrack : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Update()
    {
        if (target != null)
        {
            // ��������� ����������� �������� � ����
            //Vector3 direction = target.position - transform.position;

            // ��������� ������ ����������� � �������� ���������
            //Vector3 movement = direction.normalized * Time.deltaTime;

            // ���������� ������
            transform.position = target.position+offset;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
