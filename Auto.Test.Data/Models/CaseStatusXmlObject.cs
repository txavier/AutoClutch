using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
    [XmlRoot(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
    public class Title : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
    public class Link
    {
        [XmlAttribute(AttributeName = "rel")]
        public string Rel { get; set; }
        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }

    [XmlRoot(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
    public class Author
    {
        [XmlElement(ElementName = "name", Namespace = "http://www.w3.org/2005/Atom")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "category", Namespace = "http://www.w3.org/2005/Atom")]
    public class Category
    {
        [XmlAttribute(AttributeName = "term")]
        public string Term { get; set; }
        [XmlAttribute(AttributeName = "scheme")]
        public string Scheme { get; set; }
    }

    [XmlRoot(ElementName = "__id", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class __id : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "violation_date", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_date : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "respondent_first_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Respondent_first_name : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "respondent_last_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Respondent_last_name : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "balance_due", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Balance_due : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "violation_location_block_no", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_location_block_no : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "violation_location_lot_no", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_location_lot_no : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "violation_location_house", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_location_house : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "violation_location_street_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_location_street_name : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "violation_location_floor", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_location_floor : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "violation_location_city", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_location_city : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "violation_location_zip_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_location_zip_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "violation_location_state_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_location_state_name : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "respondent_address_house", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Respondent_address_house : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "respondent_address_street_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Respondent_address_street_name : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "respondent_address_city", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Respondent_address_city : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "respondent_address_zip_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Respondent_address_zip_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "respondent_address_state_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Respondent_address_state_name : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "hearing_result", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Hearing_result : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "scheduled_hearing_location", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Scheduled_hearing_location : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "hearing_date", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Hearing_date : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "decision_location_borough", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Decision_location_borough : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "decision_date", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Decision_date : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "total_violation_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Total_violation_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "violation_details", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_details : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "date_judgment_docketed", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Date_judgment_docketed : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "respondent_address_or_facility_number_for_fdny_and_dob_tickets", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Respondent_address_or_facility_number_for_fdny_and_dob_tickets : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "penalty_imposed", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Penalty_imposed : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "paid_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Paid_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "additional_penalties_or_late_fees", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Additional_penalties_or_late_fees : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "violation_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Violation_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_1_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_1_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    public class DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "charge_1_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_1_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_1_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_1_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_1_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_1_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "charge_2_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_2_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_2_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_2_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_2_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_2_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_2_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_2_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_3_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_3_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_3_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_3_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_3_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_3_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_3_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_3_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_4_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_4_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_4_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_4_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_4_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_4_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_4_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_4_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_5_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_5_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_5_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_5_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_5_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_5_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_5_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_5_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_6_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_6_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_6_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_6_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_6_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_6_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_6_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_6_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_7_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_7_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_7_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_7_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_7_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_7_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_7_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_7_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_8_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_8_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_8_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_8_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_8_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_8_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_8_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_8_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_9_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_9_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_9_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_9_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_9_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_9_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_9_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_9_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_10_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_10_code : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_10_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_10_code_section : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_10_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_10_code_description : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "charge_10_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
    public class Charge_10_infraction_amount : DefaultXmlValue
    {
        [XmlAttribute(AttributeName = "type", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "null", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
        public string Null { get; set; }
    }

    [XmlRoot(ElementName = "properties", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
    public class Properties
    {
        [XmlElement(ElementName = "__id", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public __id __id { get; set; }
        [XmlElement(ElementName = "ticket_number", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Ticket_number { get; set; }
        [XmlElement(ElementName = "violation_date", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_date Violation_date { get; set; }
        [XmlElement(ElementName = "violation_time", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Violation_time { get; set; }
        [XmlElement(ElementName = "issuing_agency", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Issuing_agency { get; set; }
        [XmlElement(ElementName = "respondent_first_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Respondent_first_name Respondent_first_name { get; set; }
        [XmlElement(ElementName = "respondent_last_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Respondent_last_name Respondent_last_name { get; set; }
        [XmlElement(ElementName = "balance_due", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Balance_due Balance_due { get; set; }
        [XmlElement(ElementName = "violation_location_borough", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Violation_location_borough { get; set; }
        [XmlElement(ElementName = "violation_location_block_no", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_location_block_no Violation_location_block_no { get; set; }
        [XmlElement(ElementName = "violation_location_lot_no", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_location_lot_no Violation_location_lot_no { get; set; }
        [XmlElement(ElementName = "violation_location_house", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_location_house Violation_location_house { get; set; }
        [XmlElement(ElementName = "violation_location_street_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_location_street_name Violation_location_street_name { get; set; }
        [XmlElement(ElementName = "violation_location_floor", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_location_floor Violation_location_floor { get; set; }
        [XmlElement(ElementName = "violation_location_city", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_location_city Violation_location_city { get; set; }
        [XmlElement(ElementName = "violation_location_zip_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_location_zip_code Violation_location_zip_code { get; set; }
        [XmlElement(ElementName = "violation_location_state_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_location_state_name Violation_location_state_name { get; set; }
        [XmlElement(ElementName = "respondent_address_borough", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Respondent_address_borough { get; set; }
        [XmlElement(ElementName = "respondent_address_house", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Respondent_address_house Respondent_address_house { get; set; }
        [XmlElement(ElementName = "respondent_address_street_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Respondent_address_street_name Respondent_address_street_name { get; set; }
        [XmlElement(ElementName = "respondent_address_city", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Respondent_address_city Respondent_address_city { get; set; }
        [XmlElement(ElementName = "respondent_address_zip_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Respondent_address_zip_code Respondent_address_zip_code { get; set; }
        [XmlElement(ElementName = "respondent_address_state_name", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Respondent_address_state_name Respondent_address_state_name { get; set; }
        [XmlElement(ElementName = "hearing_status", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Hearing_status { get; set; }
        [XmlElement(ElementName = "hearing_result", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Hearing_result Hearing_result { get; set; }
        [XmlElement(ElementName = "scheduled_hearing_location", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Scheduled_hearing_location Scheduled_hearing_location { get; set; }
        [XmlElement(ElementName = "hearing_date", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Hearing_date Hearing_date { get; set; }
        [XmlElement(ElementName = "hearing_time", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Hearing_time { get; set; }
        [XmlElement(ElementName = "decision_location_borough", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Decision_location_borough Decision_location_borough { get; set; }
        [XmlElement(ElementName = "decision_date", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Decision_date Decision_date { get; set; }
        [XmlElement(ElementName = "total_violation_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Total_violation_amount Total_violation_amount { get; set; }
        [XmlElement(ElementName = "violation_details", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_details Violation_details { get; set; }
        [XmlElement(ElementName = "date_judgment_docketed", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Date_judgment_docketed Date_judgment_docketed { get; set; }
        [XmlElement(ElementName = "respondent_address_or_facility_number_for_fdny_and_dob_tickets", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Respondent_address_or_facility_number_for_fdny_and_dob_tickets Respondent_address_or_facility_number_for_fdny_and_dob_tickets { get; set; }
        [XmlElement(ElementName = "penalty_imposed", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Penalty_imposed Penalty_imposed { get; set; }
        [XmlElement(ElementName = "paid_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Paid_amount Paid_amount { get; set; }
        [XmlElement(ElementName = "additional_penalties_or_late_fees", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Additional_penalties_or_late_fees Additional_penalties_or_late_fees { get; set; }
        [XmlElement(ElementName = "compliance_status", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public string Compliance_status { get; set; }
        [XmlElement(ElementName = "violation_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Violation_description Violation_description { get; set; }
        [XmlElement(ElementName = "charge_1_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_1_code Charge_1_code { get; set; }
        [XmlElement(ElementName = "charge_1_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_1_code_section Charge_1_code_section { get; set; }
        [XmlElement(ElementName = "charge_1_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_1_code_description Charge_1_code_description { get; set; }
        [XmlElement(ElementName = "charge_1_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_1_infraction_amount Charge_1_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_2_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_2_code Charge_2_code { get; set; }
        [XmlElement(ElementName = "charge_2_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_2_code_section Charge_2_code_section { get; set; }
        [XmlElement(ElementName = "charge_2_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_2_code_description Charge_2_code_description { get; set; }
        [XmlElement(ElementName = "charge_2_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_2_infraction_amount Charge_2_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_3_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_3_code Charge_3_code { get; set; }
        [XmlElement(ElementName = "charge_3_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_3_code_section Charge_3_code_section { get; set; }
        [XmlElement(ElementName = "charge_3_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_3_code_description Charge_3_code_description { get; set; }
        [XmlElement(ElementName = "charge_3_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_3_infraction_amount Charge_3_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_4_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_4_code Charge_4_code { get; set; }
        [XmlElement(ElementName = "charge_4_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_4_code_section Charge_4_code_section { get; set; }
        [XmlElement(ElementName = "charge_4_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_4_code_description Charge_4_code_description { get; set; }
        [XmlElement(ElementName = "charge_4_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_4_infraction_amount Charge_4_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_5_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_5_code Charge_5_code { get; set; }
        [XmlElement(ElementName = "charge_5_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_5_code_section Charge_5_code_section { get; set; }
        [XmlElement(ElementName = "charge_5_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_5_code_description Charge_5_code_description { get; set; }
        [XmlElement(ElementName = "charge_5_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_5_infraction_amount Charge_5_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_6_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_6_code Charge_6_code { get; set; }
        [XmlElement(ElementName = "charge_6_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_6_code_section Charge_6_code_section { get; set; }
        [XmlElement(ElementName = "charge_6_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_6_code_description Charge_6_code_description { get; set; }
        [XmlElement(ElementName = "charge_6_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_6_infraction_amount Charge_6_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_7_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_7_code Charge_7_code { get; set; }
        [XmlElement(ElementName = "charge_7_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_7_code_section Charge_7_code_section { get; set; }
        [XmlElement(ElementName = "charge_7_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_7_code_description Charge_7_code_description { get; set; }
        [XmlElement(ElementName = "charge_7_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_7_infraction_amount Charge_7_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_8_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_8_code Charge_8_code { get; set; }
        [XmlElement(ElementName = "charge_8_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_8_code_section Charge_8_code_section { get; set; }
        [XmlElement(ElementName = "charge_8_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_8_code_description Charge_8_code_description { get; set; }
        [XmlElement(ElementName = "charge_8_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_8_infraction_amount Charge_8_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_9_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_9_code Charge_9_code { get; set; }
        [XmlElement(ElementName = "charge_9_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_9_code_section Charge_9_code_section { get; set; }
        [XmlElement(ElementName = "charge_9_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_9_code_description Charge_9_code_description { get; set; }
        [XmlElement(ElementName = "charge_9_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_9_infraction_amount Charge_9_infraction_amount { get; set; }
        [XmlElement(ElementName = "charge_10_code", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_10_code Charge_10_code { get; set; }
        [XmlElement(ElementName = "charge_10_code_section", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_10_code_section Charge_10_code_section { get; set; }
        [XmlElement(ElementName = "charge_10_code_description", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_10_code_description Charge_10_code_description { get; set; }
        [XmlElement(ElementName = "charge_10_infraction_amount", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices")]
        public Charge_10_infraction_amount Charge_10_infraction_amount { get; set; }
    }

    //[XmlRoot(ElementName = "content", Namespace = "http://www.w3.org/2005/Atom")]
    //public class Content
    //{
    //    [XmlElement(ElementName = "properties", Namespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata")]
    //    public Properties Properties { get; set; }
    //    [XmlAttribute(AttributeName = "type")]
    //    public string Type { get; set; }
    //}

    //[XmlRoot(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
    //public class Entry
    //{
    //    [XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
    //    public string Id { get; set; }
    //    [XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
    //    public Title Title { get; set; }
    //    [XmlElement(ElementName = "updated", Namespace = "http://www.w3.org/2005/Atom")]
    //    public string Updated { get; set; }
    //    [XmlElement(ElementName = "author", Namespace = "http://www.w3.org/2005/Atom")]
    //    public Author Author { get; set; }
    //    [XmlElement(ElementName = "category", Namespace = "http://www.w3.org/2005/Atom")]
    //    public Category Category { get; set; }
    //    [XmlElement(ElementName = "content", Namespace = "http://www.w3.org/2005/Atom")]
    //    public List<Content> Content { get; set; }
    //}

    //[XmlRoot(ElementName = "feed", Namespace = "http://www.w3.org/2005/Atom")]
    //public class Feed
    //{
    //    [XmlElement(ElementName = "title", Namespace = "http://www.w3.org/2005/Atom")]
    //    public Title Title { get; set; }
    //    [XmlElement(ElementName = "id", Namespace = "http://www.w3.org/2005/Atom")]
    //    public string Id { get; set; }
    //    [XmlElement(ElementName = "updated", Namespace = "http://www.w3.org/2005/Atom")]
    //    public string Updated { get; set; }
    //    [XmlElement(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
    //    public List<Link> Link { get; set; }
    //    [XmlElement(ElementName = "entry", Namespace = "http://www.w3.org/2005/Atom")]
    //    public List<Entry> Entry { get; set; }
    //    [XmlAttribute(AttributeName = "xmlns")]
    //    public string Xmlns { get; set; }
    //    [XmlAttribute(AttributeName = "base", Namespace = "http://www.w3.org/XML/1998/namespace")]
    //    public string Base { get; set; }
    //    [XmlAttribute(AttributeName = "d", Namespace = "http://www.w3.org/2000/xmlns/")]
    //    public string D { get; set; }
    //    [XmlAttribute(AttributeName = "m", Namespace = "http://www.w3.org/2000/xmlns/")]
    //    public string M { get; set; }
    //}

}
