using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RI_SOLICITUD
    /// </summary>
    public interface IRiSolicitudRepository
    {
        int Save(RiSolicitudDTO entity);
        void Update(RiSolicitudDTO entity);
        void Delete(int solicodi);
        RiSolicitudDTO GetById(int solicodi);
        List<RiSolicitudDTO> List();
        List<RiSolicitudDTO> GetByCriteria();

        List<RiSolicitudDTO> ListPend(string soliestado, int nroPage, int pageSize);
        int ObtenerTotalListPend(string soliestado);

        List<RiSolicitudDTO> ListPendporEmpresa(string soliestado, int emprcodi, int nroPage, int pageSize);
        int ObtenerTotalListPendporEmpresa(string soliestado, int emprcodi);

        int DarConformidad(int solicodi);

        int DarNotificar(int solicodi);
        int FinalizarSolicitud(int solicodi, string estado, string observacion);
        int ActualizarFechaProceso(int solicodi, DateTime fecha, int usuario);
        int SolicitudEnCurso(int emprcodi, int codigoTipoSolicitud);
    }
}
