using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using VK1.SCGExpress.Models;
using VK1.SCGExpress.Services;

namespace VK1.SCGExpress.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AlphasController : ControllerBase {
        private readonly AppQuery appQuery;
        private readonly Util util;

        public AlphasController(AppQuery appQuery, Util util) {
            this.appQuery = appQuery;
            this.util = util;
        }
        [HttpGet]
        public IActionResult Get() {
            return Ok("Not Implement...");
        }

        [HttpPost]
        public async Task<IActionResult> Post() {
            try {
                #region Qurey Booking Data from MySql then add to object
                TimeSpan fromTime = new TimeSpan(7, 0, 0);
                TimeSpan toTime = new TimeSpan(20, 0, 0);
                var from = (DateTime.Now.Date).Add(fromTime);
                var to = DateTime.Now.Date.Add(toTime);
                var strSql = $"select '' as No,substring(item.barcode,2,12) as QRCode,substring(item.barcode, 2, 12) as Reference1,'' as Reference2,'' as Reference3,'' as Reference4,if (parcels.cod_amount > 0 ,'True','False')as IsPOSTPAY,if (parcels.cod_amount > 0 ,parcels.cod_amount,0)as POSTPAYAmount,parcels.recipient_name as ReceiveName,parcels.recipient_tel as Mobile,'' as Mobile2,parcels.recipient_address as Address,'' as Subdistrict,if (parcels.recipient_district <> '',parcels.recipient_district,postalCode.district)as District,postalCode.province as Province,parcels.recipient_zipcode as PostalCode,parcels.user_note as Remark,'' as ExchangeType,'' as ExchangeRemark,'' as Email,'' as Email2,1 as ItemNo,'' as SupplierSKU,'' as VendorSKU,'' as MerchantSKU,'' as ProductEANUPC,'Products 1' as ProductName,'' as ProductSize,'' as ProductColor,1 as ProductQuantity,'' as FulfillmentRemark,org.name as MerchantName,postalCode.province as MerchantAddress,org.name as MerchantContactPerson,org.phone as MerchantMobile ,orgGeo.json_info as GeographicLocation from tbl_manifest_items item left join tbl_manifest_sheets sheet on sheet.id = item.sheet_id left join tbl_scg_parcels parcels on parcels.tracking_number = item.barcode left join tbl_scg_postalCode postalCode on postalCode.postal_code = parcels.recipient_zipcode left join tbl_organization org on org.customer_code = parcels.sender_code left join tbl_auth_users user on user.id = item.user_created left join tbl_organization orgGeo on orgGeo.organization_id = user.organization_id where sheet.branch_destination = 'Alpha Fast' and item.status_deleted = 'N' and item.date_created between '2020-04-03 00:00:00' and '2020-04-03 20:00:00' and org.name is not null group by item.barcode";

                var data = await appQuery.AlphaBookingViewModelsAsync<AlphaBookingViewModel>(strSql);
                var merchantName = from x in data.ToList() group x by x.MerchantName into g orderby g.Key
                                   select new {
                                       MerchantName = g.FirstOrDefault().MerchantName,
                                       MerchantAddress = g.FirstOrDefault().MerchantAddress,
                                       MerchantContactPerson = g.FirstOrDefault().MerchantContactPerson,
                                       IsNewReturn = false,
                                       Province = g.FirstOrDefault().Province,
                                       District = g.FirstOrDefault().District,
                                       Subdistrict = g.FirstOrDefault().Subdistrict,
                                       Latitude = g.FirstOrDefault().GeographicLocation.Split('"').ToArray(),
                                       Longitude = g.FirstOrDefault().GeographicLocation.Split('"').ToArray(),
                                   };
                List<MerchantData> merchantDatas = new List<MerchantData>();
                //Random random = new Random();

                foreach (var m in merchantName) {
                    List<BookingData> bookingDatas = new List<BookingData>();
                    var bookings = data.Where(x => x.MerchantName == m.MerchantName);
                    foreach (var b in bookings) {
                       // var ran = random.Next(111111111, 999999999);
                        BookingData bookingData = new BookingData() {
                            QRCode = b.QRCode,
                            Reference1 = b.Reference1,
                            CustomerRemark = null,
                            IsPOSTPAY = b.IsPOSTPAY == "True" ? true : false,
                            Amount = b.POSTPAYAmount,
                            Firstname = b.ReceiveName.Length <= 50 ? b.ReceiveName : b.ReceiveName.Substring(0, 50),
                            Lastname = b.ReceiveName.Length <= 50 ? b.ReceiveName : b.ReceiveName.Substring(0, 50),
                            Mobile = util.CorrectMobileFormat(b.Mobile),
                            Address = b.Address,
                            Province = b.Province,
                            district = b.District,
                            Subdistrict = b.Subdistrict,
                            PostalCode = b.PostalCode,
                            IsExchange = false,
                            IsIVR = false,
                            IsInsurance = false,
                            IsSameDay = false
                        };
                        bookingDatas.Add(bookingData);
                    }

                    MerchantData merchantData = new MerchantData() {
                        MerchantName = m.MerchantName,
                        MerchantAddress = m.MerchantAddress,
                        MerchantContactPerson = m.MerchantContactPerson,
                        IsNewReturn = false,
                        Province = m.Province,
                        District = m.District,
                        Subdistrict = m.Subdistrict,
                        Latitude = 13.736831,//Convert.ToDouble(m.Latitude[3]),
                        Longitude = 100.606181, //Convert.ToDouble(m.Longitude[7]),
                        BookingData = bookingDatas
                    };
                    merchantDatas.Add(merchantData);

                    DeliveryOrderRequest deliveryOrder = new DeliveryOrderRequest() {
                        MerchantData = merchantDatas,
                        AppointmentDateId = 2,
                        AppointmentTimeId = 18
                    };
                }
                DeliveryOrderRequest orderRequests = new DeliveryOrderRequest() {
                    MerchantData = merchantDatas,
                    AppointmentDateId = 0,
                    AppointmentTimeId = 0
                };
                #endregion

                #region Call Alpha api usign Restsharp
                var c = new RestClient("https://apialpha.alphauat.com");
                var r = new RestRequest("api/DeliveryOrder/MarketPlaceDeliveryOrderEntry", Method.POST);
                r.AddHeader("Authorization", "Alpha vxkx619iHkU5JXLWdXVE5WQX731C18D45crxFoe9N4NI1ltTl98QpztHGB/Q8l9JLFB+vdH/zFbA3T+9LNVWZMx94swrLieqn0tLjVq6lpYn6IOT3jgNmaGfWx5sbU12Qlx+LxSrK11TUcjhDYRN2KAxjGlrhMFIk73rp3cOBv74acFjFvZQnYWZ/QUxFkThZY2UfQettfR9zbaVF4MkBH0rG+WyOFhz10a42peh65G3z0ShrKJ+MvvBMcZLf+t8jYJ/neNIC0YCQevkfaDJY5IofeIfpsLzgMrYzD2NObCcMpK1ZfUZE/Qrp/NNTPCm4hFFQfMkg/DPEO2qp52HVhBCOVvC3pxs");

                var serealize = JsonSerializer.Serialize<DeliveryOrderRequest>(orderRequests);
                r.AddJsonBody(orderRequests);
                var res = c.Execute(r);
                var model = JsonSerializer.Deserialize<CreateBookingModel>(res.Content);
                #endregion

                return StatusCode(StatusCodes.Status201Created, model);

            } catch (Exception) {

                throw;
            }
        }
    }
}