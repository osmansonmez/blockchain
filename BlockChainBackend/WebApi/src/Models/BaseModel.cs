using System;
using System.ComponentModel.DataAnnotations;

namespace BlockChainBackend.Models
{
    public class BaseBankRequestViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "CustomerId değerini gönderiniz.")]
        [MinLength(1, ErrorMessage = "CustomerId en az bir karakter olmalı")]
        [MaxLength(11, ErrorMessage = "CustomerId en fazla 11 karakter olmalı")]
        public string CustomerId { get; set; }
    }

    public class BaseBankResponseViewModel
    {
        public Result ServiceResult { get; set; } = new Result(){Code = "0",IsSuccess = true};
    }

    public class Result
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }
    }
    
    public class BaseCustomerRequestViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "CustomerId değerini gönderiniz.")]
        [MinLength(1, ErrorMessage = "CustomerId en az bir karakter olmalı")]
        [MaxLength(11, ErrorMessage = "CustomerId en fazla 11 karakter olmalı")]
        public string CustomerId { get; set; }
    }

    public class BaseCustomerResponseViewModel
    {
        public Result ServiceResult { get; set; } = new Result(){Code = "0",IsSuccess = true};
    }
}