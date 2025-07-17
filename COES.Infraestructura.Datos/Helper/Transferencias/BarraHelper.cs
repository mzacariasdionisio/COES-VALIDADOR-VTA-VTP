using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_BARRA
    /// </summary>
    public class BarraHelper : HelperBase
    {
        public BarraHelper() : base(Consultas.BarraSql)
        {
        }

        public BarraDTO Create(IDataReader dr)
        {
            BarraDTO entity = new BarraDTO();

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = dr.GetInt32(iBarrcodi);

            int iBarrnombre = dr.GetOrdinal(this.Barrnombre);
            if (!dr.IsDBNull(iBarrnombre)) entity.BarrNombre = dr.GetString(iBarrnombre);

            int iBarrtension = dr.GetOrdinal(this.Barrtension);
            if (!dr.IsDBNull(iBarrtension)) entity.BarrTension = dr.GetString(iBarrtension);

            int iBarrpuntosumirer = dr.GetOrdinal(this.Barrpuntosumirer);
            if (!dr.IsDBNull(iBarrpuntosumirer)) entity.BarrPuntoSumirer = dr.GetString(iBarrpuntosumirer);

            int iBarrbarrabgr = dr.GetOrdinal(this.Barrbarrabgr);
            if (!dr.IsDBNull(iBarrbarrabgr)) entity.BarrBarraBgr = dr.GetString(iBarrbarrabgr);

            int iBarrestado = dr.GetOrdinal(this.Barrestado);
            if (!dr.IsDBNull(iBarrestado)) entity.BarrEstado = dr.GetString(iBarrestado);

            int iBarrflagbarrtran = dr.GetOrdinal(this.Barrflagbarrtran);
            if (!dr.IsDBNull(iBarrflagbarrtran)) entity.BarrFlagBarrTran = dr.GetString(iBarrflagbarrtran);

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.AreaCodi = dr.GetInt32(iAreacodi);

            int iBarrnombbarrtran = dr.GetOrdinal(this.Barrnombbarrtran);
            if (!dr.IsDBNull(iBarrnombbarrtran)) entity.BarrNombBarrTran = dr.GetString(iBarrnombbarrtran);

            int iBarrflagdesbalance = dr.GetOrdinal(this.Barrflagdesblance);
            if (!dr.IsDBNull(iBarrflagdesbalance)) entity.BarrFlagDesbalance = dr.GetString(iBarrflagdesbalance);
            
            int iBarrusername = dr.GetOrdinal(this.Barrusername);
            if (!dr.IsDBNull(iBarrusername)) entity.BarrUserName = dr.GetString(iBarrusername);

            int iBarrfecins = dr.GetOrdinal(this.Barrfecins);
            if (!dr.IsDBNull(iBarrfecins)) entity.BarrFecIns = dr.GetDateTime(iBarrfecins);

            int iBarrfecact = dr.GetOrdinal(this.Barrfecact);
            if (!dr.IsDBNull(iBarrfecact)) entity.BarrFecAct = dr.GetDateTime(iBarrfecact);

            if (dr[this.Barrfactorperdida] != null)
            {
                int iBarrfactorperdida = dr.GetOrdinal(this.Barrfactorperdida);
                if (!dr.IsDBNull(iBarrfactorperdida)) entity.BarrFactorPerdida = dr.GetDecimal(iBarrfactorperdida);
            }

            if (dr[this.OsinergCodi] != null)
            {
                int IOsinergCodi = dr.GetOrdinal(this.OsinergCodi);
                if (!dr.IsDBNull(IOsinergCodi)) entity.OsinergCodi = dr.GetString(IOsinergCodi);
            }

            return entity;
        }

        #region Mapeo de Campos

        public string Barrcodi = "BARRCODI";
        public string Barrcoditra = "BARRCODITRA";
        public string Genemprcodi = "GENEMPRCODI";
        public string Cliemprcodi = "CLIEMPRCODI";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrtension = "BARRTENSION";
        public string Barrpuntosumirer = "BARRPUNTOSUMINISTRORER";
        public string Barrbarrabgr = "BARRBARRABGR";
        public string Barrestado = "BARRESTADO";
        public string Barrflagbarrtran = "BARRFLAGBARRATRANSFERENCIA";
        public string Areacodi = "AREACODI";
        public string Barrnombbarrtran = "BARRBARRATRANSFERENCIA";
        public string Barrflagdesblance = "BARRFLAGDESBALANCE";
        public string Barrusername = "BARRUSERNAME";
        public string Barrfecins = "BARRFECINS";
        public string Barrfecact = "BARRFECACT";
        public string Barrfactorperdida = "BARRFACTORPERDIDA";
        public string AreaNombre = "AREANOMB";

        #region CPPA-2024
        public string Barrbarratransferencia = "BARRBARRATRANSFERENCIA";
        public string Barrnombreconcatenado = "BARRNOMBRECONCATENADO";
        #endregion

        #region MonitoreoMME
        public string Emprcodi = "EMPRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Grupopadre = "GRUPOPADRE";
        public string Emprnomb = "EMPRNOMB";
        public string Gruponomb = "GRUPONOMB";
        #endregion

        #region SIOSEIN-PRIE-2021
        public string OsinergCodi = "OSINERGCODI";

        #endregion

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListaBarraTransferencia
        {
            get { return base.GetSqlXml("ListaBarraTransferencia"); }
        }

        public string SqlListaBarraSuministro
        {
            get { return base.GetSqlXml("ListaBarraSuministro"); }
        }

        public string SqlListarBarrasSuministrosRelacionada
        {
            get { return base.GetSqlXml("ListarBarrasSuministrosRelacionada"); }
        }

        public string SqlListaInterCodEnt
        {
            get { return base.GetSqlXml("ListaInterCodEnt"); }

        }

        public string SqlListaInterCoReSo
        {
            get { return base.GetSqlXml("ListaInterCoReSo"); }

        }
        public string SqlListaInterCoReSoByEmpr
        {
            get { return base.GetSqlXml("ListaInterCoReSoByEmpr"); }

        }
        public string SqlListaBarraRetirosEmpresa
        {
            get { return base.GetSqlXml("ListaBarraRetirosEmpresa"); }

        }
        public string SqlListaBarraEntregaEmpresa
        {
            get { return base.GetSqlXml("ListaBarraEntregaEmpresa"); }

        }
        public string SqlListaBarraEmpresaValorizados
        {
            get { return base.GetSqlXml("ListaBarraEmpresaValorizados"); }

        }
        
        public string SqlListaInterCoReGeByEmpr
        {
            get { return base.GetSqlXml("ListaInterCoReGeByEmpr"); }

        }
        
        public string SqlListarTodasLasBarras
        {
            get { return base.GetSqlXml("ListarTodasLasBarras"); }

        }
        public string SqlListaInterCoReSoDt
        {
            get { return base.GetSqlXml("ListaInterCoReSoDt"); }

        }

        public string SqlListaInterCoReSC
        {
            get { return base.GetSqlXml("ListaInterCoReSC"); }

        }

        public string SqlListaInterValorTrans
        {
            get { return base.GetSqlXml("ListaInterValorTrans"); }

        }

        public string SqlListVista
        {
            get { return base.GetSqlXml("ListVista"); }

        }

        public string SqlListaInterCodInfoBase 
        {
            get { return base.GetSqlXml("ListaInterCodInfoBase"); }
        }

        public string SqlListBarrasTransferenciaByReporte
        {
            get { return base.GetSqlXml("ListBarrasTransferenciaByReporte"); }

        }

        public string SqlGetByBarra
        {
            get { return base.GetSqlXml("GetByBarra"); }
        }

        public string SqlListarBarraReporteDTR
        {
            get { return base.GetSqlXml("ListarBarraReporteDTR"); }
        }

        public string SqlObtenerBarraDTR
        {
            get { return base.GetSqlXml("ObtenerBarraDTR"); }
        }

        // Inicio de Agregado - Sistema de Compensaciones
        public string SqlListByBarraCompensacion
        {
            get { return GetSqlXml("SqlListByBarraCompensacion"); }
        }
        // Fin de Agregado - Sistema de Compensaciones
        
        #region SIOSEIN
        public string SqlGetListaBarraArea
        {
            get { return GetSqlXml("GetListaBarraArea"); }
        }
        #endregion

        #region MonitoreoMME

        public string SqlListarGrupoBarraEjec
        {
            get { return base.GetSqlXml("ListarGrupoBarraEjec"); }
        }

        #endregion

        #region siosein2
        public string SqlListaCentralxBarra
        {
            get { return GetSqlXml("ListaCentralxBarra"); }
        }

        public string Equicodi = "Equicodi";
        #endregion

        public string SqlListaBarrasActivas
        {
            get { return base.GetSqlXml("ListaBarrasActivas"); }
        }
        #region SIOSEIN-PRIE-2021
        public string SqlGetBarraAreaByOsinerming
        {
            get { return GetSqlXml("GetBarraAreaByOsinerming"); }
        }

        #endregion

        #region CPPA-2024
        public string SqlFiltroBarrasTransIntegrantes
        {
            get { return base.GetSqlXml("FiltroBarrasTransIntegrantes"); }
        }
        public string SqlListaBarrasTransFormato
        {
            get { return base.GetSqlXml("ListaBarrasTransFormato"); }
        }
        #endregion

    }
}
