using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VladB.Utility {
    public static partial class Extensions {
        #region Act
        /// <summary>
        /// ��������� ��������� �������� �� ����� ���������� ������/�������/������ ������� IList
        /// </summary>
        /// <typeparam name="T">��� ��������� ������������ � IList</typeparam>
        /// <param name="iList">������, ������ ��� ����� ������ IList</param>
        /// <param name="action">�������, ����������� ������ ���� T � int(�������)</param>
        public static void Act<T>(this IList<T> iList, Action<T, int> action) {
            for(int i = 0; i < iList.Count; i++) {
                action?.Invoke(iList[i], i);
            }
        }

        /// <summary>
        /// ��������� ��������� �������� �� ����� ���������� ������/�������/������ ������� IList, ���������� ���������� ForEach(x=>{blablabla})
        /// </summary>
        /// <typeparam name="T">��� ��������� ������������ � IList</typeparam>
        /// <param name="iList">������, ������ ��� ����� ������ IList</param>
        /// <param name="action">�������, ����������� ������ ���� T</param>
        public static void Act<T>(this IList<T> iList, Action<T> action) {
            for(int i = 0; i < iList.Count; i++) {
                action?.Invoke(iList[i]);
            }
        }

        /// <summary>
        /// ��������� ��������� �������� �� ����� ���������� ������ IEnumerable
        /// </summary>
        /// <typeparam name="T">��� ��������� ������������ � IEnumerable</typeparam>
        /// <param name="list">����� IEnumerable</param>
        /// <param name="action">�������, ����������� ������ ���� T � int(�������)</param>
        public static void Act<T>(this IEnumerable<T> iEnumerable, Action<T, int> action) {
            int i = 0;
            foreach(var item in iEnumerable) {
                action?.Invoke(item, i);
                i++;
            } 
        }

        /// <summary>
        /// ��������� ��������� �������� �� ����� ���������� ������ IEnumerable
        /// </summary>
        /// <typeparam name="T">��� ��������� ������������ � IEnumerable</typeparam>
        /// <param name="list">����� IEnumerable</param>
        /// <param name="action">�������, ����������� ������ ���� T</param>
        public static void Act<T>(this IEnumerable<T> iEnumerable, Action<T> action) {
            foreach(var item in iEnumerable) {
                action?.Invoke(item);
            }
        }
        #endregion

    }
}
