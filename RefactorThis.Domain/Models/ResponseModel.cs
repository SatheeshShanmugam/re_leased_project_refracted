using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Models
{
    public class ResponseModel
    {       
        public ResponseModel(string msg)
        {
            Message = msg;
        }
        public string Message { get; set; } // Holds the error or success message        
    }

}
