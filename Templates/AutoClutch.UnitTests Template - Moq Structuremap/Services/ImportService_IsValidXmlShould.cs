using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap.AutoMocking.Moq;
using Moq;
using ContractTrackingManagement.Core.Interfaces;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;
using System.IO;

namespace ContractTrackingManagement.Core.Services.Tests
{
    [TestClass()]
    public class ImportService_IsValidXmlShould
    {
        [TestMethod()]
        public void ValidateXml()
        {
            // Arrange.
            var autoMocker = new MoqAutoMocker<XmlValidatorService>();

            var importService = autoMocker.ClassUnderTest;

            var mockImportFileGetter = Mock.Get(autoMocker.Get<IImportFilesGetter>());

            var xElementString = "<dataroot xmlns:od=\"urn:schemas-microsoft-com:officedata\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"Change%20Order.xsd\" generated=\"2016-03-05T17:18:46\">" +
                           @"<Change_x0020_Order>
                            <Contract_x0020_No>1325-CR</Contract_x0020_No>
                            <Change_x0020_Order_x0020_Type>Overrun</Change_x0020_Order_x0020_Type>
                            <Change_x0020_Order_x0020_No>1</Change_x0020_Order_x0020_No>
                            <Description>Overrun for Contract Bid Items 1, 11, 12 and 15</Description>
                            <Reason_x0020_for_x0020_Change_x0020_Order>
                            Absence of south region contract has put burden of repair and inspections on north region contract.
                            </Reason_x0020_for_x0020_Change_x0020_Order>
                            <Identification_x0020_Date>2015-01-17T00:00:00</Identification_x0020_Date>
                            <Engineer_x0027_s_x0020_Estimate>483000</Engineer_x0027_s_x0020_Estimate>
                            <Proposal_x0020_Date>2014-01-17T00:00:00</Proposal_x0020_Date>
                            <Proposal_x0020_Amount>483000</Proposal_x0020_Amount>
                            <BWT_x0020_Approval_x0020_Date>2014-01-31T00:00:00</BWT_x0020_Approval_x0020_Date>
                            <BWT_x0020_Approval_x0020_Amount>483000</BWT_x0020_Approval_x0020_Amount>
                            <Registered_x0020_Amount>483000</Registered_x0020_Amount>
                            <Registered>2014-03-05T00:00:00</Registered>
                            <MemoPDF>
                            \\lfkbwtmis\Contracts-Tracking\RegisteredCO\1325-CR - OV# 1 - 20150403_075658.pdf
                            </MemoPDF>
                            </Change_x0020_Order>
                            <Change_x0020_Order>
                            <Contract_x0020_No>1325-CR</Contract_x0020_No>
                            <Change_x0020_Order_x0020_Type>Change Order (Parts and Materials)</Change_x0020_Order_x0020_Type>
                            <Change_x0020_Order_x0020_No>1</Change_x0020_Order_x0020_No>
                            <Description>Part and material</Description>
                            <Reason_x0020_for_x0020_Change_x0020_Order>Need parts for additional repairs</Reason_x0020_for_x0020_Change_x0020_Order>
                            <Identification_x0020_Date>2014-12-30T00:00:00</Identification_x0020_Date>
                            <Engineer_x0027_s_x0020_Estimate>50000</Engineer_x0027_s_x0020_Estimate>
                            <Proposal_x0020_Date>2014-12-30T00:00:00</Proposal_x0020_Date>
                            <Proposal_x0020_Amount>50000</Proposal_x0020_Amount>
                            <BWT_x0020_Approval_x0020_Date>2014-12-30T00:00:00</BWT_x0020_Approval_x0020_Date>
                            <BWT_x0020_Approval_x0020_Amount>50000</BWT_x0020_Approval_x0020_Amount>
                            <Registered_x0020_Amount>50000</Registered_x0020_Amount>
                            <Registered>2015-04-07T00:00:00</Registered>
                            <MemoPDF>
                            \\lfkbwtmis\Contracts-Tracking\RegisteredCO\1325-CR - CO# 1 - 20150806_110945.pdf
                            </MemoPDF>
                            </Change_x0020_Order>
                            </dataroot>";

            var xdoc = XDocument.Parse(xElementString);

            mockImportFileGetter.Setup(handler => handler.GetXDocument(It.IsAny<string>())).Returns(xdoc);

            string xsdMarkup =
                @"<?xml version='1.0' encoding='UTF-8'?>
                    <xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:od='urn:schemas-microsoft-com:officedata'>
                    <xsd:element name='dataroot'>
                    <xsd:complexType>
                    <xsd:sequence>
                    <xsd:element ref='Change_x0020_Order' minOccurs='0' maxOccurs='unbounded'/>
                    </xsd:sequence>
                    <xsd:attribute name='generated' type='xsd:dateTime'/>
                    </xsd:complexType>
                    </xsd:element>
                    <xsd:element name='Change_x0020_Order'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:index index-name='BWT Approval Date' index-key='Registered ' primary='no' unique='no' clustered='no' order='asc'/>
                    <od:index index-name='CO TypeChange Order' index-key='Change_x0020_Order_x0020_Type ' primary='no' unique='no' clustered='no' order='asc'/>
                    <od:index index-name='Identification Date1' index-key='Proposal_x0020_Date ' primary='no' unique='no' clustered='no' order='asc'/>
                    <od:index index-name='Proposal Date' index-key='BWT_x0020_Approval_x0020_Date ' primary='no' unique='no' clustered='no' order='asc'/>
                    <od:tableProperty name='PublishToWeb' type='2' value='1'/>
                    <od:tableProperty name='Orientation' type='2' value='0'/>
                    <od:tableProperty name='OrderByOn' type='1' value='0'/>
                    <od:tableProperty name='DefaultView' type='2' value='2'/>
                    <od:tableProperty name='GUID' type='9' value='7s9wtv14EE+IrxS8yk+iOA==
                    '/>
                    <od:tableProperty name='DisplayViewsOnSharePointSite' type='2' value='1'/>
                    <od:tableProperty name='TotalsRow' type='1' value='0'/>
                    <od:tableProperty name='FilterOnLoad' type='1' value='0'/>
                    <od:tableProperty name='OrderByOnLoad' type='1' value='1'/>
                    <od:tableProperty name='HideNewField' type='1' value='0'/>
                    <od:tableProperty name='BackTint' type='6' value='100'/>
                    <od:tableProperty name='BackShade' type='6' value='100'/>
                    <od:tableProperty name='ThemeFontIndex' type='4' value='1'/>
                    <od:tableProperty name='AlternateBackThemeColorIndex' type='4' value='1'/>
                    <od:tableProperty name='AlternateBackTint' type='6' value='100'/>
                    <od:tableProperty name='AlternateBackShade' type='6' value='95'/>
                    <od:tableProperty name='DatasheetGridlinesThemeColorIndex' type='4' value='3'/>
                    <od:tableProperty name='DatasheetForeThemeColorIndex' type='4' value='0'/>
                    <od:tableProperty name='Filter' type='12' value='([Change Order].[Contract No] In (&quot;20131404743&quot;,&quot;20131406834&quot;))'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:complexType>
                    <xsd:sequence>
                    <xsd:element name='Contract_x0020_No' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='1'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='Change_x0020_Order_x0020_Type' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='CO_x0020_Value' minOccurs='0' od:jetType='longinteger' od:sqlSType='int' type='xsd:int'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Change_x0020_Order_x0020_No' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='Description' minOccurs='1' od:jetType='text' od:sqlSType='nvarchar' od:nonNullable='yes'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='1'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='Reason_x0020_for_x0020_Change_x0020_Order' minOccurs='1' od:jetType='memo' od:sqlSType='ntext' od:nonNullable='yes'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='1'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextFormat' type='2' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AppendOnly' type='1' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='536870910'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='Identification_x0020_Date' minOccurs='1' od:jetType='datetime' od:sqlSType='datetime' od:nonNullable='yes' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='1'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Engineer_x0027_s_x0020_Estimate' minOccurs='0' od:jetType='currency' od:sqlSType='money' type='xsd:double'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Format' type='10' value='$#,##0.00;($#,##0.00)'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='1033'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Extension_x0020_Date' minOccurs='0' od:jetType='datetime' od:sqlSType='datetime' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Proposal_x0020_Date' minOccurs='0' od:jetType='datetime' od:sqlSType='datetime' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Proposal_x0020_Amount' minOccurs='0' od:jetType='currency' od:sqlSType='money' type='xsd:double'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Format' type='10' value='$#,##0.00;($#,##0.00)'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='1033'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='BWT_x0020_Approval_x0020_Date' minOccurs='0' od:jetType='datetime' od:sqlSType='datetime' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='BWT_x0020_Approval_x0020_Amount' minOccurs='0' od:jetType='currency' od:sqlSType='money' type='xsd:double'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Format' type='10' value='$#,##0.00;($#,##0.00)'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='1033'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Registered_x0020_Amount' minOccurs='0' od:jetType='currency' od:sqlSType='money' type='xsd:double'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Format' type='10' value='$#,##0.00;($#,##0.00)'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='1033'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Registered' minOccurs='0' od:jetType='datetime' od:sqlSType='datetime' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Comments' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='MemoPDF' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    </xsd:sequence>
                    </xsd:complexType>
                    </xsd:element>
                    </xsd:schema>";

            var schemas = new XmlSchemaSet();

            schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));

