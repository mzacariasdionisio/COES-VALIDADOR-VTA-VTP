using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_GRUPO_EQUIPO_VAL
    /// </summary>
    public class PrGrupoEquipoValHelper : HelperBase
    {
        public PrGrupoEquipoValHelper(): base(Consultas.PrGrupoEquipoValSql)
        {
        }

        public PrGrupoEquipoValDTO Create(IDataReader dr)
        {
            PrGrupoEquipoValDTO entity = new PrGrupoEquipoValDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGreqvafechadat = dr.GetOrdinal(this.Greqvafechadat);
            if (!dr.IsDBNull(iGreqvafechadat)) entity.Greqvafechadat = dr.GetDateTime(iGreqvafechadat);

            int iGreqvaformuladat = dr.GetOrdinal(this.Greqvaformuladat);
            if (!dr.IsDBNull(iGreqvaformuladat)) entity.Greqvaformuladat = dr.GetString(iGreqvaformuladat);

            int iGreqvadeleted = dr.GetOrdinal(this.Greqvadeleted);
            if (!dr.IsDBNull(iGreqvadeleted)) entity.Greqvadeleted = Convert.ToInt32(dr.GetValue(iGreqvadeleted));

            int iGreqvausucreacion = dr.GetOrdinal(this.Greqvausucreacion);
            if (!dr.IsDBNull(iGreqvausucreacion)) entity.Greqvausucreacion = dr.GetString(iGreqvausucreacion);

            int iGreqvafeccreacion = dr.GetOrdinal(this.Greqvafeccreacion);
            if (!dr.IsDBNull(iGreqvafeccreacion)) entity.Greqvafeccreacion = dr.GetDateTime(iGreqvafeccreacion);

            int iGreqvausumodificacion = dr.GetOrdinal(this.Greqvausumodificacion);
            if (!dr.IsDBNull(iGreqvausumodificacion)) entity.Greqvausumodificacion = dr.GetString(iGreqvausumodificacion);

            int iGreqvafecmodificacion = dr.GetOrdinal(this.Greqvafecmodificacion);
            if (!dr.IsDBNull(iGreqvafecmodificacion)) entity.Greqvafecmodificacion = dr.GetDateTime(iGreqvafecmodificacion);

            int iGreqvacomentario = dr.GetOrdinal(this.Greqvacomentario);
            if (!dr.IsDBNull(iGreqvacomentario)) entity.Greqvacomentario = dr.GetString(iGreqvacomentario);

            int iGreqvasustento = dr.GetOrdinal(this.Greqvasustento);
            if (!dr.IsDBNull(iGreqvasustento)) entity.Greqvasustento = dr.GetString(iGreqvasustento);

            int iGreqvacheckcero = dr.GetOrdinal(this.Greqvacheckcero);
            if (!dr.IsDBNull(iGreqvacheckcero)) entity.Greqvacheckcero = Convert.ToInt32(dr.GetValue(iGreqvacheckcero));

            return entity;
        }


        #region Mapeo de Campos

        public string Grupocodi = "GRUPOCODI";
        public string Concepcodi = "CONCEPCODI";
        public string Equicodi = "EQUICODI";
        public string Greqvafechadat = "GREQVAFECHADAT";
        public string Greqvaformuladat = "GREQVAFORMULADAT";
        public string Greqvadeleted = "GREQVADELETED";
        public string Greqvausucreacion = "GREQVAUSUCREACION";
        public string Greqvafeccreacion = "GREQVAFECCREACION";
        public string Greqvausumodificacion = "GREQVAUSUMODIFICACION";
        public string Greqvafecmodificacion = "GREQVAFECMODIFICACION";
        public string Conceppadre = "CONCEPPADRE";
        public string Concepabrev = "CONCEPABREV";

        public string Greqvacomentario = "GREQVACOMENTARIO";
        public string Greqvasustento = "GREQVASUSTENTO";
        public string Greqvacheckcero = "GREQVACHECKCERO";

        #region MigracionSGOCOES-GrupoB
        public string Greqvadeleted2 = "GREQVADELETED2";
        #endregion

        #endregion

        public string SqlGetValorPropiedadDetalle
        {
            get { return base.GetSqlXml("GetValorPropiedadDetalle"); }
        }

        //-- inicio pruebas aleatorias
        public string SqlGetValorPropiedadDetalleEquipo
        {
            get { return base.GetSqlXml("GetValorPropiedadDetalleEquipo"); }
        }
        //-- fin pruebas aleatorias

        public string SqlHistoricoValores
        {
            get { return base.GetSqlXml("SqlHistoricoValores"); }
        }

        public string SqlListarPrGrupoEquipoValVigente
        {
            get { return base.GetSqlXml("ListarPrGrupoEquipoValVigente"); }
        }

    }
}
