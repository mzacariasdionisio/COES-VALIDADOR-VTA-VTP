using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnConfiguracionHelper : HelperBase
    {
        public PrnConfiguracionHelper() : base(Consultas.PrnConfiguracionSql)
        {
        }

        public PrnConfiguracionDTO Create(IDataReader dr)
        {
            PrnConfiguracionDTO entity = new PrnConfiguracionDTO();

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
            
            int iPrncfgfecha = dr.GetOrdinal(this.Prncfgfecha);
            if (!dr.IsDBNull(iPrncfgfecha)) entity.Prncfgfecha = dr.GetDateTime(iPrncfgfecha);

            int iPrncfgporcerrormin = dr.GetOrdinal(this.Prncfgporcerrormin);
            if (!dr.IsDBNull(iPrncfgporcerrormin)) entity.Prncfgporcerrormin = Convert.ToDecimal(dr.GetValue(iPrncfgporcerrormin));

            int iPrncfgporcerrormax = dr.GetOrdinal(this.Prncfgporcerrormax);
            if (!dr.IsDBNull(iPrncfgporcerrormax)) entity.Prncfgporcerrormax = Convert.ToDecimal(dr.GetValue(iPrncfgporcerrormax));

            int iPrncfgmagcargamin = dr.GetOrdinal(this.Prncfgmagcargamin);
            if (!dr.IsDBNull(iPrncfgmagcargamin)) entity.Prncfgmagcargamin = Convert.ToDecimal(dr.GetValue(iPrncfgmagcargamin));

            int iPrncfgmagcargamax = dr.GetOrdinal(this.Prncfgmagcargamax);
            if (!dr.IsDBNull(iPrncfgmagcargamax)) entity.Prncfgmagcargamax = Convert.ToDecimal(dr.GetValue(iPrncfgmagcargamax));

            int iPrncfgporcdsvptrn = dr.GetOrdinal(this.Prncfgporcdsvptrn);
            if (!dr.IsDBNull(iPrncfgporcdsvptrn)) entity.Prncfgporcdsvptrn = Convert.ToDecimal(dr.GetValue(iPrncfgporcdsvptrn));

            int iPrncfgporcmuestra = dr.GetOrdinal(this.Prncfgporcmuestra);
            if (!dr.IsDBNull(iPrncfgporcmuestra)) entity.Prncfgporcmuestra = Convert.ToDecimal(dr.GetValue(iPrncfgporcmuestra));

            int iPrncfgporcdsvcnsc = dr.GetOrdinal(this.Prncfgporcdsvcnsc);
            if (!dr.IsDBNull(iPrncfgporcdsvcnsc)) entity.Prncfgporcdsvcnsc = Convert.ToDecimal(dr.GetValue(iPrncfgporcdsvcnsc));

            int iPrncfgnrocoincidn = dr.GetOrdinal(this.Prncfgnrocoincidn);
            if (!dr.IsDBNull(iPrncfgnrocoincidn)) entity.Prncfgnrocoincidn = Convert.ToDecimal(dr.GetValue(iPrncfgnrocoincidn));

            int iPrncfgflagveda = dr.GetOrdinal(this.Prncfgflagveda);
            if (!dr.IsDBNull(iPrncfgflagveda)) entity.Prncfgflagveda = dr.GetString(iPrncfgflagveda);

            int iPrncfgflagferiado = dr.GetOrdinal(this.Prncfgflagferiado);
            if (!dr.IsDBNull(iPrncfgflagferiado)) entity.Prncfgflagferiado = dr.GetString(iPrncfgflagferiado);

            int iPrncfgflagatipico = dr.GetOrdinal(this.Prncfgflagatipico);
            if (!dr.IsDBNull(iPrncfgflagatipico)) entity.Prncfgflagatipico = dr.GetString(iPrncfgflagatipico);

            int iPrncfgflagdepauto = dr.GetOrdinal(this.Prncfgflagdepauto);
            if (!dr.IsDBNull(iPrncfgflagdepauto)) entity.Prncfgflagdepauto = dr.GetString(iPrncfgflagdepauto);

            int iPrncfgtipopatron = dr.GetOrdinal(this.Prncfgtipopatron);
            if (!dr.IsDBNull(iPrncfgtipopatron)) entity.Prncfgtipopatron = dr.GetString(iPrncfgtipopatron);

            int iPrncfgnumdiapatron = dr.GetOrdinal(this.Prncfgnumdiapatron);
            if (!dr.IsDBNull(iPrncfgnumdiapatron)) entity.Prncfgnumdiapatron = Convert.ToInt32(dr.GetValue(iPrncfgnumdiapatron));

            int iPrncfgflagdefecto = dr.GetOrdinal(this.Prncfgflagdefecto);
            if (!dr.IsDBNull(iPrncfgflagdefecto)) entity.Prncfgflagdefecto = dr.GetString(iPrncfgflagdefecto);

            //08032020
            int iPrncfgpse = dr.GetOrdinal(this.Prncfgpse);
            if (!dr.IsDBNull(iPrncfgpse)) entity.Prncfgpse = Convert.ToDecimal(dr.GetValue(iPrncfgpse));

            int iPrncfgfactorf = dr.GetOrdinal(this.Prncfgfactorf);
            if (!dr.IsDBNull(iPrncfgfactorf)) entity.Prncfgfactorf = Convert.ToDecimal(dr.GetValue(iPrncfgfactorf));
            //

            int iPrncfgusucreacion = dr.GetOrdinal(this.Prncfgusucreacion);
            if (!dr.IsDBNull(iPrncfgusucreacion)) entity.Prncfgusucreacion = dr.GetString(iPrncfgusucreacion);

            int iPrncfgfeccreacion = dr.GetOrdinal(this.Prncfgfeccreacion);
            if (!dr.IsDBNull(iPrncfgfeccreacion)) entity.Prncfgfeccreacion = dr.GetDateTime(iPrncfgfeccreacion);

            int iPrncfgusumodificacion = dr.GetOrdinal(this.Prncfgusumodificacion);
            if (!dr.IsDBNull(iPrncfgusumodificacion)) entity.Prncfgusumodificacion = dr.GetString(iPrncfgusumodificacion);

            int iPrncfgfecmodificacion = dr.GetOrdinal(this.Prncfgfecmodificacion);
            if (!dr.IsDBNull(iPrncfgfecmodificacion)) entity.Prncfgfecmodificacion = dr.GetDateTime(iPrncfgfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Ptomedicodi = "PTOMEDICODI";
        public string Prncfgfecha = "PRNCFGFECHA";
        public string Prncfgporcerrormin = "PRNCFGPORCERRORMIN";
        public string Prncfgporcerrormax = "PRNCFGPORCERRORMAX";
        public string Prncfgmagcargamin = "PRNCFGMAGCARGAMIN";
        public string Prncfgmagcargamax = "PRNCFGMAGCARGAMAX";
        public string Prncfgporcdsvptrn = "PRNCFGPORCDSVPTRN";
        public string Prncfgporcmuestra = "PRNCFGPORCMUESTRA";
        public string Prncfgporcdsvcnsc = "PRNCFGPORCDSVCNSC";
        public string Prncfgnrocoincidn = "PRNCFGNROCOINCIDN";
        public string Prncfgflagveda = "PRNCFGFLAGVEDA";
        public string Prncfgflagferiado = "PRNCFGFLAGFERIADO";
        public string Prncfgflagatipico = "PRNCFGFLAGATIPICO";
        public string Prncfgflagdepauto = "PRNCFGFLAGDEPAUTO";
        public string Prncfgtipopatron = "PRNCFGTIPOPATRON";
        public string Prncfgnumdiapatron = "PRNCFGNUMDIAPATRON";
        public string Prncfgusucreacion = "PRNCFGUSUCREACION";
        public string Prncfgfeccreacion = "PRNCFGFECCREACION";
        public string Prncfgusumodificacion = "PRNCFGUSUMODIFICACION";
        public string Prncfgfecmodificacion = "PRNCFGFECMODIFICACION";
        public string Prncfgflagdefecto = "PRNCFGFLAGDEFECTO";
        //08032020
        public string Prncfgpse = "PRNCFGPSE";
        public string Prncfgfactorf = "PRNCFGFACTORF";

        //Campos extra
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Prncfgtiporeg = "PRNCFGTIPOREG";

        //Para el BulkInsert
        public string TableName = "PRN_CONFIGURACION";

        //Formatos
        public string FormatoFecha = "dd/MM/yyyy";
        #endregion

        #region Consultas a la BD

        public string SqlCountMedicionesByRangoFechas //NO EXISTE
        {
            get { return base.GetSqlXml("CountMedicionesByRangoFechas"); }
        }

        public string SqlParametrosList
        {
            get { return base.GetSqlXml("ParametrosList"); }
        }

        public string SqlGetConfiguracion
        {
            get { return base.GetSqlXml("GetConfiguracion"); }
        }

        #endregion
    }
}
