using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.SGDoc;
using COES.Dominio.Interfaces.SGDoc;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper.SGDoc;

namespace COES.Infraestructura.Datos.Repositorio.SGDoc
{
    public class ConsultaRepository : RepositoryBase, IConsultaRepository
    {
        public ConsultaRepository(string strConn)
            : base(strConn)
        {
        }

        ConsultaHelper helper = new ConsultaHelper();

        public List<BandejaDTO> ListaDocumentosRecibidosMesaPartes(int origen, int rol, DateTime fechaIniRegistro, DateTime fechaFinRegistro,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, int idArea, string idRegistro, string numDocumento,
            string asunto, string codAtencion, string codigosEmpresa, bool indImportante, int idEtiqueta)
        {
            string query = string.Empty;

            #region Query

            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJREMITENTE = " + idEmpresaRemitente.ToString(); }
            if (idEmpresaRemitente == 0) { if (codigosEmpresa.Trim() != "") { where = " and f.FLJREMITENTE in ( " + codigosEmpresa + " )"; } }
            if (idRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(idRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (numDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(numDocumento.ToUpper()) + "%'"; }
            if (codAtencion != "00000000" && codAtencion != "")
            {
                where += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + "))) > 0";
            }
            if (indImportante) { where += " and f.fljprioridad = 'C' "; }
            query = "";

            query += " select f.fljcodi,f.corrnumproc,case when b.areaabrev = 'NONE' then '' else b.areaabrev end as NombAreaRem,b.areacode as areaorig,";
            query += " f.fljnumext,trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado,";
            query += " d.fljdetcodi,a.areaabrev as NombDestino,d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,";
            query += " d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax,h.ARCHCODIFICACION,k.ARCHSUBCODIF,s.emprnomb as NombEmpRem,";
            query += " f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib ,'N' as rpta, 'S' as leido, 0 as NMSG,d.fljestado as estadohijo,f.fljfechaterm as fatencion,f.fljprioridad as imp ";

            if (idEtiqueta < 0)
            {
                query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area b,doc_flujo_files g,doc_archivo h, doc_archivo_sub k,doc_parameter m";
                query += " where f.fljcodi = d.fljcodi";
            }
            else
            {
                query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area b,doc_flujo_files g,doc_archivo h, doc_archivo_sub k,doc_parameter m,doc_flujo_etiqueta e";
                query += " where f.fljcodi = d.fljcodi and f.fljcodi = e.fljcodi and e.etiqcode = " + idEtiqueta.ToString();

            }
            query += " and d.fljdetnivel = 0";
            query += " and d.areacode = a.areacode ";
            query += " and f.fljremitente = s.emprcodi ";
            query += " and f.fljdestext = 'N'";
            query += " and d.fljdetcodi = g.fljdetcodi(+) ";
            query += " and f.fljfecharecep between {0} and {1}";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.procodi = 1";
            query += " and d.areapadre = b.areacode ";
            query += " and f.FLJESTADO not in ('B') ";
            query += " and d.ARCHCODI = h.ARCHCODI(+)";
            query += " and d.ARCHSUBCODI = k.ARCHSUBCODI(+)";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;
            query += " order by frecepcion desc";

            #endregion



            //query a quitar
            //query = " select f.fljcodi,f.corrnumproc,case when b.areaabrev = 'NONE' then '' else b.areaabrev end as NombAreaRem,b.areacode as areaorig,";
            //query += " f.fljnumext,trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado,"; 
            //query += " d.fljdetcodi,a.areaabrev as NombDestino,d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,";
            //query += " d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax,h.ARCHCODIFICACION,k.ARCHSUBCODIF,s.emprnomb as NombEmpRem,";
            //query += " f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib ,'N' as rpta, 'S' as leido, 0 as NMSG,d.fljestado as estadohijo,f.fljfechaterm as fatencion,f.fljprioridad as imp ";
            //query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area b,doc_flujo_files g,doc_archivo h, doc_archivo_sub k,doc_parameter m";
            //query += " where f.fljcodi = d.fljcodi and rownum < 200";


            string sql = string.Format(query,
                "TO_DATE('" + fechaIniRegistro.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
                "TO_DATE('" + fechaFinRegistro.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");


            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            //dbProvider.AddInParameter(command, "fechaini", DbType.Date, fechaIniRegistro);
            //dbProvider.AddInParameter(command, "fechafin", DbType.Date, fechaFinRegistro);
            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, origen));
                }
            }


            return entitys;
        }

