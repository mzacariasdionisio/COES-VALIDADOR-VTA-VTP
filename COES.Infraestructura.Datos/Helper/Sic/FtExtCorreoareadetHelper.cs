using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_CORREOAREADET
    /// </summary>
    public class FtExtCorreoareadetHelper : HelperBase
    {
        public FtExtCorreoareadetHelper(): base(Consultas.FtExtCorreoareadetSql)
        {
        }

        public FtExtCorreoareadetDTO Create(IDataReader dr)
        {
            FtExtCorreoareadetDTO entity = new FtExtCorreoareadetDTO();

            int iFaremcodi = dr.GetOrdinal(this.Faremcodi);
            if (!dr.IsDBNull(iFaremcodi)) entity.Faremcodi = Convert.ToInt32(dr.GetValue(iFaremcodi));

            int iFaremdcodi = dr.GetOrdinal(this.Faremdcodi);
            if (!dr.IsDBNull(iFaremdcodi)) entity.Faremdcodi = Convert.ToInt32(dr.GetValue(iFaremdcodi));

            int iFaremdemail = dr.GetOrdinal(this.Faremdemail);
            if (!dr.IsDBNull(iFaremdemail)) entity.Faremdemail = dr.GetString(iFaremdemail);

            int iFaremduserlogin = dr.GetOrdinal(this.Faremduserlogin);
            if (!dr.IsDBNull(iFaremduserlogin)) entity.Faremduserlogin = dr.GetString(iFaremduserlogin);

            int iFaremdestado = dr.GetOrdinal(this.Faremdestado);
            if (!dr.IsDBNull(iFaremdestado)) entity.Faremdestado = dr.GetString(iFaremdestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Faremcodi = "FAREMCODI";
        public string Faremdcodi = "FAREMDCODI";
        public string Faremdemail = "FAREMDEMAIL";
        public string Faremduserlogin = "FAREMDUSERLOGIN";
        public string Faremdestado = "FAREMDESTADO";

        public string Faremnombre = "FAREMNOMBRE";
        
        #endregion

        public string SqlListarCorreosPorArea
        {
            get { return base.GetSqlXml("ListarCorreosPorArea"); }
        }

        public string SqlListarPorCorreo
        {
            get { return base.GetSqlXml("ListarPorCorreo"); }
        }
        
    }
}
