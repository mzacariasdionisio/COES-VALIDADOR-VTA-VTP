using COES.Dominio.DTO.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda.BD;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class BDMappers
    {
        /// <summary>
        /// Permite mapear AzDataTableEntity a entidad AzDataTableDTO
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public AzDataTableDTO DataTableEntidad2AzDataTableDTO(AzDataTableEntity resultado)
        {
            if (resultado == null) { return null; }
            AzDataTableDTO registroDto = new AzDataTableDTO
            {
                PartitionKey = resultado.PartitionKey,
                RowKey = resultado.RowKey,
                Timestamp = resultado.Timestamp,
                metadata_storage_path = resultado.metadata_storage_path,
                TipoDocumento = resultado.TipoDocumento,
                RutaSharePointOnline = resultado.RutaSharepointOnline,
                modificado_por = resultado.modificado_por,
                UsuarioAccesoTotal = resultado.UsuarioAccesoTotal,
                PalabrasClave = resultado.PalabrasClave,
                NombreArchivoConExtension = resultado.NombreArchivoConExtension,
                Autor = resultado.Author,
                Keyconcepts = resultado.Keyconcepts
            };
            return registroDto;
        }

        /// <summary>
        /// Permite mapear AzDataTableDTO a entidad AzDataTableEntity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public AzDataTableEntity AzDataTableDTO2AzDataTableEntity(AzDataTableDTO resultado)
        {
            if (resultado == null) { return null; }
            AzDataTableEntity registroDto = new AzDataTableEntity
            {
                PartitionKey = resultado.PartitionKey,
                RowKey = resultado.RowKey,
                Timestamp = resultado.Timestamp,
                metadata_storage_path = resultado.metadata_storage_path,
                TipoDocumento = resultado.TipoDocumento,
                RutaSharepointOnline = resultado.RutaSharePointOnline,
                modificado_por = resultado.modificado_por,
                UsuarioAccesoTotal = resultado.UsuarioAccesoTotal,
                PalabrasClave = resultado.PalabrasClave,
                NombreArchivoConExtension = resultado.NombreArchivoConExtension,
                Author = resultado.Autor,
                Keyconcepts = resultado.Keyconcepts
            };
            return registroDto;
        }

        /// <summary>
        /// Permite mapear BCDCalificacionBusquedaDTO a entidad BCD_Calificacion_busqueda
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BCD_Calificacion_busqueda BCDCalificacionBusquedaDTO2BCD_Calificacion_busqueda(BCDCalificacionBusquedaDTO dto)
        {
            if (dto == null) { return null; }
            return new BCD_Calificacion_busqueda
            {
                Id_qualification = dto.Id_qualification,
                Id_search_q = dto.Id_search_q,
                Id_range_q = dto.Id_range_q,
            };
        }

        /// <summary>
        /// Permite mapear entidad BCD_Calificacion_busqueda a dto BCDCalificacionBusquedaDTO
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BCDCalificacionBusquedaDTO BCD_Calificacion_busqueda2BCDCalificacionBusquedaDTO(BCD_Calificacion_busqueda registro)
        {
            if (registro == null) { return null; }

            return new BCDCalificacionBusquedaDTO
            {
                Id_qualification = registro.Id_qualification,
                Id_search_q = registro.Id_search_q,
                Id_range_q = registro.Id_range_q,
            };
        }

        /// <summary>
        /// Permite mapear RangoDTO a entidad BCD_Rangos
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BCD_Rangos BCDRangosDTO2BCD_Rangos(BCDRangosDTO dto)
        {
            if (dto == null) { return null; }
            return new BCD_Rangos
            {
                Id_range = dto.Id_range,
                Range = dto.Range,
            };
        }

        /// <summary>
        /// Permite mapear BCD_Rangos a entidad RangoDTO
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BCDRangosDTO BCD_Rangos2RangoDTO(BCD_Rangos registro)
        {
            if (registro == null) { return null; }
            return new BCDRangosDTO
            {
                Id_range = registro.Id_range,
                Range = registro.Range,
            };
        }

        /// <summary>
        /// Permite mapear BCD_Busquedas a BCDBusquedaDTO
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BCDBusquedasDTO BCD_Busquedas2BCDBusquedaDTO(BCD_Busquedas busqueda)
        {
            if (busqueda == null) { return null; }
            return new BCDBusquedasDTO
            {
                Id_search = busqueda.Id_search,
                Key_concepts = busqueda.Key_concepts,
                Key_words = busqueda.Key_words,
                Result_number = busqueda.Result_number,
                Search_date = busqueda.Search_date,
                Search_end_date = busqueda.Search_end_date,
                Search_start_date = busqueda.Search_start_date,
                Search_text = busqueda.Search_text,
                Search_user = busqueda.Search_user,
                Search_type = busqueda.Search_type,
                Search_relation = busqueda.Search_relation
            };
        }

        /// <summary>
        /// Permite mapear WbBusquedasDTO a entidad Documentos_abiertos
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Documentos_abiertos WbBusquedasDTO2Documentos_abiertos(WbBusquedasDTO dto)
        {
            if (dto == null) { return null; }
            return new Documentos_abiertos
            {
                Id_open_document = 0,
                Id_search_open = 0,
                Index_rowkey_open = dto.RowKey,
                Document_name_open = dto.NombreArchivoConExtension,
                Document_path_open = dto.RutaSharePointOnline,
                Confidence_open = (decimal?)dto.score,
            };
        }

        /// <summary>
        /// Permite mapear WbBusquedasDTO a entidad Documentos_abiertos
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Documentos_abiertos BCDResultadosRecomendadosDTO2Documentos_abiertos(BCDResultadosRecomendadosDTO dto)
        {
            if (dto == null) { return null; }
            return new Documentos_abiertos
            {
                Id_open_document = 0,
                Id_search_open = dto.Id_search_rr,
                Index_rowkey_open = dto.Index_rowkey,
                Document_name_open = dto.Document_name,
                Document_path_open = dto.Document_path,
                Confidence_open = dto.Confidence,
                Open_by = dto.Recommend_by,
            };
        }

        /// <summary>
        /// Permite mapear WbBusquedasDTO a entidad Resultados_busquedas
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Resultados_busquedas BCDResultadosRecomendadosDTO2Resultados_busquedas(BCDResultadosRecomendadosDTO dto)
        {
            if (dto == null) { return null; }
            return new Resultados_busquedas
            {
                Id_result = 0,
                Id_search_result = 0,
                Index_rowkey_result = dto.Index_rowkey,
                Document_name_result = dto.Document_name,
                Document_path_result = dto.Document_path,
                Confidence_result = dto.Confidence,
                Result_for = dto.Recommend_by,
            };
        }

        /// <summary>
        /// Mapear BusquedaDto a su entidad BCD_Busquedas
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BCD_Busquedas BusquedaDtoToEntity(BCDBusquedasDTO dto)
        {
            if (dto == null) { return null; }
            return new BCD_Busquedas
            {
                Search_date = dto.Search_date,
                Search_user = dto.Search_user,
                Search_text = dto.Search_text ?? "",
                Key_words = dto.Key_words,
                Key_concepts = dto.Key_concepts,
                Result_number = dto.Result_number,
                Search_start_date = dto.Search_start_date,
                Search_end_date = dto.Search_end_date,
                Search_relation = dto.Search_relation,
                Search_type = dto.Search_type,
                Exclude_words = dto.Exclude_words,
            };
        }

        /// <summary>
        /// Permite mapear BCDResultadosRecomendadosDTO a entidad BCD_Resultados_recomendados
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BCD_Resultados_recomendados BCDResultadosRecomendadosDTO2BCD_Resultados_recomendados(BCDResultadosRecomendadosDTO dto)
        {
            if (dto == null) { return null; }
            return new BCD_Resultados_recomendados
            {
                Id_document = dto.Id_document,
                Id_search_rr = dto.Id_search_rr,
                Index_rowkey = dto.Index_rowkey,
                Document_name = dto.Document_name,
                Document_path = dto.Document_path,
                Confidence = dto.Confidence,
                Recommend_by = dto.Recommend_by
            };
        }

        /// <summary>
        /// Permite mapear BCDResultadosRecomendadosDTO a Resultados_relacionados
        /// </summary>
        /// <param name="resultadoDTO"></param>
        /// <param name="idSearchRr"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Resultados_relacionados BCDResultadosRecomendadosDTO2Resultados_relacionados(BCDResultadosRecomendadosDTO dto)
        {
            if (dto == null) { return null; }
            return new Resultados_relacionados
            {
                Id_recommend = dto.Id_document,
                Id_search_recommend = dto.Id_search_rr,
                Index_rowkey_recommend = dto.Index_rowkey,
                Document_name_recommend = dto.Document_name,
                Document_path_recommend = dto.Document_path,
                Confidence_recommend = dto.Confidence,
                Recommend_for = dto.Recommend_by
            };
        }

        /// <summary>
        /// Permite mapear Resultados_relacionados a BCDResultadosRecomendadosDTO
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public BCDResultadosRecomendadosDTO BCD_Resultados_recomendados2BCDResultadosRecomendadosDTO(BCD_Resultados_recomendados registro)
        {
            if (registro == null) { return null; }
            return new BCDResultadosRecomendadosDTO
            {
                Id_document = registro.Id_document,
                Id_search_rr = registro.Id_search_rr,
                Index_rowkey = registro.Index_rowkey,
                Document_name = registro.Document_name,
                Document_path = registro.Document_path,
                Confidence = registro.Confidence,
                Recommend_by = registro.Recommend_by
            };
        }

        /// <summary>
        /// Permite mapear Resultados_relacionados a BCDResultadosRecomendadosDTO
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public BCDResultadosRecomendadosDTO Resultados_relacionados2BCDResultadosRecomendadosDTO(Resultados_relacionados registro)
        {
            if (registro == null) { return null; }

            return new BCDResultadosRecomendadosDTO
            {
                Id_document = registro.Id_recommend,
                Id_search_rr = registro.Id_search_recommend,
                Index_rowkey = registro.Index_rowkey_recommend,
                Document_name = registro.Document_name_recommend,
                Document_path = registro.Document_path_recommend,
                Confidence = registro.Confidence_recommend,
                Recommend_by = registro.Recommend_for
            };
        }

        /// <summary>
        /// Permite mapear Resultados_relacionados a BCDResultadosRecomendadosDTO
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public BCDResultadosRecomendadosDTO Documentos_abiertos2BCDResultadosRecomendadosDTO(Documentos_abiertos registro)
        {
            if (registro == null) { return null; }

            return new BCDResultadosRecomendadosDTO
            {
                Id_document = registro.Id_open_document,
                Id_search_rr = registro.Id_search_open,
                Index_rowkey = registro.Index_rowkey_open,
                Document_name = registro.Document_name_open,
                Document_path = registro.Document_path_open,
                Confidence = registro.Confidence_open,
                Recommend_by = registro.Open_by
            };
        }
    }
}
