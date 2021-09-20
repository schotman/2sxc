﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ToSic.Sxc.Data;
using ToSic.Sxc.Web;
using static ToSic.Sxc.Tests.Web.LinkImageTestHelpers;

namespace ToSic.Sxc.Tests.Web
{
    [TestClass]
    public class LinkImageInternalsTests
    {


        const int s = 1440; // standard value, divisible by 1/2/3/4/6/12

        [TestMethod]
        public void FigureOutBestWidthAndHeight()
        {
            // All Zero and Nulls
            Assert.IsTrue(TestBestWH(0, 0), "Null everything");
            Assert.IsTrue(TestBestWH(0, 0, 0), "Null everything");
            Assert.IsTrue(TestBestWH(0, 0, 0, 0), "Null everything");
            Assert.IsTrue(TestBestWH(0, 0, 0, 0, 0), "Null everything");
            Assert.IsTrue(TestBestWH(0, 0, 0, 0, 0, 0), "Null everything");

            // Single Aspect is set
            Assert.IsTrue(TestBestWH(s, 0, width: s), "W only");
            Assert.IsTrue(TestBestWH(0, s, height: s), "H only");
            Assert.IsTrue(TestBestWH(0, 0, factor: 7), "factor only");
            Assert.IsTrue(TestBestWH(0, 0, ar: 7), "ar only");

            // H / W
            Assert.IsTrue(TestBestWH(s, s, width: s, height: s), "W & H only");
        }

        [TestMethod]
        public void FigureOutBestWidthAndHeight_Factors()
        {
            // W / Factor - factor shouldn't apply
            Assert.IsTrue(TestBestWH(s, 0, width: s, factor: 1), "W/F only");
            Assert.IsTrue(TestBestWH(s, 0, width: s, factor: .5), "W/F only");
            Assert.IsTrue(TestBestWH(s, 0, width: s, factor: 2), "W/F only");
            Assert.IsTrue(TestBestWH(s, 0, width: s, factor: 1.5), "W/F only");

            // H / Factor - factor shouldn't apply
            Assert.IsTrue(TestBestWH(0, s, height: s, factor: 1), "H/F only");
            Assert.IsTrue(TestBestWH(0, s, height: s, factor: .5), "H/F only");
            Assert.IsTrue(TestBestWH(0, s, height: s, factor: 2), "H/F only");
            Assert.IsTrue(TestBestWH(0, s, height: s, factor: 1.5), "H/F only");

            // H / W / Factor - factor shouldn't apply
            Assert.IsTrue(TestBestWH(s, s, width: s, height: s, factor: 1), "W/H/F");
            Assert.IsTrue(TestBestWH(s, s, width: s, height: s, factor: .5), "W/H/F");
            Assert.IsTrue(TestBestWH(s, s, width: s, height: s, factor: 2), "W/H/F");
            Assert.IsTrue(TestBestWH(s, s, width: s, height: s, factor: 1.5), "W/H/F");
        }

        [TestMethod]
        public void FigureOutBestWidthAndHeight_SettingsWH()
        {
            Assert.IsTrue(TestBestWH(s, 0, settings: ToDyn(new { Width = s })), "W only");
            Assert.IsTrue(TestBestWH(0, s, settings: ToDyn(new { Height = s })), "H only");
            Assert.IsTrue(TestBestWH(0, 0, settings: ToDyn(new { })), "No params");
            Assert.IsTrue(TestBestWH(s, s, settings: ToDyn(new { Width = s, Height = s })), "W/H params");
        }

        [TestMethod]
        public void FigureOutBestWidthAndHeight_SettingsWHF()
        {
            FigureOutWHFactors(0, 1);
            FigureOutWHFactors(1, 1);
            FigureOutWHFactors(2, 2);
            FigureOutWHFactors(0.5, 0.5);
        }


        [TestMethod]
        public void FigureOutBestWH_SettingsWithNeutralizer()
        {
            Assert.IsTrue(TestBestWH(0, 0, 0, null, null, null, ToDyn(new { Width = 700 })));
        }

        private void FigureOutWHFactors(double factor, double fResult)
        {
            Console.WriteLine("Run with Factor: " + factor);
            // Factor 1
            Assert.IsTrue(TestBestWH((int)(fResult * s), 0, factor: factor, settings: ToDyn(new { Width = s })), $"W with f:{factor}");
            Assert.IsTrue(TestBestWH(0, (int)(fResult * s), factor: factor, settings: ToDyn(new { Height = s })), $"H with f:{factor}");
            Assert.IsTrue(TestBestWH(0, 0, factor: factor, settings: ToDyn(new { })), $"No params f:{factor}");
            Assert.IsTrue(TestBestWH((int)(fResult * s), (int)(fResult * s), factor: factor, settings: ToDyn(new { Width = s, Height = s })), $"W/H f:{factor}");
        }


        private bool TestBestWH(int expW, int expH, object width = null, object height = null, object factor = null, object ar = null, ICanGetNameNotFinal settings = null)
        {
            var linker = GetLinker();

            var t1 = linker.FigureOutBestWidthAndHeight(width, height, factor, ar, settings);
            var ok = Equals(new Tuple<int, int>(expW, expH), t1);
            var okW = expW.Equals(t1.Item1);
            var okH = expH.Equals(t1.Item2);
            Console.WriteLine((ok ? "ok" : "error") + "; W:" + t1.Item1 + (okW ? "==": "!=") + expW + "; H:" + t1.Item2 + (okH ? "==" : "!=") + expH);
            return ok;
        }
    }
}
