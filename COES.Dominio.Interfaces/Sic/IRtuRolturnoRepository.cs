using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RTU_ROLTURNO
    /// </summary>
    public interface IRtuRolturnoRepository
    {
        int Save(RtuRolturnoDTO entity);
        void Update(RtuRolturnoDTO entity);
        void Delete(int rturolcodi);
        RtuRolturnoDTO GetById(int anio, int mes);
        List<RtuRolturnoDTO> List();
        List<RtuRolturnoDTO> ObtenerEstructura(int anio, int mes);
        List<RtuRolturnoDTO> ObtenerDatosPorFecha(DateTime fecha, int tipoReporte);
    }
}
