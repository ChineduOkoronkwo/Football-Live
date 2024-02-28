using System.Collections;

namespace FliveCLI.Helpers
{
    public class SqlStatementList : IEnumerable<string>
    {
        private readonly List<string> sqlList;

        public SqlStatementList()
        {
            sqlList = new List<string>();
        }

        public void Add(string item)
        {
            sqlList.Add(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return sqlList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join('\0', sqlList);
        }
    }
}