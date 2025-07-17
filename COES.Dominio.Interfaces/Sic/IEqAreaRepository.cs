using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_AREA
    /// </summary>
    public interface IEqAreaRepository
    {
        int Save(EqAreaDTO entity);
        void Update(EqAreaDTO entity);
        void Delete(int areacodi);
        void Delete_UpdateAuditoria(int areacodi, string username);
        EqAreaDTO GetById(int areacodi);
        List<EqAreaDTO> ListSubEstacion();
        List<EqAreaDTO> List();
        List<EqAreaDTO> GetByCriteria();
        List<EqAreaDTO> ListarxFiltro(int idTipoArea, string strNombreArea, string strEstado, int nroPagina, int nroFilas);
        int CantidadListarxFiltro(int idTipoArea, string strNombreArea, string strEstado);
        List<EqAreaDTO> ObtenerAreaPorEmpresa(int idEmpresa);
        List<EqAreaDTO> ListaTodasAreasActivas();
        List<EqAreaDTO> ListaTodasAreasActivasPorTipoArea(int iTipoArea);
        List<EqAreaDTO> List(int minRowToFetch, int maxRowToFetch);

        #region PR5
        List<EqAreaDTO> ListarAreaPorEmpresas(string idEmpresa, string estadoEquipo);
        #endregion

        #region ZONAS
        List<EqAreaDTO> ListarZonasActivas();
        List<EqAreaDTO> ListarZonasxNivel(int anivelcodi);
        List<EqAreaDTO> ListaSoloAreasActivas();
        List<EqAreaDTO> ListaSoloAreasActivas(int tuParametro);
        #endregion

        #region Intervenciones
        List<EqAreaDTO> ListarComboUbicacionesXEmpresa(string IdEmpresa);
        #endregion

        #region FICHA TÉCNICA

        List<EqAreaDTO> ListarUbicacionFT();

        #endregion

        //GESPROTEC - 20241029
        #region GESPROTEC
        List<EqAreaDTO> ListarUbicacionCoes(string codigoTipoArea, string nombreArea, string programaExistente);

        List<EqAreaDTO> ListAreaEquipamientoCOES();
        #endregion
    }
}
