using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_JUSTIFICACION
    /// </summary>
    public interface IMeJustificacionRepository
    {
        void Update(MeJustificacionDTO entity);
        void Save(MeJustificacionDTO entity);
        void Delete();
        MeJustificacionDTO GetById(int justcodi);  //ASSETEC 201909
        List<MeJustificacionDTO> List();
        List<MeJustificacionDTO> GetByCriteria();
        List<MeJustificacionDTO> ListByIdEnvio(int idEnvio);
        //ASSETEC 201909
        List<MeJustificacionDTO> ListByIdEnvioPtoMedicodi(int idEnvio, int idLectcodi, int idPtomedicodi);
    }
}
