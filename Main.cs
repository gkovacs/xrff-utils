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
using System.Text.RegularExpressions;

namespace xrffutils
{
	class MainClass
	{
		public static void mkarff(string xrffFile)
		{
			string arffFile =  Regex.Replace(xrffFile, ".xrff$", ".arff");
			FileStream fso = new FileStream(arffFile, FileMode.Create, FileAccess.Write);
			StreamWriter ao = new StreamWriter(fso);
			ao.Write("@relation MindDVDSet");
			ao.WriteLine();
			ao.WriteLine();
			FileStream fs = new FileStream(xrffFile, FileMode.Open, FileAccess.Read);
			XmlTextReader xi = new XmlTextReader(fs);
			int mode = 0;
			bool mode1first = true;
			bool mode2nocomma = true;
			while (xi.Read())
			{
				if (mode == 0)
				{
					if (xi.Name == "attribute" && xi.IsStartElement())
					{
						String attrname = xi.GetAttribute("name");
						if (attrname != null)
						{
							ao.Write("@attribute "+attrname+" ");
						}
						string attrtype = xi.GetAttribute("type");
						if (attrtype != null)
						{
							if (attrtype == "nominal")
							{
								mode = 1;
							}
							else
							{
								ao.WriteLine(attrtype);
							}
						}
					}
				}
				else if (mode == 1)
				{
					if (xi.Name == "labels")
					{
						if (!xi.IsStartElement())
						{
							mode = 2;
							ao.Write("}");
							ao.WriteLine();
							ao.WriteLine();
							ao.Write("@data");
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
				else if (mode == 2)
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
			}
			ao.WriteLine();
			xi.Close();
			fs.Close();
			ao.Close();
			fso.Close();
		}
		public static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("not enough arguments");
				return;
			}
			if (args[0] == "mkarff")
				mkarff(args[1]);
		}
	}
}