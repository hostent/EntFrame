using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ent.Common.Cache
{
    public interface ICache
    {
        object Get(string key);

        void Remove(string key);

        void Add(string key, object data);

        void Add(string key, object data, int second);

        void Add(string key, object data, DateTime limitTime);

    }
}
