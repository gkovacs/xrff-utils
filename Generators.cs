// 
// Generators.cs
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

namespace xrffutils
{
	class TripleStringGenerator : TripleGenerator<string>
	{
		public override bool SatisfiesConstraints()
		{
			if (i >= j)
				return false;
			if (j >= k)
				return false;
			if (!l[i].CompareStartNthIndexFromLast(l[j], '_', 0))
				return false;
			if (!l[i].CompareStartNthIndexFromLast(l[k], '_', 0))
				return false;
			return true;
		}
	}
	
	class TripleGenerator<T> : PairGenerator<T>
	{
		public int k;
		public override bool SatisfiesConstraints()
		{
			if (i >= j)
				return false;
			if (j >= k)
				return false;
			return true;
		}
		public new Triple<T, T, T> GetNext()
		{
			do
			{
				if (++k >= l.Length)
				{
					if (++j >= l.Length)
					{
						if (++i >= l.Length)
						{
							return null;
						}
						else
						{
							j = 0;
						}
					}
					else
					{
						k = 0;
					}
				}
			}
			while (!SatisfiesConstraints());
			return new Triple<T, T, T>(l[i], l[j], l[k]);
		}
	}
	
	class PairGenerator<T>
	{
		public T[] l;
		public int i;
		public int j;
		public virtual bool SatisfiesConstraints()
		{
			if (i >= j)
				return false;
			return true;
		}
		public Pair<T, T> GetNext()
		{
			do
			{
				if (++j >= l.Length)
				{
					if (++i >= l.Length)
					{
						return null;
					}
					else
					{
						j = 0;
					}
				}
			}
			while (!SatisfiesConstraints());
			return new Pair<T, T>(l[i], l[j]);
		}
	}
	
	class PairStringGenerator : PairGenerator<string>
	{
		public override bool SatisfiesConstraints()
		{
			if (i >= j)
				return false;
			if (!l[i].CompareStartNthIndexFromLast(l[j], '_', 0))
				return false;
			return true;
		}
	}
	
	class PairCoordinateGenerator : PairGenerator<string>
	{
		public override bool SatisfiesConstraints()
		{
			if (i >= j)
				return false;
			if (l[i].Length < 2 || l[j].Length < 2)
				return false;
			if (l[i][l[i].Length-2] != '_' || l[i][l[i].Length-2] != '_')
				return false;
			if (l[i][l[i].Length-1] != 'x' && l[i][l[i].Length-1] != 'y' && l[i][l[i].Length-1] != 'z')
				return false;
			if (l[i][l[i].Length-1] != l[j][l[j].Length-1])
				return false;
			return true;
		}
	}
}