        public List<BandejaDTO> ListaDocumentosRemitidosMesaPartes(int origen, int rol, DateTime fechaIniRegistro, DateTime fechaFinRegistro,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, int idArea, string idRegistro, string numDocumento,
            string asunto, string codAtencion, string codigosEmpresa, bool indImportante, int idEtiqueta)
        {
            string query = string.Empty;

            #region Query

            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJDESTINATARIO = " + idEmpresaRemitente.ToString(); }
            if (idEmpresaRemitente == 0) { if (codigosEmpresa.Trim() != "") { where = " and f.FLJDESTINATARIO in ( " + codigosEmpresa + " )"; } }
            if (idRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(idRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (numDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(numDocumento.ToUpper()) + "%'"; }
            if (codAtencion != "00000000" && codAtencion != "")
            {
                where += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + "))) > 0";
            }
            if (idArea > 0) { where += " and f.areacode = " + idArea.ToString(); }

            if (indImportante) { where += " and f.fljprioridad='C'"; }

            query = @" select f.fljcodi ,f.corrnumproc,a.areaabrev as NombAreaRem,d.areapadre as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,
                     f.fljfecharecep  as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado ,
                     d.fljdetcodi,s.emprnomb as NombDestino , d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,
                     f.FLJFECHAPROCE, -1 as areadestino ,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax ,h.ARCHCODIFICACION,k.ARCHSUBCODIF,'COES' as NombEmpRem,
                     f.fljconf,f.fljanio,-1 as prioridad,f.fljfileatrib  ,'N' as rpta, 'S' as leido, 0 as NMSG  ,d.fljestado as estadohijo,f.fljfechaterm as fatencion,
                     f.fljprioridad as imp ";

            if (idEtiqueta < 0)
            {
                query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g  ,doc_archivo h,doc_archivo_sub k,doc_parameter m";
                query += " where f.fljcodi = d.fljcodi ";
            }
            else
            {
                query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g  ,doc_archivo h,doc_archivo_sub k,doc_parameter m,doc_flujo_etiqueta e";
                query += " where f.fljcodi = d.fljcodi and f.fljcodi = e.fljcodi and e.etiqcode = " + idEtiqueta.ToString();
            }

            query += " and d.fljdetnivel = 0  and d.areapadre = a.areacode and f.fljdestinatario = s.emprcodi and f.areacode is not null and f.fljdestext = 'S' ";
            query += " and d.fljdetcodi = g.fljdetcodi(+) ";
            query += " and f.fljfecharecep between {0} and {1}";
            query += " and f.fljremitente = 1 ";
            query += " and f.areacodedest <= 0 ";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.procodi = 4";
            query += " and f.FLJESTADO not in ('B') ";
            query += " and h.ARCHCODI(+) = d.ARCHCODI ";
            query += " and k.ARCHSUBCODI = d.ARCHSUBCODI";
            query += " and (h.ARCHCODI = k.ARCHICODI or k.ARCHICODI = -1 )";
            query += " and g.FILEATRIB='F'";
            query += " and g.fljcodi = f.fljcodi";
            query += " and g.fileserver = m.paramcodi";
            query += where;
            query += "  union ";

            query += @"select f.fljcodi ,f.corrnumproc,a.areaabrev as NombAreaRem, a.areacode as areaorig,f.fljnumext, f.fljnombre as ASUNTO,f.fljfecharecep  as frecepcion,
             m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado ,d.fljdetcodi, b.areaabrev as NombDestino , 
             d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario ,f.FLJFECHAPROCE,d.areacode as areadestino,
             d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax ,h.ARCHCODIFICACION,k.ARCHSUBCODIF ,'COES' as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib  ,'N' as rpta, 
             'S' as leido,  0 as NMSG   ,d.fljestado as estadohijo,f.fljfechaterm as fatencion ,f.fljprioridad as imp ";

            if (idEtiqueta < 0)
            {
                query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g,doc_archivo h,doc_archivo_sub k,doc_parameter m";
                query += " where f.fljcodi = d.fljcodi";
            }
            else
            {
                query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g,doc_archivo h,doc_archivo_sub k,doc_parameter m,doc_flujo_etiqueta e";
                query += " where f.fljcodi = d.fljcodi and f.fljcodi = e.fljcodi and e.etiqcode = " + idEtiqueta.ToString();
            }

            query += " and d.fljdetnivel = 0 ";
            query += " and f.areacode = a.areacode ";
            query += " and f.fljremitente = s.emprcodi ";
            query += " and f.fljremitente = 1 ";
            query += " and f.fljdestinatario = 1 ";
            query += " and f.fljfecharecep between {0} and {1}";
            query += " and d.fljdetcodi = g.fljdetcodi ";
            query += " and f.fljremitente = 1";
            query += " and f.areacodedest = b.areacode ";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.procodi = 4";
            query += " and f.FLJESTADO not in ('B') ";
            query += " and h.ARCHCODI = d.ARCHCODI ";
            query += " and k.ARCHSUBCODI = d.ARCHSUBCODI";
            query += " and (h.ARCHCODI = k.ARCHICODI  or k.ARCHICODI = -1)";
            query += " and g.FILEATRIB='F'";
            query += " and g.fljcodi = f.fljcodi";
            query += " and g.fileserver = m.paramcodi";
            query += where;
            query += " order by 7 desc";

            //query a modificar
            //            query = @" select f.fljcodi ,f.corrnumproc,a.areaabrev as NombAreaRem,d.areapadre as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,
            //                 f.fljfecharecep  as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado ,
            //                 d.fljdetcodi,s.emprnomb as NombDestino , d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,
            //                 f.FLJFECHAPROCE, -1 as areadestino ,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax ,h.ARCHCODIFICACION,k.ARCHSUBCODIF,'COES' as NombEmpRem,
            //                 f.fljconf,f.fljanio,-1 as prioridad,f.fljfileatrib  ,'N' as rpta, 'S' as leido, 0 as NMSG  ,d.fljestado as estadohijo,f.fljfechaterm as fatencion,
            //                 f.fljprioridad as imp ";           
            //            query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g  ,doc_archivo h,doc_archivo_sub k,doc_parameter m";
            //            query += " where f.fljcodi = d.fljcodi and rownum < 200";


            #endregion

            string sql = string.Format(query,
                "TO_DATE('" + fechaIniRegistro.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
                "TO_DATE('" + fechaFinRegistro.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");


            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            //DbCommand command = dbProvider.GetSqlStringCommand(query);
            //dbProvider.AddInParameter(command, "fechaini", DbType.Date, fechaIniRegistro);
            //dbProvider.AddInParameter(command, "fechafin", DbType.Date, fechaFinRegistro);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, origen));
                }
            }


            return entitys;
        }

        public List<BandejaDTO> ListaDocumentosRecibidosAreas(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool plazo, string codAtencion, bool conRpta, int tipoRecepcion, string codigosEmpresa, bool remExterno, bool indImportante,
            int idEtiqueta)
        {
            string query = string.Empty;

            #region Query

            string filtro = string.Empty;
            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJREMITENTE = " + idEmpresaRemitente.ToString(); }
            if (idEmpresaRemitente == 0) { if (codigosEmpresa.Trim() != "") { where = " and f.FLJREMITENTE in ( " + codigosEmpresa + " )"; } }
            if (codRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(codRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (nroDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(nroDocumento.ToUpper()) + "%'"; }
            if (plazo) { where += " and f.fljfechamax is not null"; }
            if (codAtencion != "00000000" && codAtencion != "")
            {
                where += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + "))) > 0";
            }
            if (estadoAtencion == EstadoAtencion.Pend_or_Derivado) { where += " and d.fljestado in ('P','D')"; }
            else if (estadoAtencion != EstadoAtencion.Todos) { where += " and d.fljestado = '" + (char)estadoAtencion + "'"; }
            if (indImportante) { where += " and f.fljprioridad = 'C' "; }
            filtro = " and d.percodi =" + idPersona.ToString();

            query = "";

            if (tipoRecepcion == 1 || tipoRecepcion == 3 || tipoRecepcion == -1)
            {
                query += @"  select f.fljcodi,f.corrnumproc,case when b.areaabrev ='NONE' then '' else b.areaabrev end as NombAreaRem,b.areacode as areaorig,f.fljnumext,trim(f.fljnombre) as ASUNTO,
                             f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,  f.fljremdetalle,d.fljestado as fljestado, d.fljdetcodi, a.areaabrev as NombDestino,
                             d.fljdetdestino, d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,
                             f.FLJFECHAMAX as fmax, NULL AS ARCHCODIFICACION, NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad,f.fljfileatrib,'N' as rpta, 'S' as leido, 0 as NMSG,
                             d.fljestado as estadohijo ,d.fljfechaatencion as fatencion ,f.fljprioridad as imp  ";

                if (idEtiqueta < 0)
                {
                    query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area  b,doc_flujo_files g ,doc_parameter m ";
                    query += " where f.fljcodi = d.fljcodi ";
                }
                else
                {
                    query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area  b,doc_flujo_files g ,doc_parameter m ,doc_flujo_etiqueta e";
                    query += " where f.fljcodi = d.fljcodi  and f.fljcodi = e.fljcodi and e.etiqcode = " + idEtiqueta.ToString();
                }

                query += " and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljdestext = 'N' and d.fljcodi = g.fljcodi and d.areacode = " + idArea.ToString();//Con esto son las enviad
                query += " and d.areapadre = b.areacode and d.fljfechainicio between {0} and {1}";
                query += " and d.fljdetnivel < 98 and f.FLJESTADO not in ('N', 'B') and d.fljestado not in ('N', 'B')";
                query += " and f.fljremitente <> 1";
                query += " and d.fljdetnivel = 0";
                query += " and g.FILEATRIB(+)='F'";
                query += " and g.fileserver = m.paramcodi";

                query += where + filtro;
            }
            if (tipoRecepcion == 2 || tipoRecepcion == 3 || tipoRecepcion == -1)
            {
                if (tipoRecepcion == 3 || tipoRecepcion == -1) { query += " union "; }

                query += @"  select f.fljcodi,f.corrnumproc,case when b.areaabrev ='NONE' then '' else b.areaabrev end as NombAreaRem,b.areacode as areaorig,
                         f.fljnumext,trim(f.fljnombre) as ASUNTO,d.FLJFECHAINICIO as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,  
                         f.fljremdetalle,d.fljestado as fljestado, d.fljdetcodi, a.areaabrev as NombDestino,d.fljdetdestino, d.fljdetorigen,d.fljdetnivel,
                         d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,d.FLJFECHAMAX as fmax, 
                         NULL AS ARCHCODIFICACION, NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib,'N' as rpta, 'S' as leido,
                         0 as NMSG, d.fljestado as estadohijo ,d.fljfechaatencion as fatencion,f.fljprioridad as imp  ";

                if (idEtiqueta < 0)
                {
                    query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area  b,doc_flujo_files g ,doc_parameter m";
                    query += " where f.fljcodi = d.fljcodi ";
                }
                else
                {
                    query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area  b,doc_flujo_files g ,doc_parameter m,doc_flujo_etiqueta e";
                    query += " where f.fljcodi = d.fljcodi and f.fljcodi = e.fljcodi and e.etiqcode = " + idEtiqueta.ToString();
                }

                query += " and d.areacode = a.areacode and f.fljremitente = s.emprcodi and d.fljcodi = g.fljcodi and d.areacode = " + idArea.ToString();//Con esto son las enviados
                query += " and d.areapadre = b.areacode and d.fljfechainicio between {0} and {1}";
                query += " and d.fljdetnivel < 98 and f.FLJESTADO not in ('N', 'B') and d.fljestado not in ('N', 'B')";
                query += " and d.fljdetnivel >= 0";
                query += " and d.areapadre > 0";
                query += " and g.FILEATRIB(+)='F'";
                query += " and g.fileserver = m.paramcodi";
                query += where + filtro;

                if (conRpta)
                {
                    query += " union ";

                    query += @"  select f.fljcodi,f.corrnumproc,a.areaabrev as NombAreaRem,d.areacode as areaorig,f.fljnumext,trim(f.fljnombre) as ASUNTO,m.fljfechamsg as frecepcion, t.paramvalue||g.fileruta 
                                 as xfileruta,g.filecodi, f.fljremdetalle,d.fljestado as fljestado,d.fljdetcodi,b.areaabrev as NombDestino,d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion, 
                                 d.fljpadrecomentario,f.FLJFECHAPROCE,  b.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,d.FLJFECHAMAX as fmax,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF, 'COES' as NombEmpRem,
                                 f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib,'S' as rpta, M.FLJMSGLEIDO as leido , d.fljnmsghijo as NMSG,  d.fljestado as estadohijo ,d.fljfechaatencion as fatencion,f.fljprioridad as imp  ";


                    if (idEtiqueta < 0)
                    {
                        query += " from doc_flujo f,doc_flujo_det d,fw_area a,fw_area b,doc_flujo_files g,doc_flujo_det_msg M,doc_parameter t";
                        query += " where f.fljcodi = d.fljcodi ";
                    }
                    else
                    {
                        query += " from doc_flujo f,doc_flujo_det d,fw_area a,fw_area b,doc_flujo_files g,doc_flujo_det_msg M,doc_parameter t,doc_flujo_etiqueta e";
                        query += " where f.fljcodi = d.fljcodi and f.fljcodi = e.fljcodi and e.etiqcode = " + idEtiqueta.ToString();
                    }

                    query += " and d.areacode = a.areacode and d.fljcodi = g.fljcodi and d.areapadre = " + idArea.ToString() + " and d.areapadre = b.areacode ";
                    query += " and m.fljfechamsg between {0} and {1} ";

                    query += " and d.fljdetnivel < 98 and f.FLJESTADO not in ('N', 'B') and d.fljestado not in ('N', 'B') and M.FLJMSGESTADO not in ('N', 'B') ";
                    query += " and d.fljdetnivel >= 1 and d.areapadre > 0 and g.FILEATRIB(+) = 'F'";
                    query += " and m.FLJMSGESTADO = 'A'and m.fljdetcodi = d.fljdetcodi and m.areadest = " + idArea.ToString();
                    query += " and g.fileserver = t.paramcodi";
                    if (idPersona < 0)
                    {
                        query += " and d.fljmaster = 'S'";
                    }
                    else
                    {
                        query += " and d.fljmaster = 'N'";
                        query += " and d.percodipadre = " + idPersona.ToString();
                    }
                    query += where;
                }
            }
            query += " order by frecepcion desc";

            #endregion


            string sql = string.Format(query,
                "TO_DATE('" + fechaInicial.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
                "TO_DATE('" + fechaFinal.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            //dbProvider.AddInParameter(command, "fechaini", DbType.Date, fechaInicial);
            //dbProvider.AddInParameter(command, "fechafin", DbType.Date, fechaFinal);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, origen));
                }
            }

            return entitys;
        }

        public List<BandejaDTO> ListaDocumentosRemitidosAreas(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, bool plazo, string codAtencion, bool conRpta, int tipoRecepcion, string codigosEmpresa, bool remExterno, bool indImportante,
            int idEtiqueta)
        {
            string query = string.Empty;

            #region Query

            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJDESTINATARIO = " + idEmpresaRemitente.ToString(); }
            if (idEmpresaRemitente == 0) { if (codigosEmpresa.Trim() != "") { where = " and f.FLJDESTINATARIO in ( " + codigosEmpresa + " )"; } }
            if (codRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(codRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (nroDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(nroDocumento.ToUpper()) + "%'"; }

            if (codAtencion != "00000000" && codAtencion != "")
            {
                where += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + "))) > 0";
            }
            where += " and d.percodi = " + idPersona.ToString();
            if (indImportante) { where += " and f.fljprioridad = 'C'"; }
            query = "";
            query = " select f.fljcodi ,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,s.emprnomb as NombDestino , d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,'COES' as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad,f.fljfileatrib,d.fljfechaatencion as fatencion";
            query += " ,'N' as rpta, 'S' as leido ,0 as NMSG, d.fljestado as estadohijo ,f.fljprioridad as imp ";

            if (idEtiqueta < 0)
            {
                query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m ";
                query += " where f.fljcodi = d.fljcodi ";
            }
            else
            {
                query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m ,doc_flujo_etiqueta e";
                query += " where f.fljcodi = d.fljcodi and f.fljcodi = e.fljcodi and e.etiqcode = " + idEtiqueta.ToString();
            }

            query += " and d.fljdetnivel = 0  and d.areapadre = a.areacode and f.fljdestinatario = s.emprcodi and f.areacode is not null and f.fljdestext = 'S' ";
            query += " and g.fljcodi = d.fljcodi and f.areacode =  " + idArea.ToString();
            query += " and f.fljfecharecep between {0} and {1}";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and f.fljdestinatario > 1 ";
            query += " and f.fljremitente = 1 ";
            query += " and f.procodi = 4 ";
            query += " and g.FILEATRIB='F'";
            query += " and g.fljcodi = f.fljcodi";
            query += " and g.fileserver = m.paramcodi";

            query += where;

            if (!remExterno)
            {
                query += "  union ";
                query += " select f.fljcodi ,f.corrnumproc,a.areaabrev as NombAreaRem, a.areacode as areaorig,f.fljnumext, f.fljnombre as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,";
                query += " b.areaabrev as NombDestino , d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario ,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
                query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,'COES' as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib,d.fljfechaatencion as fatencion";
                query += " ,'N' as rpta, 'S' as leido ,0 as NMSG, d.fljestado as estadohijo  ,f.fljprioridad as imp";

                if (idEtiqueta < 0)
                {
                    query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g,doc_parameter m";
                    query += " where f.fljcodi = d.fljcodi";
                }
                else
                {
                    query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g,doc_parameter m,doc_flujo_etiqueta e";
                    query += " where f.fljcodi = d.fljcodi  and f.fljcodi = e.fljcodi and e.etiqcode = " + idEtiqueta.ToString();
                }

                query += " and d.fljdetnivel = 0 ";
                query += " and d.areapadre = a.areacode ";
                query += " and f.fljremitente = s.emprcodi ";
                query += " and f.fljremitente = 1 ";
                query += " and f.fljdestinatario = 1 ";
                query += " and f.fljfecharecep between {0} and {1}";
                query += " and g.fljcodi = d.fljcodi ";
                query += " and f.areacode = " + idArea.ToString();
                query += " and f.fljremitente = 1";
                query += " and f.areacodedest = b.areacode ";
                query += " and d.fljdetnivel < 98 ";
                query += " and f.FLJESTADO not in ( 'N','B') ";
                query += " and g.FILEATRIB(+)='F'";
                query += " and g.fljcodi = f.fljcodi";
                query += " and g.fileserver = m.paramcodi";
                query += where;
            }

            query += " order by 7 desc";

            #endregion

            //query a modificar

            //query = " select f.fljcodi ,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,s.emprnomb as NombDestino , d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            //query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,'COES' as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad,f.fljfileatrib,d.fljfechaatencion as fatencion";
            //query += " ,'N' as rpta, 'S' as leido ,0 as NMSG, d.fljestado as estadohijo ,f.fljprioridad as imp ";
            //query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m ";
            //query += " where f.fljcodi = d.fljcodi and rownum < 200 ";


            string sql = string.Format(query,
               "TO_DATE('" + fechaInicial.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
               "TO_DATE('" + fechaFinal.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");


            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            //dbProvider.AddInParameter(command, "fechaini", DbType.Date, fechaInicial);
            //dbProvider.AddInParameter(command, "fechafin", DbType.Date, fechaFinal);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, origen));
                }
            }


            return entitys;
        }

        public List<BandejaDTO> ListarDocumentosReclamos(DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente, string codRegistro,
            string nroDocumento, string asunto, bool indImportante)
        {
            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJREMITENTE = " + idEmpresaRemitente.ToString(); }
            if (codRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(codRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (nroDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(nroDocumento.ToUpper()) + "%'"; }
            if (indImportante) { where += " and f.fljprioridad = 'C' "; }

            string query = string.Empty;

            query += " select f.fljcodi,f.corrnumproc,case when b.areaabrev = 'NONE' then '' else b.areaabrev end as NombAreaRem,b.areacode as areaorig,";
            query += " f.fljnumext,trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado,"; //MP toma el estado del tramite
            query += " d.fljdetcodi,a.areaabrev as NombDestino,d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,";
            query += " d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax,h.ARCHCODIFICACION,k.ARCHSUBCODIF,s.emprnomb as NombEmpRem,";
            query += " f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib ,'N' as rpta, 'S' as leido,d.fljestado as estadohijo,f.fljfechaterm as fatencion ,f.fljprioridad as imp, '' as Nmsg";
            query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area b,doc_flujo_files g,doc_archivo h, doc_archivo_sub k,doc_parameter m";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel = 0";
            query += " and d.areacode = a.areacode ";
            query += " and f.fljremitente = s.emprcodi ";
            query += " and f.fljdestext = 'N'";
            query += " and d.fljdetcodi = g.fljdetcodi(+) ";
            query += " and f.fljfecharecep between {0} and {1}";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.procodi = 1";
            query += " and d.areapadre = b.areacode ";
            query += " and f.FLJESTADO not in ('B') ";
            query += " and d.ARCHCODI = h.ARCHCODI(+)";
            query += " and d.ARCHSUBCODI = k.ARCHSUBCODI(+)";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += " and f.tipcodi = 22";
            query += where;
            query += " order by frecepcion desc";



            string sql = string.Format(query,
                "TO_DATE('" + fechaInicial.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
                "TO_DATE('" + fechaFinal.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");


            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            //dbProvider.AddInParameter(command, "fechaini", DbType.Date, fechaInicial);
            //dbProvider.AddInParameter(command, "fechafin", DbType.Date, fechaFinal);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, 0));
                }
            }

            return entitys;
        }

        public List<BandejaDTO> ListarDocumentosNoDespachadosRecibidos(DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente,
            string codRegistro, string nroDocumento, string asunto, bool indImportante)
        {
            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJREMITENTE = " + idEmpresaRemitente.ToString(); }
            if (codRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(codRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (nroDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(nroDocumento.ToUpper()) + "%'"; }
            if (indImportante) { where += " and f.fljprioridad = 'C' "; }

            string query = string.Empty;

            query += " select f.fljcodi,f.corrnumproc,case when b.areaabrev = 'NONE' then '' else b.areaabrev end as NombAreaRem,b.areacode as areaorig,";
            query += " f.fljnumext,trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado,"; //MP toma el estado del tramite
            query += " d.fljdetcodi,a.areaabrev as NombDestino,d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,";
            query += " d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax,h.ARCHCODIFICACION,k.ARCHSUBCODIF,s.emprnomb as NombEmpRem,";
            query += " f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib ,'N' as rpta, 'S' as leido,d.fljestado as estadohijo,f.fljfechaterm as fatencion ,f.fljprioridad as imp, '' as Nmsg";
            query += " from doc_flujo f,doc_flujo_det d,si_empresa s,fw_area a,fw_area b,doc_flujo_files g,doc_archivo h, doc_archivo_sub k,doc_parameter m";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel = 0";
            query += " and d.areacode = a.areacode ";
            query += " and f.fljremitente = s.emprcodi ";
            query += " and f.fljdestext = 'N'";
            query += " and d.fljdetcodi = g.fljdetcodi(+) ";
            query += " and f.fljfecharecep between {0} and {1}";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.procodi = 1";
            query += " and d.areapadre = b.areacode ";
            query += " and f.FLJESTADO not in ('B') ";
            query += " and d.ARCHCODI = h.ARCHCODI(+)";
            query += " and d.ARCHSUBCODI = k.ARCHSUBCODI(+)";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += " and f.fljestado = 'N'";
            query += where;

            string sql = string.Format(query,
                 "TO_DATE('" + fechaInicial.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
                 "TO_DATE('" + fechaFinal.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");


            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, 0));
                }
            }

            return entitys;
        }

        public List<BandejaDTO> ListarDocumentosNoDespachadosRemitidos(DateTime fechaInicial, DateTime fechaFinal, int idEmpresaRemitente,
            string codRegistro, string nroDocumento, string asunto, bool indImportante)
        {
            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJDESTINATARIO = " + idEmpresaRemitente.ToString(); }
            if (codRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(codRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (nroDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(nroDocumento.ToUpper()) + "%'"; }
            if (indImportante) { where += " and f.fljprioridad = 'C'"; }

            string query = string.Empty;

            /*COES envia a EMPRESA*/
            query = " select f.fljcodi ,f.corrnumproc,a.areaabrev as NombAreaRem,-1 as areadestino,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep  as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado ,d.fljdetcodi,s.emprnomb as NombDestino , d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areapadre as areaorig ,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " ,h.ARCHCODIFICACION,k.ARCHSUBCODIF,'COES' as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad,f.fljfileatrib ";
            query += " ,'N' as rpta, 'S' as leido  ,d.fljestado as estadohijo,f.fljfechaterm as fatencion ,f.fljprioridad as imp, '' as Nmsg";
            query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g  ,doc_archivo h,doc_archivo_sub k,doc_parameter m";
            query += " where f.fljcodi = d.fljcodi and d.fljdetnivel = 0  and d.areapadre = a.areacode and f.fljdestinatario = s.emprcodi and f.areacode is not null and f.fljdestext = 'S' ";
            query += " and d.fljdetcodi = g.fljdetcodi(+) "; //Diferente del area actual D
            query += " and f.fljfecharecep between {0} and {1} ";
            //ls_SQL += " and f.fljdestinatario > 1 "; //Diferente del COES
            query += " and f.fljremitente = 1 ";
            query += " and f.areacodedest <= 0 ";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.procodi = 4";
            query += " and f.FLJESTADO not in ('B') ";
            query += " and h.ARCHCODI(+) = d.ARCHCODI ";
            query += " and k.ARCHSUBCODI(+) = d.ARCHSUBCODI";
            query += " and (h.ARCHCODI = k.ARCHICODI or k.ARCHICODI = -1 )";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += " and f.fljestado = 'N'";
            query += where;

            query += "  union ";
            /*COES envia al COES nuevo*/

            query += " select f.fljcodi ,f.corrnumproc,a.areaabrev as NombAreaRem, a.areacode as areaorig,f.fljnumext, f.fljnombre as ASUNTO,f.fljfecharecep  as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, f.fljestado as fljestado ,d.fljdetcodi,";
            query += " b.areaabrev as NombDestino , d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario ,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " ,h.ARCHCODIFICACION,k.ARCHSUBCODIF ,'COES' as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib ";
            query += " ,'N' as rpta, 'S' as leido  ,d.fljestado as estadohijo,f.fljfechaterm as fatencion ,f.fljprioridad as imp, '' as Nmsg";
            query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g,doc_archivo h,doc_archivo_sub k,doc_parameter m";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel = 0 ";
            query += " and f.areacode = a.areacode ";
            query += " and f.fljremitente = s.emprcodi ";
            query += " and f.fljremitente = 1 ";
            query += " and f.fljdestinatario = 1 ";
            query += " and f.fljfecharecep between {0} and {1} ";
            query += " and d.fljdetcodi = g.fljdetcodi(+) ";
            query += " and f.fljremitente = 1";
            query += " and f.areacodedest = b.areacode ";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.procodi = 4";
            query += " and f.FLJESTADO not in ('B') ";
            query += " and h.ARCHCODI = d.ARCHCODI ";
            query += " and k.ARCHSUBCODI = d.ARCHSUBCODI";
            query += " and (h.ARCHCODI = k.ARCHICODI  or k.ARCHICODI = -1)";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += " and f.fljestado = 'N'";
            query += where;
            query += " order by 7 desc";


            //DbCommand command = dbProvider.GetSqlStringCommand(query);
            //dbProvider.AddInParameter(command, "fechaini", DbType.Date, fechaInicial);
            //dbProvider.AddInParameter(command, "fechafin", DbType.Date, fechaFinal);


            string sql = string.Format(query,
                "TO_DATE('" + fechaInicial.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
                "TO_DATE('" + fechaFinal.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");


            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, 0));
                }
            }

            return entitys;
        }

        public List<BandejaDTO> ListarDocumentosRecibidosPlazo(int origen, int idArea, int idPersona, int idRol, EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro,
            int idEmpresaRemitente, string codRegistro, string nroDocumento, string asunto, string codAtencion, bool indImportante)
        {
            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJREMITENTE = " + idEmpresaRemitente.ToString(); }
            if (codRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(codRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (nroDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(nroDocumento.ToUpper()) + "%'"; }
            where += " and f.fljfechamax is not null";

            if (codAtencion != "00000000")
            {
                where += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + "))) > 0";
            }
            where += " and d.percodi = " + idPersona.ToString();
            where += " and f.fljprioridad = 'C'";

            string query = string.Empty;

            /*Empresa envia al COES*/
            query = " select f.fljcodi ,f.corrnumproc,'' as NombAreaRem,-1 as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad ,f.fljfileatrib, d.fljfechaatencion as fatencion,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m  ";// ",doc_archivo h,doc_archivo_sub k ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente <> 1 and f.fljdestext = 'N'";
            query += "  and d.fljdetcodi = g.fljdetcodi and d.areacode = " + idArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B','A') ";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;

            query += "  union ";

            /*COES envia al COES nuevo*/

            query += " select f.fljcodi ,f.corrnumproc,b.areaabrev as NombAreaRem,b.areacode as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib, d.fljfechaatencion as fatencion,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g ,doc_parameter m ";// ",doc_archivo h,doc_archivo_sub k ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente = 1 and f.fljdestext = 'N'";
            query += "  and d.fljdetcodi = g.fljdetcodi and d.areacode = " + idArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D
            query += "  and b.areacode = d.areapadre ";
            query += "  and d.areapadre > 0";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B','A') ";
            query += " and d.fljfechamax is not null";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;
            query += "  union ";

            /*COES recive del COES por Derivacon NUEVO*/
            //ls_SQL += " select f.fljcodi,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext,f.fljnombre as asunto,d.fljfechainicio as frecepcion,g.fileruta,'', case when f.fljestado != 'P' then f.fljestado else d.fljestado end as fljestado ,d.fljdetcodi,b.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " select f.fljcodi,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext,f.fljnombre as asunto,d.fljfechainicio as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,'', d.fljestado as fljestado ,d.fljdetcodi,b.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem ,f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib, d.fljfechaatencion as fatencion,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo ";
            query += " from doc_flujo f,doc_flujo_det d,fw_area a,fw_area b,doc_flujo_files g,si_empresa s,doc_parameter m "; //",doc_archivo h,doc_archivo_sub k ";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel >= 1 ";
            query += " and d.areapadre = a.areacode and f.fljremitente = s.emprcodi";
            query += " and d.areacode = b.areacode ";
            query += " and f.fljcodi = g.fljcodi"; //Documento Original
            query += " and d.areacode = " + idArea.ToString();
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B','X','A') ";
            query += " and d.fljfechamax is not null";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;

            query += " order by 20 desc,2 desc ";

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, origen));
                }
            }

            return entitys;
        }

