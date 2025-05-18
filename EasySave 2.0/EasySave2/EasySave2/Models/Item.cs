namespace EasySave2.Models
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public string Type { get; protected set; }
        public long Size { get; protected set; }

        public abstract void Get();
    }
}
