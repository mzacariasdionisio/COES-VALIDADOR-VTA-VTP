using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.Urs.Contratos
{
    [ServiceContract(Namespace = "www.coes.org.pe")]
    public interface IUrsServicio
    {
        [OperationContract]
        List<PrGrupoConceptoDato> ListarParametrosGenerales();

        [OperationContract]
        List<PrGrupoConceptoDato> ObtenerDatosMO(int iGrupoCodi, DateTime fechaRegistro);

        [OperationContract]
        List<ManManttoDTO> ConsultarManttoURS(int iGrupoCodi, DateTime fechaRegistro);
    }
}