        public List<BandejaDTO> ListarDocumentosRecibidosPlazo(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, DateTime fechaFinal,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro,
            int idEmpresaRemitente, string codRegistro, string nroDocumento, string asunto, string codAtencion, bool indTodos, bool indImportante)
        {
            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJREMITENTE = " + idEmpresaRemitente.ToString(); }
            if (codRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(codRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (nroDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(nroDocumento.ToUpper()) + "%'"; }
            where += " and f.fljfechamax is not null";
            if (codAtencion != "00000000")
            {
                where += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + "))) > 0";
            }
            where += " and d.percodi = " + idPersona.ToString();

            if (indTodos) { where += " and f.FLJESTADO not in ( 'B') and d.fljestado not in ( 'A')"; }
            if (indImportante) { where += " and f.fljprioridad = 'C'"; }

            string query = string.Empty;

            /*Empresa envia al COES*/
            query = " select f.fljcodi ,f.corrnumproc,'' as NombAreaRem,-1 as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, case when f.fljestado != 'P' then f.fljestado else d.fljestado end as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad ,f.fljfileatrib,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m ";// ",doc_archivo h,doc_archivo_sub k ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente <> 1 and f.fljdestext = 'N'";
            query += "  and d.fljdetcodi = g.fljdetcodi and d.areacode = " + idArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D
            query += "  and f.fljfecharecep between {0} and {1} ";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B') ";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;

            query += "  union ";

            /*COES envia al COES nuevo*/
            query += " select f.fljcodi ,f.corrnumproc,b.areaabrev as NombAreaRem,b.areacode as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, case when f.fljestado != 'P' then f.fljestado else d.fljestado end as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio ,d.fljdetprio as prioridad,f.fljfileatrib ,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g,doc_parameter m ";// ",doc_archivo h,doc_archivo_sub k ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente = 1 and f.fljdestext = 'N'";
            query += "  and d.fljdetcodi = g.fljdetcodi and d.areacode = " + idArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D
            query += "  and b.areacode = d.areapadre ";
            query += "  and d.areapadre > 0";
            query += "  and f.fljfecharecep between {0} and {1} ";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B') ";
            query += " and d.fljfechamax is not null ";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;

            query += "  union ";


            /*COES recive del COES por Derivacon NUEVO*/
            query += " select f.fljcodi,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext,f.fljnombre as asunto,d.fljfechainicio as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,'', case when f.fljestado != 'P' then f.fljestado else d.fljestado end as fljestado ,d.fljdetcodi,b.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem ,f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib ,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += " from doc_flujo f,doc_flujo_det d,fw_area a,fw_area b,doc_flujo_files g,si_empresa s,doc_parameter m "; //",doc_archivo h,doc_archivo_sub k ";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel >= 1 ";
            query += " and d.areapadre = a.areacode ";
            query += " and d.areacode = b.areacode  and f.fljremitente = s.emprcodi";
            query += " and d.fljfechainicio between {0} and {1} ";
            query += " and f.fljcodi = g.fljcodi"; //Documento Original
            query += " and d.areacode = " + idArea.ToString();
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B','X','A') ";
            query += " and d.fljfechamax is not null ";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;

            query += " order by fljanio desc, 7 desc, 2 desc ";



            string sql = string.Format(query,
               "TO_DATE('" + fechaInicial.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
               "TO_DATE('" + fechaFinal.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");


            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            //dbProvider.AddInParameter(command, "fechaini", DbType.Date, fechaInicial);
            //dbProvider.AddInParameter(command, "fechafin", DbType.Date, fechaFinal);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, origen));
                }
            }

            return entitys;
        }

        public List<BandejaDTO> ListarDocumentosDerivadosPlazo(int origen, int idArea, int idPersona, int idRol,
            EstadoAtencion estadoAtencion, EstadoRegistro estadoRegistro, int idEmpresaRemitente, string codRegistro, string nroDocumento,
            string asunto, string codAtencion, bool indJefatura, bool indImportante)
        {
            string where = string.Empty;

            if (idEmpresaRemitente > 0) { where = " and f.FLJREMITENTE = " + idEmpresaRemitente.ToString(); }
            if (codRegistro.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.CORRNUMPROC)) like '%" + Util.RemoverTildes(codRegistro.ToUpper()) + "%'"; }
            if (asunto.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNOMBRE)) like '%" + Util.RemoverTildes(asunto.ToUpper()) + "%'"; }
            if (nroDocumento.Trim() != "") { where += " and UPPER(FN_DOC_CAMB_TILDE(f.FLJNUMEXT)) like '%" + Util.RemoverTildes(nroDocumento.ToUpper()) + "%'"; }
            where += " and d.fljfechamax is not null";
            if (codAtencion != "00000000")
            {
                where += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + "))) > 0";
            }
            if (indJefatura) //Remitido por jefatura
            {
                where += " and d.fljmaster='S' ";
            }
            else
            {
                where += " and d.PERCODIPADRE = " + idPersona.ToString();
            }

