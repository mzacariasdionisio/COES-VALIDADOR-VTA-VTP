using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class ActualizarTrasEmpFusionHelper : HelperBase
    {
        public ActualizarTrasEmpFusionHelper() : base(Consultas.ActualizarTrasEmpFusionSql)
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

        public ActualizarTrasEmpFusionDTO Create(IDataReader dr)
        {
            ActualizarTrasEmpFusionDTO entity = new ActualizarTrasEmpFusionDTO();

            //potencia contrato
            entity.SalsoCodi = Convert.ToInt32(valorReturn(dr, SalsoCodi) ?? 0);
            entity.EmprCodiOri = Convert.ToInt32(valorReturn(dr, EmprCodiOri) ?? 0);
            entity.PeriCodiOri = Convert.ToInt32(valorReturn(dr, PeriCodiOri) ?? 0);
            entity.RecaCodi = Convert.ToInt32(valorReturn(dr, RecaCodi) ?? 0);
            entity.SalsoSaldoOri = Convert.ToDecimal(valorReturn(dr, SalsoSaldoOri) ?? 0);
            entity.SalsoTipOpe =  Convert.ToInt32(valorReturn(dr, SalsoTipOpe) ?? 0);
            entity.EmprCodiDes = Convert.ToInt32(valorReturn(dr, EmprCodiDes) ?? 0);
            entity.SalsoSaldoDes = Convert.ToDecimal(valorReturn(dr, SalsoSaldoDes) ?? 0);
            entity.SalsoEstado = (valorReturn(dr, SalsoEstado)?.ToString());
            entity.SalsoFecProceso = ConvertToNull<DateTime>(valorReturn(dr, SalsoFecProceso));
            entity.SalsoVTEAVTP = (valorReturn(dr, SalsoVTEAVTP)?.ToString());
            entity.SalsoUsuCreacion = (valorReturn(dr, SalsoUsuCreacion)?.ToString());
            entity.SalsoFecCreacion = ConvertToNull<DateTime>(valorReturn(dr, SalsoFecCreacion));
            entity.SalsoUsuModificacion = (valorReturn(dr, SalsoUsuModificacion)?.ToString());
            entity.SalsoFecModificacion = ConvertToNull<DateTime>(valorReturn(dr, SalsoFecModificacion));
            entity.SalsoSaldoFinal = entity.SalsoEstado == "P" ? entity.SalsoSaldoDes : entity.SalsoSaldoOri + entity.SalsoSaldoDes;

            entity.DescEmpresaOrigen = (valorReturn(dr, DescEmpresaOrigen)?.ToString());
            entity.DescPeriodoOrigen = (valorReturn(dr, DescPeriodoOrigen)?.ToString());
            entity.DescVersionOrigen = (valorReturn(dr, DescVersionOrigen)?.ToString());
            entity.DescTipoOpe = (valorReturn(dr, DescTipoOpe)?.ToString());
            entity.DescEmpresaDestino = (valorReturn(dr, DescEmpresaDestino)?.ToString());
            entity.DescEstado = (valorReturn(dr, DescEstado)?.ToString());

            entity.DescEmpresaNI = (valorReturn(dr, DescEmpresaNI)?.ToString());
            entity.DescPeriodoNI = (valorReturn(dr, DescPeriodoNI)?.ToString());
            entity.DescVersionNI = (valorReturn(dr, DescVersionNI)?.ToString());
            entity.SaldoNI = Convert.ToDecimal(valorReturn(dr, SaldoNI) ?? 0);
            entity.FechaNI = ConvertToNull<DateTime>(valorReturn(dr, FechaNI));

            entity.SalsoFecMigracion = ConvertToNull<DateTime>(valorReturn(dr, SalsoFecMigracion));

            return entity;
        }
        #region Mapeo de Campos


        //potencia contrato
        public string SalsoCodi = "SALSOCODI";
        public string EmprCodiOri = "EMPRCODIORI";
        public string PeriCodiOri = "PERICODIORI";
        public string RecaCodi = "RECACODI";
        public string SalsoSaldoOri = "SALSOSALDOORI";
        public string SalsoTipOpe = "SALSOTIPOPE";
        public string EmprCodiDes = "EMPRCODIDES";
        public string SalsoSaldoDes = "SALSOSALDODES";
        public string SalsoEstado = "SALSOESTADO";
        public string SalsoFecProceso = "SALSOFECPROCESO";
        public string SalsoVTEAVTP = "SALSOVTEAVTP";
        public string SalsoUsuCreacion = "SALSOUSUCREACION";
        public string SalsoFecCreacion = "SALSOFECCREACION";
        public string SalsoUsuModificacion = "SALSOUSUMODIFICACION";
        public string SalsoFecModificacion = "SALSOFECMODIFICACION";
        public string PeriCodiDes = "PERICODIDES";

        public string Mensaje = "MENSAJE";

        public string DescEmpresaOrigen = "DESCEMPRESAORIGEN";
        public string DescPeriodoOrigen = "DESCPERIODOORIGEN";
        public string DescVersionOrigen = "DESCVERSIONORIGEN";
        public string DescTipoOpe = "DESCTIPOOPE";
        public string DescEmpresaDestino = "DESCEMPRESADESTINO";
        public string DescEstado = "DESCESTADO";

        public string DescEmpresaNI = "DESCEMPRESANI";
        public string DescPeriodoNI = "DESCPERIODONI";
        public string DescVersionNI = "DESCVERSIONNI";
        public string SaldoNI = "SALDONI";
        public string FechaNI = "FECHANI";

        public string SalsoFecMigracion = "SALSOFECMIGRACION";

        #endregion Mapeo de Campos


        #region SQL
        public string SqlListaSaldosSobrantes
        {
            get { return base.GetSqlXml("GetListaSaldosSobrantes"); }
        }
        public string SqlSaveUpdate
        {
            get { return base.GetSqlXml("SaveUpdate"); }
        }
        public string SqlDeleteSaldosSobrantes
        {
            get { return base.GetSqlXml("DeleteSaldosSobrantesPendientes"); }
        }
        public string SqlLista
        {
            get { return base.GetSqlXml("GetLista"); }
        }
        public string SqlSaveUpdateSaldos
        {
            get { return base.GetSqlXml("SaveUpdateSaldos"); }
        }
        public string SqlSaveUpdateSaldosVTP
        {
            get { return base.GetSqlXml("SaveUpdateSaldosVTP"); }
        }
        public string SqlListaSaldosNoIdentificados
        {
            get { return base.GetSqlXml("GetListaSaldosNoIdentificados"); }
        }

        public string SqlListaSaldosNoIdentificadosVTP
        {
            get { return base.GetSqlXml("GetListaSaldosNoIdentificadosVTP"); }
        }

        public string SqlListaSaldosSobrantesVTP
        {
            get { return base.GetSqlXml("GetListaSaldosSobrantesVTP"); }
        }

        public string SqlListaVTP
        {
            get { return base.GetSqlXml("GetListaVTP"); }
        }


        #endregion SQL
    }
}
