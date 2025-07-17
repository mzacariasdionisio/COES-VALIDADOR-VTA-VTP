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
    public class PrnConfiguracionDiaHelper : HelperBase
    {
        public PrnConfiguracionDiaHelper() : base(Consultas.PrnConfiguracionDiaSql)
        {
        }

        public PrnConfiguracionDiaDTO Create(IDataReader dr)
        {
            PrnConfiguracionDiaDTO entity = new PrnConfiguracionDiaDTO();

            int iCnfdiacodi = dr.GetOrdinal(this.Cnfdiacodi);
            if (!dr.IsDBNull(iCnfdiacodi)) entity.Cnfdiacodi = Convert.ToInt32(dr.GetValue(iCnfdiacodi));

            int iCnfdiafecha = dr.GetOrdinal(this.Cnfdiafecha);
            if (!dr.IsDBNull(iCnfdiafecha)) entity.Cnfdiafecha = dr.GetDateTime(iCnfdiafecha);

            int iCnfdiaferiado = dr.GetOrdinal(this.Cnfdiaferiado);
            if (!dr.IsDBNull(iCnfdiaferiado)) entity.Cnfdiaferiado = dr.GetString(iCnfdiaferiado);

            int iCnfdiaatipico = dr.GetOrdinal(this.Cnfdiaatipico);
            if (!dr.IsDBNull(iCnfdiaatipico)) entity.Cnfdiaatipico = dr.GetString(iCnfdiaatipico);

            int iCnfdiaveda = dr.GetOrdinal(this.Cnfdiaveda);
            if (!dr.IsDBNull(iCnfdiaveda)) entity.Cnfdiaveda = dr.GetString(iCnfdiaveda);

            return entity;
        }

        #region Mapeo de los campos
        public string Cnfdiacodi = "CNFDIACODI";
        public string Cnfdiafecha = "CNFDIAFECHA";
        public string Cnfdiaferiado = "CNFDIAFERIADO";
        public string Cnfdiaatipico = "CNFDIAATIPICO";
        public string Cnfdiaveda = "CNFDIAVEDA";
        #endregion

        #region Consultas a la BD 
        public string SqlObtenerPorRango
        {
            get { return base.GetSqlXml("ObtenerPorRango"); }
        }
        #endregion



    }
}
