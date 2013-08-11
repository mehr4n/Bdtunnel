﻿/* BoutDuTunnel Copyright (c) 2007-2010 Sebastien LEBRETON

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

#region " Inclusions "
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bdt.Shared.Runtime;
#endregion

namespace Bdt.Tests.UnitTests
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Tests sur les fonctions simples
    /// </summary>
    /// -----------------------------------------------------------------------------
    [TestClass]
    public class ProgramTest : BaseTest
    {

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Test le codage des informations
        /// </summary>
        /// -----------------------------------------------------------------------------
        [TestMethod]
        public void TestStaticXorEncoder()
        {
            for (var key = 0; key < byte.MaxValue; key++)
            {
                for (var datalength = 0; datalength < 1024; datalength = (datalength == 0) ? 1 : datalength * 2)
                {
                    var buffer = new byte[datalength];
                    var outbuffer = new byte[datalength];

                    var rnd = new Random();
                    rnd.NextBytes(buffer);
                    Array.Copy(buffer, outbuffer, datalength);

                    Program.StaticXorEncoder(ref buffer, key);
                    Program.StaticXorEncoder(ref buffer, key);

	                for (var i = 0; i < datalength; i++)
		                Assert.AreEqual(buffer[i], outbuffer[i], String.Format("Offset {0}, key={1}", i, key));
                }
            }
        }
        #endregion

    }
}