            if (indImportante) { where += " and f.fljprioridad= 'C'"; }

            string query = string.Empty;
            /*Empresa envia al COES*/

            query = " select f.fljcodi ,f.corrnumproc,'' as NombAreaRem,-1 as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad ,f.fljfileatrib,d.fljfechaatencion as fatencion,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m ";// ",doc_archivo h,doc_archivo_sub k ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente <> 1 and f.fljdestext = 'N'";
            query += "  and d.fljdetcodi = g.fljdetcodi and d.areapadre = " + idArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B','A') ";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;

            query += "  union ";

            /*COES envia al COES nuevo*/

            query += " select f.fljcodi ,f.corrnumproc,b.areaabrev as NombAreaRem,b.areacode as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib,d.fljfechaatencion as fatencion ,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g,doc_parameter m ";// ",doc_archivo h,doc_archivo_sub k ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente = 1 and f.fljdestext = 'N'";
            query += "  and d.fljdetcodi = g.fljdetcodi and d.areapadre = " + idArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D
            query += "  and b.areacode = d.areapadre ";
            query += "  and d.areapadre > 0";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B','A') ";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;
            query += "  union ";

            /*COES recive del COES por Derivacon NUEVO*/

            query += " select f.fljcodi,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext,f.fljnombre as asunto,d.fljfechainicio as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,'', d.fljestado as fljestado ,d.fljdetcodi,b.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem ,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib,d.fljfechaatencion as fatencion,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo";
            query += " from doc_flujo f,doc_flujo_det d,fw_area a,fw_area b,doc_flujo_files g,si_empresa s,doc_parameter m "; //",doc_archivo h,doc_archivo_sub k ";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel >= 1 ";
            query += " and d.areapadre = a.areacode and f.fljremitente = s.emprcodi";
            query += " and d.areacode = b.areacode ";
            query += " and f.fljcodi = g.fljcodi"; //Documento Original
            query += " and d.areapadre = " + idArea.ToString();
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B') ";
            query += " and d.fljestado not in ( 'N','B','X','A') ";
            query += " and g.FILEATRIB(+)='F'";
            query += " and g.fileserver = m.paramcodi";
            query += where;

