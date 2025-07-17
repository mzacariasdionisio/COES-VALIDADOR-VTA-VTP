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
    /// Interface de acceso a datos de la tabla TRN_CODIGO_INFOBASE
    /// </summary>
    public interface ICodigoInfoBaseRepository : IRepositoryBase
    {
        int Save(CodigoInfoBaseDTO entity);
        void Update(CodigoInfoBaseDTO entity);
        void Delete(System.Int32 CoInfBCodi);
        CodigoInfoBaseDTO GetById(System.Int32 CoInfBCodi);
        List<CodigoInfoBaseDTO> List();
        List<CodigoInfoBaseDTO> GetByCriteria(string nombreEmp, string barrTrans, string centralgene, DateTime? fechini, DateTime? fechafin, string estado, string codinfobase, int NroPagina, int PageSizeCodigoInfoBase);
        CodigoInfoBaseDTO GetByCoInfBCodigo(System.String CoInfBCodigo);
        int ObtenerNroRegistros(string sEmprNombre, string sBarrBarraTransferencia, string sEquiNomb, DateTime? dCoInfBFechaInicio, DateTime? dCoInfBFechaFin, string sCoInfBEstado,string codinfobase);
        CodigoInfoBaseDTO CodigoInfoBaseVigenteByPeriodo(int iPericodi, string sCodigo);
    }
}
