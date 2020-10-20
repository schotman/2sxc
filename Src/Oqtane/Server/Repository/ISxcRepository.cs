using System.Collections.Generic;
using ToSic.Sxc.Models;

namespace ToSic.Sxc.Repository
{
    public interface ISxcRepository
    {
        IEnumerable<Models.Sxc> GetSxcs(int ModuleId);
        Models.Sxc GetSxc(int SxcId);
        Models.Sxc AddSxc(Models.Sxc Sxc);
        Models.Sxc UpdateSxc(Models.Sxc Sxc);
        void DeleteSxc(int SxcId);
    }
}
