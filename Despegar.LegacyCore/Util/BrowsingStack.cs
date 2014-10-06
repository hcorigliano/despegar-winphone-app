using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.LegacyCore.Util
{
    public class BrowsingStack
    {
        public BrowsingStack()
        {
            UriList = new List<Uri>();
        }

        public List<Uri> UriList { get; set; }


        public void Clear()
        {
            UriList.Clear();
        }

        public void Push(Uri url)
        {
            if (!url.Equals(Peek()))
                UriList.Add(url);
        }

        public Uri Pop()
        {
            var r = UriList.Any() ? UriList.Last() : null;
            if (UriList.Any()) UriList.RemoveAt(UriList.Count - 1);
            return r;
        }

        public Uri Peek()
        {
            return UriList.Any() ? UriList.Last() : null;
        }

        public bool Any()
        {
            return UriList.Any();
        }
    }
}