using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PerturbacionHelper : HelperBase
    {
        public PerturbacionHelper(): base(Consultas.PerturbacionSql)
        {
        }

        public InformePerturbacionDTO  Create(IDataReader dr)
        {
            InformePerturbacionDTO entity = new InformePerturbacionDTO();

            int iPERTURBACIONCODI = dr.GetOrdinal(this.PERTURBACIONCODI);
            int iEVENCODI = dr.GetOrdinal(this.EVENCODI);
            int iSUBCAUSACODI = dr.GetOrdinal(this.SUBCAUSACODI);
            int iITEMTIPO = dr.GetOrdinal(this.ITEMTIPO);
            int iITEMTIME = dr.GetOrdinal(this.ITEMTIME);
            int iITEMDESCRIPCION = dr.GetOrdinal(this.ITEMDESCRIPCION);
            int iEQUICODI = dr.GetOrdinal(this.EQUICODI);
            int iINTERRUPTORCODI = dr.GetOrdinal(this.INTERRUPTORCODI);
            int iITEMSENALIZACION = dr.GetOrdinal(this.ITEMSENALIZACION);
            int iITEMAC = dr.GetOrdinal(this.ITEMAC);
            int iITEMORDEN = dr.GetOrdinal(this.ITEMORDEN);
            int iLASTDATE = dr.GetOrdinal(this.LASTDATE);
            int iLASTUSER = dr.GetOrdinal(this.LASTUSER);
            
            if (!dr.IsDBNull(iPERTURBACIONCODI)) entity.PERTURBACIONCODI = Convert.ToInt32(dr.GetValue(iPERTURBACIONCODI));
            if (!dr.IsDBNull(iEVENCODI)) entity.EVENCODI = Convert.ToInt32(dr.GetValue(iEVENCODI));
            if (!dr.IsDBNull(iSUBCAUSACODI)) entity.SUBCAUSACODI = Convert.ToInt32(dr.GetValue(iSUBCAUSACODI));
            if (!dr.IsDBNull(iITEMTIPO)) entity.ITEMTIPO = dr.GetString(iITEMTIPO);
            if (!dr.IsDBNull(iITEMTIME)) entity.ITEMTIME = dr.GetString(iITEMTIME);
            if (!dr.IsDBNull(iITEMDESCRIPCION)) entity.ITEMDESCRIPCION = dr.GetString(iITEMDESCRIPCION);
            if (!dr.IsDBNull(iEQUICODI)) entity.EQUICODI = Convert.ToInt32(dr.GetValue(iEQUICODI));
            if (!dr.IsDBNull(iINTERRUPTORCODI)) entity.INTERRUPTORCODI = Convert.ToInt32(dr.GetValue(iINTERRUPTORCODI));
            if (!dr.IsDBNull(iITEMSENALIZACION)) entity.ITEMSENALIZACION = dr.GetString(iITEMSENALIZACION);
            if (!dr.IsDBNull(iITEMAC)) entity.ITEMAC = dr.GetString(iITEMAC);
            if (!dr.IsDBNull(iITEMORDEN)) entity.ITEMORDEN = dr.GetDecimal(iITEMORDEN);
            if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);
            if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);

            return entity;
        }

        public string PERTURBACIONCODI ="PERTURBACIONCODI";
        public string EVENCODI ="EVENCODI";
        public string SUBCAUSACODI ="SUBCAUSACODI";
        public string ITEMTIPO ="ITEMTIPO";
        public string ITEMTIME ="ITEMTIME";
        public string ITEMDESCRIPCION ="ITEMDESCRIPCION";
        public string EQUICODI ="EQUICODI";
        public string INTERRUPTORCODI ="INTERRUPTORCODI";
        public string ITEMSENALIZACION ="ITEMSENALIZACION";
        public string ITEMAC ="ITEMAC";
        public string ITEMORDEN ="ITEMORDEN";
        public string LASTDATE ="LASTDATE";
        public string LASTUSER ="LASTUSER";
        public string EQUIABREV = "EQUIABREV";
        public string EQUINOMB = "EQUINOMB";
        public string SUBCAUSADESC = "SUBCAUSADESC";
        
        public string SqlGetItemArea
        {
            get { return base.GetSqlXml("GetItemArea"); }
        }

        public string SqlGetNombreEquipo
        {
            get { return base.GetSqlXml("GetNombreEquipo"); }
        }

        public string SqlObtenerCausaDesc
        {
            get { return base.GetSqlXml("ObtenerCausaDesc"); }
        }

        public string SqlEliminarPorEvento
        {
            get { return base.GetSqlXml("SqlEliminarInformePorEvento"); }
        }

        public string SqlGetMaxId
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }
    }
}

