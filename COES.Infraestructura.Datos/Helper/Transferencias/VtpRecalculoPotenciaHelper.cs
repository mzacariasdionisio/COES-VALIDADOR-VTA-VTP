using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_RECALCULO_POTENCIA
    /// </summary>
    public class VtpRecalculoPotenciaHelper : HelperBase
    {
        public VtpRecalculoPotenciaHelper()
            : base(Consultas.VtpRecalculoPotenciaSql)
        {
        }

        public VtpRecalculoPotenciaDTO Create(IDataReader dr)
        {
            VtpRecalculoPotenciaDTO entity = new VtpRecalculoPotenciaDTO();

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iRecpotnombre = dr.GetOrdinal(this.Recpotnombre);
            if (!dr.IsDBNull(iRecpotnombre)) entity.Recpotnombre = dr.GetString(iRecpotnombre);

            int iRecpotinfovtp = dr.GetOrdinal(this.Recpotinfovtp);
            if (!dr.IsDBNull(iRecpotinfovtp)) entity.Recpotinfovtp = dr.GetString(iRecpotinfovtp);

            int iRecpotfactincecontrantacion = dr.GetOrdinal(this.Recpotfactincecontrantacion);
            if (!dr.IsDBNull(iRecpotfactincecontrantacion)) entity.Recpotfactincecontrantacion = dr.GetDecimal(iRecpotfactincecontrantacion);

            int iRecpotfactincedespacho = dr.GetOrdinal(this.Recpotfactincedespacho);
            if (!dr.IsDBNull(iRecpotfactincedespacho)) entity.Recpotfactincedespacho = dr.GetDecimal(iRecpotfactincedespacho);

            int iRecpotmaxidemamensual = dr.GetOrdinal(this.Recpotmaxidemamensual);
            if (!dr.IsDBNull(iRecpotmaxidemamensual)) entity.Recpotmaxidemamensual = dr.GetDecimal(iRecpotmaxidemamensual);

            int iRecpotinterpuntames = dr.GetOrdinal(this.Recpotinterpuntames);
            if (!dr.IsDBNull(iRecpotinterpuntames)) entity.Recpotinterpuntames = dr.GetDateTime(iRecpotinterpuntames);

            int iRecpotpreciopoteppm = dr.GetOrdinal(this.Recpotpreciopoteppm);
            if (!dr.IsDBNull(iRecpotpreciopoteppm)) entity.Recpotpreciopoteppm = dr.GetDecimal(iRecpotpreciopoteppm);
            
            int iRecpotpreciopeajeppm = dr.GetOrdinal(this.Recpotpreciopeajeppm);
            if (!dr.IsDBNull(iRecpotpreciopeajeppm)) entity.Recpotpreciopeajeppm = dr.GetDecimal(iRecpotpreciopeajeppm);

            int iRecpotpreciocostracionamiento = dr.GetOrdinal(this.Recpotpreciocostracionamiento);
            if (!dr.IsDBNull(iRecpotpreciocostracionamiento)) entity.Recpotpreciocostracionamiento = dr.GetDecimal(iRecpotpreciocostracionamiento);

            int iRecpotpreciodemaservauxiliares = dr.GetOrdinal(this.Recpotpreciodemaservauxiliares);
            if (!dr.IsDBNull(iRecpotpreciodemaservauxiliares)) entity.Recpotpreciodemaservauxiliares = dr.GetDecimal(iRecpotpreciodemaservauxiliares);

            int iRecpotconsumidademanda = dr.GetOrdinal(this.Recpotconsumidademanda);
            if (!dr.IsDBNull(iRecpotconsumidademanda)) entity.Recpotconsumidademanda = dr.GetDecimal(iRecpotconsumidademanda);

            int iRecpotfechalimite = dr.GetOrdinal(this.Recpotfechalimite);
            if (!dr.IsDBNull(iRecpotfechalimite)) entity.Recpotfechalimite = dr.GetDateTime(iRecpotfechalimite);

            int iRecpothoralimite = dr.GetOrdinal(this.Recpothoralimite);
            if (!dr.IsDBNull(iRecpothoralimite)) entity.Recpothoralimite = dr.GetString(iRecpothoralimite);

            int iRecpotcuadro1 = dr.GetOrdinal(this.Recpotcuadro1);
            if (!dr.IsDBNull(iRecpotcuadro1)) entity.Recpotcuadro1 = dr.GetString(iRecpotcuadro1);

            int iRecpotnota1 = dr.GetOrdinal(this.Recpotnota1);
            if (!dr.IsDBNull(iRecpotnota1)) entity.Recpotnota1 = dr.GetString(iRecpotnota1);

            int iRecpotcomegeneral = dr.GetOrdinal(this.Recpotcomegeneral);
            if (!dr.IsDBNull(iRecpotcomegeneral)) entity.Recpotcomegeneral = dr.GetString(iRecpotcomegeneral);

            int iRecacodi = dr.GetOrdinal(this.Recacodi);
            if (!dr.IsDBNull(iRecacodi)) entity.Recacodi = Convert.ToInt32(dr.GetValue(iRecacodi));

            int iPericodidestino = dr.GetOrdinal(this.Pericodidestino);
            if (!dr.IsDBNull(iPericodidestino)) entity.Pericodidestino = Convert.ToInt32(dr.GetValue(iPericodidestino));

            int iRecpotestado = dr.GetOrdinal(this.Recpotestado);
            if (!dr.IsDBNull(iRecpotestado)) entity.Recpotestado = dr.GetString(iRecpotestado);

            int iRecpotusucreacion = dr.GetOrdinal(this.Recpotusucreacion);
            if (!dr.IsDBNull(iRecpotusucreacion)) entity.Recpotusucreacion = dr.GetString(iRecpotusucreacion);

            int iRecpotfeccreacion = dr.GetOrdinal(this.Recpotfeccreacion);
            if (!dr.IsDBNull(iRecpotfeccreacion)) entity.Recpotfeccreacion = dr.GetDateTime(iRecpotfeccreacion);

            int iRecpotusumodificacion = dr.GetOrdinal(this.Recpotusumodificacion);
            if (!dr.IsDBNull(iRecpotusumodificacion)) entity.Recpotusumodificacion = dr.GetString(iRecpotusumodificacion);

            int iRecpotfecmodificacion = dr.GetOrdinal(this.Recpotfecmodificacion);
            if (!dr.IsDBNull(iRecpotfecmodificacion)) entity.Recpotfecmodificacion = dr.GetDateTime(iRecpotfecmodificacion);

            int iRecpotcargapfr = dr.GetOrdinal(this.Recpotcargapfr);
            if (!dr.IsDBNull(iRecpotcargapfr)) entity.Recpotcargapfr = Convert.ToInt32(dr.GetValue(iRecpotcargapfr));

            return entity;
        }


        #region Mapeo de Campos

        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Recpotnombre = "RECPOTNOMBRE";
        public string Recpotinfovtp = "RECPOTINFOVTP";
        public string Recpotfactincecontrantacion = "RECPOTFACTINCECONTRANTACION";
        public string Recpotfactincedespacho = "RECPOTFACTINCEDESPACHO";
        public string Recpotmaxidemamensual = "RECPOTMAXIDEMAMENSUAL";
        public string Recpotinterpuntames = "RECPOTINTERPUNTAMES";
        public string Recpotpreciopoteppm = "RECPOTPRECIOPOTEPPM";
        public string Recpotpreciopeajeppm = "RECPOTPRECIOPEAJEPPM";
        public string Recpotpreciocostracionamiento = "RECPOTPRECIOCOSTRACIONAMIENTO";
        public string Recpotpreciodemaservauxiliares = "RECPOTPRECIODEMASERVAUXILIARES";
        public string Recpotconsumidademanda = "RECPOTCONSUMIDADEMANDA";
        public string Recpotfechalimite = "RECPOTFECHALIMITE";
        public string Recpothoralimite = "RECPOTHORALIMITE";
        public string Recpotcuadro1 = "RECPOTCUADRO1";
        public string Recpotnota1 = "RECPOTNOTA1";
        public string Recpotcomegeneral = "RECPOTCOMEGENERAL";
        public string Recacodi = "RECACODI";
        public string Pericodidestino = "PERICODIDESTINO";
        public string Recpotestado = "RECPOTESTADO";
        public string Recpotusucreacion = "RECPOTUSUCREACION";
        public string Recpotfeccreacion = "RECPOTFECCREACION";
        public string Recpotusumodificacion = "RECPOTUSUMODIFICACION";
        public string Recpotfecmodificacion = "RECPOTFECMODIFICACION";
        public string Recpotcargapfr = "RECPOTCARGAPFR";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Perinombre = "PERINOMBRE";
        public string Recanombre = "RECANOMBRE";

        public string Perinombredestino = "PERINOMBREDESTINO";  // PrimasRER.2023
        public string Perianio = "PERIANIO";                    // PrimasRER.2023
        public string Perimes = "PERIMES";                      // PrimasRER.2023
        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListView
        {
            get { return base.GetSqlXml("ListView"); }
        }

        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }

        public string SqlListByPericodi
        {
            get { return base.GetSqlXml("ListByPericodi"); }
        }

        #region FIT - Aplicativo VTD
        public string SqlGetPrecioPotencia
        {
            get { return base.GetSqlXml("GetPrecioPotencia"); }
        }
        #endregion

        //ASSETEC 202108 - TIEE
        public string SqlListMaxRecalculoByPeriodo
        {
            get { return base.GetSqlXml("ListMaxRecalculoByPeriodo"); }
        }

        public string SqlMigrarSaldosVTP
        {
            get { return base.GetSqlXml("MigrarSaldosVTP"); }
        }

        public string SqlMigrarCalculoVTP
        {
            get { return base.GetSqlXml("MigrarCalculoVTP"); }
        }
        #region PrimasRER.2023
        public string SqlListVTP
        {
            get { return base.GetSqlXml("ListVTP"); }
        }
        public string SqlGetByIdCerrado
        {
            get { return base.GetSqlXml("GetByIdCerrado"); }
        }
        #endregion

        #region CPA - CU05
        public string SqlListRecalculoByPeriodo
        {
            get { return base.GetSqlXml("ListRecalculoByPeriodo"); }
        }
        #endregion
    }
}