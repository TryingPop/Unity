using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ݴ��� �� �ִ� ��󿡰� ����� �������̽�
/// </summary>
public interface IDamageable
{

    /// <summary>
    /// �ǰ� �޼ҵ�
    /// </summary>
    /// <param name="damage">������ ũ��</param>
    /// <param name="hitPoint">���ݴ��� ��ġ</param>
    /// <param name="hitNormal">���ݴ��� ǥ���� ����</param>
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
