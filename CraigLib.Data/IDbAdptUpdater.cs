using System.Collections.Generic;
using System.Data;

namespace CraigLib.Data
{

    public interface IDbAdptUpdater
    {
        Dictionary<DataTable, string> Updating(DatabaseAdapter da, Dictionary<DataTable, string> dts);
    }
}
