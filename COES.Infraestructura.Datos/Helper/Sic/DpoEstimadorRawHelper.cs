using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoEstimadorRawHelper : HelperBase
    {
        public DpoEstimadorRawHelper() : base(Consultas.DpoEstimadorRawSql)
        {
        }

        public DpoEstimadorRawTmpDTO Create(IDataReader dr)
        {
            DpoEstimadorRawTmpDTO entity = new DpoEstimadorRawTmpDTO();

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iDpovarcodi = dr.GetOrdinal(this.Prnvarcodi);
            if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

            int iDporawfuente = dr.GetOrdinal(this.Dporawfuente);
            if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

            int iDporawtipomedi = dr.GetOrdinal(this.Dporawtipomedi);
            if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

            int iDporawfecha = dr.GetOrdinal(this.Dporawfecha);
            if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

            return entity;
        }

        public DpoEstimadorRawDTO CreateEntity(IDataReader dr)
        {
            DpoEstimadorRawDTO entity = new DpoEstimadorRawDTO();

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iDpovarcodi = dr.GetOrdinal(this.Prnvarcodi);
            if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

            int iDporawfuente = dr.GetOrdinal(this.Dporawfuente);
            if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

            int iDporawtipomedi = dr.GetOrdinal(this.Dporawtipomedi);
            if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

            int iDporawfecha = dr.GetOrdinal(this.Dporawfecha);
            if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

            int ih1 = dr.GetOrdinal(this.Dporawvalorh1);
            if (!dr.IsDBNull(ih1)) entity.Dporawvalorh1 = dr.GetDecimal(ih1);

            int ih2 = dr.GetOrdinal(this.Dporawvalorh2);
            if (!dr.IsDBNull(ih2)) entity.Dporawvalorh2 = dr.GetDecimal(ih2);

            int ih3 = dr.GetOrdinal(this.Dporawvalorh3);
            if (!dr.IsDBNull(ih3)) entity.Dporawvalorh3 = dr.GetDecimal(ih3);

            int ih4 = dr.GetOrdinal(this.Dporawvalorh4);
            if (!dr.IsDBNull(ih4)) entity.Dporawvalorh4 = dr.GetDecimal(ih4);

            int ih5 = dr.GetOrdinal(this.Dporawvalorh5);
            if (!dr.IsDBNull(ih5)) entity.Dporawvalorh5 = dr.GetDecimal(ih5);

            int ih6 = dr.GetOrdinal(this.Dporawvalorh6);
            if (!dr.IsDBNull(ih6)) entity.Dporawvalorh6 = dr.GetDecimal(ih6);

            int ih7 = dr.GetOrdinal(this.Dporawvalorh7);
            if (!dr.IsDBNull(ih7)) entity.Dporawvalorh7 = dr.GetDecimal(ih7);

            int ih8 = dr.GetOrdinal(this.Dporawvalorh8);
            if (!dr.IsDBNull(ih8)) entity.Dporawvalorh8 = dr.GetDecimal(ih8);

            int ih9 = dr.GetOrdinal(this.Dporawvalorh9);
            if (!dr.IsDBNull(ih9)) entity.Dporawvalorh9 = dr.GetDecimal(ih9);

            int ih10 = dr.GetOrdinal(this.Dporawvalorh10);
            if (!dr.IsDBNull(ih10)) entity.Dporawvalorh10 = dr.GetDecimal(ih10);

            int ih11 = dr.GetOrdinal(this.Dporawvalorh11);
            if (!dr.IsDBNull(ih11)) entity.Dporawvalorh11 = dr.GetDecimal(ih11);

            int ih12 = dr.GetOrdinal(this.Dporawvalorh12);
            if (!dr.IsDBNull(ih12)) entity.Dporawvalorh12 = dr.GetDecimal(ih12);

            int ih13 = dr.GetOrdinal(this.Dporawvalorh13);
            if (!dr.IsDBNull(ih13)) entity.Dporawvalorh13 = dr.GetDecimal(ih13);

            int ih14 = dr.GetOrdinal(this.Dporawvalorh14);
            if (!dr.IsDBNull(ih14)) entity.Dporawvalorh14 = dr.GetDecimal(ih14);

            int ih15 = dr.GetOrdinal(this.Dporawvalorh15);
            if (!dr.IsDBNull(ih15)) entity.Dporawvalorh15 = dr.GetDecimal(ih15);

            int ih16 = dr.GetOrdinal(this.Dporawvalorh16);
            if (!dr.IsDBNull(ih16)) entity.Dporawvalorh16 = dr.GetDecimal(ih16);

            int ih17 = dr.GetOrdinal(this.Dporawvalorh17);
            if (!dr.IsDBNull(ih17)) entity.Dporawvalorh17 = dr.GetDecimal(ih17);

            int ih18 = dr.GetOrdinal(this.Dporawvalorh18);
            if (!dr.IsDBNull(ih18)) entity.Dporawvalorh18 = dr.GetDecimal(ih18);

            int ih19 = dr.GetOrdinal(this.Dporawvalorh19);
            if (!dr.IsDBNull(ih19)) entity.Dporawvalorh19 = dr.GetDecimal(ih19);

            int ih20 = dr.GetOrdinal(this.Dporawvalorh20);
            if (!dr.IsDBNull(ih20)) entity.Dporawvalorh20 = dr.GetDecimal(ih20);

            int ih21 = dr.GetOrdinal(this.Dporawvalorh21);
            if (!dr.IsDBNull(ih21)) entity.Dporawvalorh21 = dr.GetDecimal(ih21);

            int ih22 = dr.GetOrdinal(this.Dporawvalorh22);
            if (!dr.IsDBNull(ih22)) entity.Dporawvalorh22 = dr.GetDecimal(ih22);

            int ih23 = dr.GetOrdinal(this.Dporawvalorh23);
            if (!dr.IsDBNull(ih23)) entity.Dporawvalorh23 = dr.GetDecimal(ih23);

            int ih24 = dr.GetOrdinal(this.Dporawvalorh24);
            if (!dr.IsDBNull(ih24)) entity.Dporawvalorh24 = dr.GetDecimal(ih24);

            int ih25 = dr.GetOrdinal(this.Dporawvalorh25);
            if (!dr.IsDBNull(ih25)) entity.Dporawvalorh25 = dr.GetDecimal(ih25);

            int ih26 = dr.GetOrdinal(this.Dporawvalorh26);
            if (!dr.IsDBNull(ih26)) entity.Dporawvalorh26 = dr.GetDecimal(ih26);

            int ih27 = dr.GetOrdinal(this.Dporawvalorh27);
            if (!dr.IsDBNull(ih27)) entity.Dporawvalorh27 = dr.GetDecimal(ih27);

            int ih28 = dr.GetOrdinal(this.Dporawvalorh28);
            if (!dr.IsDBNull(ih28)) entity.Dporawvalorh28 = dr.GetDecimal(ih28);

            int ih29 = dr.GetOrdinal(this.Dporawvalorh29);
            if (!dr.IsDBNull(ih29)) entity.Dporawvalorh29 = dr.GetDecimal(ih29);

            int ih30 = dr.GetOrdinal(this.Dporawvalorh30);
            if (!dr.IsDBNull(ih30)) entity.Dporawvalorh30 = dr.GetDecimal(ih30);

            int ih31 = dr.GetOrdinal(this.Dporawvalorh31);
            if (!dr.IsDBNull(ih31)) entity.Dporawvalorh31 = dr.GetDecimal(ih31);

            int ih32 = dr.GetOrdinal(this.Dporawvalorh32);
            if (!dr.IsDBNull(ih32)) entity.Dporawvalorh32 = dr.GetDecimal(ih32);

            int ih33 = dr.GetOrdinal(this.Dporawvalorh33);
            if (!dr.IsDBNull(ih33)) entity.Dporawvalorh33 = dr.GetDecimal(ih33);

            int ih34 = dr.GetOrdinal(this.Dporawvalorh34);
            if (!dr.IsDBNull(ih34)) entity.Dporawvalorh34 = dr.GetDecimal(ih34);

            int ih35 = dr.GetOrdinal(this.Dporawvalorh35);
            if (!dr.IsDBNull(ih35)) entity.Dporawvalorh35 = dr.GetDecimal(ih35);

            int ih36 = dr.GetOrdinal(this.Dporawvalorh36);
            if (!dr.IsDBNull(ih36)) entity.Dporawvalorh36 = dr.GetDecimal(ih36);

            int ih37 = dr.GetOrdinal(this.Dporawvalorh37);
            if (!dr.IsDBNull(ih37)) entity.Dporawvalorh37 = dr.GetDecimal(ih37);

            int ih38 = dr.GetOrdinal(this.Dporawvalorh38);
            if (!dr.IsDBNull(ih38)) entity.Dporawvalorh38 = dr.GetDecimal(ih38);

            int ih39 = dr.GetOrdinal(this.Dporawvalorh39);
            if (!dr.IsDBNull(ih39)) entity.Dporawvalorh39 = dr.GetDecimal(ih39);

            int ih40 = dr.GetOrdinal(this.Dporawvalorh40);
            if (!dr.IsDBNull(ih40)) entity.Dporawvalorh40 = dr.GetDecimal(ih40);

            int ih41 = dr.GetOrdinal(this.Dporawvalorh41);
            if (!dr.IsDBNull(ih41)) entity.Dporawvalorh41 = dr.GetDecimal(ih41);

            int ih42 = dr.GetOrdinal(this.Dporawvalorh42);
            if (!dr.IsDBNull(ih42)) entity.Dporawvalorh42 = dr.GetDecimal(ih42);

            int ih43 = dr.GetOrdinal(this.Dporawvalorh43);
            if (!dr.IsDBNull(ih43)) entity.Dporawvalorh43 = dr.GetDecimal(ih43);

            int ih44 = dr.GetOrdinal(this.Dporawvalorh44);
            if (!dr.IsDBNull(ih44)) entity.Dporawvalorh44 = dr.GetDecimal(ih44);

            int ih45 = dr.GetOrdinal(this.Dporawvalorh45);
            if (!dr.IsDBNull(ih45)) entity.Dporawvalorh45 = dr.GetDecimal(ih45);

            int ih46 = dr.GetOrdinal(this.Dporawvalorh46);
            if (!dr.IsDBNull(ih46)) entity.Dporawvalorh46 = dr.GetDecimal(ih46);

            int ih47 = dr.GetOrdinal(this.Dporawvalorh47);
            if (!dr.IsDBNull(ih47)) entity.Dporawvalorh47 = dr.GetDecimal(ih47);

            int ih48 = dr.GetOrdinal(this.Dporawvalorh48);
            if (!dr.IsDBNull(ih48)) entity.Dporawvalorh48 = dr.GetDecimal(ih48);

            int ih49 = dr.GetOrdinal(this.Dporawvalorh49);
            if (!dr.IsDBNull(ih49)) entity.Dporawvalorh49 = dr.GetDecimal(ih49);

            int ih50 = dr.GetOrdinal(this.Dporawvalorh50);
            if (!dr.IsDBNull(ih50)) entity.Dporawvalorh50 = dr.GetDecimal(ih50);

            int ih51 = dr.GetOrdinal(this.Dporawvalorh51);
            if (!dr.IsDBNull(ih51)) entity.Dporawvalorh51 = dr.GetDecimal(ih51);

            int ih52 = dr.GetOrdinal(this.Dporawvalorh52);
            if (!dr.IsDBNull(ih52)) entity.Dporawvalorh52 = dr.GetDecimal(ih52);

            int ih53 = dr.GetOrdinal(this.Dporawvalorh53);
            if (!dr.IsDBNull(ih53)) entity.Dporawvalorh53 = dr.GetDecimal(ih53);

            int ih54 = dr.GetOrdinal(this.Dporawvalorh54);
            if (!dr.IsDBNull(ih54)) entity.Dporawvalorh54 = dr.GetDecimal(ih54);

            int ih55 = dr.GetOrdinal(this.Dporawvalorh55);
            if (!dr.IsDBNull(ih55)) entity.Dporawvalorh55 = dr.GetDecimal(ih55);

            int ih56 = dr.GetOrdinal(this.Dporawvalorh56);
            if (!dr.IsDBNull(ih56)) entity.Dporawvalorh56 = dr.GetDecimal(ih56);

            int ih57 = dr.GetOrdinal(this.Dporawvalorh57);
            if (!dr.IsDBNull(ih57)) entity.Dporawvalorh57 = dr.GetDecimal(ih57);

            int ih58 = dr.GetOrdinal(this.Dporawvalorh58);
            if (!dr.IsDBNull(ih58)) entity.Dporawvalorh58 = dr.GetDecimal(ih58);

            int ih59 = dr.GetOrdinal(this.Dporawvalorh59);
            if (!dr.IsDBNull(ih59)) entity.Dporawvalorh59 = dr.GetDecimal(ih59);

            int ih60 = dr.GetOrdinal(this.Dporawvalorh60);
            if (!dr.IsDBNull(ih60)) entity.Dporawvalorh60 = dr.GetDecimal(ih60);

            return entity;
        }

        #region Mapeo de los campos
        public string Ptomedicodi = "PTOMEDICODI";
        public string Prnvarcodi = "PRNVARCODI";
        public string Dporawfuente = "DPORAWFUENTE";
        public string Dporawtipomedi = "DPORAWTIPOMEDI";
        public string Dporawfecha = "DPORAWFECHA";
        public string Dporawvalor = "DPORAWVALOR";

        public string Dporawvalorh1 = "DPORAWVALORH1";
        public string Dporawvalorh2 = "DPORAWVALORH2";
        public string Dporawvalorh3 = "DPORAWVALORH3";
        public string Dporawvalorh4 = "DPORAWVALORH4";
        public string Dporawvalorh5 = "DPORAWVALORH5";
        public string Dporawvalorh6 = "DPORAWVALORH6";
        public string Dporawvalorh7 = "DPORAWVALORH7";
        public string Dporawvalorh8 = "DPORAWVALORH8";
        public string Dporawvalorh9 = "DPORAWVALORH9";
        public string Dporawvalorh10 = "DPORAWVALORH10";
        public string Dporawvalorh11 = "DPORAWVALORH11";
        public string Dporawvalorh12 = "DPORAWVALORH12";
        public string Dporawvalorh13 = "DPORAWVALORH13";
        public string Dporawvalorh14 = "DPORAWVALORH14";
        public string Dporawvalorh15 = "DPORAWVALORH15";
        public string Dporawvalorh16 = "DPORAWVALORH16";
        public string Dporawvalorh17 = "DPORAWVALORH17";
        public string Dporawvalorh18 = "DPORAWVALORH18";
        public string Dporawvalorh19 = "DPORAWVALORH19";
        public string Dporawvalorh20 = "DPORAWVALORH20";
        public string Dporawvalorh21 = "DPORAWVALORH21";
        public string Dporawvalorh22 = "DPORAWVALORH22";
        public string Dporawvalorh23 = "DPORAWVALORH23";
        public string Dporawvalorh24 = "DPORAWVALORH24";
        public string Dporawvalorh25 = "DPORAWVALORH25";
        public string Dporawvalorh26 = "DPORAWVALORH26";
        public string Dporawvalorh27 = "DPORAWVALORH27";
        public string Dporawvalorh28 = "DPORAWVALORH28";
        public string Dporawvalorh29 = "DPORAWVALORH29";
        public string Dporawvalorh30 = "DPORAWVALORH30";
        public string Dporawvalorh31 = "DPORAWVALORH31";
        public string Dporawvalorh32 = "DPORAWVALORH32";
        public string Dporawvalorh33 = "DPORAWVALORH33";
        public string Dporawvalorh34 = "DPORAWVALORH34";
        public string Dporawvalorh35 = "DPORAWVALORH35";
        public string Dporawvalorh36 = "DPORAWVALORH36";
        public string Dporawvalorh37 = "DPORAWVALORH37";
        public string Dporawvalorh38 = "DPORAWVALORH38";
        public string Dporawvalorh39 = "DPORAWVALORH39";
        public string Dporawvalorh40 = "DPORAWVALORH40";
        public string Dporawvalorh41 = "DPORAWVALORH41";
        public string Dporawvalorh42 = "DPORAWVALORH42";
        public string Dporawvalorh43 = "DPORAWVALORH43";
        public string Dporawvalorh44 = "DPORAWVALORH44";
        public string Dporawvalorh45 = "DPORAWVALORH45";
        public string Dporawvalorh46 = "DPORAWVALORH46";
        public string Dporawvalorh47 = "DPORAWVALORH47";
        public string Dporawvalorh48 = "DPORAWVALORH48";
        public string Dporawvalorh49 = "DPORAWVALORH49";
        public string Dporawvalorh50 = "DPORAWVALORH50";
        public string Dporawvalorh51 = "DPORAWVALORH51";
        public string Dporawvalorh52 = "DPORAWVALORH52";
        public string Dporawvalorh53 = "DPORAWVALORH53";
        public string Dporawvalorh54 = "DPORAWVALORH54";
        public string Dporawvalorh55 = "DPORAWVALORH55";
        public string Dporawvalorh56 = "DPORAWVALORH56";
        public string Dporawvalorh57 = "DPORAWVALORH57";
        public string Dporawvalorh58 = "DPORAWVALORH58";
        public string Dporawvalorh59 = "DPORAWVALORH59";
        public string Dporawvalorh60 = "DPORAWVALORH60";

        public string TableName = "DPO_ESTIMADORRAW";

        public string Nomarchivoraw = "NOMARCHIVORAW";
        public string Fechaarchivoraw = "FECHAARCHIVORAW";
        public string Tipo = "TIPO";
        public string Flag = "FLAG";

        public string Hora = "HORA";
        public string Minuto = "MINUTO";

        public string Ptomedidesc = "PTOMEDIDESC";
        public string Prnvarnom = "PRNVARNOM";

        public string Ieod = "IEOD";
        #endregion

        #region Consultas a la BD
        // ------------------------------------------------------------------------------------------------------------------
        // Metodos para el proceso automatico por cada minuto de los archivos Raw
        // ------------------------------------------------------------------------------------------------------------------

        public string SqlTruncateTablaTemporal
        {
            get { return base.GetSqlXml("TruncateTablaTemporal"); }
        }

        public string SqlDeleteRawsHistoricos
        {
            get { return base.GetSqlXml("DeleteRawsHistoricos"); }
        }

        public string SqlMigrarRawsProcesadosHora
        {
            get { return base.GetSqlXml("MigrarRawsProcesadosHora"); }
        }

        public string SqlInsertLogRaw
        {
            get { return base.GetSqlXml("InsertLogRaw"); }
        }

        public string SqlDeleteLogRaw
        {
            get { return base.GetSqlXml("DeleteLogRaw"); }
        }

        public string SqlListFilesLogRaw
        {
            get { return base.GetSqlXml("ListFilesLogRaw"); }
        }

        public string SqlListFilesLogRawHora
        {
            get { return base.GetSqlXml("ListFilesLogRawHora"); }
        }

        public string SqlGetFileLogRaw
        {
            get { return base.GetSqlXml("GetFileLogRaw"); }
        }

        public string SqlGetByIdFileRaw
        {
            get { return base.GetSqlXml("GetByIdFileRaw"); }
        }

        public string SqlListFileRawTmp
        {
            get { return base.GetSqlXml("ListFileRawTmp"); }
        }

        public string SqlUpdateRawsProcesadosHora
        {
            get { return base.GetSqlXml("UpdateRawsProcesadosHora"); }
        }

        public string SqlDeleteRawsProcesados
        {
            get { return base.GetSqlXml("DeleteRawsProcesados"); }
        }

        public string SqlInsertFileRaw
        {
            get { return base.GetSqlXml("InsertFileRaw"); }
        }

        public string SqlUpdateFileRaw
        {
            get { return base.GetSqlXml("UpdateFileRaw"); }
        }

        public string SqlListFilesRaw
        {
            get { return base.GetSqlXml("ListFilesRaw"); }
        }

        public string SqlGetFileRaw
        {
            get { return base.GetSqlXml("GetFileRaw"); }
        }

        // ------------------------------------------------------------------------------------------------------------------
        // Metodos para el proceso automatico de costo marginales cada 30 minuto de los archivos Raw
        // ------------------------------------------------------------------------------------------------------------------
        public string SqlUpdateRawsProcesados30Minutos
        {
            get { return base.GetSqlXml("UpdateRawsProcesados30Minutos"); }
        }

        public string SqlDeleteREstimadorRawFileByDiaProceso
        {
            get { return base.GetSqlXml("DeleteREstimadorRawFileByDiaProceso"); }
        }

        public string SqlDeleteREstimadorRawLogByDiaProceso
        {
            get { return base.GetSqlXml("DeleteREstimadorRawLogByDiaProceso"); }
        }

        public string SqlDeleteREstimadorRawTemporalByDiaProceso
        {
            get { return base.GetSqlXml("DeleteREstimadorRawTemporalByDiaProceso"); }
        }

        public string SqlDeleteREstimadorRawCmTemporalByDiaProceso
        {
            get { return base.GetSqlXml("DeleteREstimadorRawCmTemporalByDiaProceso"); }
        }

        public string SqlDeleteREstimadorRawByDiaProceso
        {
            get { return base.GetSqlXml("DeleteREstimadorRawByDiaProceso"); }
        }

        public string SqlDeleteREstimadorRawFileByNomArchivo
        {
            get { return base.GetSqlXml("DeleteREstimadorRawFileByNomArchivo"); }
        }

        public string SqlUpdateRawsProcesados60Minutos
        {
            get { return base.GetSqlXml("UpdateRawsProcesados60Minutos"); }
        }

        // ------------------------------------------------------------------------------------------------------------------
        // Metodos para el proceso manual de los archivos Raw
        // ------------------------------------------------------------------------------------------------------------------
        public string SqlListEstimadorRawManual
        {
            get { return base.GetSqlXml("ListEstimadorRawManual"); }
        }

        public string SqlListEstimadorRawManualHora
        {
            get { return base.GetSqlXml("ListEstimadorRawManualHora"); }
        }

        public string SqlListEstimadorRawManualMinuto
        {
            get { return base.GetSqlXml("ListEstimadorRawManualMinuto"); }
        }

        public string SqlUpdateRawManual
        {
            get { return base.GetSqlXml("UpdateRawManual"); }
        }


        // ------------------------------------------------------------------------------------------------------------------
        // Metodos para reportes exportables a excel
        // ------------------------------------------------------------------------------------------------------------------
        public string SqlListFilesRawPorMinuto
        {
            get { return base.GetSqlXml("ListFilesRawPorMinuto"); }
        }

        public string SqlListFilesRawIeod
        {
            get { return base.GetSqlXml("ListFilesRawIeod"); }
        }

        public string SqlObtenerPorRangoFuente
        {
            get { return base.GetSqlXml("ObtenerPorRangoFuente"); }
        }
        public string SqlDatosPorPtoMedicion
        {
            get { return base.GetSqlXml("DatosPorPtoMedicion"); }
        }

        public string SqlObtenerUltimoEstimadorRawFiles
        {
            get { return base.GetSqlXml("ObtenerUltimoEstimadorRawFiles"); }
        }
        public string SqlObtenerEstimadorRawFilesFlag
        {
            get { return base.GetSqlXml("ObtenerEstimadorRawFilesFlag"); }
        }
        public string SqlVerificarFaltantesDia
        {
            get { return base.GetSqlXml("VerificarFaltantesDia"); }
        }
        public string SqlUpdateTMP
        {
            get { return base.GetSqlXml("UpdateTMP"); }
        }
        public string SqlObtenerDemActivaPorDia
        {
            get { return base.GetSqlXml("ObtenerDemActivaPorDia"); }
        }
        public string SqlReporteDatosEstimadorxHora
        {
            get { return base.GetSqlXml("ReporteDatosEstimadorxHora"); }
        }
        #endregion

    }
}
