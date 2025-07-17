using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_GPS
    /// </summary>
    public interface IMeGpsRepository
    {
        int Save(MeGpsDTO entity);
        void Update(MeGpsDTO entity);
        void Delete(int gpscodi);
        MeGpsDTO GetById(int gpscodi);
        List<MeGpsDTO> List();
        List<MeGpsDTO> GetByCriteria(int? empresa, string nombre, string oficial);
        List<MeGpsDTO> ObtenerListadoGPS();
        void ActualizarGPSIEOD(int gpscodi, string estado);
    }
}
