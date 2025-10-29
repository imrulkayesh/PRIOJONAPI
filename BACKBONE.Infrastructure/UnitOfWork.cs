﻿using BACKBONE.Application.Interfaces;
using BACKBONE.Application.Interfaces.SecurityInterface;
using BACKBONE.Infrastructure.SecurityRepo;
using System.Windows.Input;

namespace BACKBONE.Infrastructure
{
    public partial class UnitOfWork : IUnitOfWork
    {
        public ITokenRepository TokenRepositor { get; }    
        public IRefreshTokenService RefreshTokenService { get; }    
        public IJwtTokenService JwtTokenService { get; }    
        public IUserRepository UserRepository { get; }
        public IHrisAuthService HrisAuthService { get; }
        public ISampleDataRepository SampleDataRepository { get; }

        public ICommon CommonRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }

        public UnitOfWork(
            ITokenRepository iTokenRepository, 
            IRefreshTokenService refreshTokenService, 
            IJwtTokenService jwtTokenService, 
            IUserRepository userRepository,
            IHrisAuthService hrisAuthService,
            ISampleDataRepository sampleDataRepository,
            ICommon commonRepository,
            IInvoiceRepository invoiceRepository)
        {
            TokenRepositor = iTokenRepository;
            RefreshTokenService = refreshTokenService;
            JwtTokenService = jwtTokenService;
            UserRepository = userRepository;
            HrisAuthService = hrisAuthService;
            SampleDataRepository = sampleDataRepository;
            CommonRepository = commonRepository;
            InvoiceRepository = invoiceRepository;
        }
    }
}