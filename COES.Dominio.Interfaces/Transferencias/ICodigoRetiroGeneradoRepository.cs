using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{ /// <summary>
  /// Interface de acceso a datos de la tabla vtp_codigo_retiro_generado
  /// </summary>
    public interface ICodigoRetiroGeneradoRepository : IRepositoryBase
    {
        void DesactivarSolicitudPeriodoActual(CodigoRetiroDTO entity);
        void UpdateEstado(CodigoRetiroGeneradoDTO entity, IDbConnection conn, DbTransaction tran);
        CodigoRetiroGeneradoDTO GenerarAprobacion(CodigoRetiroGeneradoDTO entity, IDbConnection conn, DbTransaction tran);
        void GenerarPotenciasPeriodosAbiertos(CodigoRetiroGeneradoDTO entity);
        void GenerarVTAPeriodosAbiertos(CodigoRetiroGeneradoDTO entity);
        void GenerarVTPPeriodosAbiertos(CodigoRetiroGeneradoDTO entity);
        List<CodigoRetiroGeneradoDTO> ListarCodigosGeneradoVTP(List<int> coresoCodi, int? barrCodiSum);
        List<CodigoRetiroGeneradoDTO> ListarCodigosVTPByEmpBar(int barrcodisum, int genemprcodi, int cliemprcodi);
        List<CodigoRetiroGeneradoDTO> ListarCodigosGeneradoVTPExtranet();
        CodigoRetiroGeneradoDTO GetByCodigoVTP(string codigovtp);
    }
}
