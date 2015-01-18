using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodSeat.Clapte.Models
{
    public class FormulaCellList : IList<FormulaCell>
    {
        List<FormulaCell> InnerList { get; set; }


        public int IndexOf(FormulaCell item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, FormulaCell item)
        {
            InnerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InnerList.RemoveAt(index);
        }

        public FormulaCell this[int index]
        {
            get
            {
                return InnerList[index];
            }
            set
            {
                InnerList[index] = value;
            }
        }

        public void Add(FormulaCell item)
        {
            InnerList.Add(item);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public bool Contains(FormulaCell item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(FormulaCell[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(FormulaCell item)
        {
            return InnerList.Remove(item);
        }

        public IEnumerator<FormulaCell> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }
    }
}
