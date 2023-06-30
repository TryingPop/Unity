using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideMissile : MonoBehaviour
{

    private Rigidbody m_rigid = null;
    private Transform m_tfTarget = null;

    [SerializeField] private float m_speed = 0f;
    private float m_currentSpeed = 0f;
    [SerializeField] LayerMask m_layerMask = 0;
    [SerializeField] private ParticleSystem m_psEffect = null;

    private void SearchEnemy()
    {

        Collider[] t_cols = Physics.OverlapSphere(
            transform.position, 100f, m_layerMask);     // 중심, 반지름, 찾을 레이어
                                                        // x, y, z를 우선순위로 큰값을 먼저 담는다

        if (t_cols.Length > 0)
        {

            m_tfTarget = t_cols[Random.Range(0, t_cols.Length)].transform;
        }
    }

    IEnumerator LaunchDelay()
    {

        yield return new WaitUntil(() => m_rigid.velocity.y < 0f);  // 람다식 조건을 만족할 때까지 대기
        yield return new WaitForSeconds(0.1f);

        SearchEnemy();
        m_psEffect.Play();

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void Start()
    {
        
        m_rigid = GetComponent<Rigidbody>();
        StartCoroutine(LaunchDelay());
    }

    private void Update()
    {
        
        if (m_tfTarget != null)
        {

            if (m_currentSpeed <= m_speed)
            {

                m_currentSpeed += m_speed * Time.deltaTime;
            }

            transform.position += transform.up * m_currentSpeed * Time.deltaTime;

            Vector3 t_dir = (m_tfTarget.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.CompareTag("Enemy"))
        {

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
