using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RTU_CONFIGURACION
    /// </summary>
    public interface IRtuConfiguracionRepository
    {
        int Save(RtuConfiguracionDTO entity);
        void Update(RtuConfiguracionDTO entity);
        void Delete(int rtuconcodi);
        RtuConfiguracionDTO GetById(int rtuconcodi);
        RtuConfiguracionDTO GetByAnioMes(int anio, int mes);
        List<RtuConfiguracionDTO> List();
        List<RtuConfiguracionDTO> ObtenerConfguracion(int anio, int mes);
        List<RtuConfiguracionDTO> ObtenerConfiguracionReciente(int anio, int mes);
    }
}
