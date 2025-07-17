using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Enum
{
    public class EnumDbRepositorio
    {
        dbTypeReport type;

        public EnumDbRepositorio(dbTypeReport type)
        {
            this.type = type;
        }
        public string path
        {
            get
            {
                if (type == dbTypeReport.consolidadoCodigoPotencia)
                    return System.Configuration.ConfigurationManager.AppSettings["dbReportConsolidadoCodigoPotencia"].ToString();
                else
                    return "";
            }
        }


    }
    public enum dbTypeReport
    {
        consolidadoCodigoPotencia = 1
    }
}
