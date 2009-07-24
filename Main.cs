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
using System.Collections.Generic;

namespace xrffutils
{
	static class extensions
	{
		public static bool Contains<T>(this T[] strl, T item)
		{
			foreach (T x in strl)
			{
				if (x.Equals(item))
					return true;
			}
			return false;
		}
		public static int IndexOf<T>(this T[] strl, T item)
		{
			int i;
			for (i = 0; i < strl.Length; ++i)
			{
				if (strl[i].Equals(item))
					return i;
			}
			return -1;
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
			return features.ToArray();
		}
		
		public static void selectfeatures(string featureFile, string inputFile, string outputFile)
		{
			selectfeatures(listfeatures(featureFile), inputFile, outputFile);
		}
		
		public static void selectfeatures(string[] features, string inputFile, string outputFile)
		{
			// TODO stub
			FileStream fso = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
			StreamWriter tfso = new StreamWriter(fso);
			FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
			XmlTextReader xi = new XmlTextReader(fs);
			XmlTextWriter xo = new XmlTextWriter(tfso);
			xo.Indentation = 1;
			xo.IndentChar = '\t';
			xo.Formatting = Formatting.Indented;
			int[] featureIdx = new int[features.Length];
			int curidx = 0;
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
			while (xi.Read())
			{
				if (xi.NodeType == XmlNodeType.Element && xi.IsStartElement())
				{
					xo.WriteStartElement(xi.Name);
					xo.WriteAttributes(xi, true);
					if (xi.Name == "attributes")
						break;
				}
			}
			while (xi.Read())
			{
				if (xi.IsStartElement())
				{
					if (xi.Name == "attribute")
					{
						string attrname = xi.GetAttribute("name");
						if (attrname != null)
						{
							if (features.Contains(attrname))
							{
								featureIdx[features.IndexOf(attrname)] = curidx;
								xo.WriteNode(xi, true);
							}
							++curidx;
						}
					}
				}
				else if (xi.NodeType == XmlNodeType.EndElement)
				{
					xo.WriteEndElement();
					if (xi.Name == "header")
					{
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
						xo.WriteEndElement();
					}
				}
				else if (xi.Name == "value" && xi.IsStartElement())
				{
					if (featureIdx.Contains(vcount))
						xo.WriteNode(xi, true);
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
				if (args.Length < 4)
				{
					Console.WriteLine("not enough arguments for "+args[0]);
					return;
				}
				selectfeatures(args[1], args[2], args[3]);
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
			else
			{
				Console.WriteLine("Option "+args[0]+" not recognized");
			}
		}
	}
}