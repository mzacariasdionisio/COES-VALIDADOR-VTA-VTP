using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_PROPEQUI
    /// </summary>
    public interface IEqPropequiRepository
    {
        void Save(EqPropequiDTO entity);
        void Update(EqPropequiDTO entity);
        void Delete(int propcodi, int equicodi, DateTime fechapropequi);
        void Delete_UpdateAuditoria(int propcodi, int equicodi, DateTime fechapropequi, string username);
        EqPropequiDTO GetById(int propcodi, int equicodi, DateTime fechapropequi);
        List<EqPropequiDTO> List();
        List<EqPropequiDTO> GetByCriteria();
        void EliminarHistorico(int propcodi, int equicodi);
        List<EqPropequiDTO> ListarValoresVigentesPropiedades(DateTime fechaConsulta, string iEquipo, string iFamilia, string idEmpresa, string propiedades, string sNombrePropiedad, string esFichaTecnica);
        List<EqPropequiDTO> ListarValoresVigentesPropiedadesPaginado(int iEquipo, int iFamilia, string sNombrePropiedad, int nroPagina, int nroFilas);
        int TotalListarValoresVigentesPropiedadesPaginado(int iFamilia, string sNombrePropiedad);
        List<EqPropequiDTO> ListarValoresHistoricosPropiedadPorEquipo(int iEquipo, string iPropiedad);
        string GetValorPropiedad(int idPropiedad, int idEquipo);
        string ObtenerValorPropiedadEquipoFecha(int propcodi, int equicodi, string fecha);
        void CopiarValoresPropiedadEquipo(int iEquipoOriginal, int iEquipoDestino, string usuario);

        #region SIOSEIN
        List<EqPropequiDTO> GetPotEfectivaAndPotInstaladaPorUnidad(string strCodEquipos);
        #endregion
        #region NotificacionesCambiosEquipamiento
        /// <summary>
        /// Método que devuelve el listado de Propiedades con datos de Equipo, Empresa y valor que fueron modificados durante el periodo consultado
        /// </summary>
        /// <param name="dtFechaInicio">Fecha de Inicio</param>
        /// <param name="dtFechaFin">Fecha de Fin</param>
        /// <returns>Listado de Propiedades modificadas</returns>
        List<EqPropequiDTO> ListadoPropiedadesValoresModificados(int emprcodi, int famcodi, DateTime dtFechaInicio, DateTime dtFechaFin);
        #endregion

        #region Numerales Datos Base
        List<EqPropequiDTO> ListaNumerales_DatosBase_5_6_5();
        #endregion

        #region FICHA TÉCNICA
        void Save(EqPropequiDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(EqPropequiDTO entity, IDbConnection connection, IDbTransaction transaction);

        List<EqPropequiDTO> ListarEquipoConValorModificado(DateTime dtFechaInicio, DateTime dtFechaFin, string propcodis, string famcodis);

        #endregion

        #region GestProtect
        EqPropequiDTO GetIdCambioEstado(int equicodi);
        string SaveCambioEstado(EqPropequiDTO entity);
        void UpdateCambioEstado(EqPropequiDTO entity);

        #endregion
    }
}
