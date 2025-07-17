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
    /// Clase que contiene el mapeo de la tabla VTP_SALDO_EMPRESA_AJUSTE
    /// </summary>
    public class VtpSaldoEmpresaAjusteHelper : HelperBase
    {
        public VtpSaldoEmpresaAjusteHelper() : base(Consultas.VtpSaldoEmpresaAjusteSql)
        {
        }

        public VtpSaldoEmpresaAjusteDTO Create(IDataReader dr)
        {
            VtpSaldoEmpresaAjusteDTO entity = new VtpSaldoEmpresaAjusteDTO();

            int iPotseacodi = dr.GetOrdinal(this.Potseacodi);
            if (!dr.IsDBNull(iPotseacodi)) entity.Potseacodi = Convert.ToInt32(dr.GetValue(iPotseacodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPotseaajuste = dr.GetOrdinal(this.Potseaajuste);
            if (!dr.IsDBNull(iPotseaajuste)) entity.Potseaajuste = dr.GetDecimal(iPotseaajuste);

            int iPotseausucreacion = dr.GetOrdinal(this.Potseausucreacion);
            if (!dr.IsDBNull(iPotseausucreacion)) entity.Potseausucreacion = dr.GetString(iPotseausucreacion);

            int iPotseafeccreacion = dr.GetOrdinal(this.Potseafeccreacion);
            if (!dr.IsDBNull(iPotseafeccreacion)) entity.Potseafeccreacion = dr.GetDateTime(iPotseafeccreacion);

            return entity;
        }
        
        #region Mapeo de Campos

        public string Potseacodi = "POTSEACODI";
        public string Pericodi = "PERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Potseaajuste = "POTSEAAJUSTE";
        public string Potseausucreacion = "POTSEAUSUCREACION";
        public string Potseafeccreacion = "POTSEAFECCREACION";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";

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
