﻿using MvcTemplate.Data.Core;
using System;

namespace MvcTemplate.Services
{
    public abstract class BaseService : IService
    {
        public String CurrentAccountId { get; set; }
        protected IUnitOfWork UnitOfWork { get; }
        private Boolean Disposed { get; set; }

        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            UnitOfWork.Dispose();

            Disposed = true;
        }
    }
}
