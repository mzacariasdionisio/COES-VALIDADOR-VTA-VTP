using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_TIPOMENSAJE
    /// </summary>
    public class SiTipoMensajeHelper : HelperBase
    {
        public SiTipoMensajeHelper()
            : base(Consultas.SiTipoMensajeSql)
        {
        }

        public SiTipoMensajeDTO Create(IDataReader dr)
        {
            SiTipoMensajeDTO entity = new SiTipoMensajeDTO();

            int iTMsgCodi = dr.GetOrdinal(this.TMsgCodi);
            if (!dr.IsDBNull(iTMsgCodi)) entity.Tmsgcodi = Convert.ToInt32(dr.GetValue(iTMsgCodi));

            int iTMsgNombre = dr.GetOrdinal(this.TMsgNombre);
            if (!dr.IsDBNull(iTMsgNombre)) entity.Tmsgnombre = dr.GetString(iTMsgNombre);


            return entity;

        }

        #region
        public string TMsgCodi = "TMSGCODI";
        public string TMsgNombre = "TMSGNOMBRE";
        #endregion



        public string SqlListar
        {
            get { return base.GetSqlXml("Listar"); }
        }

        #region Siosein
        public string SqlListarXMod
        {
            get { return base.GetSqlXml("ListarXMod"); }
        }
        #endregion

    }
}
