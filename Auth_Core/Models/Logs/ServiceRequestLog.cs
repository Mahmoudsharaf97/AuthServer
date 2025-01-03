
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Auth_Core.Models
{
    public class ServiceRequestLog
    {
        public string ElkId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public string Method { get; set; }
        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ServiceURL { get; set; }
        public int? ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string ServiceRequest { get; set; }
        public string ServiceResponse { get; set; }
        public string ServerIP { get; set; }
        public Guid? RequestId { get; set; }
        public double? ServiceResponseTimeInSeconds { get; set; }
        public string Channel { get; set; }
        public string ServiceErrorCode { get; set; }
        public string ServiceErrorDescription { get; set; }
        public string ReferenceId { get; set; }
        public int? InsuranceTypeCode { get; set; }
        public string DriverNin { get; set; }
        public string VehicleId { get; set; }
        public string PolicyNo { get; set; }
        public string VehicleMaker { get; set; }
        public string VehicleMakerCode { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleModelCode { get; set; }
        public int? VehicleModelYear { get; set; }
        public string ExternalId { get; set; }
        public bool? VehicleAgencyRepair { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string ChassisNumber { get; set; }
        public string LogType { get; set; }
        public string AppPool { get; set; }
    }
}
