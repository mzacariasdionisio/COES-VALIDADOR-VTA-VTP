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
    /// Clase que contiene el mapeo de la tabla trn_tipo_tramite
    /// </summary>
   public class TipoTramiteHelper:HelperBase
    {
        public TipoTramiteHelper() : base(Consultas.TipoTramiteSql)
        {
        }

        public TipoTramiteDTO Create(IDataReader dr)
        {
            TipoTramiteDTO entity = new TipoTramiteDTO();

            int iTipotramcodi = dr.GetOrdinal(this.Tipotramcodi);
            if (!dr.IsDBNull(iTipotramcodi)) entity.TipoTramCodi = dr.GetInt32(iTipotramcodi);

            int iTipotramnombre = dr.GetOrdinal(this.Tipotramnombre);
            if (!dr.IsDBNull(iTipotramnombre)) entity.TipoTramNombre = dr.GetString(iTipotramnombre);

            int iTipotramestado = dr.GetOrdinal(this.Tipotramestado);
            if (!dr.IsDBNull(iTipotramestado)) entity.TipoTramEstado = dr.GetString(iTipotramestado);

            int iTipotramusername = dr.GetOrdinal(this.Tipotramusername);
            if (!dr.IsDBNull(iTipotramusername)) entity.TipoTramUserName = dr.GetString(iTipotramusername);

            int iTipotramfecins = dr.GetOrdinal(this.Tipotramfecins);
            if (!dr.IsDBNull(iTipotramfecins)) entity.TipoTramFecIns = dr.GetDateTime(iTipotramfecins);

           
            return entity;
        }

        #region Mapeo de Campos

        public string Tipotramcodi = "TIPTRMCODI";
        public string Tipotramnombre = "TIPTRMNOMBRE";
        public string Tipotramestado = "TIPTRMESTADO";
        public string Tipotramusername = "TIPTRMUSERNAME";
        public string Tipotramfecins = "TIPTRMFECINS";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
    }
}
