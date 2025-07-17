using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class GmmAgenteHelper : HelperBase
    {
        public GmmAgenteHelper()
            : base(Consultas.GmmAgenteSql)
        {

        }

        #region Mapeo de Campos
        public string Empgcodi = "EMPGCODI";
        public string Empgfecingreso = "EMPGFECINGRESO";
        public string Empgtipopart = "EMPGTIPOPART";
        public string Empgestado = "EMPGESTADO";
        public string Emprcodi = "EMPRCODI";
        public string Empgcomentario = "EMPGCOMENTARIO";
        public string Empgusucreacion = "EMPGUSUCREACION";
        public string Empgfeccreacion = "EMPGFECCREACION";
        public string Empgusumodificacion = "EMPGUSUMODIFICACION";
        public string Empgfecmodificacion = "EMPGFECMODIFICACION";
        public string Pericodi = "PERICODI";
        public string Empgfasecal = "EMPGFASECAL";
        public string Periestado = "PERIESTADO";
        public string Tipoemprcodi = "TIPOEMPRCODI";
     

        public string Emprestado = "Emprestado";
        public string Emprnombrecomercial = "Emprnombrecomercial";
        public string Emprtipoparticipante = "Emprtipoparticipante";
        public string EmprFechaIngreso = "EmprFechaIngreso";
        public string EmprmodalidadBusqueda = "EmprmodalidadBusqueda";
        public string EmprMontoGarantia = "EmprMontoGarantia";
        public string Emprnumeincumplimientos = "Emprnumeincumplimientos";

        public string EmprFechaInicio = "EmprFechaInicio";
        public string EmprFechaFin = "EmprFechaFin";
        public string EmprModalidad = "EmprModalidad";
        public string EmprTipo = "EmprTipo";
        public string EmprMonto = "EmprMonto";
        public string EmprArchivo = "EmprArchivo";
        public string EmprTrienio = "EmprTrienio";
        public string EmprTotalIncM = "EmprTotalIncM";

        public string EmprFechaRegistro = "EmprFechaRegistro";
        public string EmprEstado = "EmprEstado";
        public string EmprUsuario = "EmprUsuario";

        public string EmprTriSecuencia = "EmprTriSecuencia";
        public string EmprFecIniTrienio = "EmprFecIniTrienio";
        public string EmprFecFinTrienio = "EmprFecFinTrienio";
        public string EmprTotalInc = "EmprTotalInc";
        public string EmprCertifica = "EmprCertifica";


        public string EmpCodiEdit = "EmpCodiEdit";
        public string EmpNombreEdit = "EmpNombreEdit";
        public string EmpRucEdit = "EmpRucEdit";
        public string EmpFecIngEdit = "EmpFecIngEdit";
        public string EmptpartEdit = "EmptpartEdit";
        public string EmpestadoEdit = "EmpestadoEdit";
        public string EmpComentarioEdit = "EmpComentarioEdit";

        public string Emprrazsocial = "Emprrazsocial";
        public string EmprRuc = "EmprRuc";

        public string PericodiCal = "PericodiCal";
        public string EmprcodiCal = "EmprcodiCal";
        public string EmpgcodiCal = "EmpgcodiCal";

        public string ptomedicodi = "ptomedicodi";
        public string barrcodi = "barrcodi";
        public string tptomedicodi = "tptomedicodi";
        public string casddbbarra = "casddbbarra";


        #endregion

        #region Mapeo de Campos Garantia
        public string GaraCodi = "GARACODI";
        public string GaraFecInicio = "GARAFECINICIO";
        public string GaraFecFin = "GARAFECFIN";
        public string GaraMontoGarantia = "GARAMONTOGARANTIA";
        public string GaraArchivo = "GARAARCHIVO";
        public string GaraEstado = "GARAESTADO";
        public string GaraUsuCreacion = "GARAUSUCREACION";
        public string GaraFecCreacion = "GARAFECCREACION";
        public string GaraUsuModificacion = "GARAUSUMODIFICACION";
        public string GaraFecModificacion = "GARAFECMODIFICACION";
        public string TCerCodi = "TCERCODI";
        public string TModCodi = "TMODCODI";
        public string EMPGCodi = "EMPGCODI";

        #endregion
        public GmmEmpresaDTO Create(IDataReader dr)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            #region Agente
            int iEmpgcodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = Convert.ToInt32(dr.GetValue(iEmpgcodi));

            int iEmpgfecingreso = dr.GetOrdinal(this.Empgfecingreso);
            if (!dr.IsDBNull(iEmpgfecingreso)) entity.EMPGFECINGRESO = dr.GetDateTime(iEmpgfecingreso);

            int iEmpgtipopart = dr.GetOrdinal(this.Empgtipopart);
            if (!dr.IsDBNull(iEmpgtipopart)) entity.EMPGTIPOPART = dr.GetString(iEmpgtipopart);

            int iEmpgestado = dr.GetOrdinal(this.Empgestado);
            if (!dr.IsDBNull(iEmpgestado)) entity.EMPGESTADO = dr.GetString(iEmpgestado);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EMPRCODI = dr.GetInt32(iEmprcodi);

            int iEmpgcomentario = dr.GetOrdinal(this.Empgcomentario);
            if (!dr.IsDBNull(iEmpgcomentario)) entity.EMPGCOMENTARIO = dr.GetString(iEmpgcomentario);

            int iEmpgusucreacion = dr.GetOrdinal(this.Empgusucreacion);
            if (!dr.IsDBNull(iEmpgusucreacion)) entity.EMPGUSUCREACION = dr.GetString(iEmpgusucreacion);

            int iEmpgfeccreacion = dr.GetOrdinal(this.Empgfeccreacion);
            if (!dr.IsDBNull(iEmpgfeccreacion)) entity.EMPGFECCREACION = dr.GetDateTime(iEmpgfeccreacion);

            int iEmpgusumodificacion = dr.GetOrdinal(this.Empgusumodificacion);
            if (!dr.IsDBNull(iEmpgusumodificacion)) entity.EMPGUSUMODIFICACION = dr.GetString(iEmpgusumodificacion);

            int iEmpgfecmodificacion = dr.GetOrdinal(this.Empgfecmodificacion);
            if (!dr.IsDBNull(iEmpgfecmodificacion)) entity.EMPGFECMODIFICACION = dr.GetDateTime(iEmpgfecmodificacion);
            #endregion

            return entity;
        }
        public GmmEmpresaDTO CreateListaAgentes(IDataReader dr)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            #region Lista Agentes
            int iEmpgcodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = Convert.ToInt32(dr.GetValue(iEmpgcodi));

            int iEmprestado = dr.GetOrdinal(this.Emprestado);
            if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

            int iEmprnombrecomercial = dr.GetOrdinal(this.Emprnombrecomercial);
            if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

            int iEmprtipoparticipante = dr.GetOrdinal(this.Emprtipoparticipante);
            if (!dr.IsDBNull(iEmprtipoparticipante)) entity.Emprtipoparticipante = dr.GetString(iEmprtipoparticipante);

            int iEmprFechaIngreso = dr.GetOrdinal(this.EmprFechaIngreso);
            if (!dr.IsDBNull(iEmprFechaIngreso)) entity.EmprFechaIngreso = dr.GetDateTime(iEmprFechaIngreso);

            int iEmprFechaFin = dr.GetOrdinal(this.EmprFechaFin);
            if (!dr.IsDBNull(iEmprFechaFin)) entity.EmprFechaFin = dr.GetDateTime(iEmprFechaFin);

            int iEmprmodalidadBusqueda = dr.GetOrdinal(this.EmprmodalidadBusqueda);
            if (!dr.IsDBNull(iEmprmodalidadBusqueda)) entity.EmprmodalidadBusqueda = dr.GetString(iEmprmodalidadBusqueda);

            int iEmprMontoGarantia = dr.GetOrdinal(this.EmprMontoGarantia);
            if (!dr.IsDBNull(iEmprMontoGarantia)) entity.EmprMontoGarantia = dr.GetString(iEmprMontoGarantia);

            int iEmprnumeincumplimientos = dr.GetOrdinal(this.Emprnumeincumplimientos);
            if (!dr.IsDBNull(iEmprnumeincumplimientos)) entity.Emprnumeincumplimientos = dr.GetInt32(iEmprnumeincumplimientos);
            #endregion
            return entity;
        }
        public GmmEmpresaDTO CreateListaAgentesSelect(IDataReader dr)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            #region Lista Agentes
            int iEmpgcodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = Convert.ToInt32(dr.GetValue(iEmpgcodi));

            int iEmprnombrecomercial = dr.GetOrdinal(this.Emprnombrecomercial);
            if (!dr.IsDBNull(iEmprnombrecomercial)) entity.Emprnombrecomercial = dr.GetString(iEmprnombrecomercial);

            int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            if (!dr.IsDBNull(iTipoemprcodi)) entity.TIPOEMPRCODI = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

            #endregion
            return entity;
        }


        public GmmEmpresaDTO CreateListaParaCalculo(IDataReader dr)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            #region Lista Agentes para Calculo
            int iEmpgcodi = dr.GetOrdinal(this.EmpgcodiCal);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EmpgcodiCal = Convert.ToInt32(dr.GetValue(iEmpgcodi));
            int iEmprcodi = dr.GetOrdinal(this.EmprcodiCal);
            if (!dr.IsDBNull(iEmprcodi)) entity.EmprcodiCal = Convert.ToInt32(dr.GetValue(iEmprcodi));
            int iPericodiCal = dr.GetOrdinal(this.PericodiCal);
            if (!dr.IsDBNull(iPericodiCal)) entity.PericodiCal = Convert.ToInt32(dr.GetValue(iPericodiCal));
            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodiCal)) entity.PERICODI = Convert.ToInt32(dr.GetValue(iPericodi));
            int iEmpgfasecal = dr.GetOrdinal(this.Empgfasecal);
            if (!dr.IsDBNull(iEmpgfasecal)) entity.EMPGFASECAL = Convert.ToString(dr.GetValue(iEmpgfasecal));

            int iPeriestado = dr.GetOrdinal(this.Periestado);
            if (!dr.IsDBNull(iPeriestado)) entity.PERIESTADO = Convert.ToString(dr.GetValue(iPeriestado));

            #endregion
            return entity;
        }

        public GmmValEnergiaDTO CreateCabeceraValoresEnergia(IDataReader dr)
        {
            GmmValEnergiaDTO entity = new GmmValEnergiaDTO();

            #region Lista Cabecera de elementos para cálculo
            int iptomedicodi = dr.GetOrdinal(this.ptomedicodi);
            if (!dr.IsDBNull(iptomedicodi)) entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(iptomedicodi));
            int ibarrcodi = dr.GetOrdinal(this.barrcodi);
            if (!dr.IsDBNull(ibarrcodi)) entity.BARRCODI = Convert.ToInt32(dr.GetValue(ibarrcodi));
            int icasddbbarra = dr.GetOrdinal(this.casddbbarra);
            if (!dr.IsDBNull(icasddbbarra)) entity.CASDDBBARRA = Convert.ToString(dr.GetValue(icasddbbarra));
            int itptomedicodi = dr.GetOrdinal(this.tptomedicodi);
            if (!dr.IsDBNull(itptomedicodi)) entity.TPTOMEDICODI = Convert.ToInt32(dr.GetValue(itptomedicodi));

            #endregion
            return entity;
        }
        public GmmValEnergiaEntregaDTO CreateCabeceraValoresEnergiaEntrega(IDataReader dr)
        {
            GmmValEnergiaEntregaDTO entity = new GmmValEnergiaEntregaDTO();

            #region Lista Cabecera de elementos para cálculo
            int iptomedicodi = dr.GetOrdinal(this.ptomedicodi);
            if (!dr.IsDBNull(iptomedicodi)) entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(iptomedicodi));
            int ibarrcodi = dr.GetOrdinal(this.barrcodi);
            if (!dr.IsDBNull(ibarrcodi)) entity.BARRCODI = Convert.ToInt32(dr.GetValue(ibarrcodi));
            int icasddbbarra = dr.GetOrdinal(this.casddbbarra);
            if (!dr.IsDBNull(icasddbbarra)) entity.CASDDBBARRA = Convert.ToString(dr.GetValue(icasddbbarra));
            int itptomedicodi = dr.GetOrdinal(this.tptomedicodi);
            if (!dr.IsDBNull(itptomedicodi)) entity.TPTOMEDICODI = Convert.ToInt32(dr.GetValue(itptomedicodi));

            #endregion
            return entity;
        }

        public GmmEmpresaDTO CreateListaModalidades(IDataReader dr)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            #region Lista Agentes
            int iEmpgcodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = Convert.ToInt32(dr.GetValue(iEmpgcodi));

            int iEmprFechaInicio = dr.GetOrdinal(this.EmprFechaInicio);
            if (!dr.IsDBNull(iEmprFechaInicio)) entity.EmprFechaInicio = dr.GetDateTime(iEmprFechaInicio);

            int iEmprFechaFin = dr.GetOrdinal(this.EmprFechaFin);
            if (!dr.IsDBNull(iEmprFechaFin)) entity.EmprFechaFin = dr.GetDateTime(iEmprFechaFin);

            int iEmprModalidad = dr.GetOrdinal(this.EmprModalidad);
            if (!dr.IsDBNull(iEmprModalidad)) entity.EmprModalidad = dr.GetString(iEmprModalidad);

            int iEmprMonto = dr.GetOrdinal(this.EmprMonto);
            if (!dr.IsDBNull(iEmprMonto)) entity.EmprMonto = dr.GetDecimal(iEmprMonto);

            int iEmprArchivo = dr.GetOrdinal(this.EmprArchivo);
            if (!dr.IsDBNull(iEmprArchivo)) entity.EmprArchivo = dr.GetString(iEmprArchivo);

            int iEmprTrienio = dr.GetOrdinal(this.EmprTrienio);
            if (!dr.IsDBNull(iEmprTrienio)) entity.EmprTrienio = Convert.ToInt32(dr.GetValue(iEmprTrienio));

            int iEmprTotalIncM = dr.GetOrdinal(this.EmprTotalIncM);
            if (!dr.IsDBNull(iEmprTotalIncM)) entity.EmprTotalIncM = dr.GetInt32(iEmprTotalIncM);

            int iEmprCertifica = dr.GetOrdinal(this.EmprCertifica);
            if (!dr.IsDBNull(iEmprCertifica)) entity.EmprCertifica = dr.GetString(iEmprCertifica);

            int iGaraCodi = dr.GetOrdinal(this.GaraCodi);
            if (!dr.IsDBNull(iGaraCodi)) entity.EmprGaraCodi = Convert.ToInt32(dr.GetValue(iGaraCodi));

            #endregion
            return entity;
        }

        public GmmEmpresaDTO CreateListaEstados(IDataReader dr)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            #region Lista Estados
            int iEmpgcodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = Convert.ToInt32(dr.GetValue(iEmpgcodi));

            int iEmprFechaRegistro = dr.GetOrdinal(this.EmprFechaRegistro);
            if (!dr.IsDBNull(iEmprFechaRegistro)) entity.EmprFechaRegistro = dr.GetDateTime(iEmprFechaRegistro);

            int iEmprEstado = dr.GetOrdinal(this.EmprEstado);
            if (!dr.IsDBNull(iEmprEstado)) entity.EmprEstado = dr.GetString(iEmprEstado);

            int iEmprUsuario = dr.GetOrdinal(this.EmprUsuario);
            if (!dr.IsDBNull(iEmprUsuario)) entity.EmprUsuario = dr.GetString(iEmprUsuario);
            #endregion

            return entity;
        }

        public GmmEmpresaDTO CreateListaIncumplimientos(IDataReader dr)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            #region Lista Incumplimientos
            int iEmpgcodi = dr.GetOrdinal(this.Empgcodi);
            if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = Convert.ToInt32(dr.GetValue(iEmpgcodi));

            int iEmprTriSecuencia = dr.GetOrdinal(this.EmprTriSecuencia);
            if (!dr.IsDBNull(iEmprTriSecuencia)) entity.EmprTriSecuencia = dr.GetInt32(iEmprTriSecuencia);

            int iEmprFecIniTrienio = dr.GetOrdinal(this.EmprFecIniTrienio);
            if (!dr.IsDBNull(iEmprFecIniTrienio)) entity.EmprFecIniTrienio = dr.GetDateTime(iEmprFecIniTrienio);

            int iEmprFecFinTrienio = dr.GetOrdinal(this.EmprFecFinTrienio);
            if (!dr.IsDBNull(iEmprFecFinTrienio)) entity.EmprFecFinTrienio = dr.GetDateTime(iEmprFecFinTrienio);

            int iEmprTotalInc = dr.GetOrdinal(this.EmprTotalInc);
            if (!dr.IsDBNull(iEmprTotalInc)) entity.EmprTotalInc = dr.GetInt32(iEmprTotalInc);
            #endregion
            return entity;

        }

        public string SqlListarFiltroAgentes
        {
            get { return base.GetSqlXml("ListarFiltroAgentes"); }
        }
        public string SqlListarModalidades
        {
            get { return base.GetSqlXml("ListarModalidades"); }
        }
        public string SqlListarEstados
        {
            get { return base.GetSqlXml("ListarEstados"); }
        }
        public string SqlListarIncumplimientos
        {
            get { return base.GetSqlXml("ListarIncumplimientos"); }
        }

        public string SqlListarAgentesParaCalculo
        {
            get { return base.GetSqlXml("ListarAgentesParaCalculo"); }
        }

        public string SqlListarAgentesEntregaParaCalculo
        {
            get { return base.GetSqlXml("ListarAgentesEntregaParaCalculo"); }
        }

        public string SqlListarCabeceraValoresEnergia
        {
            get { return base.GetSqlXml("ListarCabeceraValoresEnergia"); }
        }

        public string SqlListarAgentes
        {
            get { return base.GetSqlXml("ListarAgentes"); }
        }

        public string SqlListarMaestroEmpresas
        {
            get { return base.GetSqlXml("ListarMaestroEmpresas"); }
        }
        public string SqlListarMaestroEmpresasCliente
        {
            get { return base.GetSqlXml("ListarMaestroEmpresasCliente"); }
        }
        public string SqlListarEmpresasParticipantes
        {
            get { return base.GetSqlXml("ListarEmpresasParticipantes"); }
        }
        public string SqlEstadoEmpresaSave
        {
            get { return base.GetSqlXml("EstadoEmpresaSave"); }
        }
        public string SqlEstadoEmpresaGetMaxId
        {
            get { return base.GetSqlXml("EstadoEmpresaGetMaxId"); }
        }

        public string SqlGetByIdEdit
        {
            get { return base.GetSqlXml("GetByIdEdit"); }
        }

        #region Mantenimiento Garantia - Modalidad

        public string SqlGetGarantiaById
        {
            get { return base.GetSqlXml("GetGarantiaById"); }
        }

        public GmmGarantiaDTO SqlGetGarantiaByIdEditarDTO(IDataReader dr)
        {
            GmmGarantiaDTO entity = new GmmGarantiaDTO();

            #region Agente
            int iGaraCodi = dr.GetOrdinal(this.GaraCodi);
            if (!dr.IsDBNull(iGaraCodi)) entity.GARACODI = Convert.ToInt32(dr.GetValue(iGaraCodi));


            int iGaraFecInicio = dr.GetOrdinal(this.GaraFecInicio);
            if (!dr.IsDBNull(iGaraFecInicio)) entity.GARAFECINICIO = dr.GetDateTime(iGaraFecInicio);

            int iGaraFecFin = dr.GetOrdinal(this.GaraFecFin);
            if (!dr.IsDBNull(iGaraFecFin)) entity.GARAFECFIN = dr.GetDateTime(iGaraFecFin);

            int iGaraMonto = dr.GetOrdinal(this.GaraMontoGarantia);
            if (!dr.IsDBNull(iGaraMonto)) entity.GARAMONTOGARANTIA = Convert.ToDecimal(dr.GetValue(iGaraMonto));

            int iGaraArchivo = dr.GetOrdinal(this.GaraArchivo);
            if (!dr.IsDBNull(iGaraArchivo)) entity.GARAARCHIVO = dr.GetString(iGaraArchivo);

            int iGaraEstado = dr.GetOrdinal(this.GaraEstado);
            if (!dr.IsDBNull(iGaraEstado)) entity.GARAESTADO = dr.GetString(iGaraEstado);

            int iTModCodi = dr.GetOrdinal(this.TModCodi);
            if (!dr.IsDBNull(iTModCodi)) entity.TMODCODI = dr.GetString(iTModCodi);


            int iTCerCodi = dr.GetOrdinal(this.TCerCodi);
            if (!dr.IsDBNull(iTModCodi)) entity.TCERCODI = dr.GetString(iTCerCodi);
                #endregion

            return entity;
        }

        #endregion

        public GmmEmpresaDTO SqlGetByIdEditDTO(IDataReader dr)
        {
            GmmEmpresaDTO entity = new GmmEmpresaDTO();

            #region Agente
            int iEmpCodiEdit = dr.GetOrdinal(this.EmpCodiEdit);
            if (!dr.IsDBNull(iEmpCodiEdit)) entity.EmpCodiEdit = Convert.ToInt32(dr.GetValue(iEmpCodiEdit));

            int iEmpNombreEdit = dr.GetOrdinal(this.EmpNombreEdit);
            if (!dr.IsDBNull(iEmpNombreEdit)) entity.EmpNombreEdit = dr.GetString(iEmpNombreEdit);

            int iEmpRucEdit = dr.GetOrdinal(this.EmpRucEdit);
            if (!dr.IsDBNull(iEmpRucEdit)) entity.EmpRucEdit = dr.GetString(iEmpRucEdit);

            int iEmpFecIngEdit = dr.GetOrdinal(this.EmpFecIngEdit);
            if (!dr.IsDBNull(iEmpFecIngEdit)) entity.EmpFecIngEdit = dr.GetDateTime(iEmpFecIngEdit);

            int iEmptpartEdit = dr.GetOrdinal(this.EmptpartEdit);
            if (!dr.IsDBNull(iEmptpartEdit)) entity.EmptpartEdit = dr.GetString(iEmptpartEdit);

            int iEmpestadoEdit = dr.GetOrdinal(this.EmpestadoEdit);
            if (!dr.IsDBNull(iEmpestadoEdit)) entity.EmpestadoEdit = dr.GetString(iEmpestadoEdit);

            int iEmpComentarioEdit = dr.GetOrdinal(this.EmpComentarioEdit);
            if (!dr.IsDBNull(iEmpComentarioEdit)) entity.EmpComentarioEdit = dr.GetString(iEmpComentarioEdit);

            #endregion

            return entity;
        }


    }
}
