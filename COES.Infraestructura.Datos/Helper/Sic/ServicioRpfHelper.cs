using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class ServicioRpfHelper : HelperBase
    {
        public ServicioRpfHelper(): base(Consultas.ServicioRpfSql)
        {

        }

        public ServicioRpfDTO Create(IDataReader dr)
        {
            ServicioRpfDTO entity = new ServicioRpfDTO();

            int iRPFCODI = dr.GetOrdinal(this.RPFCODI);
            if (!dr.IsDBNull(iRPFCODI)) entity.RPFCODI = Convert.ToInt32(dr.GetValue(iRPFCODI));             

            int iPTOMEDICODI = dr.GetOrdinal(this.PTOMEDICODI);
            if (!dr.IsDBNull(iPTOMEDICODI)) entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(iPTOMEDICODI));
               
            int iINDESTADO = dr.GetOrdinal(this.INDESTADO);
            if (!dr.IsDBNull(iINDESTADO)) entity.INDESTADO = dr.GetString(iINDESTADO);
          
            int iLASTUSER = dr.GetOrdinal(this.LASTUSER);
            if (!dr.IsDBNull(iLASTUSER)) entity.LASTUSER = dr.GetString(iLASTUSER);
            
            int iLASTDATE = dr.GetOrdinal(this.LASTDATE);
            if (!dr.IsDBNull(iLASTDATE)) entity.LASTDATE = dr.GetDateTime(iLASTDATE);                                       

            return entity;
        }      
      
        public string RPFCODI = "RPFCODI";
        public string PTOMEDICODI = "PTOMEDICODI";
        public string INDESTADO = "INDESTADO";
        public string LASTUSER = "LASTUSER";
        public string LASTDATE = "LASTDATE";
        public string EMPRNOMB = "EMPRNOMB";
        public string EQUINOMB = "EQUINOMB";
        public string EQUIABREV = "EQUIABREV";
        public string FECHA = "FECHAHORA";
        public string VALOR = "VALOR";
        public string SEGUNDO = "H";
        public string FAMCODI = "FAMCODI";
        public string EQUICODI = "EQUICODI";
        public string GPSCODI = "GPSCODI";
        public string GPSNOMBRE = "NOMBRE";
        public string CANTIDAD = "CANTIDAD";

        public string SqlObtenerReservaPrimaria
        {
            get { return base.GetSqlXml("ObtenerReservaPrimaria"); }
        }

        public string SqlObtenerFrecuenciasSanJuan
        {
            get { return base.GetSqlXml("ObtenerFrecuenciasSanJuan"); }
        }

        public string SqlObtenerFrecuenciaSanJuanTotal
        {
            get { return base.GetSqlXml("ObtenerFrecuenciasSanJuanTotal"); }
        }

        public string SqlObtenerFrecuenciasConsistencia
        {
            get { return base.GetSqlXml("ObtenerFrecuenciasConsistencia"); }
        }

        public string SqlVerificarHoraOperacion
        {
            get { return base.GetSqlXml("VerificarHoraOperacion"); }
        }

        public string SqlObtenerGPS
        {
            get { return base.GetSqlXml("ObtenerGPS"); }
        }

        public string SqlObtenerConsultaFrecuencia
        {
            get { return base.GetSqlXml("ObtenerConsultaFrecuencia"); }
        }

        public string SqlReemplazarFrecuencias
        {
            get { return base.GetSqlXml("ReemplazarFrecuencias"); }
        }

        public string SqlEliminarFrecuenciaGPS
        {
            get { return base.GetSqlXml("EliminarFrecuenciaGPS"); }
        }

        public string SqlVerificarFrecuenciaSanJuan
        {
            get { return base.GetSqlXml("VerificarFrecuenciaSanJuan"); }
        }

        public string SqlCompletarFrecuenciaSanJuan
        {
            get { return base.GetSqlXml("CompletarFrecuenciaSanJuan"); }
        }

        public string SqlObtenerValorActualizar
        {
            get { return base.GetSqlXml("ObtenerValorActualizar"); }
        }

        public string SqlActualizarValorFrecuencia
        {
            get { return base.GetSqlXml("ActualizarValorFrecuencia"); }
        }
    }
}
