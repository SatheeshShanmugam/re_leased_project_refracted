using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Utility
{
    public static class ResponseMessages
    {
        public const string Invalid_Invoice_Type = "Invalid Invoice Type";
        public const string Contact_System_Admin = "Please contact System Administrator";
        public const string Invoice_is_partially_paid = "Invoice is now partially paid";
        public const string Another_Partial_Payment_Received = "Another partial payment received, still not fully paid";
        public const string Final_Partial_Payment_Received = "Final partial payment received, Invoice is now fully paid";
        public const string Invoice_Is_Now_Fully_Paid = "Invoice is now fully paid";
        public const string Payment_Is_Greater_Than_Invoice_Amount = "The payment is greater than the invoice amount";
        public const string Payment_Is_Greater_Than_Partial_Amount_Remaining = "The payment is greater than the partial amount remaining";
        public const string Invoice_Was_Already_Fully_Paid = "Invoice was already fully paid";
        public const string Invalid_Invoice_With_Payments = "The invoice is in an invalid state, it has an amount of 0 and it has payments";
        public const string No_Payment_Needed = "No Payment Needed";
        public const string No_Invoice_Matching_Current_Payment = "There is no invoice matching this payment";
   
    }

}
    