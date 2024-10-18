using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// �����̴� �׷��� ��Ƴ��� �ڷᱸ��
/// ���� ����Ʈ �ڷᱸ���� Ȯ���Ų ���´�
/// </summary>
/// <typeparam name="T"></typeparam>
public class ActionGroup<T> where T : Selectable
{

    /// <summary>
    /// �ܺο��� �ش� ��带 ����� �� ����!
    /// </summary>
    protected sealed class Node
    {

        private T val;
        private Node prev;
        private Node next;

        public Node() { }

        public Node(T _val)
        {

            val = _val;
        }

        public T Val 
        { 
            
            set { val = value; }
            get { return val; } 
        }

        public Node Prev 
        {

            set { prev = value; }
            get { return prev; }
        }

        public Node Next
        {

            set { next = value; }
            get { return next; }
        }

        public void Clear()
        {

            prev = null;
            next = null;
            val = default(T);
        }
    }

    protected Node head;                       // ����
    protected Node tail;                       // ��

    protected Stack<Node> pool;              // Ǯ��
    protected Dictionary<T, Node> dic;       // �ߺ� Ȯ�ο�

    protected int capacity;                 // ��� �뷮

    /// <summary>
    /// �ش� ���� �ִ� ����
    /// </summary>
    public ActionGroup(int _capacity)
    {

        if (_capacity < 0) capacity = 0;
        else capacity = _capacity;
        
        head = new();
        tail = new();

        head.Next = tail;
        tail.Prev = head;

        dic = new(capacity);
        pool = new(capacity);
    }

    public T First
    {

        get
        {

            Node chk = head.Next;
            if (IsLast(chk)) return default(T);
            else return chk.Val;
        }
    }

    /// <summary>
    /// ���� ���� ��� ����
    /// </summary>
    public int Count => dic.Count;

    // /// <summary>
    // /// ��� �߰� ���� Ȯ�� �޼���
    // /// </summary>
    // public bool CanAdd => dic.Count < capacity;

    public void Action()
    {

        Node node = head.Next;

        while (!IsLast(node))
        {

            if (node == null)
            {

#if UNITY_EDITOR

                Debug.LogError("ActionGroup�� Node ������ �̻��� �־�\n" +
                    "�ൿ�� ���������� �۵����� �ʽ��ϴ�.");
#endif

                return;
            }

            node.Val.Action();
            node = node.Next;
        }
    }

    public void GetCommand(Command _cmd, bool _add = false)
    {

        Node node = head.Next;

        while (!IsLast(node))
        {

            if (node == null)
            {

#if UNITY_EDITOR

                Debug.LogError("ActionGroup�� Node ������ �̻��� �־�\n" +
                    "�ൿ�� ���������� �۵����� �ʽ��ϴ�.");
#endif

                return;
            }

            node.Val.GetCommand(_cmd, _add);
            node = node.Next;
        }
    }

    /// <summary>
    /// �ش� ��� ���� ����
    /// </summary>
    public bool Contains(T _chk) => dic.ContainsKey(_chk);

    /// <summary>
    /// ó�� �� Ȯ�ο�
    /// </summary>
    protected bool IsFirst(Node _chk) => _chk == head;

    /// <summary>
    /// ó�� �� Ȯ�ο�
    /// </summary>
    protected bool IsLast(Node _chk) => _chk == tail;


    /// <summary>
    /// �ڿ� ���� �̾� ���̱�
    /// </summary>
    public void AddLast(T _add)
    {

        if (dic.ContainsKey(_add))
        {

#if UNITY_EDITOR
            Debug.Log("�ش� ������ �̹� ��忡 �Ҵ�Ǿ� �ֽ��ϴ�.");
#endif

            return;
        }

        Node add;
        if (pool.Count > 0) 
        { 
            
            add = pool.Pop();
            add.Val = _add;
        }
        else add = new(_add);

        dic[_add] = add;
        AddLast(add);
    }

    /// <summary>
    /// ���� �ش� �׷쿡�� ����
    /// </summary>
    public void Pop(T _pop)
    {

        if (!dic.ContainsKey(_pop))
        {

#if UNITY_EDITOR

            Debug.LogError("�ش� ������ �����ϴ�.");
#endif
            return;
        }

        Node pop = dic[_pop];
        Pop(pop);
    }

    protected void AddLast(Node _add)
    {

        if (IsFirst(_add) || IsLast(_add))
        {

#if UNITY_EDITOR

            Debug.LogError("First �Ǵ� Last�� ������ �� �����ϴ�!");
#endif
            return;
        }

        Node tempPrev = tail.Prev;

        tempPrev.Next = _add;

        _add.Prev = tempPrev;
        _add.Next = tail;

        tail.Prev = _add;
    }

    protected void Pop(Node _pop) 
    {
        
        if (IsFirst(_pop) || IsLast(_pop))
        {

#if UNITY_EDITOR

            Debug.LogError("First�� Last�� �� �� �����ϴ�!");
#endif

            return;
        }

        Node tempPrev = _pop.Prev;
        Node tempNext = _pop.Next;

        tempPrev.Next = tempNext;
        tempNext.Prev = tempPrev;

        // dic���� ���� O(1)
        // https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.remove?view=net-8.0
        dic.Remove(_pop.Val);

        _pop.Clear();
        if (pool.Count < capacity) pool.Push(_pop);
    }
}
