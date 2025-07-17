using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RI_REVISION
    /// </summary>
    public class RiRevisionHelper : HelperBase
    {
        public RiRevisionHelper() : base(Consultas.RiRevisionSql)
        {
        }

        public RiRevisionDTO Create(IDataReader dr)
        {
            RiRevisionDTO entity = new RiRevisionDTO();

            int iRevicodi = dr.GetOrdinal(this.Revicodi);
            if (!dr.IsDBNull(iRevicodi)) entity.Revicodi = Convert.ToInt32(dr.GetValue(iRevicodi));
         
            int iReviiteracion = dr.GetOrdinal(this.Reviiteracion);
            if (!dr.IsDBNull(iReviiteracion)) entity.Reviiteracion = Convert.ToInt32(dr.GetValue(iReviiteracion));

            int iReviestado = dr.GetOrdinal(this.Reviestado);
            if (!dr.IsDBNull(iReviestado)) entity.Reviestado = dr.GetString(iReviestado);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEtrvcodi = dr.GetOrdinal(this.Etrvcodi);
            if (!dr.IsDBNull(iEtrvcodi)) entity.Etrvcodi = Convert.ToInt32(dr.GetValue(iEtrvcodi));

            int iReviusucreacion = dr.GetOrdinal(this.Reviusucreacion);
            if (!dr.IsDBNull(iReviusucreacion)) entity.Reviusucreacion = dr.GetString(iReviusucreacion);

            int iRevifeccreacion = dr.GetOrdinal(this.Revifeccreacion);
            if (!dr.IsDBNull(iRevifeccreacion)) entity.Revifeccreacion = dr.GetDateTime(iRevifeccreacion);

            int iReviusumodificacion = dr.GetOrdinal(this.Reviusumodificacion);
            if (!dr.IsDBNull(iReviusumodificacion)) entity.Reviusumodificacion = dr.GetString(iReviusumodificacion);

            int iRevifecmodificacion = dr.GetOrdinal(this.Revifecmodificacion);
            if (!dr.IsDBNull(iRevifecmodificacion)) entity.Revifecmodificacion = dr.GetDateTime(iRevifecmodificacion);

            int iReviusurevision = dr.GetOrdinal(this.Reviusurevision);
            if (!dr.IsDBNull(iReviusurevision)) entity.Reviusurevision = Convert.ToInt32(dr.GetValue(iReviusurevision));

            int iRevifecrevision = dr.GetOrdinal(this.Revifecrevision);
            if (!dr.IsDBNull(iRevifecrevision)) entity.Revifecrevision = dr.GetDateTime(iRevifecrevision);

            int iRevifinalizado = dr.GetOrdinal(this.Revifinalizado);
            if (!dr.IsDBNull(iRevifinalizado)) entity.Revifinalizado = dr.GetString(iRevifinalizado);

            int iRevifecfinalizado = dr.GetOrdinal(this.Revifecfinalizado);
            if (!dr.IsDBNull(iRevifecfinalizado)) entity.Revifecfinalizado = dr.GetDateTime(iRevifecfinalizado);

            int iRevinotificado = dr.GetOrdinal(this.Revinotificado);
            if (!dr.IsDBNull(iRevinotificado)) entity.Revinotificado = dr.GetString(iRevinotificado);

            int iRevifecnotificado = dr.GetOrdinal(this.Revifecnotificado);
            if (!dr.IsDBNull(iRevifecnotificado)) entity.Revifecnotificado = dr.GetDateTime(iRevifecnotificado);

            int iReviterminado = dr.GetOrdinal(this.Reviterminado);
            if (!dr.IsDBNull(iReviterminado)) entity.Reviterminado = dr.GetString(iReviterminado);

            int iRevifecterminado = dr.GetOrdinal(this.Revifecterminado);
            if (!dr.IsDBNull(iRevifecterminado)) entity.Revifecterminado = dr.GetDateTime(iRevifecterminado);

            int iRevienviado = dr.GetOrdinal(this.Revienviado);
            if (!dr.IsDBNull(iRevienviado)) entity.Revienviado = dr.GetString(iRevienviado);

            int iRevifecenviado = dr.GetOrdinal(this.Revifecenviado);
            if (!dr.IsDBNull(iRevifecenviado)) entity.Revifecenviado = dr.GetDateTime(iRevifecenviado);


            return entity;
        }

        public SiEmpresaDTO CreateList(IDataReader dr)
        {
            SiEmpresaDTO entity = new SiEmpresaDTO();

            int iRevicodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iRevicodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iRevicodi));

            int iReviiteracionSGI = dr.GetOrdinal(this.ReviiteracionSGI);
            if (!dr.IsDBNull(iReviiteracionSGI)) entity.ReviiteracionSGI = Convert.ToInt32(dr.GetValue(iReviiteracionSGI));

            int iReviiteracionDJR = dr.GetOrdinal(this.ReviiteracionDJR);
            if (!dr.IsDBNull(iReviiteracionDJR)) entity.ReviiteracionDRJ = Convert.ToInt32(dr.GetValue(iReviiteracionDJR));

            int iTipemprdesc = dr.GetOrdinal(this.Tipemprdesc);
            if (!dr.IsDBNull(iTipemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipemprdesc);

            int iEmprrazsocial = dr.GetOrdinal(this.Emprrazsocial);
            if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnombrecomercial = dr.GetString(iEmprnomb);

            int iEmprabrev = dr.GetOrdinal(this.Emprsigla);
            if (!dr.IsDBNull(iEmprabrev)) entity.Emprsigla = dr.GetString(iEmprabrev);

            int iEmprestado = dr.GetOrdinal(this.emprestado);
            if (!dr.IsDBNull(iEmprestado)) entity.EmpresaEstado = dr.GetString(iEmprestado);

            int iEmpfecincripcion = dr.GetOrdinal(this.empfecincripcion);
            if (!dr.IsDBNull(iEmpfecincripcion)) entity.Emprfechainscripcion = dr.GetDateTime(iEmpfecincripcion);

            int iHorasSGI = dr.GetOrdinal(this.HorasSGI);
            if (!dr.IsDBNull(iHorasSGI)) entity.HorasSGI = Convert.ToInt32(dr.GetValue(iHorasSGI));

            int iHorasDJR = dr.GetOrdinal(this.HorasDJR);
            if (!dr.IsDBNull(iHorasDJR)) entity.HorasDJR = Convert.ToInt32(dr.GetValue(iHorasDJR));

            int iReviCodiSGI = dr.GetOrdinal(this.ReviCodiSGI);
            if (!dr.IsDBNull(iReviCodiSGI)) entity.ReviCodiSGI = Convert.ToInt32(dr.GetValue(iReviCodiSGI));

            int iReviCodiDJR = dr.GetOrdinal(this.ReviCodiDJR);
            if (!dr.IsDBNull(iReviCodiDJR)) entity.ReviCodiDJR = Convert.ToInt32(dr.GetValue(iReviCodiDJR));

            int iRevifecrevisionSGI = dr.GetOrdinal(this.RevifecrevisionSGI);
            if (!dr.IsDBNull(iRevifecrevisionSGI)) entity.RevifecrevisionSGI = dr.GetDateTime(iRevifecrevisionSGI);

            int iRevifecrevisionDJR = dr.GetOrdinal(this.RevifecrevisionDJR);
            if (!dr.IsDBNull(iRevifecrevisionDJR)) entity.RevifecrevisionDJR = dr.GetDateTime(iRevifecrevisionDJR);

            int iReviFinalizadoSGI = dr.GetOrdinal(this.ReviFinalizadoSGI);
            if (!dr.IsDBNull(iReviFinalizadoSGI)) entity.ReviFinalizadoSGI = dr.GetString(iReviFinalizadoSGI);

            int iReviFinalizadoDJR = dr.GetOrdinal(this.ReviFinalizadoDJR);
            if (!dr.IsDBNull(iReviFinalizadoDJR)) entity.ReviFinalizadoDJR = dr.GetString(iReviFinalizadoDJR);

            int iReviFecFinalizadoSGI = dr.GetOrdinal(this.ReviFecFinalizadoSGI);
            if (!dr.IsDBNull(iReviFecFinalizadoSGI)) entity.ReviFecFinalizadoSGI = dr.GetDateTime(iReviFecFinalizadoSGI);

            int iReviFecFinalizadoDJR = dr.GetOrdinal(this.ReviFecFinalizadoDJR);
            if (!dr.IsDBNull(iReviFecFinalizadoDJR)) entity.ReviFecFinalizadoDJR = dr.GetDateTime(iReviFecFinalizadoDJR);


            int iReviNotificadoSGI = dr.GetOrdinal(this.ReviNotificadoSGI);
            if (!dr.IsDBNull(iReviNotificadoSGI)) entity.ReviNotificadoSGI = dr.GetString(iReviNotificadoSGI);

            int iReviNotificadoDJR = dr.GetOrdinal(this.ReviNotificadoDJR);
            if (!dr.IsDBNull(iReviNotificadoDJR)) entity.ReviNotificadoDJR = dr.GetString(iReviNotificadoDJR);

            int iReviFecNotificadoSGI = dr.GetOrdinal(this.ReviFecNotificadoSGI);
            if (!dr.IsDBNull(iReviFecNotificadoSGI)) entity.ReviFecNotificadoSGI = dr.GetDateTime(iReviFecNotificadoSGI);

            int iReviFecNotificadoDJR = dr.GetOrdinal(this.ReviFecNotificadoDJR);
            if (!dr.IsDBNull(iReviFecNotificadoDJR)) entity.ReviFecNotificadoDJR = dr.GetDateTime(iReviFecNotificadoDJR);

            int iReviEstadoSGI = dr.GetOrdinal(this.ReviEstadoSGI);
            if (!dr.IsDBNull(iReviEstadoSGI)) entity.ReviEstadoSGI = dr.GetString(iReviEstadoSGI);

            int iReviEstadoDJR = dr.GetOrdinal(this.ReviEstadoDJR);
            if (!dr.IsDBNull(iReviEstadoDJR)) entity.ReviEstadoDJR = dr.GetString(iReviEstadoDJR);

            int iReviEnviadoSGI = dr.GetOrdinal(this.ReviEnviadoSGI);
            if (!dr.IsDBNull(iReviEnviadoSGI)) entity.ReviEnviadoSGI = dr.GetString(iReviEnviadoSGI);

            int iReviEnviadoDJR = dr.GetOrdinal(this.ReviEnviadoDJR);
            if (!dr.IsDBNull(iReviEnviadoDJR)) entity.ReviEnviadoDJR = dr.GetString(iReviEnviadoDJR);

            int iReviFecEnviadoSGI = dr.GetOrdinal(this.ReviFecEnviadoSGI);
            if (!dr.IsDBNull(iReviFecEnviadoSGI)) entity.ReviFecEnviadoSGI = dr.GetDateTime(iReviFecEnviadoSGI);

            int iReviFecEnviadoDJR = dr.GetOrdinal(this.ReviFecEnviadoDJR);
            if (!dr.IsDBNull(iReviFecEnviadoDJR)) entity.ReviFecEnviadoDJR = dr.GetDateTime(iReviFecEnviadoDJR);

            int iReviTerminadoSGI = dr.GetOrdinal(this.ReviTerminadoSGI);
            if (!dr.IsDBNull(iReviTerminadoSGI)) entity.ReviTerminadoSGI = dr.GetString(iReviTerminadoSGI);

            int iReviTerminadoDJR = dr.GetOrdinal(this.ReviTerminadoDJR);
            if (!dr.IsDBNull(iReviTerminadoDJR)) entity.ReviTerminadoDJR = dr.GetString(iReviTerminadoDJR);

            int iReviFecTerminadoSGI = dr.GetOrdinal(this.ReviFecTerminadoSGI);
            if (!dr.IsDBNull(iReviFecTerminadoSGI)) entity.ReviFecTerminadoSGI = dr.GetDateTime(iReviFecTerminadoSGI);

            int iReviFecTerminadoDJR = dr.GetOrdinal(this.ReviFecTerminadoDJR);
            if (!dr.IsDBNull(iReviFecTerminadoDJR)) entity.ReviFecTerminadoDJR = dr.GetDateTime(iReviFecTerminadoDJR);


            return entity;
        }
        

        #region Consultas SQL
        public string SqlGetMaxIteracion
        {
            get { return base.GetSqlXml("GetMaxIteracion"); }
        }

        public string SqlListByEstadoAndTipEmp
        {
            get { return base.GetSqlXml("ListByEstadoAndTipEmp"); }
        }

        public string SqlGetTotalRowsListByEstadoAndTipEmp
        {
            get { return base.GetSqlXml("NroRegListByEstadoAndTipEmp"); }
        }

        public string SqlDarConformidad
        {
            get { return base.GetSqlXml("DarConformidad"); }
        }

        public string SqlDarNotificar
        {
            get { return base.GetSqlXml("DarNotificar"); }
        }
        public string SqlDarTerminar
        {
            get { return base.GetSqlXml("DarTerminar"); }
        }
        public string SqlRevAsistente
        {
            get { return base.GetSqlXml("RevAsistente"); }
        }
        public string SqlGetByEtapa
        {
            get { return base.GetSqlXml("GetByEtapa"); }
        }
        public string SqlUpdateEstadoRegistroInactivo
        {
            get { return base.GetSqlXml("UpdateEstadoRegistroInactivo"); }
        }
        //
        #endregion

        #region Mapeo de Campos

        public string Revicodi = "REVICODI";

        public string Reviusurevision = "REVIUSUREVISION";
        public string Revifecrevision = "REVIFECREVISION";

        public string Revifinalizado = "REVIFINALIZADO";
        public string Revifecfinalizado = "REVIFECFINALIZADO";

        public string Revinotificado = "REVINOTIFICADO";
        public string Revifecnotificado = "REVIFECNOTIFICADO";

        public string Reviterminado = "REVITERMINADO";
        public string Revifecterminado = "REVIFECTERMINADO";

        public string Revienviado = "REVIENVIADO";
        public string Revifecenviado = "REVIFECENVIADO";

        public string Reviiteracion = "REVIITERACION";
        public string ReviiteracionSGI = "REVIITERACIONSGI";
        public string ReviiteracionDJR = "REVIITERACIONDJR";
        public string Reviestado = "REVIESTADO";
        public string Reviestadoregistro = "REVIESTADOREGISTRO";
        
        public string Emprcodi = "EMPRCODI";
        public string Etrvcodi = "ETRVCODI";
        public string Reviusucreacion = "REVIUSUCREACION";
        public string Revifeccreacion = "REVIFECCREACION";

        public string Reviusumodificacion = "REVIUSUMODIFICACION";
        public string Revifecmodificacion = "REVIFECMODIFICACION";

        
        //Campos listado
        public string Tipemprdesc = "TIPOEMPRDESC";
        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Emprnomb = "EMPRNOMBRECOMERCIAL";
        public string Emprsigla = "EMPRSIGLA"; 
        
        public string emprcodi = "emprcodi";
        public string emprestado = "emprestado";
        public string empfecincripcion = "emprfecinscripcion";


        public string HorasSGI = "HorasSGI";
        public string HorasDJR = "HorasDJR";

        public string ReviCodiSGI = "ReviCodiSGI";
        public string ReviCodiDJR = "ReviCodiDJR";

        public string RevifecrevisionSGI = "RevifecrevisionSGI";
        public string RevifecrevisionDJR = "RevifecrevisionDJR";

        public string ReviFinalizadoSGI = "ReviFinalizadoSGI";
        public string ReviFinalizadoDJR = "ReviFinalizadoDJR";

        public string ReviFecFinalizadoSGI = "ReviFecFinalizadoSGI";
        public string ReviFecFinalizadoDJR = "ReviFecFinalizadoDJR";

        public string ReviNotificadoSGI = "ReviNotificadoSGI";
        public string ReviNotificadoDJR = "ReviNotificadoDJR";

        public string ReviFecNotificadoSGI = "ReviFecNotificadoSGI";
        public string ReviFecNotificadoDJR = "ReviFecNotificadoDJR";

        public string ReviEstadoSGI = "ReviEstadoSGI";
        public string ReviEstadoDJR = "ReviEstadoDJR";

        public string ReviEnviadoSGI = "ReviEnviadoSGI";
        public string ReviEnviadoDJR = "ReviEnviadoDJR";

        public string ReviFecEnviadoSGI = "ReviFecEnviadoSGI";
        public string ReviFecEnviadoDJR = "ReviFecEnviadoDJR";

        public string ReviTerminadoSGI = "ReviTerminadoSGI";
        public string ReviTerminadoDJR = "ReviTerminadoDJR";

        public string ReviFecTerminadoSGI = "ReviFecTerminadoSGI";
        public string ReviFecTerminadoDJR = "ReviFecTerminadoDJR";


        #endregion
    }
}
