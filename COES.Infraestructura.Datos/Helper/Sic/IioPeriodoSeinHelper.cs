using System;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_PERIODO_REMISION
    /// </summary>
    public class IioPeriodoSeinHelper : HelperBase
    {

        private const string KeyListarAnios = "ListarAnios";
        private const string KeyInsert = "Insert";
        private const string KeyUpdate = "Update";

        public IioPeriodoSeinHelper() :base(Consultas.IioPeriodoSeinSql)
        {
            
        }

        public IioPeriodoSeinDTO Create(IDataReader dr)
        {
            int iPseinCodi = dr.GetOrdinal(PseinCodi);
            int iPseinAnioMesPerrem = dr.GetOrdinal(PseinAnioMesPerrem);
            int iPseinFecPriEnvio = dr.GetOrdinal(PseinFecPriEnvio);
            int iPseinFecUltEnvio = dr.GetOrdinal(PseinFecUltEnvio);
            int iPseinConfirmado = dr.GetOrdinal(PseinConfirmado);
            int iPseinEstado = dr.GetOrdinal(PseinEstado);
            int iPseinEstRegistro = dr.GetOrdinal(PseinEstRegistro);
            int iPseinUsuCreacion = dr.GetOrdinal(PseinUsuCreacion);
            int iPseinFecCreacion = dr.GetOrdinal(PseinFecCreacion);
            int iPseinUsuModificacion = dr.GetOrdinal(PseinUsuModificacion);
            int iPseinFecModificacion = dr.GetOrdinal(PseinFecModificacion);

            return new IioPeriodoSeinDTO
            {
                PseinCodi = (!dr.IsDBNull(iPseinCodi) ? dr.GetInt32(iPseinCodi) : default(int)),
                PseinAnioMesPerrem = (!dr.IsDBNull(iPseinAnioMesPerrem) ? dr.GetString(iPseinAnioMesPerrem) : null),
                PseinFecPriEnvio = (!dr.IsDBNull(iPseinFecPriEnvio) ? dr.GetDateTime(iPseinFecPriEnvio) : new DateTime()),
                PseinFecUltEnvio = (!dr.IsDBNull(iPseinFecUltEnvio) ? dr.GetDateTime(iPseinFecUltEnvio) : new DateTime()),
                PseinConfirmado = (!dr.IsDBNull(iPseinConfirmado) ? dr.GetString(iPseinConfirmado) : null),
                PseinEstado = (!dr.IsDBNull(iPseinEstado) ? dr.GetString(iPseinEstado) : null),
                PseinEstRegistro = (!dr.IsDBNull(iPseinEstRegistro) ? dr.GetString(iPseinEstRegistro) : null),
                PseinUsuCreacion = (!dr.IsDBNull(iPseinUsuCreacion) ? dr.GetString(iPseinUsuCreacion) : null),
                PseinFecCreacion = (!dr.IsDBNull(iPseinFecCreacion) ? dr.GetDateTime(iPseinFecCreacion) : new DateTime()),
                PseinUsuModificacion = (!dr.IsDBNull(iPseinUsuModificacion) ? dr.GetString(iPseinUsuModificacion) : null),
                PseinFecModificacion = (!dr.IsDBNull(iPseinFecModificacion) ? dr.GetDateTime(iPseinFecModificacion) : new DateTime())
            };
        }

        #region Mapeo de Campos

        public static string TableName = "IIO_PERIODO_SEIN";
        public static string PseinCodi = "PSEINCODI";
        public static string PseinAnioMesPerrem = "PSEINANIOMESPERREM";
        public static string PseinFecPriEnvio = "PSEINFECPRIENVIO";
        public static string PseinFecUltEnvio = "PSEINFECULTENVIO";
        public static string PseinConfirmado = "PSEINCONFIRMADO";
        public static string PseinEstado = "PSEINESTADO";
        public static string PseinEstRegistro = "PSEINESTREGISTRO";
        public static string PseinUsuCreacion = "PSEINUSUCREACION";
        public static string PseinFecCreacion = "PSEINFECCREACION";
        public static string PseinUsuModificacion = "PSEINUSUMODIFICACION";
        public static string PseinFecModificacion = "PSEINFECMODIFICACION";

        public static string Anio = "ANIO";

        #endregion

        #region Custom SQL

        public string SqlListAnios
        {
            get { return GetSqlXml(KeyListarAnios); }
        }

        public string SqlInsert
        {
            get { return GetSqlXml(KeyInsert); }
        }

        public string SqlUpdate
        {
            get { return GetSqlXml(KeyUpdate); }
        }

        #endregion

    }
}