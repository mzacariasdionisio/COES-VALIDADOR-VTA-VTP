using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_PROPRECURSO
    /// </summary>
    public interface ICpProprecursoRepository
    {
        void Save(CpProprecursoDTO entity);
        void Update(CpProprecursoDTO entity);
        void Delete(int recurcodi, int topcodi, int propcodi, int variaccodi, DateTime fechaproprecur);
        CpProprecursoDTO GetById(int recurcodi, int propcodi, DateTime fechaproprecur, int topcodi);
        List<CpProprecursoDTO> List();
        List<CpProprecursoDTO> GetByCriteria();
        List<CpProprecursoDTO> ListarPropiedadxRecurso2(int pOrden, string catecodi, int pTopologia, int toescenario);
        List<CpProprecursoDTO> ListarPropiedadxRecursoToGams(int pOrden, string scatecodi, int pTopologia, int consideragams);
        List<CpProprecursoDTO> ListarPropiedadxRecurso(int pRecurso, int pTopologia, string sqlSoloManual);
        void CrearCopia(int topcodi1, int topcodi2);
    }
}
