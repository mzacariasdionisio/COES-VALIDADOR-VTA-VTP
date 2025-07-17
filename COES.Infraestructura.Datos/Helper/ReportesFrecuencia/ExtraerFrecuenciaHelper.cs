using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Data;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.ReportesFrecuencia
{
    public class ExtraerFrecuenciaHelper : HelperBase
    {
        public ExtraerFrecuenciaHelper() : base(Consultas.ExtraerInformacionSql)
        {
        }
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private object valorReturn(IDataReader dr, string sColumna)
        {
            object resultado = null;
            int iIndex;
            if (columnsExist(sColumna, dr))
            {
                iIndex = dr.GetOrdinal(sColumna);
                if (!dr.IsDBNull(iIndex))
                    resultado = dr.GetValue(iIndex);
            }
            return resultado?.ToString();
        }
        public static T? ConvertToNull<T>(object x) where T : struct
        {
            return x == null ? null : (T?)Convert.ChangeType(x, typeof(T));
        }

        public ExtraerFrecuenciaDTO Create(IDataReader dr)
        {
            ExtraerFrecuenciaDTO entity = new ExtraerFrecuenciaDTO();

            entity.IdCarga = Convert.ToInt32(valorReturn(dr, IdCarga) ?? 0);
            entity.GPSCodi = Convert.ToInt32(valorReturn(dr, GPSCodi) ?? 0);
            entity.GPSNombre = valorReturn(dr, GPSNombre)?.ToString();
            //entity.FechaHoraInicio = Convert.ToDateTime(valorReturn(dr, FechaHoraInicio));
            entity.FechaHoraInicioString = valorReturn(dr, FechaHoraInicio)?.ToString();
            //entity.FechaHoraFin = Convert.ToDateTime(valorReturn(dr, FechaHoraFin));
            entity.FechaHoraFinString = valorReturn(dr, FechaHoraFin)?.ToString();
            entity.ArchivoCarga = valorReturn(dr, ArchivoCarga)?.ToString();
            //entity.FechaCreacion = Convert.ToDateTime(valorReturn(dr, FechaCreacion));
            entity.FechaCreacionString = valorReturn(dr, FechaCreacion)?.ToString();
            entity.UsuCreacion = valorReturn(dr, UsuCreacion)?.ToString();
            return entity;
        }

        public LecturaVirtualDTO CreateMiliseg(IDataReader dr)
        {
            LecturaVirtualDTO entity = new LecturaVirtualDTO();

            entity.IdCarga = Convert.ToInt32(valorReturn(dr, IdCarga) ?? 0);
            //entity.FechaHora = Convert.ToDateTime(valorReturn(dr, FechaHora));
            entity.FechaHoraString = valorReturn(dr, FechaHora)?.ToString();
            entity.Frecuencia = Convert.ToDecimal(valorReturn(dr, Frecuencia) ?? 0);
            entity.Tension = Convert.ToDecimal(valorReturn(dr, Tension) ?? 0);
            entity.Miliseg = Convert.ToInt32(valorReturn(dr, Miliseg) ?? 0);
            return entity;
        }

        public EquipoGPSDTO CreateEquipoGPS(IDataReader dr)
        {
            EquipoGPSDTO entity = new EquipoGPSDTO();

            entity.GPSCodi = Convert.ToInt32(valorReturn(dr, GPSCodi) ?? 0);
            entity.NombreEquipo = valorReturn(dr, GPSNombre)?.ToString();
            entity.RutaFile = valorReturn(dr, GPSRutaFile)?.ToString();
            return entity;
        }



        #region Mapeo de Campos


        //CargaVirtual
        public string IdCarga = "IDCARGA";
        public string GPSCodi = "GPSCODI";
        public string GPSNombre = "GPSNOMBRE";
        public string FechaHoraInicio = "FECHAHORA_INICIO";
        public string FechaHoraFin = "FECHAHORA_FIN";
        public string FechaCarga = "FECHACARGA";
        public string ArchivoCarga = "ARCHIVOCARGA";
        public string DataCarga = "DATACARGA";
        public string UsuCreacion = "USUCREACION";
        public string FechaCreacion = "FECHACREACION";

        public string Mensaje = "MENSAJE";
        public string Resultado = "RESULTADO";

        public string GPSRutaFile = "RUTAFILE";

        public string FechaHora = "FECHAHORA";
        public string Frecuencia = "FRECUENCIA";
        public string Miliseg = "MILISEG";
        public string Tension = "TENSION";



        #endregion Mapeo de Campos


        #region SQL
        public string SqlListaExtraerFrecuencia
        {
            get { return base.GetSqlXml("GetListaExtraerFrecuencia"); }
        }
        public string SqlListaMilisegundos
        {
            get { return base.GetSqlXml("GetListaMilisegundos"); }
        }
        public string SqlListaEmpresasCargaVirtual
        {
            get { return base.GetSqlXml("GetListaEmpresasCargaVirtual"); }
        }

        public string SqlListaCentralPorEmpresa
        {
            get { return base.GetSqlXml("GetListaCentralPorEmpresa"); }
        }

        public string SqlListaUnidadPorCentralEmpresa
        {
            get { return base.GetSqlXml("GetListaUnidadPorCentralEmpresa"); }
        }

        public string SqlSaveUpdate
        {
            get { return GetSqlXml("SaveUpdate"); }
        }

        public string SqlSaveLecturaMiliseg
        {
            get { return GetSqlXml("SaveLecturaMiliseg"); }
        }
        #endregion SQL
    }
}
