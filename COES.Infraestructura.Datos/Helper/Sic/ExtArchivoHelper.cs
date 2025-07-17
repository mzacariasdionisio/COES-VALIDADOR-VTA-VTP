using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EXT_ARCHIVO
    /// </summary>
    public class ExtArchivoHelper : HelperBase
    {
        public ExtArchivoHelper(): base(Consultas.ExtArchivoSql)
        {
        }

        public ExtArchivoDTO Create(IDataReader dr)
        {
            ExtArchivoDTO entity = new ExtArchivoDTO();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEarcopiado = dr.GetOrdinal(this.Earcopiado);
            if (!dr.IsDBNull(iEarcopiado)) entity.Earcopiado = dr.GetString(iEarcopiado);

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iEarfecha = dr.GetOrdinal(this.Earfecha);
            if (!dr.IsDBNull(iEarfecha)) entity.Earfecha = dr.GetDateTime(iEarfecha);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iEarip = dr.GetOrdinal(this.Earip);
            if (!dr.IsDBNull(iEarip)) entity.Earip = dr.GetString(iEarip);

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iEararchruta = dr.GetOrdinal(this.Eararchruta);
            if (!dr.IsDBNull(iEararchruta)) entity.Eararchruta = dr.GetString(iEararchruta);

            int iEararchver = dr.GetOrdinal(this.Eararchver);
            if (!dr.IsDBNull(iEararchver)) entity.Eararchver = dr.GetString(iEararchver);

            int iEararchtammb = dr.GetOrdinal(this.Eararchtammb);
            if (!dr.IsDBNull(iEararchtammb)) entity.Eararchtammb = dr.GetDecimal(iEararchtammb);

            int iEararchnomb = dr.GetOrdinal(this.Eararchnomb);
            if (!dr.IsDBNull(iEararchnomb)) entity.Eararchnomb = dr.GetString(iEararchnomb);

            int iEtacodi = dr.GetOrdinal(this.Etacodi);
            if (!dr.IsDBNull(iEtacodi)) entity.Etacodi = Convert.ToInt32(dr.GetValue(iEtacodi));

            int iEarcodi = dr.GetOrdinal(this.Earcodi);
            if (!dr.IsDBNull(iEarcodi)) entity.Earcodi = Convert.ToInt32(dr.GetValue(iEarcodi));


            return entity;
        }


        #region Mapeo de Campos

        public string Emprcodi = "EMPRCODI";
        public string Earcopiado = "EARCOPIADO";
        public string Estenvcodi = "ESTENVCODI";
        public string Earfecha = "EARFECHA";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Earip = "EARIP";
        public string Usercode = "USERCODE";
        public string Eararchruta = "EARARCHRUTA";
        public string Eararchver = "EARARCHVER";
        public string Eararchtammb = "EARARCHTAMMB";
        public string Eararchnomb = "EARARCHNOMB";
        public string Etacodi = "ETACODI";
        public string Earcodi = "EARCODI";


        #endregion

        public string SqlUpdateMaxId
        {
            get { return base.GetSqlXml("UpdateMaxId"); }
        }

        public string SqlListaEnvioPagInterconexion
        {
            get { return base.GetSqlXml("ListaEnvioPagInterconexion"); }
        }

        public string SqlTotalEnvioInterconexion
        {
            get { return base.GetSqlXml("TotalEnvioInterconexion"); }
        }
    }
}
