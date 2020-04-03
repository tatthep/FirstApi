using System;
using System.Collections.Generic;
using System.Text;

namespace VK1.SCGExpress.Models {
    public class CreateBookingModel {
        public int BookingId { get; set; }
        public string QRCode { get; set; }
        public virtual ICollection<BookingQRCodeDetailModel> QRCodes { get; set; }
        public virtual ICollection<BookingQRCodeMappingQRCodeThaipost> QRCodesMappingThaiPost { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime TimeStamp { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class BookingQRCodeDetailModel {
        public string QRCode { get; set; }
        public Errors Errors { get; set; }
    }

    public class Errors {
        public int Seq { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class BookingQRCodeMappingQRCodeThaipost {
        public int Id { get; set; }
    }
}
