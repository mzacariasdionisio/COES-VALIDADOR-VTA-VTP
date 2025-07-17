using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_CAMBIO_TURNO
    /// </summary>
    public class SiCambioTurnoHelper : HelperBase
    {
        public SiCambioTurnoHelper(): base(Consultas.SiCambioTurnoSql)
        {
        }

        public SiCambioTurnoDTO Create(IDataReader dr)
        {
            SiCambioTurnoDTO entity = new SiCambioTurnoDTO();

            int iCoordinadorresp = dr.GetOrdinal(this.Coordinadorresp);
            if (!dr.IsDBNull(iCoordinadorresp)) entity.Coordinadorresp = Convert.ToInt32(dr.GetValue(iCoordinadorresp));

            int iTurno = dr.GetOrdinal(this.Turno);
            if (!dr.IsDBNull(iTurno)) entity.Turno = Convert.ToInt32(dr.GetValue(iTurno));

            int iFecturno = dr.GetOrdinal(this.Fecturno);
            if (!dr.IsDBNull(iFecturno)) entity.Fecturno = dr.GetDateTime(iFecturno);

            int iCoordinadorrecibe = dr.GetOrdinal(this.Coordinadorrecibe);
            if (!dr.IsDBNull(iCoordinadorrecibe)) entity.Coordinadorrecibe = dr.GetString(iCoordinadorrecibe);

            int iEspecialistarecibe = dr.GetOrdinal(this.Especialistarecibe);
            if (!dr.IsDBNull(iEspecialistarecibe)) entity.Especialistarecibe = dr.GetString(iEspecialistarecibe);

            int iAnalistarecibe = dr.GetOrdinal(this.Analistarecibe);
            if (!dr.IsDBNull(iAnalistarecibe)) entity.Analistarecibe = dr.GetString(iAnalistarecibe);

            int iCambioturnocodi = dr.GetOrdinal(this.Cambioturnocodi);
            if (!dr.IsDBNull(iCambioturnocodi)) entity.Cambioturnocodi = Convert.ToInt32(dr.GetValue(iCambioturnocodi));

            int iEmsoperativo = dr.GetOrdinal(this.Emsoperativo);
            if (!dr.IsDBNull(iEmsoperativo)) entity.Emsoperativo = dr.GetString(iEmsoperativo);

            int iEmsobservaciones = dr.GetOrdinal(this.Emsobservaciones);
            if (!dr.IsDBNull(iEmsobservaciones)) entity.Emsobservaciones = dr.GetString(iEmsobservaciones);

            int iHoraentregaturno = dr.GetOrdinal(this.Horaentregaturno);
            if (!dr.IsDBNull(iHoraentregaturno)) entity.Horaentregaturno = dr.GetString(iHoraentregaturno);

            int iCasosinreserva = dr.GetOrdinal(this.Casosinreserva);
            if (!dr.IsDBNull(iCasosinreserva)) entity.CasoSinReserva = dr.GetString(iCasosinreserva);

            return entity;
        }


        #region Mapeo de Campos

        public string Coordinadorresp = "COORDINADORRESP";
        public string Turno = "TURNO";
        public string Fecturno = "FECTURNO";
        public string Coordinadorrecibe = "COORDINADORRECIBE";
        public string Especialistarecibe = "ESPECIALISTARECIBE";
        public string Analistarecibe = "ANALISTARECIBE";
        public string Cambioturnocodi = "CAMBIOTURNOCODI";
        public string Emsoperativo = "EMSOPERATIVO";
        public string Emsobservaciones = "EMSOBSERVACIONES";
        public string Horaentregaturno = "HORAENTREGATURNO";
        public string Percodi = "PERCODI";
        public string Pernomb = "PERNOMB";
        public string Texto = "TEXTO";
        public string Casosinreserva = "CASOSINRESERVA";

        public string SqlObtenerResponsables
        {
            get { return base.GetSqlXml("ObtenerResponsables"); }
        }

        public string SqlVerificarExistencia
        {
            get { return base.GetSqlXml("VerificarExistencia"); }
        }

        public string SqlObtenerModosOperacion
        {
            get { return base.GetSqlXml("ObtenerModosOperacion"); }
        }

        #endregion
    }
}