            query += " order by fljanio desc,7 desc,2 desc ";

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(bandejaHelper.Create(dr, origen));
                }
            }

            return entitys;
        }

        public List<BandejaDTO> ListarDocumentosPlazo(int origen, int idArea, int idPersona, int idRol, DateTime fechaInicial, int diasPlazo, string codAtencion, bool indImportante, string indicador)
        {
            string query = string.Empty;
            string where1 = string.Empty;
            string where2 = string.Empty;

            where1 += " and d.fljfechamax is not null";
            where1 += " and ( ((d.fljfechamax - :fljfechamax) between  0 and :diasplazo ) or ( (d.fljfechamax < :fljfechamax ) and d.fljestado in ('P','D')  ) )";

            if (idPersona > 0)
            {
                where1 += " and d.percodi = " + idPersona.ToString();
            }
            if (indImportante) { where1 += " and f.fljprioridad = 'C'"; }
            if (codAtencion != "00000000")
            {
                where2 += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + "))) > 0";
            }

            query = " select f.fljcodi ,f.corrnumproc,'' as NombAreaRem,-1 as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad ,f.fljfileatrib,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m ";// ",doc_archivo h,doc_archivo_sub k ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente <> 1 and f.fljdestext = 'N'";
            query += "  and d.fljdetcodi = g.fljdetcodi ";
            query += " and g.fileserver = m.paramcodi";
            if (idArea > 0)
            {
                query += " and d.areacode = " + idArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D            
            }
            else
            {
                query += " and d.fljdetnivel = 0 ";
            }

            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','A','X') ";
            query += " and d.fljestado not in ( 'N','B','A','X') ";
            query += " and g.FILEATRIB(+)='F'";
            query += where1;
            query += "  union ";

            query += " select f.fljcodi ,f.corrnumproc,b.areaabrev as NombAreaRem,b.areacode as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib ,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g ,doc_parameter m";// ",doc_archivo h,doc_archivo_sub k ";
            query += " where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente = 1 and f.fljdestext = 'N'";
            query += " and d.fljdetcodi = g.fljdetcodi ";
            query += " and g.fileserver = m.paramcodi";
            if (idArea > 0)
            {
                query += " and d.areacode = " + idArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D
            }
            else
            {
                query += " and d.fljdetnivel = 0";
            }

            query += " and b.areacode = d.areapadre ";
            query += "  and d.areapadre > 0";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','X') ";
            query += " and d.fljestado not in ( 'N','B','A','X') ";
            query += " and d.fljfechamax is not null ";
            query += " and g.FILEATRIB(+)='F'";
            query += where1;
            query += "  union ";


            query += " select f.fljcodi,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext,f.fljnombre as asunto,d.fljfechainicio as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,'', d.fljestado as fljestado ,d.fljdetcodi,b.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem ,f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += " from doc_flujo f,doc_flujo_det d,fw_area a,fw_area b,doc_flujo_files g,si_empresa s,doc_parameter m "; //",doc_archivo h,doc_archivo_sub k ";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel >= 1 ";
            query += " and d.areapadre = a.areacode ";
            query += " and d.areacode = b.areacode  and f.fljremitente = s.emprcodi";
            query += " and f.fljcodi = g.fljcodi";
            query += " and g.fileserver = m.paramcodi";
            if (idArea > 0)
            {
                query += " and d.areacode = " + idArea.ToString();
            }
            else
            {
                query += " and d.fljdetnivel = 0";
            }

            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','X') ";
            query += " and d.fljestado not in ( 'N','B','X','A') ";
            query += " and d.fljfechamax is not null ";
            query += " and g.FILEATRIB(+)='F'";
            query += " and d.percodi = " + idPersona.ToString();
            query += where1;
            query += where2;
            query += " order by fmax desc, 2 desc ";

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();
            dbProvider.AddInParameter(command, "fljfechamax", DbType.Date, fechaInicial);
            dbProvider.AddInParameter(command, "diasplazo", DbType.Int32, diasPlazo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BandejaDTO entity = bandejaHelper.Create(dr, origen);
                    entity.IndOrigenRecordatorio = indicador;
                    entity.IndNoAtendidos = "N";
                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public List<BandejaDTO> ListarDocumentosRecibidosRecordatorio(int origen, int idArea, int idPersona, int idRol, string codAtencion, bool indImportante, string indicador)
        {
            string query = "";
            string where1 = "";
            string where2 = "";
            where1 += " and  (  (   (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC('11111100'))) > 0 ) and ((to_date(f.fljfecharecep,'dd/mm/yyyy') + 15 ) < to_date(sysdate,'dd/mm/yyyy') ) and ( to_date(f.fljfecharecep,'dd/mm/yyyy')+365 >  to_date(sysdate,'dd/mm/yyyy'))  and d.fljestado in ('P', 'D')  and d.fljfechamax is null )";

            if (idPersona > 0)
            {
                where1 += " and d.percodi = " + idPersona.ToString();
            }
            if (indImportante) { where1 += " and f.fljprioridad = 'C'"; }
            if (codAtencion != "00000000")
            {
                where2 += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + codAtencion + ") )) > 0";
            }

            query = " select f.fljcodi ,f.corrnumproc,'' as NombAreaRem,-1 as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad ,f.fljfileatrib,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente <> 1 and f.fljdestext = 'N'";
            query += " and f.fljcodi = g.fljcodi";
            query += " and g.fileserver = m.paramcodi";
            if (idArea > 0)
            {
                query += " and d.areacode = " + idArea.ToString();
            }
            else
            {
                query += " and d.fljdetnivel = 0 ";
            }

            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','A','X') ";
            query += " and d.fljestado not in ( 'N','B','A','X') ";
            query += " and g.FILEATRIB='F'";
            query += where1;
            query += "  union ";
            query += " select f.fljcodi ,f.corrnumproc,b.areaabrev as NombAreaRem,b.areacode as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib ,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g ,doc_parameter m";
            query += " where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente = 1 and f.fljdestext = 'N'";
            query += " and f.fljcodi = g.fljcodi";
            query += " and g.fileserver = m.paramcodi";
            if (idArea > 0)
            {
                query += " and d.areacode = " + idArea.ToString();
            }
            else
            {
                query += " and d.fljdetnivel = 0";
            }

            query += " and b.areacode = d.areapadre ";
            query += "  and d.areapadre > 0";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','X') ";
            query += " and d.fljestado not in ( 'N','B','A','X') ";
            query += " and g.FILEATRIB='F'";
            query += where1;
            query += "  union ";
            query += " select f.fljcodi,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext,f.fljnombre as asunto,d.fljfechainicio as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,'', d.fljestado as fljestado ,d.fljdetcodi,b.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem ,f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib,f.fljprioridad as imp, '' as rpta, '' as Nmsg, '' as Leido, '' as Estadohijo, d.fljfechaatencion as fatencion";
            query += " from doc_flujo f,doc_flujo_det d,fw_area a,fw_area b,doc_flujo_files g,si_empresa s,doc_parameter m "; //",doc_archivo h,doc_archivo_sub k ";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel >= 1 ";
            query += " and d.areapadre = a.areacode ";
            query += " and d.areacode = b.areacode  and f.fljremitente = s.emprcodi";
            query += " and f.fljcodi = g.fljcodi";
            query += " and g.fileserver = m.paramcodi";
            if (idArea > 0)
            {
                query += " and d.areacode = " + idArea.ToString();
            }
            else
            {
                query += " and d.fljdetnivel = 0";
            }

            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','X') ";
            query += " and d.fljestado not in ( 'N','B','X','A') ";
            query += " and g.FILEATRIB='F'";
            query += " and d.percodi = " + idPersona.ToString();
            query += where1;
            query += where2;
            query += " order by fmax desc,frecepcion desc, 2 desc ";


            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<BandejaDTO> entitys = new List<BandejaDTO>();
            BandejaHelper bandejaHelper = new BandejaHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    BandejaDTO entity = bandejaHelper.Create(dr, origen);
                    entity.IndOrigenRecordatorio = indicador;
                    entity.IndNoAtendidos = "S";
                    entitys.Add(entity);
                }
            }

            return entitys;


        }


        //Hasta aqui

        public int VerificarUserRol(int userCode, int rolCode)
        {
            BandejaHelper bandejaHelper = new BandejaHelper();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerificarUserRol);
            dbProvider.AddInParameter(command, bandejaHelper.Usercode, DbType.Int32, userCode);
            dbProvider.AddInParameter(command, bandejaHelper.Docrolcodi, DbType.Int32, rolCode);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<DiaEspecialDTO> ObtenerDiasFeriados()
        {
            DiaEspecialHelper diaHelper = new DiaEspecialHelper();
            List<DiaEspecialDTO> entitys = new List<DiaEspecialDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerDiasFeriados);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DiaEspecialDTO entity = new DiaEspecialDTO();

                    int iDiaCodi = dr.GetOrdinal(diaHelper.DiaCodi);
                    if (!dr.IsDBNull(iDiaCodi)) entity.DiaCodi = Convert.ToInt32(dr.GetValue(iDiaCodi));

                    int iDiaFecha = dr.GetOrdinal(diaHelper.DiaFecha);
                    if (!dr.IsDBNull(iDiaFecha)) entity.DiaFecha = dr.GetDateTime(iDiaFecha);

                    int iDiaTipo = dr.GetOrdinal(diaHelper.DiaTipo);
                    if (!dr.IsDBNull(iDiaTipo)) entity.DiaTipo = Convert.ToInt16(iDiaTipo);

                    int iDiaFrec = dr.GetOrdinal(diaHelper.DiaFrec);
                    if (!dr.IsDBNull(iDiaFrec)) entity.DiaFrec = dr.GetString(iDiaFrec);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AtencionDTO> VerAtencion(int idDetalleFlujo)
        {
            AtencionHelper atencionHelper = new AtencionHelper();
            List<AtencionDTO> entitys = new List<AtencionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerAtencion);
            dbProvider.AddInParameter(command, atencionHelper.Fljdetcodi, DbType.Int32, idDetalleFlujo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AtencionDTO entity = new AtencionDTO();

                    int iFljdetcodi = dr.GetOrdinal(atencionHelper.Fljdetcodi);
                    if (!dr.IsDBNull(iFljdetcodi)) entity.Fljdetcodi = Convert.ToInt32(dr.GetValue(iFljdetcodi));

                    int iFechaMsg = dr.GetOrdinal(atencionHelper.FechaMsg);
                    if (!dr.IsDBNull(iFechaMsg)) entity.FechaMsg = dr.GetDateTime(iFechaMsg);

                    int iNombAreaOrig = dr.GetOrdinal(atencionHelper.NombAreaOrig);
                    if (!dr.IsDBNull(iNombAreaOrig)) entity.NombAreaOrig = dr.GetString(iNombAreaOrig);

                    int iNombAreaDest = dr.GetOrdinal(atencionHelper.NombAreaDest);
                    if (!dr.IsDBNull(iNombAreaDest)) entity.NombAreaDest = dr.GetString(iNombAreaDest);

                    int iMsg = dr.GetOrdinal(atencionHelper.Msg);
                    if (!dr.IsDBNull(iMsg)) entity.Msg = dr.GetString(iMsg);

                    int iEstado = dr.GetOrdinal(atencionHelper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

                    string ruta = string.Empty;
                    int iFljfileruta = dr.GetOrdinal(atencionHelper.Fljfileruta);
                    if (!dr.IsDBNull(iFljfileruta)) ruta = dr.GetString(iFljfileruta);

                    entity.IndArchivo = false;
                    if (!string.IsNullOrEmpty(ruta))
                        entity.IndArchivo = true;

                    int iFileruta = dr.GetOrdinal(atencionHelper.Fileruta);
                    if (!dr.IsDBNull(iFileruta)) entity.Fileruta = dr.GetString(iFileruta);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SeguimientoAreaDTO> VerSeguimiento(int idFlujo, int idArea)
        {
            SeguimientoHelper seguimientoHelper = new SeguimientoHelper();
            List<SeguimientoAreaDTO> entitys = new List<SeguimientoAreaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSeguimientoArea);
            dbProvider.AddInParameter(command, seguimientoHelper.FljCodigo, DbType.Int32, idFlujo);
            //dbProvider.AddInParameter(command, seguimientoHelper.CodigoAreaPadre, DbType.Int32, idArea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SeguimientoAreaDTO entity = new SeguimientoAreaDTO();

                    int iNumMsg = dr.GetOrdinal(seguimientoHelper.NumMsg);
                    if (!dr.IsDBNull(iNumMsg)) entity.NumMsg = Convert.ToInt32(dr.GetValue(iNumMsg));

                    int iFechaMax = dr.GetOrdinal(seguimientoHelper.FechaMax);
                    if (!dr.IsDBNull(iFechaMax)) entity.FechaMax = dr.GetDateTime(iFechaMax);

                    string atencion = string.Empty;

                    int iDescripAten = dr.GetOrdinal(seguimientoHelper.DescripAten);
                    if (!dr.IsDBNull(iDescripAten)) atencion = dr.GetString(iDescripAten);

                    entity.DescripAten = helper.ObtenerAtencion(atencion);

                    int iFljCodigo = dr.GetOrdinal(seguimientoHelper.FljCodigo);
                    if (!dr.IsDBNull(iFljCodigo)) entity.FljCodigo = Convert.ToInt32(dr.GetValue(iFljCodigo));

                    int iNombreAreaPadre = dr.GetOrdinal(seguimientoHelper.NombreAreaPadre);
                    if (!dr.IsDBNull(iNombreAreaPadre)) entity.NombreAreaPadre = dr.GetString(iNombreAreaPadre);

                    int iFljDetCodigo = dr.GetOrdinal(seguimientoHelper.FljDetCodigo);
                    if (!dr.IsDBNull(iFljDetCodigo)) entity.FljDetCodigo = Convert.ToInt32(dr.GetValue(iFljDetCodigo));

                    int iNombreArea = dr.GetOrdinal(seguimientoHelper.NombreArea);
                    if (!dr.IsDBNull(iNombreArea)) entity.NombreArea = dr.GetString(iNombreArea);

                    int iFechaAsignacion = dr.GetOrdinal(seguimientoHelper.FechaAsignacion);
                    if (!dr.IsDBNull(iFechaAsignacion)) entity.FechaAsignacion = dr.GetDateTime(iFechaAsignacion);

                    string estado = string.Empty;

                    int iEstado = dr.GetOrdinal(seguimientoHelper.Estado);
                    if (!dr.IsDBNull(iEstado)) estado = dr.GetString(iEstado);

                    entity.Estado = helper.ObtenerEstado(estado);

                    int iComentarioPadre = dr.GetOrdinal(seguimientoHelper.ComentarioPadre);
                    if (!dr.IsDBNull(iComentarioPadre)) entity.ComentarioPadre = dr.GetString(iComentarioPadre);

                    int iFechaAtencion = dr.GetOrdinal(seguimientoHelper.FechaAtencion);
                    if (!dr.IsDBNull(iFechaAtencion)) entity.FechaAtencion = dr.GetDateTime(iFechaAtencion);

                    int iCodigoArea = dr.GetOrdinal(seguimientoHelper.CodigoArea);
                    if (!dr.IsDBNull(iCodigoArea)) entity.CodigoArea = Convert.ToInt32(dr.GetValue(iCodigoArea));

                    int iCodigoAreaPadre = dr.GetOrdinal(seguimientoHelper.CodigoAreaPadre);
                    if (!dr.IsDBNull(iCodigoAreaPadre)) entity.CodigoAreaPadre = Convert.ToInt32(dr.GetValue(iCodigoAreaPadre));

                    int iFljNivel = dr.GetOrdinal(seguimientoHelper.FljNivel);
                    if (!dr.IsDBNull(iFljNivel)) entity.FljNivel = Convert.ToInt32(dr.GetValue(iFljNivel));

                    int iFljDetOrigen = dr.GetOrdinal(seguimientoHelper.FljDetOrigen);
                    if (!dr.IsDBNull(iFljDetOrigen)) entity.FljDetOrigen = Convert.ToInt32(dr.GetValue(iFljDetOrigen));

                    int iFljDetDestino = dr.GetOrdinal(seguimientoHelper.FljDetDestino);
                    if (!dr.IsDBNull(iFljDetDestino)) entity.FljDetDestino = Convert.ToInt32(dr.GetValue(iFljDetDestino));

                    int iAreaCode = dr.GetOrdinal(seguimientoHelper.Areacode);
                    if (!dr.IsDBNull(iAreaCode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreaCode));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SeguimientoEspecialistaDTO> VerSeguimientoEspecialista(int idFlujo, int idDetalleFlujo, int idArea)
        {
            SeguimientoHelper seguimientoHelper = new SeguimientoHelper();
            List<SeguimientoEspecialistaDTO> entitys = new List<SeguimientoEspecialistaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSeguimientoEspecialista);
            dbProvider.AddInParameter(command, seguimientoHelper.FljCodigo, DbType.Int32, idFlujo);
            dbProvider.AddInParameter(command, seguimientoHelper.FljDetCodigo, DbType.Int32, idDetalleFlujo);
            dbProvider.AddInParameter(command, seguimientoHelper.CodigoAreaPadre, DbType.Int32, idArea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SeguimientoEspecialistaDTO entity = new SeguimientoEspecialistaDTO();

                    int iNumMsg = dr.GetOrdinal(seguimientoHelper.NumMsg);
                    if (!dr.IsDBNull(iNumMsg)) entity.NumMsg = Convert.ToInt32(dr.GetValue(iNumMsg));

                    int iPrioridad = dr.GetOrdinal(seguimientoHelper.Prioridad);
                    if (!dr.IsDBNull(iPrioridad)) entity.Prioridad = Convert.ToInt32(dr.GetValue(iPrioridad));

                    int iFechaMax = dr.GetOrdinal(seguimientoHelper.FechaMax);
                    if (!dr.IsDBNull(iFechaMax)) entity.FechaMax = dr.GetDateTime(iFechaMax);

                    string atencion = string.Empty;

                    int iDescripAten = dr.GetOrdinal(seguimientoHelper.DescripAten);
                    if (!dr.IsDBNull(iDescripAten)) atencion = dr.GetString(iDescripAten);

                    entity.DescripAten = helper.ObtenerAtencion(atencion);

                    int iFljCodigo = dr.GetOrdinal(seguimientoHelper.FljCodigo);
                    if (!dr.IsDBNull(iFljCodigo)) entity.FljCodigo = Convert.ToInt32(dr.GetValue(iFljCodigo));

                    int iNombreAreaPadre = dr.GetOrdinal(seguimientoHelper.NombreAreaPadre);
                    if (!dr.IsDBNull(iNombreAreaPadre)) entity.NombreAreaPadre = dr.GetString(iNombreAreaPadre);

                    int iFljDetCodigo = dr.GetOrdinal(seguimientoHelper.FljDetCodigo);
                    if (!dr.IsDBNull(iFljDetCodigo)) entity.FljDetCodigo = Convert.ToInt32(iFljDetCodigo);

                    int iNombreArea = dr.GetOrdinal(seguimientoHelper.NombreArea);
                    if (!dr.IsDBNull(iNombreArea)) entity.NombreArea = dr.GetString(iNombreArea);

                    int iFechaAsignacion = dr.GetOrdinal(seguimientoHelper.FechaAsignacion);
                    if (!dr.IsDBNull(iFechaAsignacion)) entity.FechaAsignacion = dr.GetDateTime(iFechaAsignacion);

                    string estado = string.Empty;

                    int iEstado = dr.GetOrdinal(seguimientoHelper.Estado);
                    if (!dr.IsDBNull(iEstado)) estado = dr.GetString(iEstado);

                    entity.Estado = helper.ObtenerEstado(estado);

                    int iComentarioPadre = dr.GetOrdinal(seguimientoHelper.ComentarioPadre);
                    if (!dr.IsDBNull(iComentarioPadre)) entity.ComentarioPadre = dr.GetString(iComentarioPadre);

                    int iFechaAtencion = dr.GetOrdinal(seguimientoHelper.FechaAtencion);
                    if (!dr.IsDBNull(iFechaAtencion)) entity.FechaAtencion = dr.GetDateTime(iFechaAtencion);

                    int iCodigoArea = dr.GetOrdinal(seguimientoHelper.CodigoArea);
                    if (!dr.IsDBNull(iCodigoArea)) entity.CodigoArea = Convert.ToInt32(dr.GetValue(iCodigoArea));

                    int iCodigoAreaPadre = dr.GetOrdinal(seguimientoHelper.CodigoAreaPadre);
                    if (!dr.IsDBNull(iCodigoAreaPadre)) entity.CodigoAreaPadre = Convert.ToInt32(dr.GetValue(iCodigoAreaPadre));

                    int iFljNivel = dr.GetOrdinal(seguimientoHelper.FljNivel);
                    if (!dr.IsDBNull(iFljNivel)) entity.FljNivel = Convert.ToInt32(dr.GetValue(iFljNivel));

                    int iFljDetOrigen = dr.GetOrdinal(seguimientoHelper.FljDetOrigen);
                    if (!dr.IsDBNull(iFljDetOrigen)) entity.FljDetOrigen = Convert.ToInt32(dr.GetValue(iFljDetOrigen));

                    int iFljDetDestino = dr.GetOrdinal(seguimientoHelper.FljDetDestino);
                    if (!dr.IsDBNull(iFljDetDestino)) entity.FljDetDestino = Convert.ToInt32(dr.GetValue(iFljDetDestino));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SelloDTO> VerSello(int idFlujo)
        {
            SelloHelper selloHelper = new SelloHelper();
            List<SelloDTO> entitys = new List<SelloDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerSello);
            dbProvider.AddInParameter(command, selloHelper.Fljcodi, DbType.Int32, idFlujo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SelloDTO entity = new SelloDTO();

                    int iFljCadAtencion = dr.GetOrdinal(selloHelper.FljCadAtencion);
                    if (!dr.IsDBNull(iFljCadAtencion)) entity.FljCadAtencion = dr.GetString(iFljCadAtencion);

                    int iAreaPadre = dr.GetOrdinal(selloHelper.AreaPadre);
                    if (!dr.IsDBNull(iAreaPadre)) entity.AreaPadre = Convert.ToInt32(dr.GetValue(iAreaPadre));

                    int iAreaCode = dr.GetOrdinal(selloHelper.AreaCode);
                    if (!dr.IsDBNull(iAreaCode)) entity.AreaCode = Convert.ToInt32(dr.GetValue(iAreaCode));

                    int iDescripDeleg = dr.GetOrdinal(selloHelper.DescripDeleg);
                    if (!dr.IsDBNull(iDescripDeleg)) entity.DescripDeleg = dr.GetString(iDescripDeleg);

                    int iFljFechaMax = dr.GetOrdinal(selloHelper.FljFechaMax);
                    if (!dr.IsDBNull(iFljFechaMax)) entity.FljFechaMax = dr.GetDateTime(iFljFechaMax);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReferenciaDTO> VerReferenciasA(int idFlujo)
        {
            ReferenciaHelper referenciaHelper = new ReferenciaHelper();
            List<ReferenciaDTO> entitys = new List<ReferenciaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerReferenciaA);
            dbProvider.AddInParameter(command, referenciaHelper.Fljcodi, DbType.Int32, idFlujo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReferenciaDTO entity = new ReferenciaDTO();

                    int iCorrelativo = dr.GetOrdinal(referenciaHelper.Correlativo);
                    if (!dr.IsDBNull(iCorrelativo)) entity.Correlativo = Convert.ToInt32(dr.GetValue(iCorrelativo));

                    int iFechaDoc = dr.GetOrdinal(referenciaHelper.FechaDoc);
                    if (!dr.IsDBNull(iFechaDoc)) entity.FechaDoc = dr.GetDateTime(iFechaDoc);

                    int iNDoc = dr.GetOrdinal(referenciaHelper.NDoc);
                    if (!dr.IsDBNull(iNDoc)) entity.NDoc = dr.GetString(iNDoc);

                    int iAsunto = dr.GetOrdinal(referenciaHelper.Asunto);
                    if (!dr.IsDBNull(iAsunto)) entity.Asunto = dr.GetString(iAsunto);

                    int iRutaArchivo = dr.GetOrdinal(referenciaHelper.RutaArchivo);
                    if (!dr.IsDBNull(iRutaArchivo)) entity.RutaArchivo = dr.GetString(iRutaArchivo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReferenciaDTO> VerReferenciasDe(int idFlujo)
        {
            ReferenciaHelper referenciaHelper = new ReferenciaHelper();
            List<ReferenciaDTO> entitys = new List<ReferenciaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerReferenciaDe);
            dbProvider.AddInParameter(command, referenciaHelper.Fljcodi, DbType.Int32, idFlujo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReferenciaDTO entity = new ReferenciaDTO();

                    int iCorrelativo = dr.GetOrdinal(referenciaHelper.Correlativo);
                    if (!dr.IsDBNull(iCorrelativo)) entity.Correlativo = Convert.ToInt32(dr.GetValue(iCorrelativo));

                    int iFechaDoc = dr.GetOrdinal(referenciaHelper.FechaDoc);
                    if (!dr.IsDBNull(iFechaDoc)) entity.FechaDoc = dr.GetDateTime(iFechaDoc);

                    int iNDoc = dr.GetOrdinal(referenciaHelper.NDoc);
                    if (!dr.IsDBNull(iNDoc)) entity.NDoc = dr.GetString(iNDoc);

                    int iAsunto = dr.GetOrdinal(referenciaHelper.Asunto);
                    if (!dr.IsDBNull(iAsunto)) entity.Asunto = dr.GetString(iAsunto);

                    int iRutaArchivo = dr.GetOrdinal(referenciaHelper.RutaArchivo);
                    if (!dr.IsDBNull(iRutaArchivo)) entity.RutaArchivo = dr.GetString(iRutaArchivo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MensajeDTO> VerMensajes(int idDetalleFlujo)
        {
            MensajeHelper mensajeHelper = new MensajeHelper();
            List<MensajeDTO> entitys = new List<MensajeDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerMensaje);
            dbProvider.AddInParameter(command, mensajeHelper.Fljdetcodi, DbType.Int32, idDetalleFlujo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MensajeDTO entity = new MensajeDTO();

                    int iFechaMsg = dr.GetOrdinal(mensajeHelper.FechaMsg);
                    if (!dr.IsDBNull(iFechaMsg)) entity.FechaMsg = dr.GetDateTime(iFechaMsg);

                    int iNombAreaOrig = dr.GetOrdinal(mensajeHelper.NombAreaOrig);
                    if (!dr.IsDBNull(iNombAreaOrig)) entity.NombAreaOrig = dr.GetString(iNombAreaOrig);

                    int iNombAreaDest = dr.GetOrdinal(mensajeHelper.NombAreaDest);
                    if (!dr.IsDBNull(iNombAreaDest)) entity.NombAreaDest = dr.GetString(iNombAreaDest);

                    int iMsg = dr.GetOrdinal(mensajeHelper.Msg);
                    if (!dr.IsDBNull(iMsg)) entity.Msg = dr.GetString(iMsg);

                    int iEstado = dr.GetOrdinal(mensajeHelper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.Estado = dr.GetString(iEstado);

                    int iFileruta = dr.GetOrdinal(mensajeHelper.Fileruta);
                    if (!dr.IsDBNull(iFileruta)) entity.Fileruta = dr.GetString(iFileruta);

                    string ruta = string.Empty;
                    entity.IndArchivo = false;
                    int iFljfileruta = dr.GetOrdinal(mensajeHelper.Fljfileruta);
                    if (!dr.IsDBNull(iFljfileruta)) ruta = dr.GetString(iFljfileruta);

                    if (!string.IsNullOrEmpty(ruta)) entity.IndArchivo = true;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DocumentoDTO> VerDocumentos(int idFlujo, int idDetalleFlujo)
        {
            DocumentoHelper documentoHelper = new DocumentoHelper();
            List<DocumentoDTO> entitys = new List<DocumentoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerDocumentos);
            dbProvider.AddInParameter(command, documentoHelper.Fljcodi, DbType.Int32, idFlujo);
            dbProvider.AddInParameter(command, documentoHelper.Fljdetcodi, DbType.Int32, idDetalleFlujo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DocumentoDTO entity = new DocumentoDTO();

                    int iFilecodi = dr.GetOrdinal(documentoHelper.Filecodi);
                    if (!dr.IsDBNull(iFilecodi)) entity.Filecodi = Convert.ToInt32(dr.GetValue(iFilecodi));

                    int iFileruta = dr.GetOrdinal(documentoHelper.Fileruta);
                    if (!dr.IsDBNull(iFileruta)) entity.Fileruta = dr.GetString(iFileruta);

                    int iLastdate = dr.GetOrdinal(documentoHelper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iLastuser = dr.GetOrdinal(documentoHelper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iFilecomentario = dr.GetOrdinal(documentoHelper.Filecomentario);
                    if (!dr.IsDBNull(iFilecomentario)) entity.Filecomentario = dr.GetString(iFilecomentario);

                    int iFileanio = dr.GetOrdinal(documentoHelper.Fileanio);
                    if (!dr.IsDBNull(iFileanio)) entity.Fileanio = Convert.ToInt32(dr.GetValue(iFileanio));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DocumentoDTO> VerDocumentosV(int idFlujo)
        {
            DocumentoHelper documentoHelper = new DocumentoHelper();
            List<DocumentoDTO> entitys = new List<DocumentoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlVerDocumentosV);
            dbProvider.AddInParameter(command, documentoHelper.Fljcodi, DbType.Int32, idFlujo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DocumentoDTO entity = new DocumentoDTO();

                    int iFilecodi = dr.GetOrdinal(documentoHelper.Filecodi);
                    if (!dr.IsDBNull(iFilecodi)) entity.Filecodi = Convert.ToInt32(dr.GetValue(iFilecodi));

                    int iFileruta = dr.GetOrdinal(documentoHelper.Fileruta);
                    if (!dr.IsDBNull(iFileruta)) entity.Fileruta = dr.GetString(iFileruta);

                    int iFilecomentario = dr.GetOrdinal(documentoHelper.Filecomentario);
                    if (!dr.IsDBNull(iFilecomentario)) entity.Filecomentario = dr.GetString(iFilecomentario);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AreaDTO> VerAreas(int idArea)
        {
            AreaHelper areaHelper = new AreaHelper();
            List<AreaDTO> entitys = new List<AreaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerArea);
            dbProvider.AddInParameter(command, areaHelper.AreaCodi, DbType.Int32, idArea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AreaDTO entity = new AreaDTO();

                    int iAreaCodi = dr.GetOrdinal(areaHelper.AreaCodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    int iAreaName = dr.GetOrdinal(areaHelper.AreaName);
                    if (!dr.IsDBNull(iAreaName)) entity.AreaName = dr.GetString(iAreaName);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AreaDTO> ListarAreas()
        {
            AreaHelper areaHelper = new AreaHelper();
            List<AreaDTO> entitys = new List<AreaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarAreas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AreaDTO entity = new AreaDTO();

                    int iAreaCodi = dr.GetOrdinal(areaHelper.AreaCodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    int iAreaName = dr.GetOrdinal(areaHelper.AreaName);
                    if (!dr.IsDBNull(iAreaName)) entity.AreaName = dr.GetString(iAreaName);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RolUsuarioDTO> LeerRolesxUsuario(int idUsuario)
        {
            RolUsuarioHelper usuarioHelper = new RolUsuarioHelper();
            List<RolUsuarioDTO> entitys = new List<RolUsuarioDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlRolesxUsuario);
            dbProvider.AddInParameter(command, usuarioHelper.UserCode, DbType.Int32, idUsuario);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RolUsuarioDTO entity = new RolUsuarioDTO();

                    int iRolCodi = dr.GetOrdinal(usuarioHelper.RolCodi);
                    if (!dr.IsDBNull(iRolCodi)) entity.RolCodi = Convert.ToInt32(dr.GetValue(iRolCodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EtiquetaDTO> LeerEtiquetas(int idArea, int bandejaM)
        {
            List<EtiquetaDTO> entitys = new List<EtiquetaDTO>();
            EtiquetaHelper etiquetaHelper = new EtiquetaHelper();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlLeerEtiquetas);
            dbProvider.AddInParameter(command, etiquetaHelper.AreaCode, DbType.Int32, idArea);
            dbProvider.AddInParameter(command, etiquetaHelper.BandejaLvl, DbType.Int32, bandejaM);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EtiquetaDTO entity = new EtiquetaDTO();

                    int iEtiqCode = dr.GetOrdinal(etiquetaHelper.EtiqCode);
                    if (!dr.IsDBNull(iEtiqCode)) entity.EtiqCode = Convert.ToInt32(dr.GetValue(iEtiqCode));

                    int iEtiqNomb = dr.GetOrdinal(etiquetaHelper.EtiqNomb);
                    if (!dr.IsDBNull(iEtiqNomb)) entity.EtiqNomb = dr.GetString(iEtiqNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EtiquetaDTO> ListarEtiquetasPorArea(int idArea)
        {
            List<EtiquetaDTO> entitys = new List<EtiquetaDTO>();
            EtiquetaHelper etiquetaHelper = new EtiquetaHelper();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEtiquetasPorArea);
            dbProvider.AddInParameter(command, etiquetaHelper.AreaCode, DbType.Int32, idArea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EtiquetaDTO entity = new EtiquetaDTO();

                    int iEtiqCode = dr.GetOrdinal(etiquetaHelper.EtiqCode);
                    if (!dr.IsDBNull(iEtiqCode)) entity.EtiqCode = Convert.ToInt32(dr.GetValue(iEtiqCode));

                    int iEtiqNomb = dr.GetOrdinal(etiquetaHelper.EtiqNomb);
                    if (!dr.IsDBNull(iEtiqNomb)) entity.EtiqNomb = dr.GetString(iEtiqNomb);

                    int iBandejalvl = dr.GetOrdinal(etiquetaHelper.BandejaLvl);
                    if (!dr.IsDBNull(iBandejalvl)) entity.BandejaCodi = Convert.ToInt32(dr.GetValue(iBandejalvl));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReporteDTO> VerReportes(int idAreaPadre, int idAreaDestino, bool conPlazo, DateTime fechaInicial, DateTime fechaFin,
            string atencion)
        {
            #region Query

            string query = string.Empty;

            query = " select f.fljcodi,d.fljdetcodi,f.corrnumproc,b.areaabrev as Remitente,a.areaabrev as Destino,b.areacode as areaorig, d.areacode as areadestino";
            query += " ,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion  ,h.paramvalue || g.fileruta as xfileruta,d.fljestado";
            query += " ,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.FLJFECHAMAX as fmax,d.fljfechainicio as fasignacion,f.FLJDIASMAXATEN ,f.FLJFECHAMAX as fmaxgen";
            query += " from doc_flujo f,doc_flujo_det d, fw_area a,fw_area b,doc_flujo_files g,doc_parameter h ";
            query += " where f.fljcodi = d.fljcodi and d.fljdetnivel >= 0 and d.areacode = a.areacode ";
            query += " and d.fljcodi = g.fljcodi and d.areacode = " + idAreaDestino;
            query += " and b.areacode = d.areapadre ";
            query += " and d.areapadre > 0";
            query += " and d.fljfechainicio between {0} and {1}";
            query += " and g.fileatrib='F'";
            query += " and d.percodi = -1";
            query += " and g.fileserver = h.paramcodi";

            //if (idAreaPadre > 0)
            //{
            //    query += " and d.areapadre=" + idAreaPadre;
            //}

            query += "  and d.fljdetnivel < 98 ";
            query += "  and f.FLJESTADO not in ( 'B','X','N') ";
            query += "  and d.fljestado not in ( 'N','B','A','X') ";

            if (conPlazo)
            {
                query += " and d.FLJFECHAMAX is not null ";
            }
            if (atencion != "00000000" && atencion != "")
            {
                query += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + atencion + "))) > 0";
            }
            query += " order by f.fljanio desc, f.corrnumproc desc";

            #endregion

            //query a modificar

            //            query = @" select f.fljcodi,d.fljdetcodi,f.corrnumproc,b.areaabrev as Remitente,a.areaabrev as Destino,b.areacode as areaorig, d.areacode as areadestino ,f.fljnumext, trim(f.fljnombre) as ASUNTO,
            //                     f.fljfecharecep as frecepcion  ,h.paramvalue || g.fileruta as xfileruta,d.fljestado ,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.FLJFECHAMAX as fmax,d.fljfechainicio as fasignacion,
            //                     f.FLJDIASMAXATEN ,f.FLJFECHAMAX as fmaxgen from doc_flujo f,doc_flujo_det d, fw_area a,fw_area b,doc_flujo_files g,doc_parameter h  where f.fljcodi = d.fljcodi and d.fljdetnivel >= 0 
            //                     and d.areacode = a.areacode  and d.fljcodi = g.fljcodi and d.areacode = -1 and b.areacode = d.areapadre  and d.areapadre > 0 
            //                     order by f.fljanio desc, f.corrnumproc desc";


            string sql = string.Format(query,
               "TO_DATE('" + fechaInicial.ToString(ConstantesBase.FormatoFecha) + " 00:00:00','YYYY-MM-DD HH24:MI:SS')",
               "TO_DATE('" + fechaFin.ToString(ConstantesBase.FormatoFecha) + " 23:59:59','YYYY-MM-DD HH24:MI:SS')");

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            //dbProvider.AddInParameter(command, "fechaini", DbType.Date, fechaInicial);
            //dbProvider.AddInParameter(command, "fechafin", DbType.Date, fechaFin);


            List<ReporteDTO> entitys = new List<ReporteDTO>();
            ReporteHelper ReporteHelper = new ReporteHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(ReporteHelper.Create(dr));
                }
            }

            return entitys;
        }

        public List<AlertaTramiteDTO> LeerAlertasTramites(int codigoArea, int codigoPersona, int codigoRol, DateTime fechaInicial,
            int diasAnticip, string atencion)
        {
            #region Query

            string query = string.Empty;
            string where1 = "";
            string where2 = "";

            where1 += " and d.fljfechamax is not null";
            where1 += " and ( ((d.fljfechamax - :fecha1) between  0 and :nrodias ) or ( (d.fljfechamax < :fecha2 ) and d.fljestado in ('P','D')  ) )";

            if (codigoPersona > 0)
            {
                where1 += " and d.percodi = " + codigoPersona.ToString();
            }
            if (atencion != "00000000" && atencion != "")
            {
                where2 += " and (bitand (FN_BIN2DEC(d.FLJCADATENCION),FN_BIN2DEC(" + atencion + "))) > 0";
            }

            /*Empresa envia al COES*/

            query = " select f.fljcodi ,f.corrnumproc,'' as NombAreaRem,-1 as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad ,f.fljfileatrib";
            query += "  from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m ";// ",doc_archivo h,doc_archivo_sub k ";
            query += "  where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente <> 1 and f.fljdestext = 'N'";
            query += "  and d.fljdetcodi = g.fljdetcodi ";
            query += " and g.fileserver = m.paramcodi";
            if (codigoArea > 0)
            {
                query += " and d.areacode = " + codigoArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D            
            }
            else
            {
                query += " and d.fljdetnivel = 0 ";
            }

            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','A','X') ";
            query += " and d.fljestado not in ( 'N','B','A','X') ";
            query += " and g.FILEATRIB(+)='F'";
            query += where1;

            query += "  union ";

            /*COES envia al COES nuevo*/

            query += " select f.fljcodi ,f.corrnumproc,b.areaabrev as NombAreaRem,b.areacode as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,d.fljdetprio as prioridad,f.fljfileatrib ";
            query += " from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,fw_area b,doc_flujo_files g ,doc_parameter m";// ",doc_archivo h,doc_archivo_sub k ";
            query += " where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente = 1 and f.fljdestext = 'N'";
            query += " and d.fljdetcodi = g.fljdetcodi ";
            query += " and g.fileserver = m.paramcodi";
            if (codigoArea > 0)
            {
                query += " and d.areacode = " + codigoArea.ToString(); //Con esto son las enviadas entre areas del COES con destino diferente de D
            }
            else
            {
                query += " and d.fljdetnivel = 0";
            }

            query += " and b.areacode = d.areapadre ";
            query += "  and d.areapadre > 0";
            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','A','X') ";
            query += " and d.fljestado not in ( 'N','B','A','X') ";
            query += " and d.fljfechamax is not null ";
            query += " and g.FILEATRIB(+)='F'";
            query += where1;
            query += "  union ";


            /*COES recive del COES por Derivacon NUEVO*/
            query += " select f.fljcodi,f.corrnumproc,a.areaabrev as NombAreaRem,a.areacode as areaorig,f.fljnumext,f.fljnombre as asunto,d.fljfechainicio as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,'', d.fljestado as fljestado ,d.fljdetcodi,b.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,f.FLJFECHAPROCE,d.areacode as areadestino,d.FLJTIEMPOREQ as Conplazo,d.FLJFECHAMAX as fmax";
            query += " ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem ,f.fljconf,f.fljanio,d.fljdetprio as prioridad ,f.fljfileatrib";
            query += " from doc_flujo f,doc_flujo_det d,fw_area a,fw_area b,doc_flujo_files g,si_empresa s,doc_parameter m "; //",doc_archivo h,doc_archivo_sub k ";
            query += " where f.fljcodi = d.fljcodi";
            query += " and d.fljdetnivel >= 1 ";
            query += " and d.areapadre = a.areacode ";
            query += " and d.areacode = b.areacode  and f.fljremitente = s.emprcodi";
            query += " and f.fljcodi = g.fljcodi"; //Documento Original
            query += " and g.fileserver = m.paramcodi";
            if (codigoArea > 0)
            {
                query += " and d.areacode = " + codigoArea.ToString();
            }
            else
            {
                query += " and d.fljdetnivel = 0";
            }

            query += " and d.fljdetnivel < 98 ";
            query += " and f.FLJESTADO not in ( 'N','B','A','X') ";
            query += " and d.fljestado not in ( 'N','B','X','A') ";
            query += " and d.fljfechamax is not null ";
            query += " and g.FILEATRIB(+)='F'";

            query += " and d.percodi = " + codigoPersona.ToString();

            query += where1;
            query += where2;
            query += " order by fmax desc, 2 desc ";

            #endregion


            //query a modificar

            //            query = @"select f.fljcodi ,f.corrnumproc,'' as NombAreaRem,-1 as areaorig,f.fljnumext, trim(f.fljnombre) as ASUNTO,f.fljfecharecep as frecepcion,m.paramvalue||g.fileruta as xfileruta,g.filecodi,f.fljremdetalle, 
            //                    d.fljestado as fljestado ,d.fljdetcodi,a.areaabrev as NombDestino, d.fljdetdestino,d.fljdetorigen,d.fljdetnivel,d.fljcadatencion,d.fljpadrecomentario,
            //                    f.FLJFECHAPROCE,d.areacode as areadestino,f.FLJDIASMAXATEN as Conplazo,f.FLJFECHAMAX as fmax
            //                    ,NULL AS ARCHCODIFICACION,NULL AS ARCHSUBCODIF,s.emprnomb as NombEmpRem,f.fljconf,f.fljanio,-1 as prioridad ,f.fljfileatrib
            //                    from doc_flujo f,doc_flujo_det d,si_empresa s, fw_area a,doc_flujo_files g,doc_parameter m 
            //                    where f.fljcodi = d.fljcodi and d.fljdetnivel = 0 and d.areacode = a.areacode and f.fljremitente = s.emprcodi and f.fljremitente <> 1 and f.fljdestext = 'N'
            //                    and d.fljdetcodi = g.fljdetcodi 
            //                    and g.fileserver = m.paramcodi";


            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, "fecha1", DbType.Date, fechaInicial);
            dbProvider.AddInParameter(command, "nrodias", DbType.Int32, diasAnticip);
            dbProvider.AddInParameter(command, "fecha2", DbType.Date, fechaInicial);

            List<AlertaTramiteDTO> entitys = new List<AlertaTramiteDTO>();

            AlertaTramiteHelper alertaHelper = new AlertaTramiteHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(alertaHelper.Create(dr));
                }
            }

            return entitys;
        }

        public List<TipoAtencionDTO> LeerTipoAtencion()
        {
            List<TipoAtencionDTO> entitys = new List<TipoAtencionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlLeerTipoAtencion);

            TipoAtencionHelper tipoAtencionHelper = new TipoAtencionHelper();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TipoAtencionDTO entity = new TipoAtencionDTO();

                    int iTipoAtencionCodi = dr.GetOrdinal(tipoAtencionHelper.TipoAtencionCodi);
                    if (!dr.IsDBNull(iTipoAtencionCodi)) entity.TipoAtencionCodi = dr.GetString(iTipoAtencionCodi);

                    int iTipoAtencionNomb = dr.GetOrdinal(tipoAtencionHelper.TipoAtencionNomb);
                    if (!dr.IsDBNull(iTipoAtencionNomb)) entity.TipoAtencionNomb = dr.GetString(iTipoAtencionNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TipoEmpresaRemitenteDTO> LeerTipoEmpresaRemitente()
        {
            List<TipoEmpresaRemitenteDTO> entitys = new List<TipoEmpresaRemitenteDTO>();

            entitys.Add(new TipoEmpresaRemitenteDTO { TipoEmprCodi = 3, TipoEmprNomb = "Integrantes Generadores" });
            entitys.Add(new TipoEmpresaRemitenteDTO { TipoEmprCodi = 1, TipoEmprNomb = "Integrantes Transmisores" });
            entitys.Add(new TipoEmpresaRemitenteDTO { TipoEmprCodi = 2, TipoEmprNomb = "Integrantes Distribuidores" });
            entitys.Add(new TipoEmpresaRemitenteDTO { TipoEmprCodi = 4, TipoEmprNomb = "Integrantes Usuarios Libres" });
            entitys.Add(new TipoEmpresaRemitenteDTO { TipoEmprCodi = -1, TipoEmprNomb = "Otros" });

            return entitys;
        }

        public string LeerDirectorioRoot(int idFlujo)
        {
            ContenidoCdHelper cdHelper = new ContenidoCdHelper();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlLeerDirectorioRoot);
            dbProvider.AddInParameter(command, cdHelper.Fljcodi, DbType.Int32, idFlujo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iFileruta = dr.GetOrdinal(cdHelper.Fileruta);
                    if (!dr.IsDBNull(iFileruta)) return dr.GetString(iFileruta);
                }
            }

            return string.Empty;
        }

        public string LeerPathArchivo(int filecodi)
        {
            ContenidoCdHelper cdHelper = new ContenidoCdHelper();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlLeerPathArchivo);
            dbProvider.AddInParameter(command, cdHelper.Filecodi, DbType.Int32, filecodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iFileruta = dr.GetOrdinal(cdHelper.Fileruta);
                    if (!dr.IsDBNull(iFileruta)) return dr.GetString(iFileruta);
                }
            }

            return string.Empty;
        }

        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarMaestroEmpresas()
        {
            COES.Infraestructura.Datos.Helper.Sic.SiEmpresaHelper empresaHelper = new COES.Infraestructura.Datos.Helper.Sic.SiEmpresaHelper();
            List<COES.Dominio.DTO.Sic.SiEmpresaDTO> entitys = new List<COES.Dominio.DTO.Sic.SiEmpresaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarMaestroEmpresas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    COES.Dominio.DTO.Sic.SiEmpresaDTO entity = new COES.Dominio.DTO.Sic.SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(empresaHelper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(empresaHelper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(empresaHelper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasNoRI()
        {
            COES.Infraestructura.Datos.Helper.Sic.SiEmpresaHelper empresaHelper = new COES.Infraestructura.Datos.Helper.Sic.SiEmpresaHelper();
            List<COES.Dominio.DTO.Sic.SiEmpresaDTO> entitys = new List<COES.Dominio.DTO.Sic.SiEmpresaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresasNoRI);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    COES.Dominio.DTO.Sic.SiEmpresaDTO entity = new COES.Dominio.DTO.Sic.SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(empresaHelper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(empresaHelper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(empresaHelper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<COES.Dominio.DTO.Sic.SiEmpresaDTO> ListarEmpresasRIPorTipo(int tipoEmprCodi)
        {
            COES.Infraestructura.Datos.Helper.Sic.SiEmpresaHelper empresaHelper = new COES.Infraestructura.Datos.Helper.Sic.SiEmpresaHelper();
            List<COES.Dominio.DTO.Sic.SiEmpresaDTO> entitys = new List<COES.Dominio.DTO.Sic.SiEmpresaDTO>();

            string query = string.Format(helper.SqlListarEmpresasRIPorTipo, tipoEmprCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    COES.Dominio.DTO.Sic.SiEmpresaDTO entity = new COES.Dominio.DTO.Sic.SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(empresaHelper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(empresaHelper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(empresaHelper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<MigracionDTO> ObtenerDatosMigracion(int mes, int anio)
        {
            List<MigracionDTO> entitys = new List<MigracionDTO>();
            string query = string.Format(helper.SqlObtenerQueryMigracion, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MigracionDTO entity = new MigracionDTO();

                    int iAnio = dr.GetOrdinal("ANIO");
                    if (!dr.IsDBNull(iAnio)) entity.Anio = Convert.ToInt32(dr.GetValue(iAnio));

                    int iMes = dr.GetOrdinal("MES");
                    if (!dr.IsDBNull(iMes)) entity.Mes = Convert.ToInt32(dr.GetValue(iMes));

                    int iFlujodetcodi = dr.GetOrdinal("FLJDETCODI");
                    if (!dr.IsDBNull(iFlujodetcodi)) entity.Flujodetcodi = Convert.ToInt32(dr.GetValue(iFlujodetcodi));

                    int iFlujocodi = dr.GetOrdinal("FLJCODI");
                    if (!dr.IsDBNull(iFlujocodi)) entity.Flujocodi = Convert.ToInt32(dr.GetValue(iFlujocodi));

                    int iFileruta = dr.GetOrdinal("FILERUTA");
                    if (!dr.IsDBNull(iFileruta)) entity.Fileruta = dr.GetString(iFileruta);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public void ActualizarDatosMigracion(string pathPDF, string pathVOL, string pathCD, string tipoVOL, int flujocodi, int flujodetcodi)
        {
            string query = string.Format(helper.SqlActualizarDatosMigracion, pathPDF, pathCD, pathVOL, tipoVOL, flujocodi, flujodetcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
