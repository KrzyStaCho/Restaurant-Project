using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.Item
{
    class ObjectPair<T1, T2>
    {
        public T1 FirstItem { get; set; }
        public T2 SecondItem { get; set;}

        public ObjectPair(T1 firstItem, T2 secondItem)
        {
            FirstItem = firstItem;
            SecondItem = secondItem;
        }
    }
}
