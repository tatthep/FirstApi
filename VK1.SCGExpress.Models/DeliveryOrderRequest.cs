using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace VK1.SCGExpress.Models {
    public class BookingData {
        public string QRCode { get; set; }
        public string Reference1 { get; set; }
        public string CustomerRemark { get; set; }
        public bool IsPOSTPAY { get; set; }
        public int Amount { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string district { get; set; }
        public string Subdistrict { get; set; }
        public string PostalCode { get; set; }
        public bool IsExchange { get; set; }
        public bool IsIVR { get; set; }
        public bool IsInsurance { get; set; }
        public bool IsSameDay { get; set; }
    }

    public class MerchantData {
        public string MerchantName { get; set; }
        public string MerchantAddress { get; set; }
        public string MerchantContactPerson { get; set; }
        public string MerchantMobile { get; set; }
        public bool IsNewReturn { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Subdistrict { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<BookingData> BookingData { get; set; }
    }

    public class DeliveryOrderRequest {
        public List<MerchantData> MerchantData { get; set; }
        public int AppointmentDateId { get; set; }
        public int AppointmentTimeId { get; set; }
    }
}
