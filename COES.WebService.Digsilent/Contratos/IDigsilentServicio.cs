using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using COES.WebService.Digsilent.Models;

namespace COES.WebService.Digsilent.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    //[ServiceContract]
    public interface IDigsilentServicio
    {
        [OperationContract]
        MigracionesModel ProcesarDigsilent(string program, string fecha, int rdchk, string bloq, int fuente, int topcodiYupana);
    }
}
