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
    public class VceCostoVariableHelper : HelperBase
    {
        public VceCostoVariableHelper() : base(Consultas.VceCostoVariableSql)
        {
        }

        public VceCostoVariableDTO Create(IDataReader dr)
        {
            VceCostoVariableDTO entity = new VceCostoVariableDTO();

            int iCrcvcvtbajaefic = dr.GetOrdinal(this.Crcvcvtbajaefic);
            if (!dr.IsDBNull(iCrcvcvtbajaefic)) entity.Crcvcvtbajaefic = dr.GetDecimal(iCrcvcvtbajaefic);

            int iCrcvcvcbajaefic = dr.GetOrdinal(this.Crcvcvcbajaefic);
            if (!dr.IsDBNull(iCrcvcvcbajaefic)) entity.Crcvcvcbajaefic = dr.GetDecimal(iCrcvcvcbajaefic);

            int iCrcvconsumobajaefic = dr.GetOrdinal(this.Crcvconsumobajaefic);
            if (!dr.IsDBNull(iCrcvconsumobajaefic)) entity.Crcvconsumobajaefic = dr.GetDecimal(iCrcvconsumobajaefic);

            int iCrcvpotenciabajaefic = dr.GetOrdinal(this.Crcvpotenciabajaefic);
            if (!dr.IsDBNull(iCrcvpotenciabajaefic)) entity.Crcvpotenciabajaefic = dr.GetDecimal(iCrcvpotenciabajaefic);

            int iCrcvaplicefectiva = dr.GetOrdinal(this.Crcvaplicefectiva);
            if (!dr.IsDBNull(iCrcvaplicefectiva)) entity.Crcvaplicefectiva = dr.GetString(iCrcvaplicefectiva);

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCrcvcvt = dr.GetOrdinal(this.Crcvcvt);
            if (!dr.IsDBNull(iCrcvcvt)) entity.Crcvcvt = dr.GetDecimal(iCrcvcvt);

            int iCrcvcvnc = dr.GetOrdinal(this.Crcvcvnc);
            if (!dr.IsDBNull(iCrcvcvnc)) entity.Crcvcvnc = dr.GetDecimal(iCrcvcvnc);

            int iCrcvcvc = dr.GetOrdinal(this.Crcvcvc);
            if (!dr.IsDBNull(iCrcvcvc)) entity.Crcvcvc = dr.GetDecimal(iCrcvcvc);

            int iCrcvprecioaplic = dr.GetOrdinal(this.Crcvprecioaplic);
            if (!dr.IsDBNull(iCrcvprecioaplic)) entity.Crcvprecioaplic = dr.GetDecimal(iCrcvprecioaplic);

            int iCrcvconsumo = dr.GetOrdinal(this.Crcvconsumo);
            if (!dr.IsDBNull(iCrcvconsumo)) entity.Crcvconsumo = dr.GetDecimal(iCrcvconsumo);

            int iCrcvpotencia = dr.GetOrdinal(this.Crcvpotencia);
            if (!dr.IsDBNull(iCrcvpotencia)) entity.Crcvpotencia = dr.GetDecimal(iCrcvpotencia);

            int iCrcvfecmod = dr.GetOrdinal(this.Crcvfecmod);
            if (!dr.IsDBNull(iCrcvfecmod)) entity.Crcvfecmod = dr.GetDateTime(iCrcvfecmod);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Crcvcvtbajaefic = "CRCVCVTBAJAEFIC";
        public string Crcvcvcbajaefic = "CRCVCVCBAJAEFIC";
        public string Crcvconsumobajaefic = "CRCVCONSUMOBAJAEFIC";
        public string Crcvpotenciabajaefic = "CRCVPOTENCIABAJAEFIC";
        public string Crcvaplicefectiva = "CRCVAPLICEFECTIVA";
        public string Barrcodi = "BARRCODI";
        public string Crcvcvt = "CRCVCVT";
        public string Crcvcvnc = "CRCVCVNC";
        public string Crcvcvc = "CRCVCVC";
        public string Crcvprecioaplic = "CRCVPRECIOAPLIC";
        public string Crcvconsumo = "CRCVCONSUMO";
        public string Crcvpotencia = "CRCVPOTENCIA";
        public string Crcvfecmod = "CRCVFECMOD";
        public string Grupocodi = "GRUPOCODI";
        public string Pecacodi = "PECACODI";

        //Adicional

        public string Gruponomb = "GRUPONOMB";
        public string Dia = "DIA";
        public string Crdcgprecioaplicunid = "CRDCGPRECIOAPLICUNID";
        public string Barrbarratransferencia = "BARRBARRATRANSFERENCIA";

        #endregion

        public string SqlListCostoVariable
        {
            get { return base.GetSqlXml("ListCostoVariable"); }
        }

        public string SqlSaveFromOtherVersion
        {
            get { return base.GetSqlXml("SaveFromOtherVersion"); }
        }

        public string SqlDeleteByVersion
        {
            get { return base.GetSqlXml("DeleteByVersion"); }
        }
    }
}
