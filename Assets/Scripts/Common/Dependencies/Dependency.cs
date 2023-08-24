using UnityEngine;

public abstract class Dependency : MonoBehaviour
{
    protected virtual void BindAll(MonoBehaviour mono) { }

    protected void FindAllObjectsToBind()
    {
        MonoBehaviour[] allMonoBehavioursInScene = FindObjectsOfType<MonoBehaviour>();

        for (int i = 0; i < allMonoBehavioursInScene.Length; i++)
        {
            BindAll(allMonoBehavioursInScene[i]);
        }
    }

    protected void Bind<T>(MonoBehaviour bindObject, MonoBehaviour target) where T : class
    {
        if (target is IDependency<T>) (target as IDependency<T>).Construct(bindObject as T);
    }
}
