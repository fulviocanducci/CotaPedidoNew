using System;
using System.Collections.Generic;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioAnexoItem : EFRepositoryBase<AnexoItem, COTAPEDIDOContext>, IRepositorioAnexoItem
    {
        public RepositorioAnexoItem()
            : base(new COTAPEDIDOContext())
        {}
    }
}
