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
    public class VceHoraOperacionHelper  : HelperBase
    {
        public VceHoraOperacionHelper() : base(Consultas.VceHoraOperacionSql)
        {
        }

        public VceHoraOperacionDTO Create(IDataReader dr)
        {
            VceHoraOperacionDTO entity = new VceHoraOperacionDTO();

            int iCrhophorfinajust = dr.GetOrdinal(this.Crhophorfinajust);
            if (!dr.IsDBNull(iCrhophorfinajust)) entity.Crhophorfinajust = dr.GetDateTime(iCrhophorfinajust);

            int iCrhophoriniajust = dr.GetOrdinal(this.Crhophoriniajust);
            if (!dr.IsDBNull(iCrhophoriniajust)) entity.Crhophoriniajust = dr.GetDateTime(iCrhophoriniajust);

            int iCrhopcompordpard = dr.GetOrdinal(this.Crhopcompordpard);
            if (!dr.IsDBNull(iCrhopcompordpard)) entity.Crhopcompordpard = dr.GetString(iCrhopcompordpard);

            int iCrhopcompordarrq = dr.GetOrdinal(this.Crhopcompordarrq);
            if (!dr.IsDBNull(iCrhopcompordarrq)) entity.Crhopcompordarrq = dr.GetString(iCrhopcompordarrq);

            int iCrhopdesc = dr.GetOrdinal(this.Crhopdesc);
            if (!dr.IsDBNull(iCrhopdesc)) entity.Crhopdesc = dr.GetString(iCrhopdesc);

            int iCrhopcausacodi = dr.GetOrdinal(this.Crhopcausacodi);
            if (!dr.IsDBNull(iCrhopcausacodi)) entity.Crhopcausacodi = Convert.ToInt32(dr.GetValue(iCrhopcausacodi));

            int iCrhoplimtrans = dr.GetOrdinal(this.Crhoplimtrans);
            if (!dr.IsDBNull(iCrhoplimtrans)) entity.Crhoplimtrans = dr.GetString(iCrhoplimtrans);

            int iCrhopsaislado = dr.GetOrdinal(this.Crhopsaislado);
            if (!dr.IsDBNull(iCrhopsaislado)) entity.Crhopsaislado = Convert.ToInt32(dr.GetValue(iCrhopsaislado));

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iCrhophorparada = dr.GetOrdinal(this.Crhophorparada);
            if (!dr.IsDBNull(iCrhophorparada)) entity.Crhophorparada = dr.GetDateTime(iCrhophorparada);

            int iCrhophorarranq = dr.GetOrdinal(this.Crhophorarranq);
            if (!dr.IsDBNull(iCrhophorarranq)) entity.Crhophorarranq = dr.GetDateTime(iCrhophorarranq);

            int iCrhophorfin = dr.GetOrdinal(this.Crhophorfin);
            if (!dr.IsDBNull(iCrhophorfin)) entity.Crhophorfin = dr.GetDateTime(iCrhophorfin);

            int iCrhophorini = dr.GetOrdinal(this.Crhophorini);
            if (!dr.IsDBNull(iCrhophorini)) entity.Crhophorini = dr.GetDateTime(iCrhophorini);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iHopcodi = dr.GetOrdinal(this.Hopcodi);
            if (!dr.IsDBNull(iHopcodi)) entity.Hopcodi = Convert.ToInt32(dr.GetValue(iHopcodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Crhophorfinajust = "CRHOPHORFINAJUST";
        public string Crhophoriniajust = "CRHOPHORINIAJUST";
        public string Crhopcompordpard = "CRHOPCOMPORDPARD";
        public string Crhopcompordarrq = "CRHOPCOMPORDARRQ";
        public string Crhopdesc = "CRHOPDESC";
        public string Crhopcausacodi = "CRHOPCAUSACODI";
        public string Crhoplimtrans = "CRHOPLIMTRANS";
        public string Crhopsaislado = "CRHOPSAISLADO";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Crhophorparada = "CRHOPHORPARADA";
        public string Crhophorarranq = "CRHOPHORARRANQ";
        public string Crhophorfin = "CRHOPHORFIN";
        public string Crhophorini = "CRHOPHORINI";
        public string Grupocodi = "GRUPOCODI";
        public string Hopcodi = "HOPCODI";
        public string Pecacodi = "PECACODI";

        //Adicionales
        public string Empresa = "EMPRESA";
        public string Central = "CENTRAL";
        public string Grupo = "GRUPO";
        public string ModoOperacion = "MODO_OPERACION";
        public string TipoOperacion = "TIPO_OPERACION";
        public string Hopcodi2 = "HOPCODI2";
        public string Crhophorfin2 = "CRHOPHORFIN2";
        public string Crhophorini2 = "CRHOPHORINI2";
        public string TipoOperacion2 = "TIPO_OPERACION2";
      
        #endregion


        public string SqlListFiltro
        {
            get { return base.GetSqlXml("ListFiltro"); }
        }

        public string SqlSaveByRango
        {
            get { return base.GetSqlXml("SaveByRango"); }
        }

        public string SqlSaveFromOtherVersion
        {
            get { return base.GetSqlXml("SaveFromOtherVersion"); }
        }

        public string SqlDeleteById
        {
            get { return base.GetSqlXml("DeleteById"); }
        }

        public string SqlUpdateRangoHora
        {
            get { return base.GetSqlXml("UpdateRangoHora"); }
        }

        public string SqlListById
        {
            get { return base.GetSqlXml("ListById"); }
        }

        public string SqlListVerificarHoras
        {
            get { return base.GetSqlXml("ListVerificarHoras"); }
        }
        public string SqlGetDataById
        {
            get { return base.GetSqlXml("GetDataById"); }
        }
    }
}
