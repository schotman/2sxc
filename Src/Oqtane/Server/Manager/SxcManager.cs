using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using ToSic.Sxc.Models;
using ToSic.Sxc.Repository;

namespace ToSic.Sxc.Manager
{
    public class SxcManager : IInstallable, IPortable
    {
        private ISxcRepository _SxcRepository;
        private ISqlRepository _sql;

        public SxcManager(ISxcRepository SxcRepository, ISqlRepository sql)
        {
            _SxcRepository = SxcRepository;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "ToSic.Sxc." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "ToSic.Sxc.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.Sxc> Sxcs = _SxcRepository.GetSxcs(module.ModuleId).ToList();
            if (Sxcs != null)
            {
                content = JsonSerializer.Serialize(Sxcs);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.Sxc> Sxcs = null;
            if (!string.IsNullOrEmpty(content))
            {
                Sxcs = JsonSerializer.Deserialize<List<Models.Sxc>>(content);
            }
            if (Sxcs != null)
            {
                foreach(var Sxc in Sxcs)
                {
                    _SxcRepository.AddSxc(new Models.Sxc { ModuleId = module.ModuleId, Name = Sxc.Name });
                }
            }
        }
    }
}