            mockImportFileGetter.Setup(handler => handler.GetXmlSchema(It.IsAny<string>())).Returns(schemas);

            // Act.
            var valid = importService.IsValidXml("mockXmlFilePath", "mockXsdFilePath");

            // Assert.
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void NotValidateInvalidXml()
        {
            // Arrange.
            var autoMocker = new MoqAutoMocker<XmlValidatorService>();

            var importService = autoMocker.ClassUnderTest;

            var mockImportFileGetter = Mock.Get(autoMocker.Get<IImportFilesGetter>());

            // Changed 'Contract_x0020_No' to 'Contract_x0020_N' to test to see if this would not validate.
            var xElementString = "<dataroot xmlns:od=\"urn:schemas-microsoft-com:officedata\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"Change%20Order.xsd\" generated=\"2016-03-05T17:18:46\">" +
                           @"<Change_x0020_Order>
                            <Contract_x0020_N>1325-CR</Contract_x0020_N>
                            <Change_x0020_Order_x0020_Type>Overrun</Change_x0020_Order_x0020_Type>
                            <Change_x0020_Order_x0020_No>1</Change_x0020_Order_x0020_No>
                            <Description>Overrun for Contract Bid Items 1, 11, 12 and 15</Description>
                            <Reason_x0020_for_x0020_Change_x0020_Order>
                            Absence of south region contract has put burden of repair and inspections on north region contract.
                            </Reason_x0020_for_x0020_Change_x0020_Order>
                            <Identification_x0020_Date>2015-01-17T00:00:00</Identification_x0020_Date>
                            <Engineer_x0027_s_x0020_Estimate>483000</Engineer_x0027_s_x0020_Estimate>
                            <Proposal_x0020_Date>2014-01-17T00:00:00</Proposal_x0020_Date>
                            <Proposal_x0020_Amount>483000</Proposal_x0020_Amount>
                            <BWT_x0020_Approval_x0020_Date>2014-01-31T00:00:00</BWT_x0020_Approval_x0020_Date>
                            <BWT_x0020_Approval_x0020_Amount>483000</BWT_x0020_Approval_x0020_Amount>
                            <Registered_x0020_Amount>483000</Registered_x0020_Amount>
                            <Registered>2014-03-05T00:00:00</Registered>
                            <MemoPDF>
                            \\lfkbwtmis\Contracts-Tracking\RegisteredCO\1325-CR - OV# 1 - 20150403_075658.pdf
                            </MemoPDF>
                            </Change_x0020_Order>
                            <Change_x0020_Order>
                            <Contract_x0020_No>1325-CR</Contract_x0020_No>
                            <Change_x0020_Order_x0020_Type>Change Order (Parts and Materials)</Change_x0020_Order_x0020_Type>
                            <Change_x0020_Order_x0020_No>1</Change_x0020_Order_x0020_No>
                            <Description>Part and material</Description>
                            <Reason_x0020_for_x0020_Change_x0020_Order>Need parts for additional repairs</Reason_x0020_for_x0020_Change_x0020_Order>
                            <Identification_x0020_Date>2014-12-30T00:00:00</Identification_x0020_Date>
                            <Engineer_x0027_s_x0020_Estimate>50000</Engineer_x0027_s_x0020_Estimate>
                            <Proposal_x0020_Date>2014-12-30T00:00:00</Proposal_x0020_Date>
                            <Proposal_x0020_Amount>50000</Proposal_x0020_Amount>
                            <BWT_x0020_Approval_x0020_Date>2014-12-30T00:00:00</BWT_x0020_Approval_x0020_Date>
                            <BWT_x0020_Approval_x0020_Amount>50000</BWT_x0020_Approval_x0020_Amount>
                            <Registered_x0020_Amount>50000</Registered_x0020_Amount>
                            <Registered>2015-04-07T00:00:00</Registered>
                            <MemoPDF>
                            \\lfkbwtmis\Contracts-Tracking\RegisteredCO\1325-CR - CO# 1 - 20150806_110945.pdf
                            </MemoPDF>
                            </Change_x0020_Order>
                            </dataroot>";

            var xdoc = XDocument.Parse(xElementString);

            mockImportFileGetter.Setup(handler => handler.GetXDocument(It.IsAny<string>())).Returns(xdoc);

            string xsdMarkup =
                @"<?xml version='1.0' encoding='UTF-8'?>
                    <xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:od='urn:schemas-microsoft-com:officedata'>
                    <xsd:element name='dataroot'>
                    <xsd:complexType>
                    <xsd:sequence>
                    <xsd:element ref='Change_x0020_Order' minOccurs='0' maxOccurs='unbounded'/>
                    </xsd:sequence>
                    <xsd:attribute name='generated' type='xsd:dateTime'/>
                    </xsd:complexType>
                    </xsd:element>
                    <xsd:element name='Change_x0020_Order'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:index index-name='BWT Approval Date' index-key='Registered ' primary='no' unique='no' clustered='no' order='asc'/>
                    <od:index index-name='CO TypeChange Order' index-key='Change_x0020_Order_x0020_Type ' primary='no' unique='no' clustered='no' order='asc'/>
                    <od:index index-name='Identification Date1' index-key='Proposal_x0020_Date ' primary='no' unique='no' clustered='no' order='asc'/>
                    <od:index index-name='Proposal Date' index-key='BWT_x0020_Approval_x0020_Date ' primary='no' unique='no' clustered='no' order='asc'/>
                    <od:tableProperty name='PublishToWeb' type='2' value='1'/>
                    <od:tableProperty name='Orientation' type='2' value='0'/>
                    <od:tableProperty name='OrderByOn' type='1' value='0'/>
                    <od:tableProperty name='DefaultView' type='2' value='2'/>
                    <od:tableProperty name='GUID' type='9' value='7s9wtv14EE+IrxS8yk+iOA==
                    '/>
                    <od:tableProperty name='DisplayViewsOnSharePointSite' type='2' value='1'/>
                    <od:tableProperty name='TotalsRow' type='1' value='0'/>
                    <od:tableProperty name='FilterOnLoad' type='1' value='0'/>
                    <od:tableProperty name='OrderByOnLoad' type='1' value='1'/>
                    <od:tableProperty name='HideNewField' type='1' value='0'/>
                    <od:tableProperty name='BackTint' type='6' value='100'/>
                    <od:tableProperty name='BackShade' type='6' value='100'/>
                    <od:tableProperty name='ThemeFontIndex' type='4' value='1'/>
                    <od:tableProperty name='AlternateBackThemeColorIndex' type='4' value='1'/>
                    <od:tableProperty name='AlternateBackTint' type='6' value='100'/>
                    <od:tableProperty name='AlternateBackShade' type='6' value='95'/>
                    <od:tableProperty name='DatasheetGridlinesThemeColorIndex' type='4' value='3'/>
                    <od:tableProperty name='DatasheetForeThemeColorIndex' type='4' value='0'/>
                    <od:tableProperty name='Filter' type='12' value='([Change Order].[Contract No] In (&quot;20131404743&quot;,&quot;20131406834&quot;))'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:complexType>
                    <xsd:sequence>
                    <xsd:element name='Contract_x0020_No' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='1'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='Change_x0020_Order_x0020_Type' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='CO_x0020_Value' minOccurs='0' od:jetType='longinteger' od:sqlSType='int' type='xsd:int'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Change_x0020_Order_x0020_No' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='Description' minOccurs='1' od:jetType='text' od:sqlSType='nvarchar' od:nonNullable='yes'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='1'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='Reason_x0020_for_x0020_Change_x0020_Order' minOccurs='1' od:jetType='memo' od:sqlSType='ntext' od:nonNullable='yes'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='1'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextFormat' type='2' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AppendOnly' type='1' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='536870910'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='Identification_x0020_Date' minOccurs='1' od:jetType='datetime' od:sqlSType='datetime' od:nonNullable='yes' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='1'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Engineer_x0027_s_x0020_Estimate' minOccurs='0' od:jetType='currency' od:sqlSType='money' type='xsd:double'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Format' type='10' value='$#,##0.00;($#,##0.00)'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='1033'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Extension_x0020_Date' minOccurs='0' od:jetType='datetime' od:sqlSType='datetime' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Proposal_x0020_Date' minOccurs='0' od:jetType='datetime' od:sqlSType='datetime' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Proposal_x0020_Amount' minOccurs='0' od:jetType='currency' od:sqlSType='money' type='xsd:double'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Format' type='10' value='$#,##0.00;($#,##0.00)'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='1033'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='BWT_x0020_Approval_x0020_Date' minOccurs='0' od:jetType='datetime' od:sqlSType='datetime' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='BWT_x0020_Approval_x0020_Amount' minOccurs='0' od:jetType='currency' od:sqlSType='money' type='xsd:double'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Format' type='10' value='$#,##0.00;($#,##0.00)'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='1033'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Registered_x0020_Amount' minOccurs='0' od:jetType='currency' od:sqlSType='money' type='xsd:double'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Format' type='10' value='$#,##0.00;($#,##0.00)'/>
                    <od:fieldProperty name='DecimalPlaces' type='2' value='255'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='1033'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Registered' minOccurs='0' od:jetType='datetime' od:sqlSType='datetime' type='xsd:dateTime'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ShowDatePicker' type='3' value='1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    </xsd:element>
                    <xsd:element name='Comments' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    <xsd:element name='MemoPDF' minOccurs='0' od:jetType='text' od:sqlSType='nvarchar'>
                    <xsd:annotation>
                    <xsd:appinfo>
                    <od:fieldProperty name='ColumnWidth' type='3' value='-1'/>
                    <od:fieldProperty name='ColumnOrder' type='3' value='0'/>
                    <od:fieldProperty name='ColumnHidden' type='1' value='0'/>
                    <od:fieldProperty name='Required' type='1' value='0'/>
                    <od:fieldProperty name='AllowZeroLength' type='1' value='1'/>
                    <od:fieldProperty name='DisplayControl' type='3' value='109'/>
                    <od:fieldProperty name='IMEMode' type='2' value='0'/>
                    <od:fieldProperty name='IMESentenceMode' type='2' value='3'/>
                    <od:fieldProperty name='UnicodeCompression' type='1' value='1'/>
                    <od:fieldProperty name='TextAlign' type='2' value='0'/>
                    <od:fieldProperty name='AggregateType' type='4' value='-1'/>
                    <od:fieldProperty name='ResultType' type='2' value='0'/>
                    <od:fieldProperty name='CurrencyLCID' type='4' value='0'/>
                    </xsd:appinfo>
                    </xsd:annotation>
                    <xsd:simpleType>
                    <xsd:restriction base='xsd:string'>
                    <xsd:maxLength value='255'/>
                    </xsd:restriction>
                    </xsd:simpleType>
                    </xsd:element>
                    </xsd:sequence>
                    </xsd:complexType>
                    </xsd:element>
                    </xsd:schema>";

            var schemas = new XmlSchemaSet();

            schemas.Add("", XmlReader.Create(new StringReader(xsdMarkup)));

            mockImportFileGetter.Setup(handler => handler.GetXmlSchema(It.IsAny<string>())).Returns(schemas);

            // Act.
            var valid = importService.IsValidXml("mockXmlFilePath", "mockXsdFilePath");

            // Assert.
            // Changed 'Contract_x0020_No' to 'Contract_x0020_N' to test to see if this would not validate.
            Assert.IsFalse(valid);
        }
    }
}