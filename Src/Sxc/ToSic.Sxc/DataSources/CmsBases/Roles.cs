﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ToSic.Eav.Data;
using ToSic.Eav.DataSources;
using ToSic.Eav.DataSources.Queries;
using ToSic.Lib.Logging;
using ToSic.Eav.Plumbing;
using ToSic.Lib.Documentation;

// Important Info to people working with this
// It depends on abstract provder, that must be overriden in each platform
// In addition, each platform must make sure to register a TryAddTransient with the platform specific provider implementation
// This is because any constructor DI should be able to target this type, and get the real provider implementation

namespace ToSic.Sxc.DataSources
{
    /// <summary>
    /// Will get all (or just some) roles of the current site.
    /// </summary>
    [PublicApi]
    [VisualQuery(
        NiceName = "Roles (User Roles)",
        Icon = Icons.UserCircled,
        UiHint = "Roles in this site",
        HelpLink = "https://r.2sxc.org/ds-roles",
        GlobalName = "eee54266-d7ad-4f5e-9422-2d00c8f93b45",
        Type = DataSourceType.Source,
        ExpectsDataOfType = "1b9fd9d1-dde0-40ad-bb66-5cd7f30de18d",
        Difficulty = DifficultyBeta.Default
    )]
    public class Roles : ExternalData
    {
        private readonly RolesDataSourceProvider _provider;

        #region Other Constants

        private const char Separator = ',';

        #endregion

        #region Configuration-properties
        [PrivateApi] internal const string RoleIdsKey = "RoleIds";
        [PrivateApi] internal const string ExcludeRoleIdsKey = "ExcludeRoleIds";

        /// <summary>
        /// Optional (single value or comma-separated integers) filter,
        /// include roles based on roleId
        /// </summary>
        public virtual string RoleIds
        {
            get => Configuration[RoleIdsKey];
            set => Configuration[RoleIdsKey] = value;
        }

        /// <summary>
        /// Optional (single value or comma-separated integers) filter,
        /// exclude roles based on roleId
        /// </summary>
        public virtual string ExcludeRoleIds
        {
            get => Configuration[ExcludeRoleIdsKey];
            set => Configuration[ExcludeRoleIdsKey] = value;
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor to tell the system what out-streams we have
        /// </summary>
        [PrivateApi]
        public Roles(Dependencies dependencies, RolesDataSourceProvider provider) : base(dependencies, $"SDS.Roles")
        {
            ConnectServices(
                _provider = provider
            );
            Provide(GetList); // default out, if accessed, will deliver GetList

            ConfigMask(RoleIdsKey);
            ConfigMask(ExcludeRoleIdsKey);
        }

        #endregion

        [PrivateApi]
        public IImmutableList<IEntity> GetList() => Log.Func(l =>
        {
            var roles = _provider.GetRolesInternal()?.ToList();
            l.A($"found {roles?.Count} roles");

            if (roles == null || !roles.Any()) 
                return (new List<IEntity>().ToImmutableList(), "null/empty");

            // This will resolve the tokens before starting
            Configuration.Parse();

            var includeRolesPredicate = IncludeRolesPredicate();
            l.A($"includeRoles: {includeRolesPredicate == null}");
            if (includeRolesPredicate != null) roles = roles.Where(includeRolesPredicate).ToList();

            var excludeRolesPredicate = ExcludeRolesPredicate();
            l.A($"excludeRoles: {excludeRolesPredicate == null}");
            if (excludeRolesPredicate != null) roles = roles.Where(excludeRolesPredicate).ToList();

            var builder = new DataBuilderQuickWIP(DataBuilder, typeName: "Role", titleField: nameof(CmsRoleInfo.Name));

            var result = roles
                .Select(p => builder.CreateWithEavNullId(p))
                .ToImmutableList();

            l.A($"returning {result.Count} roles");
            return (result, "found");
        });

        private Func<CmsRoleInfo, bool> IncludeRolesPredicate()
        {
            var includeRolesFilter = RolesCsvListToInt(RoleIds);
            return includeRolesFilter.Any() 
                ? (Func<CmsRoleInfo, bool>) (r => includeRolesFilter.Contains(r.Id)) 
                : null;
        }

        private Func<CmsRoleInfo, bool> ExcludeRolesPredicate()
        {
            var excludeRolesFilter = RolesCsvListToInt(ExcludeRoleIds);
            return excludeRolesFilter.Any()
                ? (Func<CmsRoleInfo, bool>)(r => !excludeRolesFilter.Contains(r.Id))
                : null;
        }

        [PrivateApi]
        internal static List<int> RolesCsvListToInt(string stringList)
        {
            if (!stringList.HasValue()) return new List<int>();
            return stringList.Split(Separator)
                .Select(r => int.TryParse(r.Trim(), out var roleId) ? roleId : int.MinValue)
                .Where(r => r != int.MinValue)
                .ToList();
        }
    }
}
