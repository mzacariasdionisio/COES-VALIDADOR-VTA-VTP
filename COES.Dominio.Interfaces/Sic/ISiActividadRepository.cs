using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_ACTIVIDAD
    /// </summary>
    public interface ISiActividadRepository
    {
        int Save(SiActividadDTO entity);
        void Update(SiActividadDTO entity);
        void Delete(int actcodi);
        SiActividadDTO GetById(int actcodi);
        List<SiActividadDTO> List();
        List<SiActividadDTO> GetByCriteria();
        List<SiActividadDTO> GetListaActividadesPersonal(string areacodi);
    }
}
