using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Distribuidos.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace COES.Servicios.Distribuidos.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class TiempoRealServicio : ITiempoRealServicio
    {
        TiempoRealAppServicio ServTiempoReal = new TiempoRealAppServicio();
        public List<TrLogdmpSp7DTO> ListExpTrLogdmpSp7s(string estado)
        {
            return ServTiempoReal.ListExpTrLogdmpSp7s(estado);
        }

    }
}