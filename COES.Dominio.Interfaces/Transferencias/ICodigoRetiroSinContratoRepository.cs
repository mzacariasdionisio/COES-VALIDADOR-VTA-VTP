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
    /// Interface de acceso a datos de la tabla TRN_CODIGO_RETIRO_SINCONTRATO
    /// </summary>
    public interface ICodigoRetiroSinContratoRepository : IRepositoryBase
    {
        int Save(CodigoRetiroSinContratoDTO entity);
        void Update(CodigoRetiroSinContratoDTO entity);
        void Delete(System.Int32 id);
        CodigoRetiroSinContratoDTO GetById(System.Int32 id);
        List<CodigoRetiroSinContratoDTO> List();
        List<CodigoRetiroSinContratoDTO> GetByCriteria(string nombreCli, string nombreBarra, DateTime? fechaini, DateTime? fechafin, string estado,string codretirosc, int NroPagina, int PageSizeCodigoRetiroSC);
        CodigoRetiroSinContratoDTO GetByCodigoRetiroSinContratoCodigo(string Codretisinconcodigo);
        int ObtenerNroRegistros(string sCliEmprNomb, string sBarrBarraTransferencia, DateTime? dCoReSCFechaInicio, DateTime? dCoReSCFechaFin, string sCoReSCtEstado, string codretirosc);
    }
}

