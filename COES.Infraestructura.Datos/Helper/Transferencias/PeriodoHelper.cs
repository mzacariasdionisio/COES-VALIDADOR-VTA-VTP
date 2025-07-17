using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla trn_periodo
    /// </summary>
    public class PeriodoHelper : HelperBase
    {
        public PeriodoHelper() : base(Consultas.PeriodoSql)
        {
        }

        public PeriodoDTO Create(IDataReader dr)
        {
            PeriodoDTO entity = new PeriodoDTO();

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.PeriCodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iPerinombre = dr.GetOrdinal(this.Perinombre);
            if (!dr.IsDBNull(iPerinombre)) entity.PeriNombre = dr.GetString(iPerinombre);

            int iAniocodi = dr.GetOrdinal(this.Aniocodi);
            if (!dr.IsDBNull(iAniocodi)) entity.AnioCodi = Convert.ToInt32(dr.GetValue(iAniocodi));

            int iMescodi = dr.GetOrdinal(this.Mescodi);
            if (!dr.IsDBNull(iMescodi)) entity.MesCodi = Convert.ToInt32(dr.GetValue(iMescodi));

            int iRecanombre = dr.GetOrdinal(this.Recanombre);
            if (!dr.IsDBNull(iRecanombre)) entity.RecaNombre = dr.GetString(iRecanombre);

            int iPerifechavalorizacion = dr.GetOrdinal(this.Perifechavalorizacion);
            if (!dr.IsDBNull(iPerifechavalorizacion)) entity.PeriFechaValorizacion = dr.GetDateTime(iPerifechavalorizacion);

            int iPerifechalimite = dr.GetOrdinal(this.Perifechalimite);
            if (!dr.IsDBNull(iPerifechalimite)) entity.PeriFechaLimite = dr.GetDateTime(iPerifechalimite);

            int iPerifechaobservacion = dr.GetOrdinal(this.Perifechaobservacion);
            if (!dr.IsDBNull(iPerifechaobservacion)) entity.PeriFechaObservacion = dr.GetDateTime(iPerifechaobservacion);

            int iPeriestado = dr.GetOrdinal(this.Periestado);
            if (!dr.IsDBNull(iPeriestado)) entity.PeriEstado = dr.GetString(iPeriestado);

            int iPeriusername = dr.GetOrdinal(this.Periusername);
            if (!dr.IsDBNull(iPeriusername)) entity.PeriUserName = dr.GetString(iPeriusername);

            int iPerifecins = dr.GetOrdinal(this.Perifecins);
            if (!dr.IsDBNull(iPerifecins)) entity.PeriFecIns = dr.GetDateTime(iPerifecins);

            int iPerifecact = dr.GetOrdinal(this.Perifecact);
            if (!dr.IsDBNull(iPerifecact)) entity.PerifecAct = dr.GetDateTime(iPerifecact);

            int iPerianiomes = dr.GetOrdinal(this.Perianiomes);
            if (!dr.IsDBNull(iPerianiomes)) entity.PeriAnioMes = Convert.ToInt32(dr.GetValue(iPerianiomes));

            int iPerihoralimite = dr.GetOrdinal(this.Perihoralimite);
            if (!dr.IsDBNull(iPerihoralimite)) entity.PeriHoraLimite = dr.GetString(iPerihoralimite);

            int iPeriFormNuevo = dr.GetOrdinal(this.PeriFormNuevo);
            if (!dr.IsDBNull(iPeriFormNuevo)) entity.PeriFormNuevo = Convert.ToInt32(dr.GetValue(iPeriFormNuevo));

            return entity;
        }

        #region Mapeo de Campos

        public string Pericodi = "PERICODI";
        public string Perinombre = "PERINOMBRE";
        public string Aniocodi = "PERIANIO";
        public string Mescodi = "PERIMES";
        public string Recanombre = "RECANOMBRE";
        public string Perifechavalorizacion = "PERIFECHAVALORIZACION";
        public string Perifechalimite = "PERIFECHALIMITE";
        public string Perihoralimite = "PERIHORALIMITE";
        public string Perifechaobservacion = "PERIFECHAOBSERVACION";
        public string Periestado = "PERIESTADO";
        public string Periusername = "PERIUSERNAME";
        public string Perifecins = "PERIFECINS";
        public string Perifecact = "PERIFECACT";
        public string Perianiomes = "PERIANIOMES";

        // Inicio de Agregados - Sistema de Compensaciones
        public string PeriDescripcion = "PERIDESCRIPCION";
        public string Fechaini = "FECHAINI";
        public string Fechafin = "FECHAFIN";
        public string Pecanombre = "PECANOMBRE";
        public string Pecaversioncomp = "PECAVERSIONCOMP";
        public string Pecaversionvtea = "PECAVERSIONVTEA";
        public string RecaNombreComp = "RECANOMBRECOMP";
        public string Pecaestadoregistro = "PECAESTREGISTRO";
        public string Pecadscestado = "PECADSCESTADO";
        public string Periinforme = "PERIINFORME";
        public string PeriFormNuevo = "PERIFORMNUEVO";
        // Fin de Agregados - Sistema de Compensaciones

        //2018.Setiembre - Agregados por ASSETEC para la tabla TRN_CONTADOR
        public string Trncntcodi = "TRNCNTCODI";
        public string Trncntcontador = "TRNCNTCONTADOR";
        public string Trncnttabla = "TRNCNTTABLA";
        public string Trncntcolumna = "TRNCNTCOLUMNA";
        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlGetByAnioMes
        {
            get { return base.GetSqlXml("GetByAnioMes"); }
        }

        public string SqlGetNumRegistros
        {
            get { return base.GetSqlXml("GetNumRegistros"); }
        }

        public string SqlListarByEstado
        {
            get { return base.GetSqlXml("ListarByEstado"); }
        }

        public string SqlListarByEstadoPublicarCerrado 
        {
            get { return base.GetSqlXml("ListarByEstadoPublicarCerrado"); }
        }

        public string SqlGetPeriodoAnteriorById 
        {
            get { return base.GetSqlXml("BuscarPeriodoAnterior"); }
        }

        public string SqlListarPeriodosFuturos
        {
            get { return base.GetSqlXml("ListarPeriodosFuturos"); }
        }

        public string SqlObtenerPeriodoDTR
        {
            get { return base.GetSqlXml("ObtenerPeriodoDTR"); }
        }

        // Inicio de Agregados - Sistema de Compensaciones
        public string SqlListarPeriodosTC
        {
            get { return base.GetSqlXml("ListarPeriodosTC"); }
        }

        public string SqlListPeriodoByIdProcesa
        {
            get { return base.GetSqlXml("ListPeriodoByIdProcesa"); }
        }
        
        public string SqlListarPeriodosCompensacion
        {
            get { return base.GetSqlXml("ListarPeriodosCompensacion"); }
        }
        // Fin de Agregados - Sistema de Compensaciones

        //2018.Setiembre - Agregados por ASSETEC para la tabla TRN_CONTADOR
        public string SqlGetPKTrnContador
        {
            get { return base.GetSqlXml("GetPKTrnContador"); }
        }

        public string SqlUpdatePKTrnContador
        {
            get { return base.GetSqlXml("UpdatePKTrnContador"); }
        }

        public string SqlListPeriodoPotencia
        {
            get { return base.GetSqlXml("ListPeriodoPotencia"); }
        }
        public string SqlGetFirstPeriodoFormatNew
        {
            get { return base.GetSqlXml("GetFirstPeriodoFormatNew"); }
        }
    }
}


