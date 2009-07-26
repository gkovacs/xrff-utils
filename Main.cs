// 
// Main.cs
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
using System.Xml;
using System.Xml.Schema;
using System.Collections;
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
			IEnumerator<float> n = strl.GetEnumerator();
			float total = 0.0f;
			float diff = 0.0f;
			while (n.MoveNext())
			{
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
				if (arr[i] == 0 || arr[i] >= arr.Length)
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
	}
	
	static class globvars
	{
		static public Pair<string, string>[] labels;
	}
	
	class Pair<T, U>
	{
		public T first;
		public U second;
		public Pair()
		{
			
		}
		public Pair(T f, U s)
		{
			first = f;
			second = s;
		}
	}
	
	class comboGenerator
	{
		public string[] l;
		public int i;
		public int j;
		public bool SatisfiesConstraints()
		{
			if (j > i)
				return true;
			return false;
		}
		public string GetNext()
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
			if (SatisfiesConstraints())
				return "("+l[i]+","+l[j]+")";
			else
				return GetNext();
		}
	}
	
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
		// N = N-dimensional distance
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
			else if (input[0] == 'N')
			{
				op = 'N';
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
				else if (op == 'N')
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
	
	class MainClass
	{
		public static void mkarff(string xrffFile, string arffFile)
		{
			FileStream fso = new FileStream(arffFile, FileMode.Create, FileAccess.Write);
			StreamWriter ao = new StreamWriter(fso);
			ao.Write("@relation MindDVDSet");
			ao.WriteLine();
			ao.WriteLine();
			FileStream fs = new FileStream(xrffFile, FileMode.Open, FileAccess.Read);
			XmlTextReader xi = new XmlTextReader(fs);
			bool mode1first = true;
			bool mode2nocomma = true;
			while (xi.Read())
			{
				if (xi.Name == "attribute" && xi.IsStartElement())
				{
					string attrname = xi.GetAttribute("name");
					if (attrname != null)
					{
						ao.Write("@attribute "+attrname+" ");
					}
					string attrtype = xi.GetAttribute("type");
					if (attrtype != null)
					{
						if (attrtype == "nominal")
						{
							break;
						}
						else
						{
							ao.WriteLine(attrtype);
						}
					}
				}
			}
			while (xi.Read())
			{
				if (xi.Name == "labels")
				{
					if (!xi.IsStartElement())
					{
						ao.Write("}");
						ao.WriteLine();
						ao.WriteLine();
						ao.Write("@data");
						break;
					}
				}
				else if (xi.Name == "label" && xi.IsStartElement())
				{
					string ltext = xi.ReadElementContentAsString();
					if (ltext != null)
					{
						if (mode1first)
						{
							mode1first = false;
							ao.Write("{");
						}
						else
						{
							ao.Write(",");
						}
						ao.Write(ltext);
					}
				}
			}
			while (xi.Read())
			{
				if (xi.Name == "instance")
				{
					if (xi.IsStartElement())
					{
						mode2nocomma = true;
					}
				}
				else if (xi.Name == "value")
				{
					if (mode2nocomma)
					{
						ao.WriteLine();
						mode2nocomma = false;
					}
					else
					{
						ao.Write(",");
					}
					string vtext = xi.ReadElementContentAsString();
					if (vtext != null)
					{
						ao.Write(vtext);
					}
				}
			}
			ao.WriteLine();
			xi.Close();
			fs.Close();
			ao.Close();
			fso.Close();
		}
		
		public static string[] listfeatures(string featureFile)
		{
			FileStream fs = new FileStream(featureFile, FileMode.Open, FileAccess.Read);
			StreamReader ao = new StreamReader(fs);
			List<string> features = new List<string>();
			while (!ao.EndOfStream)
			{
				string curf = ao.ReadLine().Trim();
				if (curf != null && curf != string.Empty && !features.Contains(curf))
					features.Add(curf);
			}
			ao.Close();
			fs.Close();
			if (!features.Contains("class"))
				features.Add("class");
			return features.ToArray();
		}
		
		public static Pair<string, string>[] listlabels(string featureFile)
		{
			FileStream fs = new FileStream(featureFile, FileMode.Open, FileAccess.Read);
			StreamReader ao = new StreamReader(fs);
			List<Pair<string, string> > features = new List<Pair<string, string> >();
			while (!ao.EndOfStream)
			{
				string curf = ao.ReadLine().Trim();
				Pair<string, string> p = new Pair<string, string>();
				p.first = curf.Split(':')[0];
				p.second = curf.Split(':')[1];
				if (curf != null && curf != string.Empty && !features.Contains(p))
					features.Add(p);
			}
			ao.Close();
			fs.Close();
			return features.ToArray();
		}
		
		public static void orderfeatures(string[] features, string inputFile)
		{
			FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
			XmlTextReader xi = new XmlTextReader(fs);
			List<string> ofeatures = new List<string>();
			while (xi.Read())
			{
				if (xi.IsStartElement())
				{
					if (xi.Name == "attribute")
					{
						string attrname = xi.GetAttribute("name");
						if (attrname != null)
						{
							if (attrname == "class")
								continue;
							if (features.Contains(attrname))
							{
								if (!ofeatures.Contains(attrname))
								{
									ofeatures.Add(attrname);
								}
							}
						}
					}
				}
				else if (xi.NodeType == XmlNodeType.EndElement)
				{
					if (xi.Name == "attributes")
					{
						break;
					}
				}
			}
			foreach (string x in features)
			{
				if (x == "class")
					continue;
				if (!ofeatures.Contains(x))
					ofeatures.Add(x);
			}
			ofeatures.Add("class");
			for (int i = 0; i < ofeatures.Count; ++i)
			{
				features[i] = ofeatures[i];
			}
			ofeatures = null;
			xi.Close();
			xi = null;
			fs.Close();
			fs = null;
		}
		
		public static void SetMatchAttrFile(string inputFile, opgroup[] featureIdx)
		{
			int curidx = 0;
			FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
			XmlTextReader xi = new XmlTextReader(fs);
			while (xi.Read())
			{
				if (xi.IsStartElement())
				{
					if (xi.Name == "attribute")
					{
						string attrname = xi.GetAttribute("name");
						if (attrname != null)
						{
							if (featureIdx.Contains(attrname))
							{
								Console.WriteLine(attrname);
								featureIdx.SetMatchAttr(attrname, curidx);
							}
						}
						++curidx;
					}
				}
			}
			xi.Close();
			fs.Close();
		}
		
		public static void selectfeatures(string featureFile, string labelFile, string inputFile, string outputFile)
		{
			selectfeatures(listfeatures(featureFile), listlabels(labelFile), inputFile, outputFile);
		}
		
		public static void selectfeatures(string[] features, Pair<string, string>[] labels, string inputFile, string outputFile)
		{
			orderfeatures(features, inputFile);
			opgroup[] featureIdx = new opgroup[features.Length];
			for (int i = 0; i < features.Length; ++i)
			{
				featureIdx[i] = new opgroup(features[i]);
			}
			globvars.labels = labels;
			SetMatchAttrFile(inputFile, featureIdx);
			FileStream fso = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
			StreamWriter tfso = new StreamWriter(fso);
			FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
			XmlTextReader xi = new XmlTextReader(fs);
			XmlTextWriter xo = new XmlTextWriter(tfso);
			xo.Indentation = 1;
			xo.IndentChar = '\t';
			xo.Formatting = Formatting.Indented;
			while (xi.Read())
			{
				if (xi.NodeType == XmlNodeType.Element && xi.IsStartElement())
				{
					if (xi.Name == "dataset")
					{
						xo.WriteStartElement(xi.Name); // dataset
						xo.WriteAttributes(xi, true);
						break;
					}
				}
				else
				{
					xo.WriteNode(xi, true);
				}
			}
			xo.WriteStartElement("header");
			xo.WriteStartElement("attributes");
			foreach (opgroup opg in featureIdx)
			{
				string s = opg.ToString();
				xo.WriteStartElement("attribute");
				if (s == "class")
				{
					xo.WriteAttributeString("class", "yes");
					xo.WriteAttributeString("name", "class");
					xo.WriteAttributeString("type", "nominal");
					xo.WriteStartElement("labels");
					foreach (string x in labels.GetUnique())
					{
						xo.WriteElementString("label", x);
					}
					xo.WriteEndElement(); // labels
				}
				else
				{
					xo.WriteAttributeString("name", s);
					xo.WriteAttributeString("type", "numeric");
				}
				xo.WriteEndElement(); // attribute
			}

			while (xi.Read())
			{
				if (xi.NodeType == XmlNodeType.EndElement)
				{
					if (xi.Name == "attributes")
					{
						xo.WriteEndElement();
					}
					else if (xi.Name == "header")
					{
						xo.WriteEndElement();
						break;
					}
				}
			}
			while (xi.Read())
			{
				if (xi.NodeType == XmlNodeType.Element && xi.IsStartElement())
				{
					xo.WriteStartElement(xi.Name);
					xo.WriteAttributes(xi, true);
					if (xi.Name == "instances")
						break;
				}
			}
			int vcount = 0;
			while (xi.Read())
			{
				if (xi.Name == "instance")
				{
					if (xi.IsStartElement())
					{
						vcount = 0;
						xo.WriteStartElement(xi.Name);
						xo.WriteAttributes(xi, true);
					}
					else if (xi.NodeType == XmlNodeType.EndElement)
					{
						foreach (opgroup x in featureIdx)
						{
							xo.WriteElementString("value", x.GetVal());
						}
						xo.WriteEndElement();
					}
				}
				else if (xi.Name == "value" && xi.IsStartElement())
				{
					while (xi.Value == string.Empty)
						xi.Read();
					featureIdx.SetValAtIdx(vcount, xi.Value);
					++vcount;
				}
			}
			xi.Close();
			fs.Close();
			xo.Close();
			tfso.Close();
			fso.Close();
		}
		
		public static void addfeatures(string featureFile, string[] allargs)
		{
			FileStream fs = new FileStream(featureFile, FileMode.Append, FileAccess.Write);
			StreamWriter ao = new StreamWriter(fs);
			for (int i = 2; i < allargs.Length; ++i)
			{
				ao.WriteLine(allargs[i]);
			}
			ao.Close();
			fs.Close();
		}
		
		public static void delfeatures(string featureFile, string[] allargs)
		{
			List<string> allfeatures = new List<string>(listfeatures(featureFile));
			FileStream fs = new FileStream(featureFile, FileMode.Create, FileAccess.Write);
			StreamWriter ao = new StreamWriter(fs);
			foreach (string x in allfeatures)
			{
				int i;
				for (i = 2; i < allargs.Length; ++i)
				{
					if (x == allargs[i])
						break;
				}
				if (i == allargs.Length)
					ao.WriteLine(x);
			}
			ao.Close();
			fs.Close();
		}
		
		public static string[] listfeaturesXrff(string xrffFile)
		{
			FileStream fs = new FileStream(xrffFile, FileMode.Open, FileAccess.Read);
			XmlTextReader xi = new XmlTextReader(fs);
			List<string> allfeatures = new List<string>();
			while (xi.Read())
			{
				if (xi.IsStartElement())
				{
					if (xi.Name == "attribute")
					{
						string attrname = xi.GetAttribute("name");
						if (attrname != null)
						{
							allfeatures.Add(attrname);
						}
					}
					else if (xi.Name == "body")
					{
						break;
					}
				}
			}
			xi.Close();
			fs.Close();
			return allfeatures.ToArray();
		}
		
		public static void validate(string xrffFile)
		{
			FileStream fs = new FileStream(xrffFile, FileMode.Open, FileAccess.Read);
			XmlReaderSettings s = new XmlReaderSettings();
			s.ProhibitDtd = false;
			s.ValidationType = ValidationType.DTD;
			s.ValidationEventHandler += new ValidationEventHandler(xrffValidationCallback);
			XmlReader xi = XmlReader.Create(fs, s);
			while (xi.Read())
			{
				
			}
			xi.Close();
			fs.Close();
		}
		
		public static void xrffValidationCallback(object sender, ValidationEventArgs e)
		{
			Console.WriteLine("error while validating: "+e.Message);
		}
		
		public static string[] listlabelsxrff(string inputFile)
		{
			List<string> labelsl = new List<string>();
			FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
			XmlTextReader xi = new XmlTextReader(fs);
			while (xi.Read())
			{
				if (xi.IsStartElement() && xi.Name == "label")
				{
					while (xi.Value == string.Empty)
					{
						xi.Read();
					}
					labelsl.Add(xi.Value);
				}
			}
			xi.Close();
			fs.Close();
			return labelsl.ToArray();
		}
		
		public static void combogen(string inputFile)
		{
			FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
			StreamReader ao = new StreamReader(fs);
			List<string> features = new List<string>();
			while (!ao.EndOfStream)
			{
				string curf = ao.ReadLine().Trim();
				if (curf != null && curf != string.Empty && !features.Contains(curf))
					features.Add(curf);
			}
			ao.Close();
			fs.Close();
			ao = null;
			fs = null;
			comboGenerator cbg = new comboGenerator();
			cbg.l = features.ToArray();
			features = null;
			string s;
			while ((s = cbg.GetNext()) != null)
			{
				Console.WriteLine(s);
			}
		}
		
		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("not enough arguments");
				return;
			}
			if (args[0] == "mkarff")
			{
				if (args.Length < 3)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				mkarff(args[1], args[2]);
			}
			else if (args[0] == "listfeatures")
			{
				if (args.Length < 2)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				string[] allfeatures = listfeaturesXrff(args[1]);
				foreach (string x in allfeatures)
				{
					Console.WriteLine(x);
				}
			}
			else if (args[0] == "selectfeatures")
			{
				if (args.Length < 5)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				selectfeatures(args[1], args[2], args[3], args[4]);
			}
			else if (args[0] == "addfeatures")
			{
				if (args.Length < 3)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				addfeatures(args[1], args);
			}
			else if (args[0] == "delfeatures")
			{
				if (args.Length < 3)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				delfeatures(args[1], args);
			}
			else if (args[0] == "validate")
			{
				if (args.Length < 2)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				validate(args[1]);
			}
			else if (args[0] == "orderfeatures")
			{
				if (args.Length < 3)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				string[] curfeatures = listfeatures(args[1]);
				orderfeatures(curfeatures, args[2]);
				foreach (string x in curfeatures)
				{
					Console.WriteLine(x);
				}
			}
			else if (args[0] == "listlabelsxrff")
			{
				if (args.Length < 2)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				string[] alllabels = listlabelsxrff(args[1]);
				foreach (string x in alllabels)
				{
					Console.WriteLine(x);
				}
			}
			else if (args[0] == "combogen")
			{
				if (args.Length < 2)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				combogen(args[1]);
			}
			else
			{
				Console.WriteLine("Option "+args[0]+" not recognized");
			}
		}
	}
}