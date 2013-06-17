using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EpgTimer
{
    public class NumericSortClass<T> : Comparer<T>
    {
        IEnumerable<SortDesc> sortdesc;

        public struct SortDesc
        {
            public Func<T, Object> sortby;
            public ListSortDirection direction;

            public SortDesc(Func<T, Object> SortBy, ListSortDirection Direction)
            {
                sortby = SortBy;
                direction = Direction;
            }
        };

        public NumericSortClass(Func<T, Object> sortBy, ListSortDirection direction = ListSortDirection.Ascending)
        {
            sortdesc = new[] { new SortDesc(sortBy, direction) };
        }

        public NumericSortClass(IEnumerable<SortDesc> sortDescList)
        {
            sortdesc = sortDescList;
        }

        public override int Compare(T x, T y)
        {
            int result = 0;
            foreach (var sortDesc in sortdesc)
            {
                // 比較するプロパティをdelegateを使って取得する
                // データ件数が大したことないのでリフレクションでもいいかも
                Object x1 = sortDesc.sortby(x);
                Object y1 = sortDesc.sortby(y);

                var type = x1.GetType();
                if (type == typeof(String))
                {
                    result = NumericCompare((String)x1, (String)y1);
                }
                else if (type == typeof(Int32))
                {
                    result = (Int32)x1 - (Int32)y1;
                }
                if (sortDesc.direction == ListSortDirection.Descending) result = -result;

                if (result != 0) break;
            }
            return result;
        }

        public static int NumericCompare(string x, string y)
        {
            int px = 0;
            int py = 0;
            int lenx = x.Length;
            int leny = y.Length;

            while (px < lenx && py < leny)
            {
                if (isDigit(x[px]) && isDigit(y[py]))
                {
                    string nx = extractDigits(x, ref px);
                    string ny = extractDigits(y, ref py);

                    int d = 0;
                    try
                    {
                        d = Int32.Parse(nx.Normalize(NormalizationForm.FormKC)) - Int32.Parse(ny.Normalize(NormalizationForm.FormKC));
                        if (d != 0) return d;
                    }
                    catch { };

                    // AKB48とAKB0048のような例のために文字列でも比較する
                    d = String.Compare(nx, ny);
                    if (d != 0) return d;
                }
                else
                {
                    int ex = px;
                    int ey = py;
                    while (ex < lenx && ey < leny)
                    {
                        if (isDigit(x[ex]) && isDigit(y[ey])) break;
                        ex++;
                        ey++;
                    }
                    string sx = x.Substring(px, ex - px);
                    string sy = y.Substring(py, ey - py);

                    // 大文字小文字カタカナひらがな全角半角を区別せずに比較
                    System.Globalization.CompareInfo ci = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;
                    int result = ci.Compare(sx, sy, System.Globalization.CompareOptions.IgnoreCase | System.Globalization.CompareOptions.IgnoreKanaType | System.Globalization.CompareOptions.IgnoreWidth);
                    if (result != 0) return result;

                    // 優劣が付かなければ単純に比較 (実用上必要無い？)
                    result = String.Compare(sx, sy);
                    if (result != 0) return result;

                    px = ex;
                    py = ey;
                }
            }
            return (lenx - px) - (leny - py);
        }

        private static bool isDigit(Char c)
        {
            return c >= '0' && c <= '9' || c >= '０' && c <= '９';
        }

        private static string extractDigits(string a, ref int pos)
        {
            int end = pos;
            while (end < a.Length && isDigit(a[end]))
                end++;

            string result = a.Substring(pos, end - pos);
            pos = end;

            return result;
        }
    }
}
