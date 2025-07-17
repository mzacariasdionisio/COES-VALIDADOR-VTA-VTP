using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_EVENTOEQUIPO
    /// </summary>
    public class EveEventoEquipoHelper : HelperBase
    {
        public EveEventoEquipoHelper()
            : base(Consultas.EveEventoEquipoSql)
        {
        }

        public EveEventoEquipoDTO Create(IDataReader dr)
        {
            EveEventoEquipoDTO entity = new EveEventoEquipoDTO();

            int iEeqcodi = dr.GetOrdinal(this.Eeqcodi);
            if (!dr.IsDBNull(iEeqcodi)) entity.Eeqcodi = Convert.ToInt32(dr.GetValue(iEeqcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iEeqestado = dr.GetOrdinal(this.Eeqestado);
            if (!dr.IsDBNull(iEeqestado)) entity.Eeqestado = Convert.ToInt32(dr.GetValue(iEeqestado));

            int iEeqfechaini = dr.GetOrdinal(this.Eeqfechaini);
            if (!dr.IsDBNull(iEeqfechaini)) entity.Eeqfechaini = dr.GetDateTime(iEeqfechaini);

            int iEeqdescripcion = dr.GetOrdinal(this.Eeqdescripcion);
            if (!dr.IsDBNull(iEeqdescripcion)) entity.Eeqdescripcion = dr.GetString(iEeqdescripcion);

            int iEeqfechafin = dr.GetOrdinal(this.Eeqfechafin);
            if (!dr.IsDBNull(iEeqfechafin)) entity.Eeqfechafin = dr.GetDateTime(iEeqfechafin);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEquicodi)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iSubcausadesc = dr.GetOrdinal(this.Subcausadesc);
            if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iFamnomb = dr.GetOrdinal(this.Famnomb);
            if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);


            return entity;
        }

        #region Mapeo de Campos

        public string Eeqcodi = "EEQCODI";
        public string Equicodi = "EQUICODI";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Eeqfechaini = "EEQFECHAINI";
        public string Eeqestado = "EEQESTADO";
        public string Eeqdescripcion = "EEQDESCRIPCION";
        public string Eeqfechafin = "EEQFECHAFIN";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Subcausadesc = "SUBCAUSADESC";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";

        public string SqlObtenerEquiposPorEvento
        {
            get { return base.GetSqlXml("ObtenerEquiposPorEvento"); }
        }

        public string SqlDeleteEquipo
        {
            get { return base.GetSqlXml("DeleteEquipo"); }
        }

        public string SqlListarDetalleEquiposSEIN
        {
            get { return base.GetSqlXml("ListarDetalleEquiposSEIN"); }
        }
        public string SqlListarPendientesEquiposSEIN
        {
            get { return base.GetSqlXml("ListarPendientesEquiposSEIN"); }
        }
        public string SqlAprobarEquiposSEIN
        {
            get { return base.GetSqlXml("Aprobar"); }
        }
        public string SqlUpdEstadoEquiposSEIN
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }
        public string SqlGetPropEquiposSEIN
        {
            get { return base.GetSqlXml("GetPropCodi"); }
        }
        public string SqlInsPropEqEquiposSEIN
        {
            get { return base.GetSqlXml("InsPropEq"); }
        }
        public string SqlUpdUbicEqEquiposSEIN
        {
            get { return base.GetSqlXml("UpdUbicEq"); }
        }

        public string SqlListarDetalleEquiposSEIN02
        {
            get { return base.GetSqlXml("ListarDetalleEquiposSEIN02"); }
        }


        public string SqlListarIngresoSalidaOperacionComercialSEIN
        {
            get { return base.GetSqlXml("ListarIngresoSalidaOperacionComercialSEIN"); }
        }
        #endregion
    }
}
