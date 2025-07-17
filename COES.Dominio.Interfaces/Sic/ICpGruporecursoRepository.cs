using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_GRUPORECURSO
    /// </summary>
    public interface ICpGruporecursoRepository
    {
        void Save(CpGruporecursoDTO entity);
        void Update(CpGruporecursoDTO entity);
        void Delete(int topcodi, int recurcodi, int recurcodidet);
        CpGruporecursoDTO GetById(int topcodi, int recurcodi, int recurcodidet);
        List<CpGruporecursoDTO> List(int topcodi);
        List<CpGruporecursoDTO> GetByCriteria(int pRecurso, int pTopologia);
        CpGruporecursoDTO GetRelacionURSSICOES(int topcodi, int recurcodiURS);
        List<CpGruporecursoDTO> ListaGrupoPorCategoria(int categoria, int topologia);
        void CrearCopia(int topcodi1, int topcodi2);
        List<CpGruporecursoDTO> GetByCriteriaFamilia(int pCategoria, int pTopologia); // Yupana Iteracion 3
    }
}