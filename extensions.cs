// 
// extensions.cs
//  
// Author:
//       Geza Kovacs <gkovacs@mit.edu>
// 
// Copyright (c) 2009 Geza Kovacs
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Collections.Generic;

namespace xrffutils
{
	static class extensions
	{
		public static bool Contains<T>(this IEnumerable<T> strl, T item)
		{
			foreach (T x in strl)
			{
				if (x.Equals(item))
					return true;
			}
			return false;
		}
		
		public static bool Contains(this IEnumerable<opgroup> strl, int item)
		{
			foreach (opgroup x in strl)
			{
				if (x.Contains(item))
					return true;
			}
			return false;
		}
		
		public static bool ContainsInFirst<T, U>(this IEnumerable< Pair<T, U> > strl, T item)
		{
			foreach (Pair<T, U> x in strl)
			{
				if (x.first.Equals(item))
					return true;
			}
			return false;
		}
		
		public static bool Contains(this IEnumerable<opgroup> strl, string item)
		{
			foreach (opgroup x in strl)
			{
				if (x.Contains(item))
					return true;
			}
			return false;
		}
		
		public static int IndexOf<T>(this IEnumerable<T> strl, T item)
		{
			int i = 0;
			foreach (T x in strl)
			{
				if (x.Equals(item))
					return i;
				++i;
			}
			return -1;
		}
		
		public static int IndexOfInFirst<T, U>(this IEnumerable< Pair<T, U> > strl, T item)
		{
			int i = 0;
			foreach (Pair<T, U> x in strl)
			{
				if (x.first.Equals(item))
					return i;
				++i;
			}
			return -1;
		}
		
		public static int IndexOf(this IEnumerable<opgroup> strl, string item)
		{
			int i = 0;
			foreach (opgroup x in strl)
			{
				if (x.Equals(item))
					return i;
				++i;
			}
			return -1;
		}
		
		public static string Join(this IEnumerable<string> strl, char sep)
		{
			string total = "";
			int i = 0;
			foreach (string x in strl)
			{
				if (i != 0)
					total += sep;
				total += x;
				++i;
			}
			return total;
		}
		
		public static float Sum(this IEnumerable<float> strl)
		{
			float total = 0.0f;
			foreach (float x in strl)
			{
				total += x;
			}
			return total;
		}
		
		public static float SumSquares(this IEnumerable<float> strl)
		{
			float total = 0.0f;
			foreach (float x in strl)
			{
				total += (x*x);
			}
			return total;
		}
		
		public static double SumSquares(this IEnumerable<double> strl)
		{
			double total = 0.0;
			foreach (double x in strl)
			{
				total += x.Squared();
			}
			return total;
		}
		
		public static double L2Norm(this IEnumerable<double> strl)
		{
			return strl.SumSquares().SquareRoot();
		}
		
		public static float L2Norm(this IEnumerable<float> strl)
		{
			return strl.SumSquares().SquareRoot();
		}
		
		public static float Distance(this IEnumerable<float> strl)
		{
			Console.WriteLine("invoking distance");
			IEnumerator<float> n = strl.GetEnumerator();
			float total = 0.0f;
			float diff = 0.0f;
			while (n.MoveNext())
			{
				Console.WriteLine("moved another");
				diff = n.Current;
				n.MoveNext();
				total += Math.Abs(diff - n.Current).Squared();
			}
			return total.SquareRoot();
		}
		
		public static float Distance2D(this IEnumerable<float> strl)
		{
			IEnumerator<float> n = strl.GetEnumerator();
			float total = 0.0f;
			float diff = 0.0f;
			n.MoveNext();
			diff = n.Current;
			n.MoveNext();
			total += Math.Abs(diff - n.Current).Squared();
			n.MoveNext();
			diff = n.Current;
			n.MoveNext();
			total += Math.Abs(diff - n.Current).Squared();
			return total.SquareRoot();
		}
		
		public static float Distance3D(this IEnumerable<float> strl)
		{
			IEnumerator<float> n = strl.GetEnumerator();
			float total = 0.0f;
			float diff = 0.0f;
			n.MoveNext();
			diff = n.Current;
			n.MoveNext();
			total += Math.Abs(diff - n.Current).Squared();
			n.MoveNext();
			diff = n.Current;
			n.MoveNext();
			total += Math.Abs(diff - n.Current).Squared();
			n.MoveNext();
			diff = n.Current;
			n.MoveNext();
			total += Math.Abs(diff - n.Current).Squared();
			return total.SquareRoot();
		}
		
		public static float Average(this float[] strl)
		{
			return strl.Sum() / strl.Length;
		}
		
