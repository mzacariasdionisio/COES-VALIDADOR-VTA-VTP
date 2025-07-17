using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MAP_MEDICION48
    /// </summary>
    public interface IMapMedicion48Repository
    {
        int Save(MapMedicion48DTO entity);
        void Update(MapMedicion48DTO entity);
        void Delete(int mediccodi);
        MapMedicion48DTO GetById(int mediccodi); 
        List<MapMedicion48DTO> List();
        List<MapMedicion48DTO> GetByCriteria(int vermcodi);
        void ListSave(List<MapMedicion48DTO> listaMapMedicion48);
        List<MapMedicion48DTO> ListaMapMedicion48PorFecha(DateTime fechaIni, DateTime fechaFin, string tipoccodi);
    }
}
