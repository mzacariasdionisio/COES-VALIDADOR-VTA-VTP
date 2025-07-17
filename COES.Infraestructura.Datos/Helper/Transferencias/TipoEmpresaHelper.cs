using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la vista vw_si_tipoempresa
    /// </summary>
   public class TipoEmpresaHelper: HelperBase
    {
       public TipoEmpresaHelper() : base(Consultas.TipoEmpresaSql)
       {
       }
       public TipoEmpresaDTO Create(IDataReader dr)
        {
            TipoEmpresaDTO entity = new TipoEmpresaDTO();

            int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            if (!dr.IsDBNull(iTipoemprcodi)) entity.TipoEmprCodi = dr.GetInt32(iTipoemprcodi);

            int iTipoemprdesc = dr.GetOrdinal(this.Tipoemprdesc);
            if (!dr.IsDBNull(iTipoemprdesc)) entity.TipoEmprDesc = dr.GetString(iTipoemprdesc);
         
            return entity;
        }

        #region Mapeo de Campos

        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
    }
}