		public static float AbsSubtract(this float[] strl)
		{
			return Math.Abs(strl[0] - strl[1]);
		}
		
		public static float Multiply(this IEnumerable<float> strl)
		{
			float total = 1.0f;
			foreach (float x in strl)
			{
				total *= x;
			}
			return total;
		}
		
		public static double Squared(this double s)
		{
			return s*s;
		}
		
		public static float Squared(this float s)
		{
			return s*s;
		}
		
		public static double SquareRoot(this double s)
		{
			return Math.Sqrt(s);
		}
		
		public static float SquareRoot(this float s)
		{
			return (float)Math.Sqrt(s);
		}
		
		public static void SetValAtIdx(this IEnumerable<opgroup> strl, int idx, string val)
		{
			if (idx == 0)
				return;
			foreach (opgroup x in strl)
			{
				if (x.Contains(idx))
				{
					x.SetValAtIdx(idx, val);
				}
			}
		}
		
		public static void SetMatchAttr(this IEnumerable<opgroup> strl, string attr, int idx)
		{
			foreach (opgroup x in strl)
			{
				if (x.Contains(attr))
				{
					x.SetMatchAttr(attr, idx);
				}
			}
		}
		
		public static string decodeString(this float[] arr)
		{
			string outmsg = "";
			string cmap = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
			for (int i = 0; i < arr.Length; ++i)
			{
				if ((int)arr[i] == 0 || (int)arr[i] >= cmap.Length)
					break;
				outmsg += cmap[(int)arr[i]];
			}
			return outmsg;
		}
		
		public static void encodeString(this float[] arr, string str)
		{
			string cmap = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
			for (int i = 0; i < str.Length; ++i)
			{
				arr[i] = cmap.IndexOf(str[i]);
			}
			for (int i = str.Length; i < arr.Length; ++i)
			{
				arr[i] = 0;
			}
		}
		
		public static float ToFloat(this string str)
		{
			return System.Convert.ToSingle(str);
		}
		
		public static int ToInt(this string str)
		{
			return System.Convert.ToInt32(str);
		}
		
		public static Pair<T, U[]>[] PairListToArray<T, U>(this List< Pair < T, List<U> > > l)
		{
			Pair<T, U[]>[] a = new Pair<T, U[]>[l.Count];
			for (int i = 0; i < a.Length; ++i)
			{
				a[i].first = l[i].first;
				a[i].second = l[i].second.ToArray();
			}
			return a;
		}
		
		public static string mkstring(this Pair<string, string> str)
		{
			return "("+str.first+","+str.second+")";
		}
		
		public static string mkstring(this Triple<string, string, string> str)
		{
			return "("+str.first+","+str.second+","+str.third+")";
		}
		
		public static string mkstring(this Pair< Triple<string, string, string>, Triple<string, string, string> > str)
		{
			return "{"+str.first.mkstring()+","+str.second.mkstring()+"}";
		}
		
		public static string mkstring(this Pair< Pair<string, string>, Pair<string, string> > str)
		{
			return "{"+str.first.mkstring()+","+str.second.mkstring()+"}";
		}
		
		public static string mkopequation(this Pair<string, string> str)
		{
			return "S"+str.first+"N"+str.second;
		}
		
		public static string mkopequation(this Pair< Pair<string, string>, Pair<string, string> > str)
		{
			return "W"+str.first.first+"N"+str.second.first+"N"+str.first.second+"N"+str.second.second;
		}
		
		public static string mkopequation(this Pair< Triple<string, string, string>, Triple<string, string, string> > str)
		{
			return "H"+str.first.first+"N"+str.second.first+"N"+str.first.second+"N"+str.second.second+"N"+str.first.third+"N"+str.second.third;
		}
		
		public static T[] GetUnique<T>(this Pair<T, T>[] strl)
		{
			List<T> ul = new List<T>();
			foreach (Pair<T, T> x in strl)
			{
				if (!ul.Contains(x.first))
				{
					ul.Add(x.first);
				}
			}
			return ul.ToArray();
		}
		
		public static T GetActualValue<T>(this IEnumerable< Pair<T, T> > strl, T val)
		{
			foreach (Pair<T, T> x in strl)
			{
				if (x.second.Equals(val))
				{
					return x.first;
				}
			}
			return val;
		}
		
		public static bool CompareMid(this string str, string s, int startpos1, int startpos2, int num)
		{
			if (num <= 0)
				return false;
			for (int i = 0; i < num; ++i)
			{
				if (str[startpos1+i] != s[startpos2+i])
					return false;
			}
			return true;
		}
		
		public static bool CompareEnd(this string str, string s, int num)
		{
			return str.CompareMid(s, str.Length-num, s.Length-num, num);
		}
		
