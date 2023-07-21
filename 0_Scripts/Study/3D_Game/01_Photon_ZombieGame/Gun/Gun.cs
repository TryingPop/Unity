using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPun, IPunObservable
{

    // ���� ���¸� ǥ���ϴ� �� ����� Ÿ���� ����
    public enum State
    {

        Ready,      // �߻� �غ��
        Empty,      // źâ�� ��
        Reloading,  // ������ ��
    }

    public State state { get; private set; }    // ���� ���� ����

    public Transform fireTransform;             // ź���� �߻�� ��ġ

    public ParticleSystem muzzleFlashEffect;    // �ѱ� ȭ�� ȿ��
    public ParticleSystem shellEjectEffect;     // ź�� ���� ȿ��

    private LineRenderer bulletLineRenderer;    // ź�� ������ �׸��� ���� ������

    private AudioSource gunAudioPlayer;         // �� �Ҹ� �����

    public GunData gunData;                     // ���� ���� ������

    private float fireDistance = 50f;           // �����Ÿ�

    public int ammoRemain = 100;                // ���� ��ü ź��
    public int magAmmo;                         // ���� źâ�� ���� �ִ� ź��

    private float lastFireTime;                 // ���� ���������� �߻��� ����

    // �ֱ������� �ڵ� ����Ǵ� ����ȭ �޼ҵ�
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        // ���� ������Ʈ��� ���� �κ��� �����
        if (stream.IsWriting)
        {

            // ���� ź�� ���� ��Ʈ��ũ�� ���� ������
            stream.SendNext(ammoRemain);
            // źâ�� ź�� ���� ��Ʈ��ũ�� ���� ������
            stream.SendNext(magAmmo);
            // ���� ���� ���¸� ��Ʈ��ũ�� ���� ������
            stream.SendNext(state);
        }
        else
        {

            // ����� ������Ʈ��� �б� �κ��� �����
            // ���� ź�� ���� ��Ʈ��ũ�� ���� �ޱ�
            ammoRemain = (int)stream.ReceiveNext();
            // źâ�� ź�� ���� ��Ʈ��ũ�� ���� �ޱ�
            magAmmo = (int)stream.ReceiveNext();
            // ���� ���� ���¸� ��Ʈ��ũ�� ���� �ޱ�
            state = (State)stream.ReceiveNext();
        }
    }

    // ���� ź���� �߰��ϴ� �޼ҵ�
    [PunRPC]
    public void AddAmmo(int ammo)
    {

        ammoRemain += ammo;
    }

    private void Awake()
    {

        // ����� ������Ʈ�� ���� ��������
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        // ����� ���� �� ���� ����
        bulletLineRenderer.positionCount = 2;   // �ѱ��� ��ġ�� ź���� ���� ��ġ�� �Ҵ�
        // ���� �������� ��Ȱ��ȭ
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {

        // �� ���� �ʱ�ȭ
        ammoRemain = gunData.startAmmoRemain;

        // ���� źâ�� ���� ä���
        magAmmo = gunData.magCapacity;

        // ���� ���� ���¸� ���� �� �غ� �� ���·� ����
        state = State.Ready;
        // ���������� ���� �� ������ �ʱ�ȭ
        lastFireTime = 0;
    }

    // �߻� �õ�
    public void Fire()
    {

        // ���� ���°� �߻� ������ ����
        // && ������ �ѹ߻� �������� gunData.timeBetFire �̻��� �ð��� ����
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {

            // ������ �� �߻� ���� ����
            lastFireTime = Time.time;
            // ���� �߻� ó�� ����
            Shot();
        }
    }

    // ���� �߻� ó��
    private void Shot()
    {

        // ���� �߻� ó���� ȣ��Ʈ�� �븮
        photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient);

        // ���� ź�� ���� -1
        magAmmo--;
        if (magAmmo <= 0)
        {

            // źâ�� ���� ź���� ���ٸ� ���� ���� ���¸� Empty�� ����
            state = State.Empty;
        }
    }

    // ȣ��Ʈ���� ����Ǵ� ���� �߻� ó��
    [PunRPC]
    private void ShotProcessOnServer()
    {

        // ����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� �����̳�
        RaycastHit hit;
        // ź���� ���� ���� ������ ����
        Vector3 hitPosition = Vector3.zero;

        // ����ĳ��Ʈ (���� ����, ����, �浹 ���� �����̳�, �����Ÿ�)
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {

            // ���̰� � ��ü�� �浹�� ���

            // �浹�� �������κ��� IDamageable ������Ʈ �������� �õ�
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            // �������κ��� IDamageable ������Ʈ�� �������� �� �����ߴٸ�
            if (target != null)
            {

                // ������ OnDamage �Լ��� ������� ���濡 ������ �ֱ�
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // ���̰� �浹�� ��ġ ����
            hitPosition = hit.point;
        }
        else
        {

            // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�
            // ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        // �߻� ����Ʈ ��� ����Ʈ ����� ��� Ŭ���̾�Ʈ���� ����
        photonView.RPC("ShotEffectProcessOnClients", RpcTarget.All, hitPosition);
    }

    // ����Ʈ ��� �ڷ�ƾ�� �����ϴ� �޼ҵ�
    [PunRPC]
    private void ShotEffectProcessOnClients(Vector3 hitPosition)
    {

        // �߻� ����Ʈ ��� ����
        StartCoroutine(ShotEffect(hitPosition));
    }

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� ź�� ������ �׸�
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {

        // �ѱ� ȭ�� ȿ�� ���
        muzzleFlashEffect.Play();
        // ź�� ���� ȿ�� ���
        shellEjectEffect.Play();

        // �Ѱ� �Ҹ� ���
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        // ���� �������� �ѱ��� ��ġ
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        // ���� ������ �Է����� ���� �浹 ��ġ
        bulletLineRenderer.SetPosition(1, hitPosition);
        // ���� �������� Ȱ��ȭ�Ͽ� ź�� ������ �׸�
        bulletLineRenderer.enabled = true;

        // 0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� ź�� ������ ����
        bulletLineRenderer.enabled = false;
    }

    // ������ �õ�
    public bool Reload()
    {

        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {

            // �̹� ������ ���̰ų� ���� ź���� ���ų�\
            // źâ�� ź���� �̹� ������ ��� �������� �� ����
            return false;
        }

        // ������ ó�� ����
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // ���� ������ ó���� ����
    private IEnumerator ReloadRoutine()
    {

        // ���� ���¸� ������ �� ���·� ��ȯ
        state = State.Reloading;

        // ������ �Ҹ� ���
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);

        // ������ �ҿ� �ð���ŭ ó�� ����
        yield return new WaitForSeconds(gunData.reloadTime);

        // źâ�� ä�� ź�� ���
        int ammoToFill = gunData.magCapacity - magAmmo;

        // źâ�� ä���� �� ź���� ���� ź�˺��� ���ٸ�
        // ä���� �� ź�� ���� ���� ź�� ���� ���� ����
        if (ammoRemain < ammoToFill)
        {

            ammoToFill = ammoRemain;
        }

        // źâ�� ä��
        magAmmo += ammoToFill;
        // ���� ź�˿��� źâ�� ä�ŭ ź���� ��
        ammoRemain -= ammoToFill;

        // ���� ���� ���¸� �߻� �غ�� ���·� ����
        state = State.Ready;
    }
}
