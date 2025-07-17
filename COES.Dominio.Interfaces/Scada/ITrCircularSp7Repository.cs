using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla F_LECTURA_SP7
    /// </summary>
    public interface ITrCircularSp7Repository
    {
        /*
        void Save(FLecturaSp7DTO entity);
        void Update(FLecturaSp7DTO entity);
        void Delete(DateTime fechahora, int gpscodi);
        FLecturaSp7DTO GetById(DateTime fechahora, int gpscodi);
        List<FLecturaSp7DTO> List();
        List<FLecturaSp7DTO> GetByCriteria();        
        */
        List<DatosSP7DTO> BuscarOperaciones(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin);
        /*
        int ObtenerNroFilas(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin);
        List<FLecturaSp7DTO> ObtenerMaximaFrecuencia(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin);
        List<FLecturaSp7DTO> ObtenerMinimaFrecuencia(int gpsCodi, DateTime fechaHoraIni, DateTime fechaHoraFin);
        List<FLecturaSp7DTO> ObtenerSubitaFrecuencia(int gpsCodi, string transgresiones, DateTime fechaHoraIni, DateTime fechaHoraFin);
        List<FLecturaSp7DTO> ObtenerSostenidaFrecuencia(int gpsCodi, string transgresiones, DateTime fechaHoraIni, DateTime fechaHoraFin);
        */
    }
}
