using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnPtomedpropHelper : HelperBase
    {
        public PrnPtomedpropHelper() : base(Consultas.PrnPtomedpropSql)
        {
        }

        public PrnPtomedpropDTO Create(IDataReader dr)
        {
            PrnPtomedpropDTO entity = new PrnPtomedpropDTO();
            
            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iPrnpmpvarexoproceso = dr.GetOrdinal(this.Prnpmpvarexoproceso);
            if (!dr.IsDBNull(iPrnpmpvarexoproceso)) entity.Prnpmpvarexoproceso = dr.GetString(iPrnpmpvarexoproceso);

            int iPrnpmpusucreacion = dr.GetOrdinal(this.Prnpmpusucreacion);
            if (!dr.IsDBNull(iPrnpmpusucreacion)) entity.Prnpmpusucreacion = dr.GetString(iPrnpmpusucreacion);

            int iPrnpmpfeccreacion = dr.GetOrdinal(this.Prnpmpfeccreacion);
            if (!dr.IsDBNull(iPrnpmpfeccreacion)) entity.Prnpmpfeccreacion = dr.GetDateTime(iPrnpmpfeccreacion);

            int iPrnpmpusumodificacion = dr.GetOrdinal(this.Prnpmpusumodificacion);
            if (!dr.IsDBNull(iPrnpmpusumodificacion)) entity.Prnpmpusumodificacion = dr.GetString(iPrnpmpusumodificacion);

            int iPrnpmpfecmodificacion = dr.GetOrdinal(this.Prnpmpfecmodificacion);
            if (!dr.IsDBNull(iPrnpmpfecmodificacion)) entity.Prnpmpfecmodificacion = dr.GetDateTime(iPrnpmpfecmodificacion);

            return entity;
        }

        #region Mapeo de los campos
        public string Ptomedicodi = "PTOMEDICODI";
        public string Prnpmpvarexoproceso = "PRNPMPVAREXOPROCESO";
        public string Prnpmpusucreacion = "PRNPMPUSUCREACION";
        public string Prnpmpfeccreacion = "PRNPMPFECCREACION";
        public string Prnpmpusumodificacion = "PRNPMPUSUMODIFICACION";
        public string Prnpmpfecmodificacion = "PRNPMPFECMODIFICACION";

        public string Ptomedidesc = "PTOMEDIDESC";
        public string Grupocodibarra = "GRUPOCODIBARRA";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Ptogrphijocodi = "PTOGRPHIJOCODI";
        public string Ptogrphijodesc = "PTOGRPHIJODESC";

        #endregion

        #region Consulta de BD

        public string SqlPR03Puntos
        {
            get { return base.GetSqlXml("PR03Puntos"); }
        }

        public string SqlPR03Agrupaciones
        {
            get { return base.GetSqlXml("PR03Agrupaciones"); }
        }

        #endregion
    }
}
