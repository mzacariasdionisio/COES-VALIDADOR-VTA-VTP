using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_CONCEPTO
    /// </summary>
    public class PrConceptoHelper : HelperBase
    {
        public PrConceptoHelper() : base(Consultas.PrConceptoSql)
        {
        }

        public PrConceptoDTO Create(IDataReader dr)
        {
            PrConceptoDTO entity = new PrConceptoDTO();

            int iConcepnombficha = dr.GetOrdinal(this.Concepnombficha);
            if (!dr.IsDBNull(iConcepnombficha)) entity.Concepnombficha = dr.GetString(iConcepnombficha);

            int iConcepdefinicion = dr.GetOrdinal(this.Concepdefinicion);
            if (!dr.IsDBNull(iConcepdefinicion)) entity.Concepdefinicion = dr.GetString(iConcepdefinicion);

            int iConceptipolong1 = dr.GetOrdinal(this.Conceptipolong1);
            if (!dr.IsDBNull(iConceptipolong1)) entity.Conceptipolong1 = Convert.ToInt32(dr.GetValue(iConceptipolong1));

            int iConceptipolong2 = dr.GetOrdinal(this.Conceptipolong2);
            if (!dr.IsDBNull(iConceptipolong2)) entity.Conceptipolong2 = Convert.ToInt32(dr.GetValue(iConceptipolong2));

            int iConcepusucreacion = dr.GetOrdinal(this.Concepusucreacion);
            if (!dr.IsDBNull(iConcepusucreacion)) entity.Concepusucreacion = dr.GetString(iConcepusucreacion);

            int iConcepfeccreacion = dr.GetOrdinal(this.Concepfeccreacion);
            if (!dr.IsDBNull(iConcepfeccreacion)) entity.Concepfeccreacion = dr.GetDateTime(iConcepfeccreacion);

            int iConcepusumodificacion = dr.GetOrdinal(this.Concepusumodificacion);
            if (!dr.IsDBNull(iConcepusumodificacion)) entity.Concepusumodificacion = dr.GetString(iConcepusumodificacion);

            int iConcepfecmodificacion = dr.GetOrdinal(this.Concepfecmodificacion);
            if (!dr.IsDBNull(iConcepfecmodificacion)) entity.Concepfecmodificacion = dr.GetDateTime(iConcepfecmodificacion);

            int iConcepocultocomentario = dr.GetOrdinal(this.Concepocultocomentario);
            if (!dr.IsDBNull(iConcepocultocomentario)) entity.Concepocultocomentario = dr.GetString(iConcepocultocomentario);

            int iCatecodi = dr.GetOrdinal(this.Catecodi);
            if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

            int iConcepabrev = dr.GetOrdinal(this.Concepabrev);
            if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

            int iConcepdesc = dr.GetOrdinal(this.Concepdesc);
            if (!dr.IsDBNull(iConcepdesc)) entity.Concepdesc = dr.GetString(iConcepdesc);

            int iConcepunid = dr.GetOrdinal(this.Concepunid);
            if (!dr.IsDBNull(iConcepunid)) entity.Concepunid = dr.GetString(iConcepunid);

            int iConceptipo = dr.GetOrdinal(this.Conceptipo);
            if (!dr.IsDBNull(iConceptipo)) entity.Conceptipo = dr.GetString(iConceptipo);

            int iConceporden = dr.GetOrdinal(this.Conceporden);
            if (!dr.IsDBNull(iConceporden)) entity.Conceporden = Convert.ToInt32(dr.GetValue(iConceporden));

            int iConcepfichatec = dr.GetOrdinal(this.Concepfichatec);
            if (!dr.IsDBNull(iConcepfichatec)) entity.Concepfichatec = dr.GetString(iConcepfichatec);

            int iConcepactivo = dr.GetOrdinal(this.Concepactivo);
            if (!dr.IsDBNull(iConcepactivo)) entity.Concepactivo = dr.GetString(iConcepactivo);

            //int iConceppadre = dr.GetOrdinal(this.Conceppadre);
            //if (!dr.IsDBNull(iConceppadre)) entity.Conceppadre = Convert.ToInt32(dr.GetValue(iConceppadre));

            int iConceppropeq = dr.GetOrdinal(this.Conceppropeq);
            if (!dr.IsDBNull(iConceppropeq)) entity.Conceppropeq = Convert.ToInt32(dr.GetValue(iConceppropeq));

            int iConcepliminf = dr.GetOrdinal(this.Concepliminf);
            if (!dr.IsDBNull(iConcepliminf)) entity.Concepliminf = dr.GetDecimal(iConcepliminf);

            int iConceplimsup = dr.GetOrdinal(this.Conceplimsup);
            if (!dr.IsDBNull(iConceplimsup)) entity.Conceplimsup = dr.GetDecimal(iConceplimsup);

            int iConcepflagcolor = dr.GetOrdinal(this.Concepflagcolor);
            if (!dr.IsDBNull(iConcepflagcolor)) entity.Concepflagcolor = Convert.ToInt32(dr.GetValue(iConcepflagcolor));

            return entity;
        }

        #region Mapeo de Campos

        public string Catecodi = "CATECODI";
        public string Concepcodi = "CONCEPCODI";
        public string Concepabrev = "CONCEPABREV";
        public string Concepdesc = "CONCEPDESC";
        public string Concepunid = "CONCEPUNID";
        public string Conceptipo = "CONCEPTIPO";
        public string Conceporden = "CONCEPORDEN";
        public string Concepfichatec = "CONCEPFICHATEC";
        public string Conceppropeq = "CONCEPPROPEQ";

        public string Concepactivo = "CONCEPACTIVO";
        public string Concepnombficha = "CONCEPNOMBFICHA";
        public string Concepdefinicion = "CONCEPDEFINICION";
        public string Conceptipolong1 = "CONCEPTIPOLONG1";
        public string Conceptipolong2 = "CONCEPTIPOLONG2";
        public string Concepusucreacion = "CONCEPUSUCREACION";
        public string Concepfeccreacion = "CONCEPFECCREACION";
        public string Concepusumodificacion = "CONCEPUSUMODIFICACION";
        public string Concepfecmodificacion = "CONCEPFECMODIFICACION";
        public string Concepocultocomentario = "CONCEPOCULTOCOMENTARIO";

        public string Concepliminf = "CONCEPLIMINF";
        public string Conceplimsup = "CONCEPLIMSUP";
        public string Concepflagcolor = "CONCEPFLAGCOLOR";

        #region MigracionSGOCOES-GrupoB
        public string Catenomb = "CATENOMB";
        #endregion

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListByCatecodi
        {
            get { return base.GetSqlXml("ListByCatecodi"); }
        }
        #endregion

        #region Ficha Tecnica
        public string SqlListarConceptosxFiltro
        {
            get { return base.GetSqlXml("ListarConceptosxFiltro"); }
        }
        #endregion
    }
}
