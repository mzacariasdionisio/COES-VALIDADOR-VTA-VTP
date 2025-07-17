using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_PROPEQUI
    /// </summary>
    public class EqPropequiHelper : HelperBase
    {
        public EqPropequiHelper(): base(Consultas.EqPropequiSql)
        {
        }

        public EqPropequiDTO Create(IDataReader dr)
        {
            EqPropequiDTO entity = new EqPropequiDTO();

            int iPropcodi = dr.GetOrdinal(this.Propcodi);
            if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iFechapropequi = dr.GetOrdinal(this.Fechapropequi);
            if (!dr.IsDBNull(iFechapropequi)) entity.Fechapropequi = Convert.ToDateTime(dr.GetValue(iFechapropequi));

            int iValor = dr.GetOrdinal(this.Valor);
            if (!dr.IsDBNull(iValor)) entity.Valor = Convert.ToString(dr.GetValue(iValor));

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = Convert.ToString(dr.GetValue(iLastuser));

            int iPropequiusucreacion = dr.GetOrdinal(Propequiusucreacion);
            if (!dr.IsDBNull(iPropequiusucreacion)) entity.Propequiusucreacion = Convert.ToString(dr.GetValue(iPropequiusucreacion));
            int iPropequifeccreacion = dr.GetOrdinal(Propequifeccreacion);
            if (!dr.IsDBNull(iPropequifeccreacion)) entity.Propequifeccreacion = Convert.ToDateTime(dr.GetValue(iPropequifeccreacion));

            int iPropequiusumodificacion = dr.GetOrdinal(Propequiusumodificacion);
            if (!dr.IsDBNull(iPropequiusumodificacion)) entity.Propequiusumodificacion = Convert.ToString(dr.GetValue(iPropequiusumodificacion));
            int iPropequifecmodificacion = dr.GetOrdinal(Propequifecmodificacion);
            if (!dr.IsDBNull(iPropequifecmodificacion)) entity.Propequifecmodificacion = Convert.ToDateTime(dr.GetValue(iPropequifecmodificacion));

            int iPropequiobservacion = dr.GetOrdinal(Propequiobservacion);
            if (!dr.IsDBNull(iPropequiobservacion)) entity.Propequiobservacion = Convert.ToString(dr.GetValue(iPropequiobservacion));

            int iPropequideleted = dr.GetOrdinal(Propequideleted);
            if (!dr.IsDBNull(iPropequideleted)) entity.Propequideleted = Convert.ToInt32(dr.GetValue(iPropequideleted));

            int iPropequisustento = dr.GetOrdinal(Propequisustento);
            if (!dr.IsDBNull(iPropequisustento)) entity.Propequisustento = Convert.ToString(dr.GetValue(iPropequisustento));

            int iPropequicheckcero = dr.GetOrdinal(Propequicheckcero);
            if (!dr.IsDBNull(iPropequicheckcero)) entity.Propequicheckcero = Convert.ToInt32(dr.GetValue(iPropequicheckcero));

            int iPropequicomentario = dr.GetOrdinal(Propequicomentario);
            if (!dr.IsDBNull(iPropequicomentario)) entity.Propequicomentario = Convert.ToString(dr.GetValue(iPropequicomentario));

            return entity;
        }


        #region Mapeo de Campos

        public string Propcodi = "PROPCODI";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Fechapropequi = "FECHAPROPEQUI";
        public string Valor = "VALOR";
        public string Propequiobservacion = "PROPEQUIOBSERVACION";
        public string Propequiusucreacion = "PROPEQUIUSUCREACION";
        public string Propequifeccreacion = "PROPEQUIFECCREACION";
        public string Propequiusumodificacion = "PROPEQUIUSUMODIFICACION";
        public string Propequifecmodificacion = "PROPEQUIFECMODIFICACION";
        public string Propequideleted = "PROPEQUIDELETED";
        public string Propequideleted2 = "PROPEQUIDELETED2";

        public string Propequisustento = "PROPEQUISUSTENTO";
        public string Propequicheckcero = "PROPEQUICHECKCERO";
        public string Propequicomentario = "PROPEQUICOMENTARIO";
        public string Lastuser = "LASTUSER";

        //SIOSEIN
        public string Equinomb = "EQUINOMB";
        public string Propnomb = "PROPNOMB";
        public string Propunidad = "PROPUNIDAD";
        public string Propfile = "PROPFILE";
        public string Famcodi = "FAMCODI";

        //SIOSEIN2
        public string Equipadre = "EQUIPADRE";

        
        #region MigracionSGOCOES-GrupoB
        public string Grupocodi = "GRUPOCODI";
        #endregion

        public string Areadesc = "AREADESC";
        public string Areanomb = "Areanomb";
        public string Propocultocomentario = "PROPOCULTOCOMENTARIO";

        #endregion

        public string SqlEliminarHistorico
        {
            get { return base.GetSqlXml("EliminarHistorico"); }
        }

        public string SqlValoresPropiedadesVigentes
        {
            get { return base.GetSqlXml("SqlValoresPropiedadesVigentes"); }
        }

        public string SqlValoresPropiedadesVigentesPaginado
        {
            get { return base.GetSqlXml("SqlValoresPropiedadesVigentesPaginado"); }
        }

        public string SqlTotalValoresPropiedadesVigentesPaginado
        {
            get { return base.GetSqlXml("SqlTotalValoresPropiedadesVigentesPaginado"); }
        }

        public string SqlHistoricoPropiedad
        {
            get { return base.GetSqlXml("SqlHistoricoPropiedad"); }
        }

        public string SqlGetValorPropiedad
        {
            get { return base.GetSqlXml("GetValorPropiedad"); }
        }

        public string SqlObtenerValorPropiedadEquipoFecha
        {
            get { return base.GetSqlXml("ObtenerValorPropiedadEquipoFecha"); }
        }

		public string SqlPropiedadesVigentesEquipoCopiar
        {
            get { return base.GetSqlXml("PropiedadesVigentesEquipoCopiar"); }
        }

        public string SqlListarEquipoConValorModificado
        {
            get { return base.GetSqlXml("ListarEquipoConValorModificado"); }
        }

        #region GestProtect
        public string GetIdCambioEstado
        {
            get { return base.GetSqlXml("GetIdCambioEstado"); }
        }

        public string SaveCambioEstadoFn
        {
            get { return base.GetSqlXml("SaveCambioEstadoFn"); }
        }
        public string SqlSaveCambioEstado
        {
            get { return base.GetSqlXml("SaveCambioEstado"); }
        }
        public string SqlUpdateCambioEstado
        {
            get { return base.GetSqlXml("UpdateCambioEstado"); }
        }
        #endregion



        #region SIOSEIN

        public string SqlGetPotEfectivaAndPotInstaladaPorUnidad
        {
            get { return base.GetSqlXml("GetPotEfectivaAndPotInstaladaPorUnidad"); }
        }
        #endregion

        #region Informe_Ejecutivo_Semanal
        public string SqlGetPotencia
        {
            get { return base.GetSqlXml("ListarPotencia"); }
        }
        #endregion

        #region NotificacionesCambiosEquipamiento
        public string SqlPropiedadesModificadas
        {
            get { return base.GetSqlXml("PropiedadesModificadas"); }
        }
        #endregion

        #region MigracionSGOCOES-GrupoB

        public string Emprnomb = "Emprnomb";
        public string Equifechiniopcom = "Equifechiniopcom";
        public string Equifechfinopcom = "Equifechfinopcom";
        public string Equiabrev = "Equiabrev";
        public string Famnomb = "Famnomb";
        public string Equiestado = "Equiestado";

        #endregion

        #region SIOSEIN2
        public string SqlGetValorPropiedadXEquipoYPropcodi
        {
            get { return base.GetSqlXml("GetValorPropiedadXEquipoYPropcodi"); }
        }
        public string SqlGetValorPropiedadXEquipoYPropcodiCentral
        {
            get { return base.GetSqlXml("GetValorPropiedadXEquipoYPropcodiCentral"); }
        }
        #endregion

        #region Numerales Datos Base 
        public string Osigrupocodi = "OSIGRUPOCODI";

        public string SqlDatosBase_5_6_5
        {
            get { return base.GetSqlXml("ListaDatosBase_5_6_5"); }
        }

        #endregion

        #region GESTPROTEC
            public string Epproycodi = "EPPROYCODI";
            public string Resultado = "RESULTADO";
        #endregion
    }
}
