using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_PEAJE_EMPRESA_AJUSTE
    /// </summary>
    public class VtpPeajeEmpresaAjusteHelper : HelperBase
    {
        public VtpPeajeEmpresaAjusteHelper() : base(Consultas.VtpPeajeEmpresaAjusteSql)
        {
        }

        public VtpPeajeEmpresaAjusteDTO Create(IDataReader dr)
        {
            VtpPeajeEmpresaAjusteDTO entity = new VtpPeajeEmpresaAjusteDTO();

            int iPempajcodi = dr.GetOrdinal(this.Pempajcodi);
            if (!dr.IsDBNull(iPempajcodi)) entity.Pempajcodi = Convert.ToInt32(dr.GetValue(iPempajcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iEmprcodipeaje = dr.GetOrdinal(this.Emprcodipeaje);
            if (!dr.IsDBNull(iEmprcodipeaje)) entity.Emprcodipeaje = Convert.ToInt32(dr.GetValue(iEmprcodipeaje));

            int iPingcodi = dr.GetOrdinal(this.Pingcodi);
            if (!dr.IsDBNull(iPingcodi)) entity.Pingcodi = Convert.ToInt32(dr.GetValue(iPingcodi));

            int iEmprcodicargo = dr.GetOrdinal(this.Emprcodicargo);
            if (!dr.IsDBNull(iEmprcodicargo)) entity.Emprcodicargo = Convert.ToInt32(dr.GetValue(iEmprcodicargo));

            int iPempajajuste = dr.GetOrdinal(this.Pempajajuste);
            if (!dr.IsDBNull(iPempajajuste)) entity.Pempajajuste = dr.GetDecimal(iPempajajuste);

            int iPempajusucreacion = dr.GetOrdinal(this.Pempajusucreacion);
            if (!dr.IsDBNull(iPempajusucreacion)) entity.Pempajusucreacion = dr.GetString(iPempajusucreacion);

            int iPempajfeccreacion = dr.GetOrdinal(this.Pempajfeccreacion);
            if (!dr.IsDBNull(iPempajfeccreacion)) entity.Pempajfeccreacion = dr.GetDateTime(iPempajfeccreacion);

            return entity;
        }
        
        #region Mapeo de Campos

        public string Pempajcodi = "PEMPAJCODI";
        public string Pericodi = "PERICODI";
        public string Emprcodipeaje = "EMPRCODIPEAJE";
        public string Pingcodi = "PINGCODI";
        public string Emprcodicargo = "EMPRCODICARGO";
        public string Pempajajuste = "PEMPAJAJUSTE";
        public string Pempajusucreacion = "PEMPAJUSUCREACION";
        public string Pempajfeccreacion = "PEMPAJFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnombpeaje = "EMPRNOMBPEAJE";
        public string Emprnombcargo = "EMPRNOMBCARGO";
        public string Pingnombre = "PINGNOMBRE";

        #endregion

        public string SqlDeleteByCriteria 
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlGetAjuste
        {
            get { return base.GetSqlXml("GetAjuste"); }
        }
    }
}
