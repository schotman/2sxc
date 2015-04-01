﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToSic.Eav;
using ToSic.Eav.DataSources;

namespace ToSic.SexyContent
{
	public class ContentGroups
	{
		private const string ContentGroupTypeName = "2SexyContent-ContentGroup";

		private readonly int _zoneId;
		private readonly int _appId;

		public ContentGroups(int zoneId, int appId)
		{
			_zoneId = zoneId;
			_appId = appId;
		}

		private IDataSource ContentGroupSource()
		{
			var dataSource = DataSource.GetInitialDataSource(_zoneId, _appId, false);
			dataSource = DataSource.GetDataSource<EntityTypeFilter>(_zoneId, _appId, dataSource);
			((EntityTypeFilter)dataSource).TypeName = ContentGroupTypeName;
			return dataSource;
		}

		public IEnumerable<ContentGroup> GetContentGroups()
		{
			return ContentGroupSource().List.Select(p => new ContentGroup(p.Value));
		}

		public ContentGroup GetContentGroup(Guid contentGroupGuid)
		{
			var dataSource = ContentGroupSource();
			return new ContentGroup(dataSource.List.FirstOrDefault(e => e.Value.EntityGuid == contentGroupGuid).Value);
		}

		public void UpdateContentGroup(Guid contentGroupGuid, int? templateId)
		{
			var values = new Dictionary<string, object>
			{
				{ "Template", templateId.HasValue ? new[] { templateId.Value } : new int[]{} }
			};

			var context = EavContext.Instance(_zoneId, _appId);
			context.UpdateEntity(contentGroupGuid, values);
		}

		public void RemoveContentGroupItem(Guid contentGroupGuid, string type, int sortOrder)
		{
			// ToDo: Fix removing content group item
			throw new Exception("Not implemented yet (code for removing an item in the content group)");
		}

		public bool IsConfigurationInUse(int templateId, string type)
		{
			//Templates.GetContentGroupItems().Any(c => c.TemplateID == TemplateID && c.ItemType == ItemType && c.EntityID.HasValue);

			var contentGroups = GetContentGroups().Where(p => p.Template != null && p.Template.TemplateId == templateId);

			switch (type)
			{
				case "Presentation":
					return contentGroups.Any(p => p.Presentation.Any(c => c != null));
				case "ListContent":
					return contentGroups.Any(p => p.ListContent.Any(c => c != null));
				case "ListPresentation":
					return contentGroups.Any(p => p.ListPresentation.Any(c => c != null));
				default:
					return contentGroups.Any(p => p.Content.Any(c => c != null));

			}
		}

		public Guid CreateContentGroup()
		{
			var context = EavContext.Instance(_zoneId, _appId);
			var contentType = DataSource.GetCache(_zoneId, _appId).GetContentType(ContentGroupTypeName);

			var values = new Dictionary<string, object>()
			{
				{"Template", new int[] {}},
				{"Content", new int[] {}},
				{"Presentation", new int[] {}},
				{"ListContent", new int[] {}},
				{"ListPresentation", new int[] {}}
			};

			var entity = context.AddEntity(contentType.AttributeSetId, values, null, null);
			return entity.EntityGUID;
		}

	}
}