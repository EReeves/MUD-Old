using System.Collections.Generic;

namespace Client
{
    class ObjectPool<T> where T : IPoolable, new()
    {
        private List<T> objects = new List<T>();

        public ObjectPool(int initialSpawn)
        {
            //Create an initial amount.
            for(int i=0;i<initialSpawn;i++)
            {
                T obj = new T();
                objects.Add(obj);
            }
        }

        public T GetObject()
        {
            foreach (T obj in objects)
            {
                if (obj.Free)
                {
                    obj.UnPool();
                    return obj;
                }
            }
            //No free objects, spawn a new one.
            T newObj = new T();
            newObj.UnPool();
            objects.Add(newObj);
            return newObj;
        }
    }
}