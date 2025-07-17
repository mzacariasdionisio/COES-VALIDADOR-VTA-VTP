using COES.Dominio.DTO.Scada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace COES.Servicios.Distribuidos.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface ITiempoRealServicio
    {
        [OperationContract]
        List<TrLogdmpSp7DTO> ListExpTrLogdmpSp7s(string estado);

    }
}