		public static bool CompareEnd(this string str, string s)
		{
			return str.CompareEnd(s, s.Length);
		}
		
		public static bool CompareStart(this string str, string s, int num)
		{
			return str.CompareMid(s, 0, 0, num);
		}
		
		public static bool CompareStart(this string str, string s)
		{
			return str.CompareStart(s, s.Length);
		}
		
		public static string GetFirst(this string str, int num)
		{
			return str.GetMid(0, num);
		}
		
		public static string GetLast(this string str, int num)
		{
			return str.GetMid(str.Length-num, num);
		}
		
		public static string GetMid(this string str, int startpos, int num)
		{
			string outmsg = "";
			for (int i = startpos; i < startpos+num; ++i)
			{
				outmsg += str[i];
			}
			return outmsg;
		}
		
		public static string GetUntilEnd(this string str, int startpos)
		{
			return str.GetMid(startpos, str.Length);
		}
		
		public static bool CompareStartNthIndexOf(this string str, string s, char c, int idx)
		{
			return str.CompareStart(s, str.NthIndexOf(c, idx));
		}
		
		public static bool CompareStartNthIndexOf(this string str, string s, string c, int idx)
		{
			return str.CompareStart(s, str.NthIndexOf(c, idx));
		}
		
		public static bool CompareStartNthIndexFromLast(this string str, string s, char c, int idx)
		{
			return str.CompareStart(s, str.NthIndexFromLast(c, idx));
		}
		
		public static bool CompareStartNthIndexFromLast(this string str, string s, string c, int idx)
		{
			return str.CompareStart(s, str.NthIndexFromLast(c, idx));
		}
		
		public static bool CompareEndNthIndexOf(this string str, string s, char c, int idx)
		{
			return str.CompareEnd(s, str.NthIndexOf(c, idx));
		}
		
		public static bool CompareEndNthIndexOf(this string str, string s, string c, int idx)
		{
			return str.CompareEnd(s, str.NthIndexOf(c, idx));
		}
		
		public static bool CompareEndNthIndexFromLast(this string str, string s, char c, int idx)
		{
			return str.CompareEnd(s, str.NthIndexFromLast(c, idx));
		}
		
		public static bool CompareEndNthIndexFromLast(this string str, string s, string c, int idx)
		{
			return str.CompareEnd(s, str.NthIndexFromLast(c, idx));
		}
		
		public static int NthIndexOf(this string str, char s, int idx)
		{
			int i;
			for (i = 0; i < str.Length; ++i)
			{
				if (str[i] == s)
				{
					if (--idx == -1)
						break;
				}
			}
			if (idx == -1)
				return i;
			else
				return -1;
		}
		
		public static int NthIndexOf(this string str, string s, int idx)
		{
			int i;
			for (i = 0; i < str.Length+1-s.Length; ++i)
			{
				if (str.GetMid(i, s.Length) == s)
				{
					if (--idx == -1)
						break;
					else
						i += s.Length-1;
				}
			}
			if (idx == -1)
				return i;
			else
				return -1;
		}
		
		public static int NthIndexFromLast(this string str, string s, int idx)
		{
			return str.NthIndexOf(s, str.Count(s)-1-idx);
		}
		
		public static int NthIndexFromLast(this string str, char s, int idx)
		{
			return str.NthIndexOf(s, str.Count(s)-1-idx);
		}
		
		public static int Count(this string str, string s)
		{
			int num = 0;
			for (int i = 0; i < str.Length+1-s.Length; ++i)
			{
				if (str.GetMid(i, s.Length) == s)
				{
					++num;
					i += s.Length-1;
				}
			}
			return num;
		}
		
		public static int Count(this string str, char s)
		{
			int num = 0;
			for (int i = 0; i < str.Length; ++i)
			{
				if (str[i] == s)
				{
					++num;
				}
			}
			return num;
		}
		
		public static string[] Filter(this IEnumerable<string> l, string s)
		{
			List<string> n = new List<string>();
			foreach (string x in l)
			{
				if (x.Contains(s))
					n.Add(x);
			}
			return n.ToArray();
		}
		
		public static T GetLastElem<T>(this T[] l)
		{
			return l[l.Length-1];
		}
		
		public static string GetParentDir(this string x)
		{
			return Directory.GetParent(x).ToString();
		}
		
		public static string GetClassFromPath(this string fpath)
		{
			return fpath.GetParentDir().Split('_').GetLastElem();
		}
		
		public static string GetSubClassFromPath(this string fpath)
		{
			return fpath.Split('-')[0].Split('V').GetLastElem();
		}
	}
}
