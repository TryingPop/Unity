using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ƽ ���ظ� ���� ���� ��ũ��Ʈ
// ������Ƽ�� ������ �ƴ� Ư���� ������ �޼ҵ��̴�
// ������Ƽ�� ���ͱ���� �ϰ� ����� �� �����Ƿ� �ڵ��� �������� �����ش�
// �ѹ� ���͸��� ��ġ�Ƿ� �׳� ���� ��뺸�ٴ� ������ �� ���̰� �ſ� �̹��ϴ�
public class VolumeInfo
{

    // �����ϰ� �پ��� ������ ����ϰ� ����Ѵٰ� ����
    // �����δ� �ٸ���
    public float megaBytes
    {

        get { return m_bytes * 0.000001f; }

        set
        {

            if (value <= 0)
            {

                m_bytes = 0;
            }
            else
            {

                m_bytes = value * 1000000f;
            }
        }
    }

    public float kiloBytes
    {

        get { return m_bytes * 0.001f; }

        set
        {

            if (value <= 0)
            {

                m_bytes = 0;
            }
            else
            {

                m_bytes = value * 1000f;
            }
        }
    }

    public float bytes
    {

        get { return m_bytes; }

        set
        {

            if (value <= 0)
            {

                m_bytes = 0;
            }
            else
            {

                m_bytes = value;
            }
        }
    }

    private float m_bytes = 0;              // ����Ʈ ������ �뷮 ���

    // public float bytes { get; private set; }
    // �� ���� �ڵ�� ����
    /*
    
    private float m_bytes;
    public float bytes
    {

        get { return m_bytes; }

        private set { m_bytes = value; }
    }
    */

    // ������Ƽ�� �����Կ� �־ �Ʒ��� ���� �ڱ��ڽ��� �����ϰ� �����ؼ��� �ȵȴ�
    // �����Ϸ����� �Ʒ� �ڵ尡 ������ ���ٰ� ���� ���� �����ϳ�
    // ������ ���� ��Ű�� ���� �����÷ο츦 �߻���Ű�� �Ǽ��ڵ尡 �ȴ�
    /*
    public float Bytes
    {

        get { return Bytes; }
    }
    */
}
