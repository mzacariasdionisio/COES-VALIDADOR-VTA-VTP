using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_CAMBIO_TURNO_SUBSECCION
    /// </summary>
    public class SiCambioTurnoSubseccionHelper : HelperBase
    {
        public SiCambioTurnoSubseccionHelper(): base(Consultas.SiCambioTurnoSubseccionSql)
        {
        }

        public SiCambioTurnoSubseccionDTO Create(IDataReader dr)
        {
            SiCambioTurnoSubseccionDTO entity = new SiCambioTurnoSubseccionDTO();

            int iSubseccioncodi = dr.GetOrdinal(this.Subseccioncodi);
            if (!dr.IsDBNull(iSubseccioncodi)) entity.Subseccioncodi = Convert.ToInt32(dr.GetValue(iSubseccioncodi));

            int iSumhorainicio = dr.GetOrdinal(this.Sumhorainicio);
            if (!dr.IsDBNull(iSumhorainicio)) entity.Sumhorainicio = dr.GetString(iSumhorainicio);

            int iSumreposicion = dr.GetOrdinal(this.Sumreposicion);
            if (!dr.IsDBNull(iSumreposicion)) entity.Sumreposicion = dr.GetString(iSumreposicion);

            int iSumconsideraciones = dr.GetOrdinal(this.Sumconsideraciones);
            if (!dr.IsDBNull(iSumconsideraciones)) entity.Sumconsideraciones = dr.GetString(iSumconsideraciones);

            int iRegopecentral = dr.GetOrdinal(this.Regopecentral);
            if (!dr.IsDBNull(iRegopecentral)) entity.Regopecentral = dr.GetString(iRegopecentral);

            int iRegcentralsubestacion = dr.GetOrdinal(this.Regcentralsubestacion);
            if (!dr.IsDBNull(iRegcentralsubestacion)) entity.Regcentralsubestacion = dr.GetString(iRegcentralsubestacion);

            int iRegcentralhorafin = dr.GetOrdinal(this.Regcentralhorafin);
            if (!dr.IsDBNull(iRegcentralhorafin)) entity.Regcentralhorafin = dr.GetString(iRegcentralhorafin);

            int iReglineas = dr.GetOrdinal(this.Reglineas);
            if (!dr.IsDBNull(iReglineas)) entity.Reglineas = dr.GetString(iReglineas);

            int iReglineasubestacion = dr.GetOrdinal(this.Reglineasubestacion);
            if (!dr.IsDBNull(iReglineasubestacion)) entity.Reglineasubestacion = dr.GetString(iReglineasubestacion);

            int iReglineahorafin = dr.GetOrdinal(this.Reglineahorafin);
            if (!dr.IsDBNull(iReglineahorafin)) entity.Reglineahorafin = dr.GetString(iReglineahorafin);

            int iGesequipo = dr.GetOrdinal(this.Gesequipo);
            if (!dr.IsDBNull(iGesequipo)) entity.Gesequipo = dr.GetString(iGesequipo);

            int iGesaceptado = dr.GetOrdinal(this.Gesaceptado);
            if (!dr.IsDBNull(iGesaceptado)) entity.Gesaceptado = dr.GetString(iGesaceptado);

            int iGesdetalle = dr.GetOrdinal(this.Gesdetalle);
            if (!dr.IsDBNull(iGesdetalle)) entity.Gesdetalle = dr.GetString(iGesdetalle);

            int iEveequipo = dr.GetOrdinal(this.Eveequipo);
            if (!dr.IsDBNull(iEveequipo)) entity.Eveequipo = dr.GetString(iEveequipo);

            int iEveresumen = dr.GetOrdinal(this.Everesumen);
            if (!dr.IsDBNull(iEveresumen)) entity.Everesumen = dr.GetString(iEveresumen);

            int iEvehorainicio = dr.GetOrdinal(this.Evehorainicio);
            if (!dr.IsDBNull(iEvehorainicio)) entity.Evehorainicio = dr.GetString(iEvehorainicio);

            int iEvereposicion = dr.GetOrdinal(this.Evereposicion);
            if (!dr.IsDBNull(iEvereposicion)) entity.Evereposicion = dr.GetString(iEvereposicion);

            int iInfequipo = dr.GetOrdinal(this.Infequipo);
            if (!dr.IsDBNull(iInfequipo)) entity.Infequipo = dr.GetString(iInfequipo);

            int iInfplazo = dr.GetOrdinal(this.Infplazo);
            if (!dr.IsDBNull(iInfplazo)) entity.Infplazo = dr.GetString(iInfplazo);

            int iInfestado = dr.GetOrdinal(this.Infestado);
            if (!dr.IsDBNull(iInfestado)) entity.Infestado = dr.GetString(iInfestado);            

            int iSeccioncodi = dr.GetOrdinal(this.Seccioncodi);
            if (!dr.IsDBNull(iSeccioncodi)) entity.Seccioncodi = Convert.ToInt32(dr.GetValue(iSeccioncodi));

            int iSubseccionnumber = dr.GetOrdinal(this.Subseccionnumber);
            if (!dr.IsDBNull(iSubseccionnumber)) entity.Subseccionnumber = Convert.ToInt32(dr.GetValue(iSubseccionnumber));

            int iDespcentromarginal = dr.GetOrdinal(this.Despcentromarginal);
            if (!dr.IsDBNull(iDespcentromarginal)) entity.Despcentromarginal = dr.GetString(iDespcentromarginal);

            int iDespursautomatica = dr.GetOrdinal(this.Despursautomatica);
            if (!dr.IsDBNull(iDespursautomatica)) entity.Despursautomatica = dr.GetString(iDespursautomatica);

            int iDespmagautomatica = dr.GetOrdinal(this.Despmagautomatica);
            if (!dr.IsDBNull(iDespmagautomatica)) entity.Despmagautomatica = dr.GetDecimal(iDespmagautomatica);

            int iDespursmanual = dr.GetOrdinal(this.Despursmanual);
            if (!dr.IsDBNull(iDespursmanual)) entity.Despursmanual = dr.GetString(iDespursmanual);

            int iDespmagmanual = dr.GetOrdinal(this.Despmagmanual);
            if (!dr.IsDBNull(iDespmagmanual)) entity.Despmagmanual = dr.GetDecimal(iDespmagmanual);

            int iDespcentralaislado = dr.GetOrdinal(this.Despcentralaislado);
            if (!dr.IsDBNull(iDespcentralaislado)) entity.Despcentralaislado = dr.GetString(iDespcentralaislado);

            int iDespmagaislado = dr.GetOrdinal(this.Despmagaislado);
            if (!dr.IsDBNull(iDespmagaislado)) entity.Despmagaislado = dr.GetDecimal(iDespmagaislado);

            int iDespreprogramas = dr.GetOrdinal(this.Despreprogramas);
            if (!dr.IsDBNull(iDespreprogramas)) entity.Despreprogramas = dr.GetString(iDespreprogramas);

            int iDesphorareprog = dr.GetOrdinal(this.Desphorareprog);
            if (!dr.IsDBNull(iDesphorareprog)) entity.Desphorareprog = dr.GetString(iDesphorareprog);

            int iDespmotivorepro = dr.GetOrdinal(this.Despmotivorepro);
            if (!dr.IsDBNull(iDespmotivorepro)) entity.Despmotivorepro = dr.GetString(iDespmotivorepro);

            int iDesppremisasreprog = dr.GetOrdinal(this.Desppremisasreprog);
            if (!dr.IsDBNull(iDesppremisasreprog)) entity.Desppremisasreprog = dr.GetString(iDesppremisasreprog);

            int iManequipo = dr.GetOrdinal(this.Manequipo);
            if (!dr.IsDBNull(iManequipo)) entity.Manequipo = dr.GetString(iManequipo);

            int iMantipo = dr.GetOrdinal(this.Mantipo);
            if (!dr.IsDBNull(iMantipo)) entity.Mantipo = dr.GetString(iMantipo);

            int iManhoraconex = dr.GetOrdinal(this.Manhoraconex);
            if (!dr.IsDBNull(iManhoraconex)) entity.Manhoraconex = dr.GetString(iManhoraconex);

            int iManconsideraciones = dr.GetOrdinal(this.Manconsideraciones);
            if (!dr.IsDBNull(iManconsideraciones)) entity.Manconsideraciones = dr.GetString(iManconsideraciones);

            int iSumsubestacion = dr.GetOrdinal(this.Sumsubestacion);
            if (!dr.IsDBNull(iSumsubestacion)) entity.Sumsubestacion = dr.GetString(iSumsubestacion);

            int iSummotivocorte = dr.GetOrdinal(this.Summotivocorte);
            if (!dr.IsDBNull(iSummotivocorte)) entity.Summotivocorte = dr.GetString(iSummotivocorte);

            int iPafecha = dr.GetOrdinal(this.Pafecha);
            if (!dr.IsDBNull(iPafecha)) entity.Pafecha = dr.GetString(iPafecha);

            int iPasorteo = dr.GetOrdinal(this.Pasorteo);
            if (!dr.IsDBNull(iPasorteo)) entity.Pasorteo = dr.GetString(iPasorteo);

            int iParesultado = dr.GetOrdinal(this.Paresultado);
            if (!dr.IsDBNull(iParesultado)) entity.Paresultado = dr.GetString(iParesultado);

            int iPagenerador = dr.GetOrdinal(this.Pagenerador);
            if (!dr.IsDBNull(iPagenerador)) entity.Pagenerador = dr.GetString(iPagenerador);

            int iPaprueba = dr.GetOrdinal(this.Paprueba);
            if (!dr.IsDBNull(iPaprueba)) entity.Paprueba = dr.GetString(iPaprueba);

            #region EdicionReprog
            int iDesptiporeprog = dr.GetOrdinal(this.Desptiporeprog);
            if (!dr.IsDBNull(iDesptiporeprog)) entity.Desptiporeprog = dr.GetString(iDesptiporeprog);

            int iDesparchivoatr = dr.GetOrdinal(this.Desparchivoatr);
            if (!dr.IsDBNull(iDesparchivoatr)) entity.Desparchivoatr = dr.GetString(iDesparchivoatr);
            #endregion

            return entity;
        }


        #region Mapeo de Campos

        public string Sumhorainicio = "SUMHORAINICIO";
        public string Sumreposicion = "SUMREPOSICION";
        public string Sumconsideraciones = "SUMCONSIDERACIONES";
        public string Regopecentral = "REGOPECENTRAL";
        public string Regcentralsubestacion = "REGCENTRALSUBESTACION";
        public string Regcentralhorafin = "REGCENTRALHORAFIN";
        public string Reglineas = "REGLINEAS";
        public string Reglineasubestacion = "REGLINEASUBESTACION";
        public string Reglineahorafin = "REGLINEAHORAFIN";
        public string Gesequipo = "GESEQUIPO";
        public string Gesaceptado = "GESACEPTADO";
        public string Gesdetalle = "GESDETALLE";
        public string Eveequipo = "EVEEQUIPO";
        public string Everesumen = "EVERESUMEN";
        public string Evehorainicio = "EVEHORAINICIO";
        public string Evereposicion = "EVEREPOSICION";
        public string Infequipo = "INFEQUIPO";
        public string Infplazo = "INFPLAZO";
        public string Infestado = "INFESTADO";
        public string Subseccioncodi = "SUBSECCIONCODI";
        public string Seccioncodi = "SECCIONCODI";
        public string Subseccionnumber = "SUBSECCIONNUMBER";
        public string Despcentromarginal = "DESPCENTROMARGINAL";
        public string Despursautomatica = "DESPURSAUTOMATICA";
        public string Despmagautomatica = "DESPMAGAUTOMATICA";
        public string Despursmanual = "DESPURSMANUAL";
        public string Despmagmanual = "DESPMAGMANUAL";
        public string Despcentralaislado = "DESPCENTRALAISLADO";
        public string Despmagaislado = "DESPMAGAISLADO";
        public string Despreprogramas = "DESPREPROGRAMAS";
        public string Desphorareprog = "DESPHORAREPROG";
        public string Despmotivorepro = "DESPMOTIVOREPRO";
        public string Desppremisasreprog = "DESPPREMISASREPROG";
        public string Manequipo = "MANEQUIPO";
        public string Mantipo = "MANTIPO";
        public string Manhoraconex = "MANHORACONEX";
        public string Manconsideraciones = "MANCONSIDERACIONES";
        public string Sumsubestacion = "SUMSUBESTACION";
        public string Summotivocorte = "SUMMOTIVOCORTE";
        public string Rus = "RUS";
        public string Icvalor1 = "ICVALOR1";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Reprograma = "REPROGRAMA";
        public string Hora = "HORA";
        public string Descripcion = "DESCRIPCION";
        public string Central = "CENTRAL";
        public string Equiabrev = "EQUIABREV";
        public string UrsNomb = "GRUPONOMB";
        public string UrsValor = "RSFDETVALAUT";
        public string Pafecha = "PAFECHA";
        public string Pasorteo = "PASORTEO";
        public string Paresultado = "PARESULTADO";
        public string Pagenerador = "PAGENERADOR";
        public string Paprueba = "PAPRUEBA";
        #region EdicionReprogCampos
        public string Desptiporeprog = "DESPTIPOREPROG";
        public string Desparchivoatr = "DESPARCHIVOATR";
        #endregion

        #endregion

        public string SqlObtenerRSF
        {
            get { return base.GetSqlXml("ObtenerRSF"); }
        }

        public string SqlObtenerReprogramas
        {
            get { return base.GetSqlXml("ObtenerReprogramas"); }
        }

        public string SqlObtenerMantenimientos
        {
            get { return base.GetSqlXml("ObtenerMantenimientos"); }
        }

        public string SqlObtenerMantenimientosComentario
        {
            get { return base.GetSqlXml("ObtenerMantenimientosComentario"); }
        }

        public string SqlObtenerSuministros
        {
            get { return base.GetSqlXml("ObtenerSuministros"); }
        }

        public string SqlObtenerOperacionCentral
        {
            get { return base.GetSqlXml("ObtenerOperacionCentral");}
        }

        public string SqlObtenerLineasDesconectadas
        {
            get { return base.GetSqlXml("ObtenerLineasDesconectadas"); }
        }

        public string SqlObtenerEventosImportantes
        {
            get { return base.GetSqlXml("ObtenerEventosImportantes"); }
        }

        public string SqlObtenerInformeFalla
        {
            get { return base.GetSqlXml("ObtenerInformeFalla"); }
        }
    }

}
