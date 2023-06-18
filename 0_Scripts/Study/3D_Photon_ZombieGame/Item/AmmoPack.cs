using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 탄알을 충전하는 아이템
public class AmmoPack : MonoBehaviour, IItem
{

    public int ammo = 30;           // 충전할 탄알 수

    public void Use(GameObject target)
    {

        // target에 탄알을 추가하는 처리
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        // Playershooter 컴포넌트가 있으며 총 오브젝트가 존재하면
        if (playerShooter != null && playerShooter.gun != null)
        {

            // 총의 남은 탄알 수를 ammo 만큼 더함
            playerShooter.gun.ammoRemain += ammo;
        }

        // 사용되었으므로 자신을 파괴
        Destroy(gameObject);
    }
}
