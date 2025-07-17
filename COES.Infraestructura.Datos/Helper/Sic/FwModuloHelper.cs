using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class FwModuloHelper : HelperBase
    {
        public FwModuloHelper()
            : base(Consultas.FwModuloSql)
        {
        }

        public FwModuloDTO Create(IDataReader dr)
        {
            FwModuloDTO entity = new FwModuloDTO();

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iModnombre = dr.GetOrdinal(this.Modnombre);
            if (!dr.IsDBNull(iModnombre)) entity.Modnombre = dr.GetString(iModnombre);

            int iModestado = dr.GetOrdinal(this.Modestado);
            if (!dr.IsDBNull(iModestado)) entity.Modestado = dr.GetString(iModestado);

            int iRolcode = dr.GetOrdinal(this.Rolcode);
            if (!dr.IsDBNull(iRolcode)) entity.Rolcode = Convert.ToInt32(dr.GetValue(iRolcode));

            int iPathfile = dr.GetOrdinal(this.Pathfile);
            if (!dr.IsDBNull(iPathfile)) entity.Pathfile = dr.GetString(iPathfile);

            int iFuenterepositorio = dr.GetOrdinal(this.Fuenterepositorio);
            if (!dr.IsDBNull(iFuenterepositorio)) entity.Fuenterepositorio = dr.GetString(iFuenterepositorio);

            int iUsermanual = dr.GetOrdinal(this.Usermanual);
            if (!dr.IsDBNull(iUsermanual)) entity.Usermanual = dr.GetString(iUsermanual);

            int iInddefecto = dr.GetOrdinal(this.Inddefecto);
            if (!dr.IsDBNull(iInddefecto)) entity.Inddefecto = dr.GetString(iInddefecto);

            return entity;
        }

        #region Mapeo de Campos

        public string Modcodi = "MODCODI";
        public string Modnombre = "MODNOMBRE";
        public string Modestado = "MODESTADO";
        public string Rolcode = "ROLCODE";
        public string Pathfile = "PATHFILE";
        public string Fuenterepositorio = "FUENTEREPOSITORIO";
        public string Usermanual = "USERMANUAL";
        public string Inddefecto = "INDDEFECTO";

        #endregion
    }
}
