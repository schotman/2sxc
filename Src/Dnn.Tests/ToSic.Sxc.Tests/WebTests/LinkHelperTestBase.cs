﻿using ToSic.Sxc.Web;
using ToSic.Testing.Shared;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ToSic.Sxc.Tests.WebTests
{
    public class LinkHelperTestBase: EavTestBase
    {
        /// <summary>
        /// 
        /// </summary>
        public LinkHelperTestBase()
        {
            // @STV - don't use statics in tests - can cause unexpected results across tests
            // Every test should run by itself

            Link = Resolve<ILinkHelper>();
        }

        internal ILinkHelper Link;


        // @STV - don't use statics in tests - results in object-reuse, but we want to always run clean
        internal /*static*/ void ToUrlAreEqual(string testUrl, string part = null) 
            => AreEqual(testUrl, Link.To(part: part));
    }
}
