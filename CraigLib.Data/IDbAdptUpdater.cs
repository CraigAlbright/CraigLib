using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraigLib.Data
{

    public interface IDbAdptUpdater
    {
        Dictionary<DataTable, string> Updating(DatabaseAdapter da, Dictionary<DataTable, string> dts);
    }
}
