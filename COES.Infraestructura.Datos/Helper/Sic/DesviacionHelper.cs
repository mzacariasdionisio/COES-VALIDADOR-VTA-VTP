using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DesviacionHelper : HelperBase
    {
        public DesviacionHelper()
            : base(Consultas.DatosDesviacionSql)
        {
        }

        public DesviacionDTO Create(IDataReader dr)
        {
            DesviacionDTO entity = new DesviacionDTO();

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iDesvfecha = dr.GetOrdinal(this.Desvfecha);
            if (!dr.IsDBNull(iDesvfecha)) entity.Desvfecha = dr.GetDateTime(iDesvfecha);

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iMedorigdesv = dr.GetOrdinal(this.Medorigdesv);
            if (!dr.IsDBNull(iMedorigdesv)) entity.Medorigdesv = Convert.ToInt32(dr.GetValue(iMedorigdesv));  

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }
        
        #region Mapeo de Campos

        public string Lectcodi = "LECTCODI";
        public string Desvfecha = "DESVFECHA";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Medorigdesv = "MEDORIGDESV";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Ptomedidesc = "PTOMEDIDESC";
        
        #endregion
    }
}
