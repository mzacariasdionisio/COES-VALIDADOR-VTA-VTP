using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_PERSONA
    /// </summary>
    public interface ISiPersonaRepository
    {
        int Save(SiPersonaDTO entity);
        void Update(SiPersonaDTO entity);
        void Delete(int percodi);
        void Delete_UpdateAuditoria(int percodi, string username);
        SiPersonaDTO GetById(int percodi);
        List<SiPersonaDTO> List();
        SiPersonaDTO GetByCriteria(int usercode);
        List<SiPersonaDTO> GetByCriteriaArea(int areacodi);
        string GetCargo(string Nombre);
        string GetArea(string Nombre);
        string GetTelefono(string Nombre);
        string GetMail(string Nombre);
        List<SiPersonaDTO> ListaEspecialistasSME();
        #region MigracionSGOCOES-GrupoB
        List<SiPersonaDTO> ListaPersonalRol(string areacodi, DateTime fecIni, DateTime fecFin);
        #endregion
    }
}
