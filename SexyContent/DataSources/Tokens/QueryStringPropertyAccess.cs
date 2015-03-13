﻿using System;
using System.Web;
using DotNetNuke.Services.Tokens;

namespace ToSic.SexyContent.DataSources.Tokens
{
	public class QueryStringPropertyAccess : ToSic.Eav.ValueProvider.IValueProvider, DotNetNuke.Services.Tokens.IPropertyAccess
	{
		#region Properties

		public string Name { get; private set; }

		public CacheLevel Cacheability
		{
			get { return CacheLevel.notCacheable; }
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public QueryStringPropertyAccess(string name)
		{
			Name = name;
		}

		public string Get(string propertyName, string format, ref bool propertyNotFound)
		{
            // todo: add system to prevent properties from returning tokens (so QueryString:Id should never give a "[Module:ModuleId]" back

			var context = HttpContext.Current;

			if (context == null)
			{
				propertyNotFound = false;
				return null;
			}

			return context.Request.QueryString[propertyName.ToLower()];
		}
        /// <summary>
        /// Shorthand version, will return the string value or a null if not found. 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual string Get(string property)
        {
            bool temp = false;
            return Get(property, "", ref temp);
        }


		public string GetProperty(string propertyName, string format, System.Globalization.CultureInfo formatProvider, DotNetNuke.Entities.Users.UserInfo accessingUser, DotNetNuke.Services.Tokens.Scope accessLevel, ref bool propertyNotFound)
		{
			return Get(propertyName, format, ref propertyNotFound);
		}

	    public bool Has(string property)
	    {
	        throw new NotImplementedException();
	    }
	}
}