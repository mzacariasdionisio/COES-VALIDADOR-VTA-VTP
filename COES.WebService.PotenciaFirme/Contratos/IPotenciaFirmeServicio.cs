using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.PotenciaFirme.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IPotenciaFirmeServicio
    {
        [OperationContract]
        int EjecutarPotenciaRemunerable(int pfpericodi, int pfrecacodi, int indrecacodiant, int recpotcodi, string usuario);

    }
}

