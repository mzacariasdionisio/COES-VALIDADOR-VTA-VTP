using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_CODIGO_ENTREGA
    /// </summary>
    public interface ICodigoEntregaRepository : IRepositoryBase
    {
        int Save(CodigoEntregaDTO entity);
        void Update(CodigoEntregaDTO entity);
        void Delete(System.Int32 id);
        CodigoEntregaDTO GetById(System.Int32 id);
        List<CodigoEntregaDTO> List();
        List<CodigoEntregaDTO> GetByCriteria(string nombreEmp, string barrTrans, string centralgene, DateTime? fechini, DateTime? fechafin, string estado,string codentrega, int NroPagina, int PageSizeCodigoEntrega);
        int ObtenerNroRegistros(string nombreEmp, string barrTrans, string centralgene, DateTime? fechini, DateTime? fechafin, string estado, string codentrega);
        CodigoEntregaDTO GetByCodiEntrCodigo(System.String codientrCodigo);
        CodigoEntregaDTO GetByCodiEntrEmpresaCodigo(int iEmprCodi, string sCodEntCodigo);
        CodigoEntregaDTO CodigoEntregaVigenteByPeriodo(int iPericodi, string sCodigo);
    }
}
