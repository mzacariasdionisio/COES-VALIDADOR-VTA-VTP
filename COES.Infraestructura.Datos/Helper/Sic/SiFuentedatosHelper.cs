using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_FUENTEDATOS
    /// </summary>
    public class SiFuentedatosHelper : HelperBase
    {
        public SiFuentedatosHelper(): base(Consultas.SiFuentedatosSql)
        {
        }

        public SiFuentedatosDTO Create(IDataReader dr)
        {
            SiFuentedatosDTO entity = new SiFuentedatosDTO();

            int iFdatcodi = dr.GetOrdinal(this.Fdatcodi);
            if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

            int iFdatnombre = dr.GetOrdinal(this.Fdatnombre);
            if (!dr.IsDBNull(iFdatnombre)) entity.Fdatnombre = dr.GetString(iFdatnombre);

            int iFdattabla = dr.GetOrdinal(this.Fdattabla);
            if (!dr.IsDBNull(iFdattabla)) entity.Fdattabla = dr.GetString(iFdattabla);

            int iFdatpk = dr.GetOrdinal(this.Fdatpk);
            if (!dr.IsDBNull(iFdatpk)) entity.Fdatpk = dr.GetString(iFdatpk);

            int iFdatpadre = dr.GetOrdinal(this.Fdatpadre);
            if (!dr.IsDBNull(iFdatpadre)) entity.Fdatpadre = Convert.ToInt32(dr.GetValue(iFdatpadre));

            return entity;
        }


        #region Mapeo de Campos

        public string Fdatcodi = "FDATCODI";
        public string Fdatnombre = "FDATNOMBRE";
        public string Fdattabla = "FDATTABLA";
        public string Fdatpk = "FDATPK";
        public string Fdatpadre = "FDATPADRE";

        #endregion


        #region PR5
        public string SqlGetByModulo
        {
            get { return base.GetSqlXml("ListarXModulo"); }
        }
        #endregion
    }
}
