using System;
using System.Collections.Generic;
using System.Text;
namespace MyLib.MyBase.MyAjax
{
    public class AjaxResult
    {
        public enum TypeResult
        {
            Error = 0,
            Success = 1,
            Fail = 2,
            Warring = 3,
            Notice = 4,
            UnSuccess = 5,
            NoData = 6,
        }

        public int CurrentTypeResult
        {
            get;
            set;
        }
        public string Parameter
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }

        public AjaxResult(string Parameter, int CurrentTypeResult, string Description)
        {
            this.Parameter = Parameter;
            this.CurrentTypeResult = CurrentTypeResult;
            this.Description = Description;
        }
        public AjaxResult(string Parameter, int CurrentTypeResult, string Value, string Description)
        {
            this.Parameter = Parameter;
            this.CurrentTypeResult = CurrentTypeResult;
            this.Description = Description;
            this.Value = Value;
        }
        public AjaxResult(string Parameter, string Description)
        {
            this.Parameter = Parameter;
            this.CurrentTypeResult = (int)TypeResult.Warring;
            this.Description = Description;
        }
       
    }
}
