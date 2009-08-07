// 
// opgroup.cs
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
	class opgroup
	{
		public char op;
		// I = identity (value)
		// S = subtract (absolute value)
		// A = average (mean)
		// T = total (sum)
		// M = multiply
		// D = divide
		// L = L2 norm
		// W = tWo-dimensional distance
		// H = tHree-dimensional distance
		// G = General N-dimensional distance
		public string[] names;
		public int[] idxs;
		public float[] values;
		
		public opgroup(string input)
		{
			string ninput;
			if (input[0] == 'S')
			{
				op = 'S';
				ninput = input.Remove(0, 1);
			}
			else if (input[0] == 'A')
			{
				op = 'A';
				ninput = input.Remove(0, 1);
			}
			else if (input[0] == 'L')
			{
				op = 'L';
				ninput = input.Remove(0, 1);
			}
			else if (input[0] == 'W')
			{
				op = 'W';
				ninput = input.Remove(0, 1);
			}
			else if (input[0] == 'H')
			{
				op = 'H';
				ninput = input.Remove(0, 1);
			}
			else if (input[0] == 'G')
			{
				op = 'G';
				ninput = input.Remove(0, 1);
			}
			else if (input[0] == 'I')
			{
				op = 'I';
				ninput = input.Remove(0, 1);
			}
			else
			{
				op = 'I';
				ninput = input;
			}
			if (op == 'I')
			{
				names = new string[1];
				names[0] = ninput;
			}
			else
			{
				names = ninput.Split('N');
			}
			idxs = new int[names.Length];
			if (ninput == "class")
				values = new float[20];
			else
				values = new float[names.Length];
		}
		
		public bool Equals(string item)
		{
			return (this.ToString() == item);
		}
		
		public bool Contains(int item)
		{
			return idxs.Contains(item);
		}
		
		public bool Contains(string item)
		{
			return names.Contains(item);
		}
		
		public void SetMatchAttr(string attr, int item)
		{
			for (int i = 0; i < names.Length; ++i)
			{
				if (names[i] == attr)
				{
					idxs[i] = item;
				}
			}
		}
		
		public override string ToString()
		{
			if (op == 'I')
				return names[0];
			else
				return op+names.Join('N');
		}
		
		public string GetVal()
		{
			if (names[0] == "class")
			{
				return globvars.labels.GetActualValue(values.decodeString());
			}
			else
			{
				if (op == 'I')
					return values[0].ToString();
				else if (op == 'S')
					return values.AbsSubtract().ToString();
				else if (op == 'A')
					return values.Average().ToString();
				else if (op == 'L')
					return values.L2Norm().ToString();
				else if (op == 'W')
					return values.Distance2D().ToString();
				else if (op == 'H')
					return values.Distance3D().ToString();
				else if (op == 'G')
					return values.Distance().ToString();
				else
					return null;
			}
		}
		
		public void SetValAtIdx(int idx, float val)
		{
			int i;
			for (i = 0; i < idxs.Length; ++i)
			{
				if (idxs[i] == idx)
					break;
			}
			values[i] = val;
		}
		
		public void SetValAtIdx(int idx, string val)
		{
			if (names[0] == "class")
			{
				values.encodeString(val);
			}
			else
				SetValAtIdx(idx, val.ToFloat());
		}
	}
}
