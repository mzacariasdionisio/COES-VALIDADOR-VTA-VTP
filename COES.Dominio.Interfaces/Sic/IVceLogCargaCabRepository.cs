using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_LOG_CARGA_CAB
    /// </summary>
    public interface IVceLogCargaCabRepository
    {
        int Save(VceLogCargaCabDTO entity);
        void Update(VceLogCargaCabDTO entity);
        void Delete(int crlcccodi);
        void DeleteCabPeriodo(int pecacodi);
        VceLogCargaCabDTO GetById(int crlcccodi);
        List<VceLogCargaCabDTO> List();
        List<VceLogCargaCabDTO> GetByCriteria();

        //- conpensaciones.JDEL - Inicio 03/01/2017: Cambio para atender el requerimiento.

        void Init(int pecacodi);

        //- JDEL Fin
       // DSH 09-05-2017 : Se agrego por requerimiento
        int GetMinIdByVersion(int pecacodi, string NombTabla);

    }
}
