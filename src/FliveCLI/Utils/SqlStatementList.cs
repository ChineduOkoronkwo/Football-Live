using System.Collections;

namespace FliveCLI.Utils
{
    public class StringContentList : IEnumerable<string>
    {
        private readonly List<string> data;

        public StringContentList()
        {
            data = new List<string>();
        }

        public void Add(string item)
        {
            data.Add(item);
        }

        public int Count => data.Count;

        public string this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(' ', data);
        }
    }
}