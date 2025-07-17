using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_PROPIEDAD
    /// </summary>
    public class EqPropiedadHelper : HelperBase
    {
        public EqPropiedadHelper() : base(Consultas.EqPropiedadSql)
        {
        }

        public EqPropiedadDTO Create(IDataReader dr)
        {
            EqPropiedadDTO entity = new EqPropiedadDTO();

            int iPropnombficha = dr.GetOrdinal(this.Propnombficha);
            if (!dr.IsDBNull(iPropnombficha)) entity.Propnombficha = dr.GetString(iPropnombficha);

            int iProptipolong1 = dr.GetOrdinal(this.Proptipolong1);
            if (!dr.IsDBNull(iProptipolong1)) entity.Proptipolong1 = Convert.ToInt32(dr.GetValue(iProptipolong1));

            int iProptipolong2 = dr.GetOrdinal(this.Proptipolong2);
            if (!dr.IsDBNull(iProptipolong2)) entity.Proptipolong2 = Convert.ToInt32(dr.GetValue(iProptipolong2));

            int iPropactivo = dr.GetOrdinal(this.Propactivo);
            if (!dr.IsDBNull(iPropactivo)) entity.Propactivo = Convert.ToInt32(dr.GetValue(iPropactivo));

            int iPropusucreacion = dr.GetOrdinal(this.Propusucreacion);
            if (!dr.IsDBNull(iPropusucreacion)) entity.Propusucreacion = dr.GetString(iPropusucreacion);

            int iPropfeccreacion = dr.GetOrdinal(this.Propfeccreacion);
            if (!dr.IsDBNull(iPropfeccreacion)) entity.Propfeccreacion = dr.GetDateTime(iPropfeccreacion);

            int iPropcodi = dr.GetOrdinal(this.Propcodi);
            if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iPropabrev = dr.GetOrdinal(this.Propabrev);
            if (!dr.IsDBNull(iPropabrev)) entity.Propabrev = dr.GetString(iPropabrev);

            int iPropnomb = dr.GetOrdinal(this.Propnomb);
            if (!dr.IsDBNull(iPropnomb)) entity.Propnomb = dr.GetString(iPropnomb);

            int iPropunidad = dr.GetOrdinal(this.Propunidad);
            if (!dr.IsDBNull(iPropunidad)) entity.Propunidad = dr.GetString(iPropunidad);

            int iOrden = dr.GetOrdinal(this.Orden);
            if (!dr.IsDBNull(iOrden)) entity.Orden = Convert.ToInt32(dr.GetValue(iOrden));

            int iProptipo = dr.GetOrdinal(this.Proptipo);
            if (!dr.IsDBNull(iProptipo)) entity.Proptipo = dr.GetString(iProptipo);

            int iPropdefinicion = dr.GetOrdinal(this.Propdefinicion);
            if (!dr.IsDBNull(iPropdefinicion)) entity.Propdefinicion = dr.GetString(iPropdefinicion);

            int iPropcodipadre = dr.GetOrdinal(this.Propcodipadre);
            if (!dr.IsDBNull(iPropcodipadre)) entity.Propcodipadre = Convert.ToInt32(dr.GetValue(iPropcodipadre));

            int iPropusumodificacion = dr.GetOrdinal(this.Propusumodificacion);
            if (!dr.IsDBNull(iPropusumodificacion)) entity.Propusumodificacion = dr.GetString(iPropusumodificacion);

            int iPropfecmodificacion = dr.GetOrdinal(this.Propfecmodificacion);
            if (!dr.IsDBNull(iPropfecmodificacion)) entity.Propfecmodificacion = dr.GetDateTime(iPropfecmodificacion);

            int iPropfichaoficial = dr.GetOrdinal(this.Propfichaoficial);
            if (!dr.IsDBNull(iPropfichaoficial)) entity.Propfichaoficial = dr.GetString(iPropfichaoficial);

            int iPropocultocomentario = dr.GetOrdinal(this.Propocultocomentario);
            if (!dr.IsDBNull(iPropocultocomentario)) entity.Propocultocomentario = dr.GetString(iPropocultocomentario);

            int iPropliminf = dr.GetOrdinal(this.Propliminf);
            if (!dr.IsDBNull(iPropliminf)) entity.Propliminf = dr.GetDecimal(iPropliminf);

            int iProplimsup = dr.GetOrdinal(this.Proplimsup);
            if (!dr.IsDBNull(iProplimsup)) entity.Proplimsup = dr.GetDecimal(iProplimsup);

            int iPropflagcolor = dr.GetOrdinal(this.Propflagcolor);
            if (!dr.IsDBNull(iPropflagcolor)) entity.Propflagcolor = Convert.ToInt32(dr.GetValue(iPropflagcolor));

            return entity;
        }


        #region Mapeo de Campos

        public string Propnombficha = "PROPNOMBFICHA";
        public string Proptipolong1 = "PROPTIPOLONG1";
        public string Proptipolong2 = "PROPTIPOLONG2";
        public string Propactivo = "PROPACTIVO";
        public string Propusucreacion = "PROPUSUCREACION";
        public string Propfeccreacion = "PROPFECCREACION";
        public string Propusumodificacion = "PROPUSUMODIFICACION";
        public string Propfecmodificacion = "PROPFECMODIFICACION";
        public string Propocultocomentario = "PROPOCULTOCOMENTARIO";
        public string Propfichaoficial = "PROPFICHAOFICIAL";
        public string Propcodi = "PROPCODI";
        public string Famcodi = "FAMCODI";
        public string Propabrev = "PROPABREV";
        public string Propnomb = "PROPNOMB";
        public string Propunidad = "PROPUNIDAD";
        public string Orden = "ORDEN";
        public string Proptipo = "PROPTIPO";
        public string Propdefinicion = "PROPDEFINICION";
        public string Propcodipadre = "PROPCODIPADRE";
        public string Propliminf = "PROPLIMINF";
        public string Proplimsup = "PROPLIMSUP";
        public string Propflagcolor = "PROPFLAGCOLOR";

        public string Propformula = "PROPFORMULA";


        #region MigracionSGOCOES-GrupoB
        public string Famnomb = "FAMNOMB";
        #endregion

        #endregion
        public string SqlPropiedadesPorFiltro
        {
            get { return base.GetSqlXml("PropiedadesByFiltro"); }
        }
        public string SqlCantidadPropiedadesPorFiltro
        {
            get { return base.GetSqlXml("CantidadPropiedadesByFiltro"); }
        }

        #region MigracionSGOCOES-GrupoB
        public string SqlListByFamcodi
        {
            get { return base.GetSqlXml("ListByFamcodi"); }
        }
        #endregion
        #region Ficha Tecnica 2023
        public string SqlListByIds
        {
            get { return base.GetSqlXml("ListByIds"); }
        }
        #endregion
        
    }
}
