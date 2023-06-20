using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// �־��� Gun ������Ʈ�� ��ų� ����
// �˸��� �ִϸ��̼��� ����ϰ� IK�� ����� ĳ���� ����� �ѿ� ��ġ�ϵ��� ����
public class PlayerShooter : MonoBehaviourPun
{

    public Gun gun;                     // ����� ��
    public Transform gunPivot;          // �� ��ġ�� ������
    public Transform leftHandMount;     // ���� ���� ������, �޼��� ��ġ�� ����
    public Transform rightHandMount;    // ���� ������ ������, �������� ��ġ�� ����

    private PlayerInput playerInput;    // �÷��̾��� �Է�
    private Animator playerAnimator;    // �ִϸ����� ������Ʈ

    private void Start()
    {

        // ����� ������Ʈ ��������
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {

        // ���Ͱ� Ȱ��ȭ�� �� �ѵ� �Բ� Ȱ��ȭ
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {

        // ���Ͱ� ��Ȱ��ȭ�� �� �ѵ� �Բ� ��Ȱ��ȭ
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        
        // ���� �÷��̾ ���� ���� ���, ź�� UI ���� ����
        if (!photonView.IsMine)
        {

            return;
        }

        // �Է��� �����ϰ� ���� �߻��ϰų� ������
        if (playerInput.fire)
        {

            // �߻� �Է� ���� �� �� �߻�
            gun.Fire();
        }
        else if (playerInput.reload)
        {

            // ������ �Է� ���� �� ������
            if (gun.Reload())
            {

                // ������ ���� �ÿ��� ������ �ִϸ��̼� ���
                playerAnimator.SetTrigger("Reload");
            }
        }

        // ���� ź�� UI ����
        UpdateUI();
    }

    // ź�� UI ����
    private void UpdateUI()
    {

        if (gun != null && UIManager.instance != null)
        {

            // UI �Ŵ����� ź�� �ؽ�Ʈ�� źâ�� ź�˰� ���� ��ü ź�� ǥ��
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    // �ִϸ������� IK ����
    private void OnAnimatorIK(int layerIndex)
    {

        // ���� ������ gunPivot�� 3D ���� ������ �Ȳ�ġ ��ġ�� �̵�
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� ���� ���� �����̿� ����
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
        // 1.0f�� ����ġ ����ġ w�� 0 ~ 1 ���� ���� �� ������ 
        // �� ��ġ * (1 - w) + ��ǥ ��ġ * w �� �̵� ��ġ (���� ��Į�� ��)

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        // IK�� ����Ͽ� �������� ��ġ�� ȸ���� ���� �����̿� ����
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
