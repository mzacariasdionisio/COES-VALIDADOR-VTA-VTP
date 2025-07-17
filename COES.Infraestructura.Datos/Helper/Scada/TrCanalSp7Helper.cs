using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_CANAL_SP7
    /// </summary>
    public class TrCanalSp7Helper : HelperBase
    {
        public TrCanalSp7Helper(): base(Consultas.TrCanalSp7Sql)
        {
        }

        /// <summary>
        /// Consultar campos que se encuentran en la BD SICOES
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public TrCanalSp7DTO Create(IDataReader dr)
        {
            TrCanalSp7DTO entity = new TrCanalSp7DTO();

            int iCanalmseg = dr.GetOrdinal(this.Canalmseg);
            if (!dr.IsDBNull(iCanalmseg)) entity.Canalmseg = Convert.ToInt32(dr.GetValue(iCanalmseg));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iCanalvalor = dr.GetOrdinal(this.Canalvalor);
            if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

            int iAlarmcodi = dr.GetOrdinal(this.Alarmcodi);
            if (!dr.IsDBNull(iAlarmcodi)) entity.Alarmcodi = Convert.ToInt32(dr.GetValue(iAlarmcodi));

            int iCanalcalidad = dr.GetOrdinal(this.Canalcalidad);
            if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

            int iCanalfhora = dr.GetOrdinal(this.Canalfhora);
            if (!dr.IsDBNull(iCanalfhora)) entity.Canalfhora = dr.GetDateTime(iCanalfhora);

            int iCanalnomb = dr.GetOrdinal(this.Canalnomb);
            if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCanaliccp = dr.GetOrdinal(this.Canaliccp);
            if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

            int iCanaltdato = dr.GetOrdinal(this.Canaltdato);
            if (!dr.IsDBNull(iCanaltdato)) entity.Canaltdato = Convert.ToInt32(dr.GetValue(iCanaltdato));

            int iCanalunidad = dr.GetOrdinal(this.Canalunidad);
            if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

            int iZonacodi = dr.GetOrdinal(this.Zonacodi);
            if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

            int iCanaltipo = dr.GetOrdinal(this.Canaltipo);
            if (!dr.IsDBNull(iCanaltipo)) entity.Canaltipo = dr.GetString(iCanaltipo);

            int iCanalabrev = dr.GetOrdinal(this.Canalabrev);
            if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

            int iCanalfhora2 = dr.GetOrdinal(this.Canalfhora2);
            if (!dr.IsDBNull(iCanalfhora2)) entity.Canalfhora2 = dr.GetDateTime(iCanalfhora2);

            int iCanalcodscada = dr.GetOrdinal(this.Canalcodscada);
            if (!dr.IsDBNull(iCanalcodscada)) entity.Canalcodscada = dr.GetString(iCanalcodscada);

            int iCanalflags = dr.GetOrdinal(this.Canalflags);
            if (!dr.IsDBNull(iCanalflags)) entity.Canalflags = Convert.ToInt32(dr.GetValue(iCanalflags));

            int iCanalcalidadforzada = dr.GetOrdinal(this.Canalcalidadforzada);
            if (!dr.IsDBNull(iCanalcalidadforzada)) entity.Canalcalidadforzada = Convert.ToInt32(dr.GetValue(iCanalcalidadforzada));

            int iCanalvalor2 = dr.GetOrdinal(this.Canalvalor2);
            if (!dr.IsDBNull(iCanalvalor2)) entity.Canalvalor2 = dr.GetDecimal(iCanalvalor2);

            int iCanalestado = dr.GetOrdinal(this.Canalestado);
            if (!dr.IsDBNull(iCanalestado)) entity.Canalestado = dr.GetString(iCanalestado);

            int iCanalfhestado = dr.GetOrdinal(this.Canalfhestado);
            if (!dr.IsDBNull(iCanalfhestado)) entity.Canalfhestado = dr.GetDateTime(iCanalfhestado);

            int iAlarmmin1 = dr.GetOrdinal(this.Alarmmin1);
            if (!dr.IsDBNull(iAlarmmin1)) entity.Alarmmin1 = dr.GetDecimal(iAlarmmin1);

            int iAlarmmax1 = dr.GetOrdinal(this.Alarmmax1);
            if (!dr.IsDBNull(iAlarmmax1)) entity.Alarmmax1 = dr.GetDecimal(iAlarmmax1);

            int iAlarmmin2 = dr.GetOrdinal(this.Alarmmin2);
            if (!dr.IsDBNull(iAlarmmin2)) entity.Alarmmin2 = dr.GetDecimal(iAlarmmin2);

            int iAlarmmax2 = dr.GetOrdinal(this.Alarmmax2);
            if (!dr.IsDBNull(iAlarmmax2)) entity.Alarmmax2 = dr.GetDecimal(iAlarmmax2);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iCanaldescripcionestado = dr.GetOrdinal(this.Canaldescripcionestado);
            if (!dr.IsDBNull(iCanaldescripcionestado)) entity.Canaldescripcionestado = dr.GetString(iCanaldescripcionestado);

            int iCanalprior = dr.GetOrdinal(this.Canalprior);
            if (!dr.IsDBNull(iCanalprior)) entity.Canalprior = Convert.ToInt32(dr.GetValue(iCanalprior));

            int iCanaldec = dr.GetOrdinal(this.Canaldec);
            if (!dr.IsDBNull(iCanaldec)) entity.Canaldec = Convert.ToInt32(dr.GetValue(iCanaldec));

            int iCanalntension = dr.GetOrdinal(this.Canalntension);
            if (!dr.IsDBNull(iCanalntension)) entity.Canalntension = dr.GetDecimal(iCanalntension);

            int iCanalinvert = dr.GetOrdinal(this.Canalinvert);
            if (!dr.IsDBNull(iCanalinvert)) entity.Canalinvert = dr.GetString(iCanalinvert);

            int iCanaldispo = dr.GetOrdinal(this.Canaldispo);
            if (!dr.IsDBNull(iCanaldispo)) entity.Canaldispo = Convert.ToInt32(dr.GetValue(iCanaldispo));

            int iCanalcritico = dr.GetOrdinal(this.Canalcritico);
            if (!dr.IsDBNull(iCanalcritico)) entity.Canalcritico = dr.GetString(iCanalcritico);

            int iCanaliccpreenvio = dr.GetOrdinal(this.Canaliccpreenvio);
            if (!dr.IsDBNull(iCanaliccpreenvio)) entity.Canaliccpreenvio = dr.GetString(iCanaliccpreenvio);

            int iCanalcelda = dr.GetOrdinal(this.Canalcelda);
            if (!dr.IsDBNull(iCanalcelda)) entity.Canalcelda = dr.GetString(iCanalcelda);

            int iCanaldescrip2 = dr.GetOrdinal(this.Canaldescrip2);
            if (!dr.IsDBNull(iCanaldescrip2)) entity.Canaldescrip2 = dr.GetString(iCanaldescrip2);

            int iRdfid = dr.GetOrdinal(this.Rdfid);
            if (!dr.IsDBNull(iRdfid)) entity.Rdfid = dr.GetString(iRdfid);

            int iGisid = dr.GetOrdinal(this.Gisid);
            if (!dr.IsDBNull(iGisid)) entity.Gisid = Convert.ToInt32(dr.GetValue(iGisid));

            int iPathb = dr.GetOrdinal(this.Pathb);
            if (!dr.IsDBNull(iPathb)) entity.Pathb = dr.GetString(iPathb);

            int iPointType = dr.GetOrdinal(this.PointType);
            if (!dr.IsDBNull(iPointType)) entity.PointType = dr.GetString(iPointType);

            /*int iLastdatesp7 = dr.GetOrdinal(this.Lastdatesp7);
            if (!dr.IsDBNull(iLastdatesp7)) entity.Lastdatesp7 = dr.GetDateTime(iLastdatesp7);*/

            int iGpscodi = dr.GetOrdinal(this.Gpscodi);
            if (!dr.IsDBNull(iGpscodi)) entity.Gpscodi = Convert.ToInt32(dr.GetValue(iGpscodi));

            int iCanalfeccreacion = dr.GetOrdinal(this.Canalfeccreacion);
            if (!dr.IsDBNull(iCanalfeccreacion)) entity.Canalfeccreacion = dr.GetDateTime(iCanalfeccreacion);

            int iCanalusucreacion = dr.GetOrdinal(this.Canalusucreacion);
            if (!dr.IsDBNull(iCanalusucreacion)) entity.Canalusucreacion = dr.GetString(iCanalusucreacion);

            return entity;
        }

        /// <summary>
        /// Consultar campos que se encuentran en la BD SICOES
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public TrCanalSp7DTO CreateFromTrcoes(IDataReader dr)
        {
            TrCanalSp7DTO entity = new TrCanalSp7DTO();

            int iCanalmseg = dr.GetOrdinal(this.Canalmseg);
            if (!dr.IsDBNull(iCanalmseg)) entity.Canalmseg = Convert.ToInt32(dr.GetValue(iCanalmseg));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iCanalvalor = dr.GetOrdinal(this.Canalvalor);
            if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

            int iCanalcalidad = dr.GetOrdinal(this.Canalcalidad);
            if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

            int iCanalfhora = dr.GetOrdinal(this.Canalfhora);
            if (!dr.IsDBNull(iCanalfhora)) entity.Canalfhora = dr.GetDateTime(iCanalfhora);

            int iCanalnomb = dr.GetOrdinal(this.Canalnomb);
            if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iCanaliccp = dr.GetOrdinal(this.Canaliccp);
            if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

            int iCanalunidad = dr.GetOrdinal(this.Canalunidad);
            if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

            int iZonacodi = dr.GetOrdinal(this.Zonacodi);
            if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

            int iCanalabrev = dr.GetOrdinal(this.Canalabrev);
            if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

            int iCanalfhora2 = dr.GetOrdinal(this.Canalfhora2);
            if (!dr.IsDBNull(iCanalfhora2)) entity.Canalfhora2 = dr.GetDateTime(iCanalfhora2);

            int iPathb = dr.GetOrdinal(this.Pathb);
            if (!dr.IsDBNull(iPathb)) entity.Pathb = dr.GetString(iPathb);

            int iPointType = dr.GetOrdinal(this.PointType);
            if (!dr.IsDBNull(iPointType)) entity.PointType = dr.GetString(iPointType);

            int iTrCanalRemota = dr.GetOrdinal(this.TrCanalRemota);
            if (!dr.IsDBNull(iTrCanalRemota)) entity.CanalRemota = dr.GetString(iTrCanalRemota);

            int iTrCanalContenedor = dr.GetOrdinal(this.TrCanalContenedor);
            if (!dr.IsDBNull(iTrCanalContenedor)) entity.CanalContenedor = dr.GetString(iTrCanalContenedor);

            int iTrCanalEnlace = dr.GetOrdinal(this.TrCanalEnlace);
            if (!dr.IsDBNull(iTrCanalEnlace)) entity.CanalEnlace = dr.GetString(iTrCanalEnlace);

            return entity;
        }

        /// <summary>
        /// Consultar campos que se encuentran en la BD SCADA_PRD
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public TrCanalSp7DTO CreateBdTreal(IDataReader dr)
        {
            TrCanalSp7DTO entity = new TrCanalSp7DTO();

            int iCanalabrev = dr.GetOrdinal(this.Canalabrev);
            if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

            int iCanalcalidad = dr.GetOrdinal(this.Canalcalidad);
            if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            //CANALCONTENEDOR

            int iCanalcritico = dr.GetOrdinal(this.Canalcritico);
            if (!dr.IsDBNull(iCanalcritico)) entity.Canalcritico = dr.GetString(iCanalcritico);

            //CANALENLACE

            int iCanalfeccreacion = dr.GetOrdinal(this.Canalfeccreacion);
            if (!dr.IsDBNull(iCanalfeccreacion)) entity.Canalfeccreacion = dr.GetDateTime(iCanalfeccreacion);

            int iCanalfhora = dr.GetOrdinal(this.Canalfhora);
            if (!dr.IsDBNull(iCanalfhora)) entity.Canalfhora = dr.GetDateTime(iCanalfhora);

            int iCanalfhora2 = dr.GetOrdinal(this.Canalfhora2);
            if (!dr.IsDBNull(iCanalfhora2)) entity.Canalfhora2 = dr.GetDateTime(iCanalfhora2);

            int iGisid = dr.GetOrdinal(this.Gisid);
            if (!dr.IsDBNull(iGisid)) entity.Gisid = Convert.ToInt32(dr.GetValue(iGisid));

            int iGpscodi = dr.GetOrdinal(this.Gpscodi);
            if (!dr.IsDBNull(iGpscodi)) entity.Gpscodi = Convert.ToInt32(dr.GetValue(iGpscodi));

            int iCanaliccp = dr.GetOrdinal(this.Canaliccp);
            if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

            int iCanalmseg = dr.GetOrdinal(this.Canalmseg);
            if (!dr.IsDBNull(iCanalmseg)) entity.Canalmseg = Convert.ToInt32(dr.GetValue(iCanalmseg));

            int iCanalnomb = dr.GetOrdinal(this.Canalnomb);
            if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

            int iPathb = dr.GetOrdinal(this.Pathb);
            if (!dr.IsDBNull(iPathb)) entity.Pathb = dr.GetString(iPathb);

            int iPointType = dr.GetOrdinal(this.PointType);
            if (!dr.IsDBNull(iPointType)) entity.PointType = dr.GetString(iPointType);

            int iRdfid = dr.GetOrdinal(this.Rdfid);
            if (!dr.IsDBNull(iRdfid)) entity.Rdfid = dr.GetString(iRdfid);

            //CANALREMOTA

            int iCanalunidad = dr.GetOrdinal(this.Canalunidad);
            if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

            //CANALURS

            int iCanalusucreacion = dr.GetOrdinal(this.Canalusucreacion);
            if (!dr.IsDBNull(iCanalusucreacion)) entity.Canalusucreacion = dr.GetString(iCanalusucreacion);

            int iCanalvalor = dr.GetOrdinal(this.Canalvalor);
            if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iZonacodi = dr.GetOrdinal(this.Zonacodi);
            if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

            /*
             * campos que no existen en SP7 pero sí en SICOES
            int iAlarmcodi = dr.GetOrdinal(this.Alarmcodi);
            if (!dr.IsDBNull(iAlarmcodi)) entity.Alarmcodi = Convert.ToInt32(dr.GetValue(iAlarmcodi));

            int iCanaltdato = dr.GetOrdinal(this.Canaltdato);
            if (!dr.IsDBNull(iCanaltdato)) entity.Canaltdato = Convert.ToInt32(dr.GetValue(iCanaltdato));

            int iCanaltipo = dr.GetOrdinal(this.Canaltipo);
            if (!dr.IsDBNull(iCanaltipo)) entity.Canaltipo = dr.GetString(iCanaltipo);

            int iCanalcodscada = dr.GetOrdinal(this.Canalcodscada);
            if (!dr.IsDBNull(iCanalcodscada)) entity.Canalcodscada = dr.GetString(iCanalcodscada);

            int iCanalflags = dr.GetOrdinal(this.Canalflags);
            if (!dr.IsDBNull(iCanalflags)) entity.Canalflags = Convert.ToInt32(dr.GetValue(iCanalflags));

            int iCanalcalidadforzada = dr.GetOrdinal(this.Canalcalidadforzada);
            if (!dr.IsDBNull(iCanalcalidadforzada)) entity.Canalcalidadforzada = Convert.ToInt32(dr.GetValue(iCanalcalidadforzada));

            int iCanalvalor2 = dr.GetOrdinal(this.Canalvalor2);
            if (!dr.IsDBNull(iCanalvalor2)) entity.Canalvalor2 = dr.GetDecimal(iCanalvalor2);

            int iCanalestado = dr.GetOrdinal(this.Canalestado);
            if (!dr.IsDBNull(iCanalestado)) entity.Canalestado = dr.GetString(iCanalestado);

            int iCanalfhestado = dr.GetOrdinal(this.Canalfhestado);
            if (!dr.IsDBNull(iCanalfhestado)) entity.Canalfhestado = dr.GetDateTime(iCanalfhestado);

            int iAlarmmin1 = dr.GetOrdinal(this.Alarmmin1);
            if (!dr.IsDBNull(iAlarmmin1)) entity.Alarmmin1 = dr.GetDecimal(iAlarmmin1);

            int iAlarmmax1 = dr.GetOrdinal(this.Alarmmax1);
            if (!dr.IsDBNull(iAlarmmax1)) entity.Alarmmax1 = dr.GetDecimal(iAlarmmax1);

            int iAlarmmin2 = dr.GetOrdinal(this.Alarmmin2);
            if (!dr.IsDBNull(iAlarmmin2)) entity.Alarmmin2 = dr.GetDecimal(iAlarmmin2);

            int iAlarmmax2 = dr.GetOrdinal(this.Alarmmax2);
            if (!dr.IsDBNull(iAlarmmax2)) entity.Alarmmax2 = dr.GetDecimal(iAlarmmax2);

            int iCanaldescripcionestado = dr.GetOrdinal(this.Canaldescripcionestado);
            if (!dr.IsDBNull(iCanaldescripcionestado)) entity.Canaldescripcionestado = dr.GetString(iCanaldescripcionestado);

            int iCanalprior = dr.GetOrdinal(this.Canalprior);
            if (!dr.IsDBNull(iCanalprior)) entity.Canalprior = Convert.ToInt32(dr.GetValue(iCanalprior));

            int iCanaldec = dr.GetOrdinal(this.Canaldec);
            if (!dr.IsDBNull(iCanaldec)) entity.Canaldec = Convert.ToInt32(dr.GetValue(iCanaldec));

            int iCanalntension = dr.GetOrdinal(this.Canalntension);
            if (!dr.IsDBNull(iCanalntension)) entity.Canalntension = dr.GetDecimal(iCanalntension);

            int iCanalinvert = dr.GetOrdinal(this.Canalinvert);
            if (!dr.IsDBNull(iCanalinvert)) entity.Canalinvert = dr.GetString(iCanalinvert);

            int iCanaldispo = dr.GetOrdinal(this.Canaldispo);
            if (!dr.IsDBNull(iCanaldispo)) entity.Canaldispo = Convert.ToInt32(dr.GetValue(iCanaldispo));

            int iCanaliccpreenvio = dr.GetOrdinal(this.Canaliccpreenvio);
            if (!dr.IsDBNull(iCanaliccpreenvio)) entity.Canaliccpreenvio = dr.GetString(iCanaliccpreenvio);

            int iCanalcelda = dr.GetOrdinal(this.Canalcelda);
            if (!dr.IsDBNull(iCanalcelda)) entity.Canalcelda = dr.GetString(iCanalcelda);

            int iCanaldescrip2 = dr.GetOrdinal(this.Canaldescrip2);
            if (!dr.IsDBNull(iCanaldescrip2)) entity.Canaldescrip2 = dr.GetString(iCanaldescrip2);
            */

            return entity;
        }

        #region Mapeo de Campos

        public string Canalmseg = "CANALMSEG";
        public string Canalcodi = "CANALCODI";
        public string Canalvalor = "CANALVALOR";
        public string Alarmcodi = "ALARMCODI";
        public string Canalcalidad = "CANALCALIDAD";
        public string Canalfhora = "CANALFHORA";
        public string Canalnomb = "CANALNOMB";
        public string Emprcodi = "EMPRCODI";
        public string Canaliccp = "CANALICCP";
        public string Canaltdato = "CANALTDATO";
        public string Canalunidad = "CANALUNIDAD";
        public string Zonacodi = "ZONACODI";
        public string Canaltipo = "CANALTIPO";
        public string Canalabrev = "CANALABREV";
        public string Canalfhora2 = "CANALFHORA2";
        public string Canalcodscada = "CANALCODSCADA";
        public string Canalflags = "CANALFLAGS";
        public string Canalcalidadforzada = "CANALCALIDADFORZADA";
        public string Canalvalor2 = "CANALVALOR2";
        public string Canalestado = "CANALESTADO";
        public string Canalfhestado = "CANALFHESTADO";
        public string Alarmmin1 = "ALARMMIN1";
        public string Alarmmax1 = "ALARMMAX1";
        public string Alarmmin2 = "ALARMMIN2";
        public string Alarmmax2 = "ALARMMAX2";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Canaldescripcionestado = "CANALDESCRIPCIONESTADO";
        public string Canalprior = "CANALPRIOR";
        public string Canaldec = "CANALDEC";
        public string Canalntension = "CANALNTENSION";
        public string Canalinvert = "CANALINVERT";
        public string Canaldispo = "CANALDISPO";
        public string Canalcritico = "CANALCRITICO";
        public string Canaliccpreenvio = "CANALICCPREENVIO";
        public string Canalcelda = "CANALCELDA";
        public string Canaldescrip2 = "CANALDESCRIP2";
        public string Rdfid = "CANALRDFID";
        public string Gisid = "CANALGISID";
        public string Pathb = "CANALPATHB";
        public string PointType = "CANALPOINTTYPE";
        public string Gpscodi = "CANALGPSCODI";
        public string Canalusucreacion = "CANALUSUCREACION";
        public string Canalfeccreacion = "CANALFECCREACION";

        #region Mejoras IEOD

        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Tipoinfoabrev = "TIPOINFOABREV";

        #endregion

        public string CanalPointType = "CANALPOINTTYPE";
        public string Zonanomb = "ZONANOMB";
        public string Zonaabrev = "ZONAABREV";
        public string TrEmprcodi = "TREMPRCODI";
        public string TrEmprnomb = "TREMPRNOMB";
        public string TrEmprabrev = "TREMPRABREV";

        //SOLO PARA LECTURA DESDE TRCOES
        public string TrCanalRemota = "CANALREMOTA";
        public string TrCanalContenedor = "CANALCONTENEDOR";
        public string TrCanalEnlace = "CANALENLACE";

        #endregion

        public string SqlGetByIds
        {
            get { return base.GetSqlXml("GetByCriteriaCanalcodi"); }
        }

        public string SqlGetByCanalnomb
        {
            get { return base.GetSqlXml("GetByCriteriaCanalnomb"); }
        }

        public string SqlGetByZona
        {
            get { return base.GetSqlXml("GetByCriteriaZona"); }
        }

        public string SqlGetByZonaAnalogico
        {
            get { return base.GetSqlXml("GetByCriteriaZonaAnalogico"); }
        }


        public string SqlGetByFiltro
        {
            get { return base.GetSqlXml("GetByCriteriaFiltro"); }
        }

        public string SqlListByZonaAndUnidad
        {
            get { return base.GetSqlXml("ListByZonaAndUnidad"); }
        }

        #region Mejoras IEOD

        public string SqlListarUnidadPorZona
        {
            get { return base.GetSqlXml("ListarUnidadPorZona"); }
        }

        public string SqlGetByCriteriaBdTreal
        {
            get { return base.GetSqlXml("GetByCriteriaBdTreal"); }
        }

        #endregion

        public string SqlListarDatosSP7
        {
            get { return base.GetSqlXml("ListarDatosSP7"); }
        }
    }
}
