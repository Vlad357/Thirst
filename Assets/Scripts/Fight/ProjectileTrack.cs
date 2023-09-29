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
            // Вычисляем направление движения к цели
            //Vector3 direction = target.position - transform.position;

            // Вычисляем вектор перемещения с заданной скоростью
            //Vector3 movement = direction.normalized * Time.deltaTime;

            // Перемещаем объект
            transform.position = target.position+offset;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
