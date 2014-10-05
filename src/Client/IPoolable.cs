
namespace Client
{
    interface IPoolable
    {
        void Pool();
        void UnPool();
        bool Free { get; set; }
    }

    static class PoolableExtension
    {
        public static void Pool(this IPoolable ext)
        {
            ext.Pool();
            ext.Free = true;
        }

        public static void UnPool(this IPoolable ext)
        {
            ext.UnPool();
            ext.Free = false;
        }
    }
}
