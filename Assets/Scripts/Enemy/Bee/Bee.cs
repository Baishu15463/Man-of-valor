using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 泛型对象池，可复用任何组件类型
/// </summary>
/// <typeparam name="T">必须是Unity组件（Component）</typeparam>
public class ObjectPool<T> where T : Component
{
    // ========== 池配置 ==========
    private T prefab;                    // 要池化的预制体
    private Transform parent;            // 父物体（用于组织层级）
    private int defaultSize;             // 默认池大小

    // ========== 池存储 ==========
    private Queue<T> availableObjects = new Queue<T>();  // 可用对象队列
    private List<T> allObjects = new List<T>();          // 所有对象列表（用于监控）

    /// <summary>
    /// 对象池构造函数
    /// </summary>
    /// <param name="prefab">预制体模板</param>
    /// <param name="initialSize">初始池大小</param>
    /// <param name="parent">父物体（可选）</param>
    public ObjectPool(T prefab, int initialSize = 10, Transform parent = null)
    {
        this.prefab = prefab;
        this.defaultSize = initialSize;
        this.parent = parent;

        // 初始化时创建指定数量的对象
        InitializePool();
    }

    /// <summary>
    /// 初始化对象池，创建初始对象
    /// </summary>
    private void InitializePool()
    {
        for (int i = 0; i < defaultSize; i++)
        {
            CreateNewObject();
        }
        Debug.Log($"对象池初始化完成，类型: {typeof(T).Name}, 大小: {defaultSize}");
    }

    /// <summary>
    /// 创建新对象并加入池中
    /// </summary>
    /// <returns>新创建的对象</returns>
    private T CreateNewObject()
    {
        // 1. 实例化预制体
        T newObj = Object.Instantiate(prefab, parent);

        // 2. 设置对象名称（便于调试）
        newObj.name = $"{prefab.name}_Pooled_{allObjects.Count}";

        // 3. 初始状态设为禁用
        newObj.gameObject.SetActive(false);

        // 4. 加入管理列表
        allObjects.Add(newObj);
        availableObjects.Enqueue(newObj);

        return newObj;
    }

    /// <summary>
    /// 从池中获取一个对象
    /// </summary>
    /// <returns>可用的对象</returns>
    public T Get()
    {
        // 情况1：池中有可用对象
        if (availableObjects.Count > 0)
        {
            T obj = availableObjects.Dequeue();  // 从队列取出
            obj.gameObject.SetActive(true);      // 激活对象
            Debug.Log($"从池中获取对象: {obj.name}");
            return obj;
        }

        // 情况2：池已空，动态扩容
        Debug.LogWarning($"对象池已空，动态创建新对象: {typeof(T).Name}");
        T newObj = CreateNewObject();
        newObj.gameObject.SetActive(true);
        return newObj;
    }

    /// <summary>
    /// 归还对象到池中
    /// </summary>
    /// <param name="obj">要归还的对象</param>
    public void Return(T obj)
    {
        // 1. 安全校验
        if (obj == null)
        {
            Debug.LogError("尝试归还空对象到对象池");
            return;
        }

        // 2. 禁用对象
        obj.gameObject.SetActive(false);

        // 3. 重置对象位置（可选）
        obj.transform.position = Vector3.zero;

        // 4. 放回可用队列
        availableObjects.Enqueue(obj);

        Debug.Log($"对象归还到池中: {obj.name}");
    }

    /// <summary>
    /// 预加载对象（预热池子）
    /// </summary>
    /// <param name="count">预加载数量</param>
    public void Preload(int count)
    {
        int needToCreate = count - allObjects.Count;
        if (needToCreate > 0)
        {
            Debug.Log($"预加载 {needToCreate} 个对象");
            for (int i = 0; i < needToCreate; i++)
            {
                CreateNewObject();
            }
        }
    }

    /// <summary>
    /// 清空对象池（场景切换时调用）
    /// </summary>
    public void Clear()
    {
        foreach (T obj in allObjects)
        {
            if (obj != null)
                Object.Destroy(obj.gameObject);
        }

        availableObjects.Clear();
        allObjects.Clear();
        Debug.Log($"对象池已清空: {typeof(T).Name}");
    }

    // ========== 属性访问器 ==========

    /// <summary>
    /// 当前可用对象数量
    /// </summary>
    public int AvailableCount => availableObjects.Count;

    /// <summary>
    /// 总对象数量（包括已使用的）
    /// </summary>
    public int TotalCount => allObjects.Count;

    /// <summary>
    /// 正在使用的对象数量
    /// </summary>
    public int InUseCount => TotalCount - AvailableCount;
}