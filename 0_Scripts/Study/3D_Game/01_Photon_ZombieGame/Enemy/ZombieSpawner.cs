using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

// ���� ���� ������Ʈ�� �ֱ������� ����
public class ZombieSpawner : MonoBehaviourPun, IPunObservable
{

    public Zombie zombiePrefab;                             // ������ ���� ���� ������

    public ZombieData[] zombieDatas;                         // ����� ���� �¾� ������
    public Transform[] spawnPoints;                         // ���� AI�� ��ȯ�� ��ġ

    private List<Zombie> zombies = new List<Zombie>();      // ������ ���� ��� ����Ʈ

    private int zombieCount = 0;                            // ���� ���� ��
    private int wave;                                       // ���� ���̺�


    // �ֱ������� �ڵ� ����Ǵ� ����ȭ �޼ҵ�
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        // ���� ������Ʈ��� ���� �κ��� �����
        if (stream.IsWriting)
        {

            // ���� ���� ���� ��Ʈ��ũ�� ���� ������
            stream.SendNext(zombies.Count);
            // ���� ���̺긦 ��Ʈ��ũ�� ���� ������
            stream.SendNext(wave);
        }
        else
        {

            // ����Ʈ ������Ʈ��� �б� �κ��� �����
            // ���� ���� ���� ��Ʈ��ũ�� ���� �ޱ�
            zombieCount = (int)stream.ReceiveNext();
            // ���� ���̺긦 ��Ʈ��ũ�� ���� �ޱ�
            wave = (int)stream.ReceiveNext();
        }
    }

    private void Awake()
    {

        PhotonPeer.RegisterType(typeof(Color), 128, 
            ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);        // Color �����͸� �����ϱ� ���� ���
                                                                                            // ColorSerialization�� ���� ������ Ŭ����
    }

    private void Update()
    {
        
        // ȣ��Ʈ�� ���� ���� ������ �� ����
        // �ٸ� Ŭ���̾�Ʈ�� ȣ��Ʈ�� ������ ���� ����ȭ�� ���� �޾ƿ�
        if (PhotonNetwork.IsMasterClient)
        {

            // ���ӿ��� ������ ���� �������� ����
            if (GameManager.instance != null && GameManager.instance.isGameover)
            {

                return;
            }

            // ���� ��� ����ģ ��� ���� ���� ����
            if (zombies.Count <= 0)
            {

                SpawnWave();
            }
        }

        // UI ����
        UpdateUI();
    }

    // ���̺� ������ UI�� ǥ��
    private void UpdateUI()
    {

        if (PhotonNetwork.IsMasterClient)
        {

            // ȣ��Ʈ�� ���� ������ ���� ����Ʈ�� �̿��� ���� ���� �� ǥ��
            UIManager.instance.UpdateWaveText(wave, zombies.Count);
        }
        else
        {

            // Ŭ���̾�Ʈ�� ���� ����Ʈ�� ������ �� �����Ƿ�
            // ȣ��Ʈ�� ������ zombieCount�� �̿��� ���� �� ǥ��
            UIManager.instance.UpdateWaveText(wave, zombieCount);
        }
    }

    // ���� ���̺꿡 ���� ���� ����
    private void SpawnWave()
    {

        // ���̺� 1 ����
        wave++;

        // ���� ���̺� * 1.5 �� �ݿø��� ����ŭ ���� ����
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        // spawnCount ��ŭ ���� ����
        for (int i = 0; i < spawnCount; i++)
        {

            // ���� ���� ó�� ����
            CreateZombie();
        }
    }

    // ���� �����ϰ� ���� ������ ��� �Ҵ�
    private void CreateZombie()
    {

        // ����� ���� ������ �������� ���� 
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];

        // ������ ��ġ�� �������� ����
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ���� ���������κ��� ���� ����, ��Ʈ��ũ���� ��� Ŭ���̾�Ʈ�� ������
        GameObject createdZombie = PhotonNetwork.Instantiate(zombiePrefab.gameObject.name, spawnPoint.position, spawnPoint.rotation);

        // ������ ���� �¾��ϱ� ���� Zombie ������Ʈ�� ������
        Zombie zombie = createdZombie.GetComponent<Zombie>();

        // ������ ������ �ɷ�ġ ����
        zombie.photonView.RPC("Setup", RpcTarget.All, zombieData.health, zombieData.damage, 
            zombieData.speed, zombieData.skinColor);

        // ������ ���� ����Ʈ�� �߰�
        zombies.Add(zombie);

        // ������ onDeath �̺�Ʈ�� �͸� �޼ҵ� ���
        // ����� ���� ����Ʈ���� ����
        zombie.onDeath += () => zombies.Remove(zombie);

        // ����� ���� 10�� �ڿ� �ı�
        zombie.onDeath += () => StartCoroutine(DestroyAfter(zombie.gameObject, 10f));

        // ���� ��� �� ���� ���
        zombie.onDeath += () => GameManager.instance.AddScore(100);
    }

    // ������ Network.Destroy()�� ���� �ı��� �������� �����Ƿ� �����ı��� ���� ������
    IEnumerator DestroyAfter(GameObject target, float delay)
    {

        // delay��ŭ ����
        yield return new WaitForSeconds(delay);

        // target�� ���� �ı����� �ʾҴٸ�
        if (target != null)
        {

            // target�� ��� ��Ʈ��ũ�󿡼� �ı�
            PhotonNetwork.Destroy(target);
        }
    }
}
