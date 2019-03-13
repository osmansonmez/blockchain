using System;
using System.Threading.Tasks;
using BlockChainBackend.Helpers;
using BlockChainBackend.Models;
using BlockChainBackend.Models.Customer;
using BlockChainBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContractInterface.Common
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IConfiguration _config;
        private ILogger<CustomerController> _logger;
        private IWalletService _walletService;
        private ICheckService _checkService;

        public CustomerController(IConfiguration configuration,
            IWalletService walletService,
            ICheckService checkService,
            ILogger<CustomerController> logger)
        {
            _config = configuration;
            _logger = logger;
            _walletService = walletService;
            _checkService = checkService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheck(CreateCheckRequest request)
        {
            var fromAddress = await _walletService.GetWalletAdress(request.CustomerId);
            var toAddress = await _walletService.GetWalletAdress(request.ToUser);

            var checkId =
                await _checkService.CreateCheck(fromAddress, toAddress, request.CheckDate, (uint) request.Amount);
            var response = new CreateCheckResponse() {CheckId = checkId.ToString()};
            return await Task.FromResult(Ok(response));
        }

        [HttpPost]
        public async Task<IActionResult> CreatedLastCheck(BaseCustomerRequestViewModel request)
        {
            CheckInfoResponse response = new CheckInfoResponse();
            var fromAddress = await _walletService.GetWalletAdress(request.CustomerId);
            if (!string.IsNullOrEmpty(fromAddress))
            {
                var checkIdResponse = await _checkService.CreatedLastCheck(fromAddress);
                var datas = await _checkService.CheckInfo(fromAddress, checkIdResponse.CheckId);
                response = new CheckInfoResponse()
                {
                    Check = new Check()
                    {
                        Amount = datas.Amount,
                        Creator = datas.Creator,
                        CheckDate = TimeHelper.UnixTimestampToDateTime(datas.CheckDate),
                        Owner = datas.Owner
                    }
                };
            }

            return await Task.FromResult(Ok(response));
        }

        [HttpPost]
        public async Task<IActionResult> CheckInfo(CheckInfoRequest request)
        {
            var fromAddress = await _walletService.GetWalletAdress(request.CustomerId);
            CheckInfoResponse response = new CheckInfoResponse();
            if (!string.IsNullOrEmpty(fromAddress))
            {
                var datas = await _checkService.CheckInfo(fromAddress, request.CheckId);
                response = new CheckInfoResponse()
                {
                    Check = new Check()
                    {
                        Amount = datas.Amount,
                        Creator = datas.Creator,
                        CheckDate = TimeHelper.UnixTimestampToDateTime(datas.CheckDate),
                        Owner = datas.Owner
                    }
                };
            }

            return await Task.FromResult(Ok(response));
        }

        [HttpPost]
        public async Task<IActionResult> CheckCreatedList(CheckCretedListRequest request)
        {
            var response = new CheckListResponse();
            var fromAddress = await _walletService.GetWalletAdress(request.CustomerId);
            var list = await _checkService.CheckListCreated(fromAddress);
            response.CheckList = list;
            return await Task.FromResult(Ok(response));
        }
        
        [HttpPost]
        public async Task<IActionResult> CheckOwnerList(CheckCretedListRequest request)
        {
            var response = new CheckListResponse();
            var fromAddress = await _walletService.GetWalletAdress(request.CustomerId);
            var list = await _checkService.CheckListOwner(fromAddress);
            response.CheckList = list;
            return await Task.FromResult(Ok(response));
        }
    }
}