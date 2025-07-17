using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class CrEtapaEventoHelper :HelperBase
    {
        public CrEtapaEventoHelper() : base(Consultas.CrEtapaEventoSql)
        {
        }

        public CrEtapaEventoDTO Create(IDataReader dr)
        {
            CrEtapaEventoDTO entity = new CrEtapaEventoDTO();

            int iCretapacodi = dr.GetOrdinal(this.Cretapacodi);
            if (!dr.IsDBNull(iCretapacodi)) entity.CRETAPACODI = dr.GetInt32(iCretapacodi);

            int iCrevencodi = dr.GetOrdinal(this.Crevencodi);
            if (!dr.IsDBNull(iCrevencodi)) entity.CREVENCODI = dr.GetInt32(iCrevencodi);

            int iCretapa = dr.GetOrdinal(this.Cretapa);
            if (!dr.IsDBNull(iCretapa)) entity.CRETAPA = dr.GetInt32(iCretapa);

            int iCrfechdesicion = dr.GetOrdinal(this.Crfechdesicion);
            if (!dr.IsDBNull(iCrfechdesicion)) entity.CRFECHDESICION = dr.GetDateTime(iCrfechdesicion);

            int iCreventodescripcion = dr.GetOrdinal(this.Creventodescripcion);
            if (!dr.IsDBNull(iCreventodescripcion)) entity.CREVENTODESCRIPCION = dr.GetString(iCreventodescripcion);

            int iCrresumencriterio = dr.GetOrdinal(this.Crresumencriterio);
            if (!dr.IsDBNull(iCrresumencriterio)) entity.CRRESUMENCRITERIO = dr.GetString(iCrresumencriterio);

            int iCrcomentarios_Responsables = dr.GetOrdinal(this.Crcomentarios_Responsables);
            if (!dr.IsDBNull(iCrcomentarios_Responsables)) entity.CRCOMENTARIOS_RESPONSABLES = dr.GetString(iCrcomentarios_Responsables);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.LASTDATE = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.LASTUSER = dr.GetString(iLastuser);

            return entity;
        }

        #region Mapeo de Campos

        public string Cretapacodi = "CRETAPACODI";
        public string Crevencodi = "CREVENCODI";
        public string Cretapa = "CRETAPA";
        public string Crfechdesicion = "CRFECHDESICION";
        public string Creventodescripcion = "CREVENTODESCRIPCION";
        public string Crresumencriterio = "CRRESUMENCRITERIO";
        public string Crcomentarios_Responsables = "CRCOMENTARIOS_RESPONSABLES";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Crcriteriocodi = "CRCRITERIOCODI";

        #endregion

        public string SqlObtenerCrEtapaEvento
        {
            get { return base.GetSqlXml("ObtenerCrEtapaEvento"); }
        }
        public string SqlListarCrEtapaEventoAf
        {
            get { return base.GetSqlXml("ListarCrEtapaEventoAf"); }
        }
        public string SqlObtenerCriterioxEtapaEvento
        {
            get { return base.GetSqlXml("ObtenerCriterioxEtapaEvento"); }
        }
    }
}
