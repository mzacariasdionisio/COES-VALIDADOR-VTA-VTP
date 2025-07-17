using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_PROGRAMACION
    /// </summary>
    public interface IInProgramacionRepository
    {
        int Save(InProgramacionDTO entity);
        void Update(InProgramacionDTO programacion);
        void ActualizarSoloLectura(int progCodigo, int flagLectura, string usuario, DateTime fechaAprob, IDbConnection conn, DbTransaction tran);

        List<InProgramacionDTO> ListarProgramaciones(int idTipoProgramacion, string idProgramacion);
        List<InProgramacionDTO> ListProgramacionById(string progrcodis);
        InProgramacionDTO ObtenerProgramacionesPorId(int idProgramacion);
        InProgramacionDTO ObtenerProgramacionesPorFechaYTipo(DateTime fecInicio, int idTipoProgramacion);
        void ActualizarAprobadoReversion(int progCodigo, int flagAprobadoRevertido, string usuario, DateTime fechaAprob, IDbConnection conn, DbTransaction tran);
        void HabilitarReversion(int progCodigo, DateTime fechaMaxEnReversion, string usuario, DateTime fechaHabilitaReversion, IDbConnection conn, DbTransaction tran);
    }
}
