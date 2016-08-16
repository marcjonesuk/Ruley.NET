﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.IO;
using System.Dynamic;
using Microsoft.CSharp;

namespace Ruley.Tests
{
	[TestFixture]
    public class YamlParsingTests
    {
		private string LoadFromResource()
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("Ruley.Tests.YamlParsing.Map1.yaml"))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		[Test]
		public void Test1()
		{
			var yaml = LoadFromResource();
			var x = YamlParser.Load(yaml);

			Event last = null;
			x.Subscribe(s => { last = s; });
			x.Start();

			dynamic next = new Event();
			next.a = "abc";
			x.OnNext(next);

			Assert.AreEqual("XYZ", last["newfield"]);
		}
    }
}
