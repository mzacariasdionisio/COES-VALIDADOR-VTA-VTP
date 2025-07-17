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
    public class PrnFormularelHelper : HelperBase
    {
        public PrnFormularelHelper() : base(Consultas.PrnFormularelSql)
        {
        }

        public PrnFormularelDTO Create(IDataReader dr)
        {
            PrnFormularelDTO entity = new PrnFormularelDTO();

            int iPrfrelcodi = dr.GetOrdinal(this.Prfrelcodi);
            if (!dr.IsDBNull(iPrfrelcodi)) entity.Prfrelcodi = Convert.ToInt32(dr.GetValue(iPrfrelcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iPtomedicodicalc = dr.GetOrdinal(this.Ptomedicodicalc);
            if (!dr.IsDBNull(iPtomedicodicalc)) entity.Ptomedicodicalc = Convert.ToInt32(dr.GetValue(iPtomedicodicalc));

            int iPrfrelfactor = dr.GetOrdinal(this.Prfrelfactor);
            if (!dr.IsDBNull(iPrfrelfactor)) entity.Prfrelfactor = Convert.ToInt32(dr.GetValue(iPrfrelfactor));

            int iPrfrelusucreacion = dr.GetOrdinal(this.Prfrelusucreacion);
            if (!dr.IsDBNull(iPrfrelusucreacion)) entity.Prfrelusucreacion = dr.GetString(iPrfrelusucreacion);

            int iPrfrelfeccreacion = dr.GetOrdinal(this.Prfrelfeccreacion);
            if (!dr.IsDBNull(iPrfrelfeccreacion)) entity.Prfrelfeccreacion = dr.GetDateTime(iPrfrelfeccreacion);

            int iPrfrelusumodificacion = dr.GetOrdinal(this.Prfrelusumodificacion);
            if (!dr.IsDBNull(iPrfrelusumodificacion)) entity.Prfrelusumodificacion = dr.GetString(iPrfrelusumodificacion);

            int iPrfrelfecmodificacion = dr.GetOrdinal(this.Prfrelfecmodificacion);
            if (!dr.IsDBNull(iPrfrelfecmodificacion)) entity.Prfrelfecmodificacion = dr.GetDateTime(iPrfrelfecmodificacion);

            return entity;
        }

        #region Mapeo de los campos

        public string Prfrelcodi = "PRFRELCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Areacodi = "AREACODI";
        public string Ptomedicodicalc = "PTOMEDICODICALC";
        public string Prfrelfactor = "PRFRELFACTOR";
        public string Prfrelusucreacion = "PRFRELUSUCREACION";
        public string Prfrelfeccreacion = "PRFRELFECCREACION";
        public string Prfrelusumodificacion = "PRFRELUSUMODIFICACION";
        public string Prfrelfecmodificacion = "PRFRELFECMODIFICACION";

        public string Prrulastuser = "PRRULASTUSER";
        public string Ptomedidesc = "PTOMEDIDESC";
        #endregion

        #region Consultas a la BD

        public string SqlListFormulasByUsuario
        {
            get { return base.GetSqlXml("ListFormulasByUsuario"); }
        }

        public string SqlListFormulasRelacionadas
        {
            get { return base.GetSqlXml("ListFormulasRelacionadas"); }
        }

        public string SqlDeleteByPtomedicodi
        {
            get { return base.GetSqlXml("DeleteByPtomedicodi"); }
        }

        #endregion
    }
}
