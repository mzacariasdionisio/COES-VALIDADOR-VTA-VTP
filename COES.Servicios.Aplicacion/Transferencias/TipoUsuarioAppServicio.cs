using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class TipoUsuarioAppServicio : AppServicioBase
    {

        /// <summary>
        /// Inserta o actualiza un registro de tipo usuario
        /// </summary>
        /// <param name="entity">TipoUsuarioDTO</param>
        /// <returns>Código de la tabla TRN_TIPO_USUARIO</returns>
        public int SaveOrUpdateTipoUsuario(TipoUsuarioDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.TipoUsuaCodi == 0)
                {
                    id = FactoryTransferencia.GetTipoUsuarioRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetTipoUsuarioRepository().Update(entity);
                    id = entity.TipoUsuaCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_TIPO_USUARIO
        /// </summary>
        /// <param name="iTipUsuCodi">Código de la tabla TRN_TIPO_USUARIO</param>
        /// <returns>Codigo del registro eliminado</returns>
        public int DeleteTipoUsuario(int idTipoUsuario)
        {
            try
            {
                FactoryTransferencia.GetTipoUsuarioRepository().Delete(idTipoUsuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return idTipoUsuario;
        }

        /// <summary>
        /// Permite obtener el Tipo de Usuario en base al id
        /// </summary>
        /// <param name="iTipUsuCodi">Código de la tabla TRN_TIPO_USUARIO</param>
        /// <returns>TipoUsuarioDTO</returns>
        public TipoUsuarioDTO GetByTipoUsuario(int iTipUsuCodi)
        {
            return FactoryTransferencia.GetTipoUsuarioRepository().GetById(iTipUsuCodi);
        }

        /// <summary>
        /// Permite listar todos los registros del Tipo de Usuario
        /// </summary>
        /// <returns>Lista de TipoUsuarioDTO</returns>
        public List<TipoUsuarioDTO> ListTipoUsuario()
        {
            return FactoryTransferencia.GetTipoUsuarioRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas de TipoUsuario en base al nombre
        /// </summary>
        /// <param name="sTipUsuNombre">Nombre del tipo de susuario</param>
        /// <returns>Lista de TipoUsuarioDTO</returns>
        public List<TipoUsuarioDTO> BuscarTipoUsuario(string sTipUsuNombre)
        {
            return FactoryTransferencia.GetTipoUsuarioRepository().GetByCriteria(sTipUsuNombre);
        }

    }
}
