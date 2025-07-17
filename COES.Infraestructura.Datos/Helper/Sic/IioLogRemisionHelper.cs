using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SCO_LOG_REMISION
    /// </summary>
    public class IioLogRemisionHelper : HelperBase
    {
        public IioLogRemisionHelper(): base(Consultas.IioLogRemisionSql)
        {
            
        }

        public IioLogRemisionDTO Create(IDataReader dr)
        {
            int iRlogCodi = dr.GetOrdinal(RlogCodi);
            int iRlogControlCargaCodi = dr.GetOrdinal(IioControlCargaHelper.RccaCodi);
            int iRlogDescripcionError = dr.GetOrdinal(RlogDescripcionError);
            int iRlogNroLinea = dr.GetOrdinal(RlogNroLinea);
            int iEnviocodi = dr.GetOrdinal(Enviocodi);

            return new IioLogRemisionDTO
            {
                RlogCodi = (!dr.IsDBNull(iRlogCodi) ? dr.GetInt32(iRlogCodi) : default(int)),
                RccaCodi = (!dr.IsDBNull(iRlogControlCargaCodi) ? dr.GetInt32(iRlogControlCargaCodi) : default(int)),
                RlogDescripcionError = (!dr.IsDBNull(iRlogDescripcionError) ? dr.GetString(iRlogDescripcionError) : null),
                RlogNroLinea = (!dr.IsDBNull(iRlogNroLinea) ? dr.GetInt32(iRlogNroLinea) : default(int)),
                Enviocodi = (!dr.IsDBNull(iEnviocodi) ? dr.GetInt32(iEnviocodi) : default(int))
            };
        }

        

        #region Mapeo de Campos

        public static string TableName = "IIO_LOG_REMISION";
        public static string RlogCodi = "RLOGCODI";
        public static string RlogDescripcionError = "RLOGDESCRIPCIONERROR";
        public static string RlogNroLinea = "RLOGNROLINEA";

        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        public static string Enviocodi = "ENVIOCODI";

        #endregion
    }
}