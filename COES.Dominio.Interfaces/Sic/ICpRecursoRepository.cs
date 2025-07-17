using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_RECURSO
    /// </summary>
    public interface ICpRecursoRepository
    {
        void Save(CpRecursoDTO entity);
        void Update(CpRecursoDTO entity);
        void Delete(int topcodi, int recurcodi);
        CpRecursoDTO GetById(int topcodi, int recurcodi);
        List<CpRecursoDTO> GetByCriteria();
        List<CpRecursoDTO> ObtenerPorTopologiaYCategoria(int topcodi, string propcodi);
        List<CpRecursoDTO> ObtenerListaRelacionBarraCentral(int topcodi);
        CpRecursoDTO GetByCriteria(int topcodi, int catcodi, int recurcodisicoes);

        //Yupana
        List<CpRecursoDTO> ListaUrsEmpresaAnexo5(int catcodi, int catecodiGrupo, int topcodi);

        #region Yupana Continuo
        List<CpRecursoDTO> ListarRecursoPorTopologia(int topcodi);
        List<CpCategoriaDTO> ListaCategoria();
        List<CpRecursoDTO> ListarLinea(int tipoRecurso, int pTopologiaID);
        List<CpRecursoDTO> ListaRecursoXCategoria(int tipoRecurso, int pTopologiaID);
        List<CpRecursoDTO> List(int topcodi, string codigos);
        List<CpRecursoDTO> ListaRecursoXCategoriaInNodoT(int tipoRecurso, int pTopologiaID, int ttermcodi);
        List<CpRecursoDTO> ListaModoInNodoT(int tipoRecurso, int pTopologiaID, int ttermcodi);
        List<CpRecursoDTO> ListarEquiposConecANodoTop(int topcodi);
        void CrearCopia(int topcodi1, int topcodi2);
        #endregion

        #region CMgCP_PR07

        List<CpRecursoDTO> ObtenerEmbalsesYupana();

        #endregion
    }
}
