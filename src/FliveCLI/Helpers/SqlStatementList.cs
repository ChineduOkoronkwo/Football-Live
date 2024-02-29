using System.Collections;

namespace FliveCLI.Helpers
{
    public class SqlStatementList : IEnumerable<string>
    {
        private readonly List<string> data;

        public SqlStatementList()
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
            return string.Join('\n', data);
        }
    }
}