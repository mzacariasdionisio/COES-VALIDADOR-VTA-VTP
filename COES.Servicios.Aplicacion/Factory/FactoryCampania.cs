using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Repositorio.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Factory
{
    public class FactoryCampania
    {
        public static string StrConexion = "ContextoSIC";

        public static CamPeriodoRepository ObtenerCamPeriodoDao()
        {
            return new CamPeriodoRepository(StrConexion);
        }

        public static CamDetalleperiodoRepository ObtenerCamDetallePeriodoDao()
        {
            return new CamDetalleperiodoRepository(StrConexion);
        }

        public static CamCatalogoRepository ObtenerCamCatalogoDao()
        {
            return new CamCatalogoRepository(StrConexion);
        }


        public static CamDatacatalogoRepository ObtenerCamDatacatalogoDao()
        {
            return new CamDatacatalogoRepository(StrConexion);
        }

        public static CamTipoproyectoRepository ObtenerCamTipoproyectoDao()
        {
            return new CamTipoproyectoRepository(StrConexion);
        }


        public static CamDepartamentoRepository ObtenerCamDepartamentoDao()
        {
            return new CamDepartamentoRepository(StrConexion);
        }

        public static CamProvinciaRepository ObtenerCamProvinciaDao()
        {
            return new CamProvinciaRepository(StrConexion);
        }

        public static CamDistritoRepository ObtenerCamDistritoDao()
        {
            return new CamDistritoRepository(StrConexion);
        }
        public static CamRegHojaARepository ObtenerCamRegHojaADao()
        {
            return new CamRegHojaARepository(StrConexion);
        }
        public static CamRegHojaBRepository ObtenerCamRegHojaBDao()
        {
            return new CamRegHojaBRepository(StrConexion);
        }

        public static CamRegHojaDRepository ObtenerCamRegHojaDDao()
        {
            return new CamRegHojaDRepository(StrConexion);
        }

        public static CamTransmisionProyectoRepository ObtenerCamTransmisionProyectoDao()
        {
            return new CamTransmisionProyectoRepository(StrConexion);
        }

        public static CamPlanTransmisionRepository ObtenerCamPlanTransmisionDao()
        {
            return new CamPlanTransmisionRepository(StrConexion);
        }

        public static CamArchivoInfoRepository ObtenerCamArchivoInfoDao()
        {
            return new CamArchivoInfoRepository(StrConexion);
        }

        public static CamSeccionHojaRepository ObtenerCamSeccionHojaDao()
        {
            return new CamSeccionHojaRepository(StrConexion);
        }

        public static CamDetRegHojaDRepository ObtenerCamDetRegHojaDDao()
        {
            return new CamDetRegHojaDRepository(StrConexion);
        }

        public static CamSolHojaARepository ObtenerCamSolHojaADao()
        {
            return new CamSolHojaARepository(StrConexion);
        }

        public static CamSolHojaBRepository ObtenerCamSolHojaBDao()
        {
            return new CamSolHojaBRepository(StrConexion);
        }

        public static CamRegHojaEolARepository ObtenerCamRegHojaEolADao()
        {
            return new CamRegHojaEolARepository(StrConexion);
        }
        public static CamRegHojaEolADetRepository ObtenerCamRegHojaEolADetDao()
        {
            return new CamRegHojaEolADetRepository(StrConexion);
        }
        public static CamRegHojaEolBRepository ObtenerCamRegHojaEolBDao()
        {
            return new CamRegHojaEolBRepository(StrConexion);
        }
        public static CamRegHojaEolCRepository ObtenerCamRegHojaEolCDao()
        {
            return new CamRegHojaEolCRepository(StrConexion);
        }

        public static CamDetRegHojaEolCRepository ObtenerDetCamRegHojaEolCDao()
        {
            return new CamDetRegHojaEolCRepository(StrConexion);
        }

        public static CamSolHojaCRepository ObtenerCamSolHojaCDao()
        {
            return new CamSolHojaCRepository(StrConexion);
        }

        public static CamDetSolHojaCRepository ObtenerCamDetSolHojaCDao()
        {
            return new CamDetSolHojaCRepository(StrConexion);
        }

        public static CamBioHojaARepository ObtenerCamBioHojaADao()
        {
            return new CamBioHojaARepository(StrConexion);
        }

        public static CamBioHojaBRepository ObtenerCamBioHojaBDao()
        {
            return new CamBioHojaBRepository(StrConexion);
        }

        public static CamBioHojaCRepository ObtenerCamBioHojaCDao()
        {
            return new CamBioHojaCRepository(StrConexion);
        }

        public static CamDetBioHojaCRepository ObtenerCamDetBioHojaCDao()
        {
            return new CamDetBioHojaCRepository(StrConexion);
        }
        public static CamITCFE01Repository ObtenerITCE01()
        {
            return new CamITCFE01Repository(StrConexion);
        }
        public static CamITCFE01Repository GetRegITCFE01ById()
        {
            return new CamITCFE01Repository(StrConexion);
        }

        public static  CamItcdf104Repository ObtenerItcdf104Dao()
        {
            return new CamItcdf104Repository(StrConexion);
        }
        public static CamItcdf108Repository ObtenerItcdf108Dao()
        {
            return new CamItcdf108Repository(StrConexion);
        }
        public static CamItcdf110Repository ObtenerItcdf110Dao()
        {
            return new CamItcdf110Repository(StrConexion);
        }
        public static CamItcdf110DetRepository ObtenerItcdf110DetDao()
        {
            return new CamItcdf110DetRepository(StrConexion);
        }
        public static CamItcdf116Repository ObtenerItcdf116Dao()
        {
            return new CamItcdf116Repository(StrConexion);
        }
        public static CamItcdf116DetRepository ObtenerItcdf116DetDao()
        {
            return new CamItcdf116DetRepository(StrConexion);
        }
        public static CamItcdf121Repository ObtenerItcdf121Dao()
        {
            return new CamItcdf121Repository(StrConexion);
        }
        public static CamItcdf121DetRepository ObtenerItcdf121DetDao()
        {
            return new CamItcdf121DetRepository(StrConexion);
        }

        
        public static CamItcdf123Repository ObtenerItcdf123Dao()
        {
            return new CamItcdf123Repository(StrConexion);
        }
        public static CamItcdfp011Repository ObtenerItcdfp011Dao()
        {
            return new CamItcdfp011Repository(StrConexion);
        }
        public static CamItcdfp011DetRepository ObtenerItcdfp011DetDao()
        {
            return new CamItcdfp011DetRepository(StrConexion);
        }
        public static CamItcdfp012Repository ObtenerItcdfp012Dao()
        {
            return new CamItcdfp012Repository(StrConexion);
        }
        public static CamItcdfp013Repository ObtenerItcdfp013Dao()
        {
            return new CamItcdfp013Repository(StrConexion);
        }
        public static CamItcdfp013DetRepository ObtenerItcdfp013DetDao()
        {
            return new CamItcdfp013DetRepository(StrConexion);
        }


        public static CamRegHojaCRepository ObtenerCamRegHojaCDao()
        {
            return new CamRegHojaCRepository(StrConexion);
        }

        public static CamDetRegHojaCRepository ObtenerCamDetRegHojaCDao()
        {
            return new CamDetRegHojaCRepository(StrConexion);
        }

        public static CamRegHojaCCTTARepository ObtenerCamRegHojaCCTTADao()
        {
            return new CamRegHojaCCTTARepository(StrConexion);
        }
        public static CamRegHojaCCTTBRepository ObtenerCamRegHojaCCTTBDao()
        {
            return new CamRegHojaCCTTBRepository(StrConexion);
        }

        public static CamRegHojaCCTTCRepository ObtenerCamRegHojaCCTTCDao()
        {
            return new CamRegHojaCCTTCRepository(StrConexion);
        }

        public static CamDet1RegHojaCCTTCRepository ObtenerCamDet1RegHojaCCTTCDao()
        {
            return new CamDet1RegHojaCCTTCRepository(StrConexion);
        }
        public static CamDet2RegHojaCCTTCRepository ObtenerCamDet2RegHojaCCTTCDao()
        {
            return new CamDet2RegHojaCCTTCRepository(StrConexion);
        }

        public static CamRegHojaASubestRepository ObtenerCamRegHojaASubestDao()
        {
            return new CamRegHojaASubestRepository(StrConexion);
        }

        public static CamDetRegHojaASubestRepository ObtenerCamDetRegHojaASubestDao()
        {
            return new CamDetRegHojaASubestRepository(StrConexion);
        }

        public static CamItcPrm1Repository ObtenerItcprm1Dao()
        {
            return new CamItcPrm1Repository(StrConexion);
        }

        public static CamItcPrm2Repository ObtenerItcprm2Dao()
        {
            return new CamItcPrm2Repository(StrConexion);
        }

        public static CamItcRed1Repository ObtenerItcred1Dao()
        {
            return new CamItcRed1Repository(StrConexion);
        }

        public static CamItcRed2Repository ObtenerItcred2Dao()
        {
            return new CamItcRed2Repository(StrConexion);
        }

        public static CamItcRed3Repository ObtenerItcred3Dao()
        {
            return new CamItcRed3Repository(StrConexion);
        }

        public static CamItcRed4Repository ObtenerItcred4Dao()
        {
            return new CamItcRed4Repository(StrConexion);
        }

        public static CamItcRed5Repository ObtenerItcred5Dao()
        {
            return new CamItcRed5Repository(StrConexion);
        }

        public static CamCCGDARepository ObtenerCcgdaDao()
        {
            return new CamCCGDARepository(StrConexion);
        }

        public static CamCCGDBRepository ObtenerCcgdbDao()
        {
            return new CamCCGDBRepository(StrConexion);
        }

        public static CamCCGDCOptRepository ObtenerCcgdcOptDao()
        {
            return new CamCCGDCOptRepository(StrConexion);
        }

        public static CamCCGDCPesRepository ObtenerCcgdcPesDao()
        {
            return new CamCCGDCPesRepository(StrConexion);
        }


        public static CamCCGDDRepository ObtenerCcgddDao()
        {
            return new CamCCGDDRepository(StrConexion);
        }

        public static CamCCGDERepository ObtenerCcgdeDao()
        {
            return new CamCCGDERepository(StrConexion);
        }

        public static CamCCGDFRepository ObtenerCcgdfDao()
        {
            return new CamCCGDFRepository(StrConexion);
        }

        public static CamFormatoD1ARepository ObtenerFormatoD1ADao()
        {
            return new CamFormatoD1ARepository(StrConexion);
        }

        public static CamFormatoD1ADet1Repository ObtenerFormatoD1ADet1Dao()
        {
            return new CamFormatoD1ADet1Repository(StrConexion);
        }

        public static CamFormatoD1ADet2Repository ObtenerFormatoD1ADet2Dao()
        {
            return new CamFormatoD1ADet2Repository(StrConexion);
        }

        public static CamFormatoD1ADet3Repository ObtenerFormatoD1ADet3Dao()
        {
            return new CamFormatoD1ADet3Repository(StrConexion);
        }

        public static CamFormatoD1ADet4Repository ObtenerFormatoD1ADet4Dao()
        {
            return new CamFormatoD1ADet4Repository(StrConexion);
        }

        public static CamFormatoD1ADet5Repository ObtenerFormatoD1ADet5Dao()
        {
            return new CamFormatoD1ADet5Repository(StrConexion);
        }

        public static CamFormatoD1BRepository ObtenerFormatoD1BDao()
        {
            return new CamFormatoD1BRepository(StrConexion);
        }

        public static CamFormatoD1BDetRepository ObtenerFormatoD1BDetDao()
        {
            return new CamFormatoD1BDetRepository(StrConexion);
        }

        public static CamFormatoD1CRepository ObtenerFormatoD1CDao()
        {
            return new CamFormatoD1CRepository(StrConexion);
        }

        public static CamFormatoD1CDetRepository ObtenerFormatoD1CDetDao()
        {
            return new CamFormatoD1CDetRepository(StrConexion);
        }

        public static CamFormatoD1DRepository ObtenerFormatoD1DDao()
        {
            return new CamFormatoD1DRepository(StrConexion);
        }

        public static CamCuestionarioH2VARepository ObtenerCuestionarioH2VADao()
        {
            return new CamCuestionarioH2VARepository(StrConexion);
        }

        public static CamCuestionarioH2VADet1Repository ObtenerCuestionarioH2VADet1Dao()
        {
            return new CamCuestionarioH2VADet1Repository(StrConexion);
        }

        public static CamCuestionarioH2VADet2Repository ObtenerCuestionarioH2VADet2Dao()
        {
            return new CamCuestionarioH2VADet2Repository(StrConexion);
        }

        public static CamCuestionarioH2VBRepository ObtenerCuestionarioH2VBDao()
        {
            return new CamCuestionarioH2VBRepository(StrConexion);
        }

        public static CamCuestionarioH2VCRepository ObtenerCuestionarioH2VCDao()
        {
            return new CamCuestionarioH2VCRepository(StrConexion);
        }

        public static CamCuestionarioH2VERepository ObtenerCuestionarioH2VEDao()
        {
            return new CamCuestionarioH2VERepository(StrConexion);
        }

        public static CamCuestionarioH2VFRepository ObtenerCuestionarioH2VFDao()
        {
            return new CamCuestionarioH2VFRepository(StrConexion);
        }

        public static CamCuestionarioH2VGRepository ObtenerCuestionarioH2VGDao()
        {
            return new CamCuestionarioH2VGRepository(StrConexion);
        }

        public static CamCroFicha1Repository ObtenerCamCroFicha1Dao()
        {
            return new CamCroFicha1Repository(StrConexion);
        }

        public static CamCroFicha1DetRepository ObtenerCamCroFicha1DetDao()
        {
            return new CamCroFicha1DetRepository(StrConexion);
        }
        public static CamLineasFichaARepository ObtenerCamLineasFichaADao()
        {
            return new CamLineasFichaARepository(StrConexion);
        }

     
        public static CamLineasFichaADet1Repository ObtenerCamLineasFichaADet1Dao()
        {
            return new CamLineasFichaADet1Repository(StrConexion);
        }
        public static CamLineasFichaADet2Repository ObtenerCamLineasFichaADet2Dao()
        {
            return new CamLineasFichaADet2Repository(StrConexion);
        }

        public static CamT1LineasFichaARepository ObtenerCamT1LineasFichaADao()
        {
            return new CamT1LineasFichaARepository(StrConexion);
        }

        public static CamT1LineasFichaADet1Repository ObtenerCamT1LineasFichaADet1Dao()
        {
            return new CamT1LineasFichaADet1Repository(StrConexion);
        }
        public static CamT1LineasFichaADet2Repository ObtenerCamT1LineasFichaADet2Dao()
        {
            return new CamT1LineasFichaADet2Repository(StrConexion);
        }

        public static CamLineasFichaBRepository ObtenerCamLineasFichaBDao()
        {
            return new CamLineasFichaBRepository(StrConexion);
        }

        public static CamLineasFichaBDetRepository ObtenerCamLineasFichaBDetDao()
        {
            return new CamLineasFichaBDetRepository(StrConexion);
        }

        public static CamSubestFicha1Repository ObtenerCamSubestFicha1Dao()
        {
            return new CamSubestFicha1Repository(StrConexion);
        }

        public static CamSubestFicha1Det1Repository ObtenerCamSubestFicha1Det1Dao()
        {
            return new CamSubestFicha1Det1Repository(StrConexion);
        }

        public static CamSubestFicha1Det2Repository ObtenerCamSubestFicha1Det2Dao()
        {
            return new CamSubestFicha1Det2Repository(StrConexion);
        }

        public static CamSubestFicha1Det3Repository ObtenerCamSubestFicha1Det3Dao()
        {
            return new CamSubestFicha1Det3Repository(StrConexion);
        }
        public static CamT2SubestFicha1Repository ObtenerCamT2SubestFicha1Dao()
        {
            return new CamT2SubestFicha1Repository(StrConexion);
        }

        public static CamT2SubestFicha1Det1Repository ObtenerCamT2SubestFicha1Det1Dao()
        {
            return new CamT2SubestFicha1Det1Repository(StrConexion);
        }

        public static CamT2SubestFicha1Det2Repository ObtenerCamT2SubestFicha1Det2Dao()
        {
            return new CamT2SubestFicha1Det2Repository(StrConexion);
        }

        public static CamT2SubestFicha1Det3Repository ObtenerCamT2SubestFicha1Det3Dao()
        {
            return new CamT2SubestFicha1Det3Repository(StrConexion);
        }

        public static CamObservacionRepository ObtenerCamObservacionDao()
        {
            return new CamObservacionRepository(StrConexion);
        }

        public static CamRespuestaObsRepository ObtenerCamRespuestaObsDao()
        {
            return new CamRespuestaObsRepository(StrConexion);
        }

        public static CamArchivoObsRepository ObtenerCamArchivoObsDao()
        {
            return new CamArchivoObsRepository(StrConexion);
        }
    }
}
