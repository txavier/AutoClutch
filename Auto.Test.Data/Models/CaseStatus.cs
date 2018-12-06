using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Test.Data.Models
{
    //public partial class OpenDataOathCaseStatus
    //{
    //    [JsonProperty("@odata.context")]
    //    public Uri OdataContext { get; set; }

    //    //[JsonProperty("value")]
    //    //public Value[] Value { get; set; }

    //    public CaseStatus[] value { get; set; }
    //}

    public class CaseStatus
    {
        public string __id { get; set; }
        public string ticket_number { get; set; }
        public DateTime? violation_date { get; set; }
        public string violation_time { get; set; }
        public string issuing_agency { get; set; }
        public object respondent_first_name { get; set; }
        public string respondent_last_name { get; set; }
        public object balance_due { get; set; }
        public string violation_location_borough { get; set; }
        public string violation_location_block_no { get; set; }
        public string violation_location_lot_no { get; set; }
        public string violation_location_house { get; set; }
        public string violation_location_street_name { get; set; }
        public object violation_location_floor { get; set; }
        public string violation_location_city { get; set; }
        public string violation_location_zip_code { get; set; }
        public string violation_location_state_name { get; set; }
        public string respondent_address_borough { get; set; }
        public string respondent_address_house { get; set; }
        public string respondent_address_street_name { get; set; }
        public string respondent_address_city { get; set; }
        public string respondent_address_zip_code { get; set; }
        public string respondent_address_state_name { get; set; }
        public string hearing_status { get; set; }
        public string hearing_result { get; set; }
        public string scheduled_hearing_location { get; set; }
        public DateTime? hearing_date { get; set; }
        public string hearing_time { get; set; }
        public string decision_location_borough { get; set; }
        public DateTime? decision_date { get; set; }
        public int? total_violation_amount { get; set; }
        public string violation_details { get; set; }
        public DateTime? date_judgment_docketed { get; set; }
        public object respondent_address_or_facility_number_for_fdny_and_dob_tickets { get; set; }
        public int? penalty_imposed { get; set; }
        public int? paid_amount { get; set; }
        public object additional_penalties_or_late_fees { get; set; }
        public string compliance_status { get; set; }
        public object violation_description { get; set; }
        public string charge_1_code { get; set; }
        public string charge_1_code_section { get; set; }
        public string charge_1_code_description { get; set; }
        public int? charge_1_infraction_amount { get; set; }
        public object charge_2_code { get; set; }
        public object charge_2_code_section { get; set; }
        public object charge_2_code_description { get; set; }
        public object charge_2_infraction_amount { get; set; }
        public object charge_3_code { get; set; }
        public object charge_3_code_section { get; set; }
        public object charge_3_code_description { get; set; }
        public object charge_3_infraction_amount { get; set; }
        public object charge_4_code { get; set; }
        public object charge_4_code_section { get; set; }
        public object charge_4_code_description { get; set; }
        public object charge_4_infraction_amount { get; set; }
        public object charge_5_code { get; set; }
        public object charge_5_code_section { get; set; }
        public object charge_5_code_description { get; set; }
        public object charge_5_infraction_amount { get; set; }
        public object charge_6_code { get; set; }
        public object charge_6_code_section { get; set; }
        public object charge_6_code_description { get; set; }
        public object charge_6_infraction_amount { get; set; }
        public object charge_7_code { get; set; }
        public object charge_7_code_section { get; set; }
        public object charge_7_code_description { get; set; }
        public object charge_7_infraction_amount { get; set; }
        public object charge_8_code { get; set; }
        public object charge_8_code_section { get; set; }
        public object charge_8_code_description { get; set; }
        public object charge_8_infraction_amount { get; set; }
        public object charge_9_code { get; set; }
        public object charge_9_code_section { get; set; }
        public object charge_9_code_description { get; set; }
        public object charge_9_infraction_amount { get; set; }
        public object charge_10_code { get; set; }
        public object charge_10_code_section { get; set; }
        public object charge_10_code_description { get; set; }
        public object charge_10_infraction_amount { get; set; }
    }

    public class RootObject
    {
        //public string __invalid_name__@odata.context { get; set; }
        public List<CaseStatus> value { get; set; }
    }
}
