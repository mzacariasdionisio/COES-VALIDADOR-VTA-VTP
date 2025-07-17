using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    class PmoConfIndispEquipoHelper: HelperBase
    {
        public PmoConfIndispEquipoHelper()
            : base(Consultas.PmoConfIndispEquipo)
        {
        }
        public PmoConfIndispEquipoDTO Create(IDataReader dr)
        {
            PmoConfIndispEquipoDTO entity = new PmoConfIndispEquipoDTO();

            int iPmCindCodi = dr.GetOrdinal(this.PmCindCodi);
            if (!dr.IsDBNull(iPmCindCodi)) entity.PmCindCodi = dr.GetInt32(iPmCindCodi);

            int iSddpcodi = dr.GetOrdinal(this.Sddpcodi);
            if (!dr.IsDBNull(iSddpcodi)) entity.Sddpcodi = Convert.ToInt32(dr.GetValue(iSddpcodi));

            int iEquiCodi = dr.GetOrdinal(this.EquiCodi);
            if (!dr.IsDBNull(iEquiCodi)) entity.EquiCodi = dr.GetInt32(iEquiCodi);

            int iPmCindPorcentaje = dr.GetOrdinal(this.PmCindPorcentaje);
            if (!dr.IsDBNull(iPmCindPorcentaje)) entity.PmCindPorcentaje = dr.GetDecimal(iPmCindPorcentaje);

            int iPmCindEstRegistro = dr.GetOrdinal(this.PmCindEstRegistro);
            if (!dr.IsDBNull(iPmCindEstRegistro)) entity.PmCindEstRegistro = dr.GetString(iPmCindEstRegistro);

            int iPmCindUsuCreacion = dr.GetOrdinal(this.PmCindUsuCreacion);
            if (!dr.IsDBNull(iPmCindUsuCreacion)) entity.PmCindUsuCreacion = dr.GetString(iPmCindUsuCreacion);

            int iPmCindFecCreacion = dr.GetOrdinal(this.PmCindFecCreacion);
            if (!dr.IsDBNull(iPmCindFecCreacion)) entity.PmCindFecCreacion = dr.GetDateTime(iPmCindFecCreacion);

            int iPmCindUsuModificacion = dr.GetOrdinal(this.PmCindUsuModificacion);
            if (!dr.IsDBNull(iPmCindUsuModificacion)) entity.PmCindUsuModificacion = dr.GetString(iPmCindUsuModificacion);

            int iPmCindFecModificacion = dr.GetOrdinal(this.PmCindFecModificacion);
            if (!dr.IsDBNull(iPmCindFecModificacion)) entity.PmCindFecModificacion = dr.GetDateTime(iPmCindFecModificacion);

            int iPmCindConJuntoEqp = dr.GetOrdinal(this.PmCindConJuntoEqp);
            if (!dr.IsDBNull(iPmCindConJuntoEqp)) entity.PmCindConJuntoEqp = dr.GetString(iPmCindConJuntoEqp);

            int iPmCindRelInversa = dr.GetOrdinal(this.PmCindRelInversa);
            if (!dr.IsDBNull(iPmCindRelInversa)) entity.PmCindRelInversa = dr.GetString(iPmCindRelInversa);

            int iGrupocodimodo = dr.GetOrdinal(this.Grupocodimodo);
            if (!dr.IsDBNull(iGrupocodimodo)) entity.Grupocodimodo = dr.GetInt32(iGrupocodimodo);

            return entity;
        }

        #region Mapeo de Campos PmoConfIndispEquipo
                
        public string PmCindCodi = "PMCINDCODI";
        public string Sddpcodi = "SDDPCODI";
        public string GrupoCodi = "GRUPOCODI";
        public string EquiCodi = "EQUICODI";
        public string PmCindPorcentaje = "PMCINDPORCENTAJE";
        public string PmCindEstRegistro = "PMCINDESTREGISTRO";
        public string PmCindUsuCreacion = "PMCINDUSUCREACION";
        public string PmCindFecCreacion = "PMCINDFECCREACION";
        public string PmCindUsuModificacion = "PMCINDUSUMODIFICACION";
        public string PmCindFecModificacion = "PMCINDFECMODIFICACION";
        public string PmCindConJuntoEqp = "PMCINDCONJUNTOEQP";
        public string PmCindRelInversa = "PMCINDRELINVERSA";
        public string Grupocodimodo = "Grupocodimodo";

        public string Catenomb = "CATENOMB";
        public string UsuarioMod = "USUARIO_MOD";
        public string FechaMod = "FECHA_MOD";

        #endregion

        #region Mapeo de Campos adicional List

        public string GrupoCodiSDDP = "GRUPOCODISDDP";
        public string GrupoNomb = "GRUPONOMB";
        public string EmprCodi = "EMPRCODI";
        public string Emprnomb = "Emprnomb";
        public string Equipadre = "EQUIPADRE";
        public string EquiAbrev = "EQUIABREV";
        public string AreaNomb = "AREANOMB";
        public string TareaAbrev = "TAREAABREV";

        #endregion

        public string CateCodi = "CATECODI";

        public string FamCodi = "FAMCODI"; 
        public string FamNombr = "Famnomb";
        public string Famabrev = "Famabrev";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Ptomedielenomb = "Ptomedielenomb";
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Tsddpcodi = "TSDDPCODI";

        public string SqlEliminarCorrelacion
        {
            get { return base.GetSqlXml("EliminarCorrelacion"); }
        }

    }
}
