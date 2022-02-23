using System.Collections.Generic;

namespace DeribitFutureSubscriber
{
    public interface IDbAccess<T>
    {
        void Insert(IList<T> records);
    }
}
