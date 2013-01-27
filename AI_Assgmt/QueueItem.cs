/// <summary>
/// Generic class of queue item, consist of key (int) and a generic object (T).
/// Used in BasePriorityQueue class.
/// </summary>
public class QueueItem<T>
{
    private T obj;
    private int key;

    public QueueItem(int _key, T _obj)
    {
        obj = _obj;
        key = _key;
    }

    public T GetObj()
    {
        return obj;
    }

    public int GetKey()
    {
        return key;
    }
}