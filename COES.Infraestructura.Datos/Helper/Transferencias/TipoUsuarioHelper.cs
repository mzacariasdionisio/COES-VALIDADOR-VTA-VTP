using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla trn_tipo_usuario
    /// </summary>
    public class TipoUsuarioHelper : HelperBase
    {
        public TipoUsuarioHelper() : base(Consultas.TipoUsuarioSql)
        {
        }

        public TipoUsuarioDTO Create(IDataReader dr)
        {
            TipoUsuarioDTO entity = new TipoUsuarioDTO();

            int iTIPOUSUACODI = dr.GetOrdinal(this.TIPOUSUACODI);
            if (!dr.IsDBNull(iTIPOUSUACODI)) entity.TipoUsuaCodi = dr.GetInt32(iTIPOUSUACODI);

            int iTIPOUSUANOMBRE = dr.GetOrdinal(this.TIPOUSUANOMBRE);
            if (!dr.IsDBNull(iTIPOUSUANOMBRE)) entity.TipoUsuaNombre = dr.GetString(iTIPOUSUANOMBRE);

            int iTIPOUSUAESTADO = dr.GetOrdinal(this.TIPOUSUAESTADO);
            if (!dr.IsDBNull(iTIPOUSUAESTADO)) entity.TipoUsuaEstado = dr.GetString(iTIPOUSUAESTADO);

            int iTIPOUSUAUSERNAME = dr.GetOrdinal(this.TIPOUSUAUSERNAME);
            if (!dr.IsDBNull(iTIPOUSUAUSERNAME)) entity.TipoUsuaUserName = dr.GetString(iTIPOUSUAUSERNAME);

            int iTIPOUSUAFECINS = dr.GetOrdinal(this.TIPOUSUAFECINS);
            if (!dr.IsDBNull(iTIPOUSUAFECINS)) entity.TipoUsuaFecIns = dr.GetDateTime(iTIPOUSUAFECINS);

            int iTIPOUSUAFECACT = dr.GetOrdinal(this.TIPOUSUAFECACT);
            if (!dr.IsDBNull(iTIPOUSUAFECACT)) entity.TipoUsuaFecAct = dr.GetDateTime(iTIPOUSUAFECACT);

            return entity;
        }

        #region Mapeo de Campos

        public string TIPOUSUACODI = "TIPUSUCODI";
        public string TIPOUSUANOMBRE = "TIPUSUNOMBRE";
        public string TIPOUSUAESTADO = "TIPUSUESTADO";
        public string TIPOUSUAUSERNAME = "TIPUSUUSERNAME";
        public string TIPOUSUAFECINS = "TIPUSUFECINS";
        public string TIPOUSUAFECACT = "TIPUSUFECACT";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
    }
}

