using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.SqlClient;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_AREA
    /// </summary>
    public class EqAreaHelper : HelperBase
    {
        public EqAreaHelper(): base(Consultas.EqAreaSql)
        {
        }

        public EqAreaDTO Create(IDataReader dr)
        {
            EqAreaDTO entity = new EqAreaDTO();                      

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iTareacodi = dr.GetOrdinal(this.Tareacodi);
            if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

            int iAreaabrev = dr.GetOrdinal(this.Areaabrev);
            if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

            int iAreanomb = dr.GetOrdinal(this.Areanomb);
            if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

            int iAreapadre = dr.GetOrdinal(this.Areapadre);
            if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToInt32(dr.GetValue(iAreapadre));

            int iAreaestado = dr.GetOrdinal(this.Areaestado);
            if (!dr.IsDBNull(iAreaestado)) entity.Areaestado = dr.GetString(iAreaestado);

            int iUsuariocreacion = dr.GetOrdinal(this.Usuariocreacion);
            if (!dr.IsDBNull(iUsuariocreacion)) entity.UsuarioCreacion = dr.GetString(iUsuariocreacion);

            int iFechacreacion = dr.GetOrdinal(this.Fechacreacion);
            if (!dr.IsDBNull(iFechacreacion)) entity.FechaCreacion = Convert.ToDateTime(dr.GetValue(iFechacreacion));

            int iFechaupdate = dr.GetOrdinal(this.Fechaupdate);
            if (!dr.IsDBNull(iFechaupdate)) entity.FechaUpdate = Convert.ToDateTime(dr.GetValue(iFechaupdate));

            int iUsuarioupdate = dr.GetOrdinal(this.Usuarioupdate);
            if (!dr.IsDBNull(iUsuarioupdate)) entity.UsuarioUpdate = dr.GetString(iUsuarioupdate);

            int iAnivelcodi = dr.GetOrdinal(this.Anivelcodi);
            if (!dr.IsDBNull(iAnivelcodi)) entity.ANivelCodi = Convert.ToInt32(dr.GetValue(iAnivelcodi));

            //int iTareanomb = dr.GetOrdinal(this.Tareanomb);
            //if (!dr.IsDBNull(iTareanomb)) entity.Tareanomb = dr.GetString(iTareanomb);

            //GESPROTEC - 20241029
            #region GESPROTECT
            if (validaColumna(dr, this.Zona))
            {
                int iZona = dr.GetOrdinal(this.Zona);
                if (!dr.IsDBNull(iZona)) entity.Zona = Convert.ToString(dr.GetValue(iZona));
            }

            if (validaColumna(dr, this.Areanombenprotec))
            {
                int iAreanombenprotec = dr.GetOrdinal(this.Areanombenprotec);
                if (!dr.IsDBNull(iAreanombenprotec)) entity.Areanombenprotec = Convert.ToString(dr.GetValue(iAreanombenprotec));
            }

            if (validaColumna(dr, this.Flagenprotec))
            {
                int iFlagenprotec = dr.GetOrdinal(Flagenprotec);
                if (!dr.IsDBNull(iFlagenprotec)) entity.Flagenprotec = Convert.ToString(dr.GetValue(iFlagenprotec));
            }
            #endregion

            return entity;
        }

        //GESPROTEC - 20241029
        #region GESPROTECT
        bool validaColumna(IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Mapeo de Campos

        public string Anivelcodi = "ANIVELCODI";
        public string Areacodi = "AREACODI";
        public string Tareacodi = "TAREACODI";
        public string Areaabrev = "AREAABREV";
        public string Areanomb = "AREANOMB";
        public string Areapadre = "AREAPADRE";
        public string Tareanomb = "TAREANOMB";
        public string Emprcodi = "EMPRCODI";
        public string Areaestado = "AREAESTADO";
        public string Usuariocreacion = "USUARIOCREACION";
        public string Fechacreacion = "FECHACREACION";
        public string Usuarioupdate = "USUARIOUPDATE";
        public string Fechaupdate = "FECHAUPDATE";
        public string Areadesc = "AREADESC";

        public string Tareaabrev = "TAREAABREV";

        #endregion

        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        //GESPROTEC - 20241029
        #region GESPROTEC
        public string Zona = "ZONA";
        public string Areanombenprotec = "AREANOMBENPROTEC";
        public string Flagenprotec = "FLAGENPROTEC";
        public string Epareacodi = "EPAREACODI";
        #endregion
        
        public string SqlAreasPorFiltro
        {
            get { return base.GetSqlXml("AreasPorFiltro"); }
        }

        public string SqlCantidadAreasPorFiltro
        {
            get { return base.GetSqlXml("CantidadAreasPorFiltro"); }
        }

        public string SqlObtenerAreaPorEmpresa
        {
            get { return base.GetSqlXml("ObtenerAreaPorEmpresa"); }
        }

        public string SqlListSubEstacion
        {
            get { return base.GetSqlXml("ListSubEstacion"); }
        }

        public string SqlTodasAreasActivas
        {
            get { return base.GetSqlXml("ListAreasActivas"); }
        }

        public string SqlTodasAreasActivasPorTipoArea
        {
            get { return base.GetSqlXml("ListAreasActivasTipoArea"); }
        }

        #region PR5
        public string SqlListarAreaPorEmpresas
        {
            get { return base.GetSqlXml("ListarAreaPorEmpresas"); }
        }
        #endregion

        #region ZONAS
        public string SqlListarZonasActivas
        {
            get { return base.GetSqlXml("ListarZonasActivas"); }
        }

        public string SqlListarZonasxNivel
        {
            get { return base.GetSqlXml("ListarZonasxNivel"); }
        }

        public string SqlSoloAreasActivas
        {
            get { return base.GetSqlXml("ListSoloAreas"); }
        }
        #endregion

        #region Intervenciones
        public string SqlListarComboUbicacionesXEmpresa
        {
            get { return base.GetSqlXml("ListarComboUbicacionesXEmpresa"); }
        }
        #endregion

        #region FICHA TÉCNICA
        public string SqlListarUbicacionFT
        {
            get { return base.GetSqlXml("ListarUbicacionFT"); }
        }

        #endregion

        //GESPROTEC - 20241029
        #region GESPROTEC
        public string SqlListarUbicacionCOES
        {
            get { return base.GetSqlXml("ListarUbicacionCOES"); }
        }

        #endregion
    }
}
