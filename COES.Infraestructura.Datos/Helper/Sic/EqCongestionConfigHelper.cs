using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_CONGESTION_CONFIG
    /// </summary>
    public class EqCongestionConfigHelper : HelperBase
    {
        public EqCongestionConfigHelper(): base(Consultas.EqCongestionConfigSql)
        {
        }

        public EqCongestionConfigDTO Create(IDataReader dr)
        {
            EqCongestionConfigDTO entity = new EqCongestionConfigDTO();

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iGrulincodi = dr.GetOrdinal(this.Grulincodi);
            if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
            
            //int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            //if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iEstado = dr.GetOrdinal(this.Estado);
            if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

            int iNombrencp = dr.GetOrdinal(this.Nombrencp);
            if(!dr.IsDBNull(iNombrencp))entity.Nombrencp = dr.GetString(iNombrencp);

            int iCodincp = dr.GetOrdinal(this.Codincp);
            if (!dr.IsDBNull(iCodincp)) entity.Codincp = Convert.ToInt32(dr.GetValue(iCodincp));
            
            int iNodobarra1 = dr.GetOrdinal(this.Nodobarra1);
            if (!dr.IsDBNull(iNodobarra1)) entity.Nodobarra1 = dr.GetString(iNodobarra1);

            int iNodobarra2 = dr.GetOrdinal(this.Nodobarra2);
            if (!dr.IsDBNull(iNodobarra2)) entity.Nodobarra2 = dr.GetString(iNodobarra2);

            int iNodobarra3 = dr.GetOrdinal(this.Nodobarra3);
            if (!dr.IsDBNull(iNodobarra3)) entity.Nodobarra3 = dr.GetString(iNodobarra3);

            int iIdems = dr.GetOrdinal(this.Idems);
            if (!dr.IsDBNull(iIdems)) entity.Idems= dr.GetString(iIdems);

            int iNombretna1 = dr.GetOrdinal(this.Nombretna1);
            if (!dr.IsDBNull(iNombretna1)) entity.Nombretna1 = dr.GetString(iNombretna1);

            int iNombretna2 = dr.GetOrdinal(this.Nombretna2);
            if (!dr.IsDBNull(iNombretna2)) entity.Nombretna2 = dr.GetString(iNombretna2);

            int iNombretna3 = dr.GetOrdinal(this.Nombretna3);
            if (!dr.IsDBNull(iNombretna3)) entity.Nombretna3 = dr.GetString(iNombretna3);

            return entity;
        }

        #region Mapeo de Campos

        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Grulincodi = "GRULINCODI";
        public string Configcodi = "CONFIGCODI";
        public string Equicodi = "EQUICODI";
        public string Canalcodi = "CANALCODI";
        public string Estado = "ESTADO";
        public string Nombrencp = "NOMBRENCP";
        public string Codincp = "CODINCP";        
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Grulinnomb = "GRULINNOMB";
        public string Nodobarra1 = "NODOBARRA1";
        public string Nodobarra2 = "NODOBARRA2";
        public string Nodobarra3 = "NODOBARRA3";        
        public string Idems = "IDEMS";
        public string Famcodi = "FAMCODI";
        public string Nombretna1 = "NOMBRETNA1";
        public string Nombretna2 = "NOMBRETNA2";
        public string Nombretna3 = "NOMBRETNA3";

        public string SqlObtenerPorGrupo
        {
            get { return base.GetSqlXml("ObtenerPorGrupo"); }
        }

        public string SqlObtenerEmpresasFiltro
        {
            get { return base.GetSqlXml("ObtenerEmpresasFiltro"); }
        }

        public string SqlObtenerEmpresasLineas
        {
            get { return base.GetSqlXml("ObtenerEmpresasLineas"); }
        }

        public string SqlObtenerEmpresasLineaTrafo
        {
            get { return base.GetSqlXml("ObtenerEmpresasLineaTrafo"); }
        }
                
        public string SqlListarEquipoLineaEmpresa
        {
            get { return base.GetSqlXml("ListarEquipoLineaEmpresa"); }
        }

        public string SqlListarEquipoLineaTrafoEmpresa
        {
            get { return base.GetSqlXml("ListarEquipoLineaTrafoEmpresa"); }
        }               

        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }        
        }

        public string SqlObtenerLineaPorGrupo
        {
            get { return base.GetSqlXml("ObtenerLineaPorGrupo"); }
        }

        public string SqlObtenerListadoEquipos
        {
            get { return base.GetSqlXml("ObtenerListadoEquipos"); }
        }


        #endregion
    }
}
