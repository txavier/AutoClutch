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

namespace ContractTrackingManagement.Core.Services.Tests
{
    [TestClass()]
    public class ImportService_ImportShould
    {
        /// <summary>
        /// http://stackoverflow.com/questions/16107697/multiline-string-variable
        /// </summary>
        [TestMethod()]
        public void ImportChangeOrder()
        {
            // Arrange.
            var autoMocker = new MoqAutoMocker<ImportService>();

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

            var xElement = XElement.Parse(xElementString);

            mockImportFileGetter.Setup(handler => handler.GetXML(It.IsAny<string>(), It.IsAny<string>())).Returns(xElement);

            var mockXmlValidatorService = Mock.Get(autoMocker.Get<IXmlValidatorService>());

            mockXmlValidatorService.Setup(handler => handler.IsValidXml(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var directory = @"R:\dev2\ContractTrackingManagement\ContractTrackingManagement\ContractTrackingManagement.Console\bin\Debug\access-contractdb-xml";

            // Act.
            importService.ImportChangeOrders(directory);

            // Assert.
            Assert.IsTrue(!importService.Errors.Any());
        }

        /// <summary>
        /// http://stackoverflow.com/questions/16107697/multiline-string-variable
        /// </summary>
        [TestMethod()]
        public void ImportDeductions()
        {
            // Arrange.
            var autoMocker = new MoqAutoMocker<ImportService>();

            var importService = autoMocker.ClassUnderTest;

            var mockImportFileGetter = Mock.Get(autoMocker.Get<IImportFilesGetter>());

            var xElementString = 
                            @"<dataroot xmlns:od='urn:schemas-microsoft-com:officedata' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'  xsi:noNamespaceSchemaLocation='Deductions.xsd' generated='2016-03-05T17:22:43'>
                                <Deductions>
                                <Contract_x0020_No>1389-HPB</Contract_x0020_No>
                                <Payment_x0020_No>1</Payment_x0020_No>
                                <Date>2015-03-09T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>2594.84</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1389-HPB - 20150319_154035.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1389-HPB</Contract_x0020_No>
                                <Payment_x0020_No>2</Payment_x0020_No>
                                <Date>2015-03-16T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>2594.84</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1389-HPB - 20150319_154059.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>2</Payment_x0020_No>
                                <Date>2015-04-23T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>2544.29</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20150423_140558.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1340-SLRC</Contract_x0020_No>
                                <Payment_x0020_No>2</Payment_x0020_No>
                                <Date>2015-03-02T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>28984</Amount>
                                <Memo></Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1337-BSN</Contract_x0020_No>
                                <Payment_x0020_No>6</Payment_x0020_No>
                                <Date>2015-06-09T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>3167.1</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1337-BSN - 20150610_101633.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>PW-86</Contract_x0020_No>
                                <Payment_x0020_No>8</Payment_x0020_No>
                                <Date>2014-03-21T00:00:00</Date>
                                <Deduction_x0020_Type>Line A Deduction</Deduction_x0020_Type>
                                <Amount>10344.95</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\PW-86 - 20150618_144325.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>PW-86</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2014-05-22T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>27969</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\PW-86 - 20150624_135930.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>3</Payment_x0020_No>
                                <Date>2015-07-07T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>2228.14</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20150708_124845.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>3</Payment_x0020_No>
                                <Date>2015-07-07T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>316.15</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20150708_125531.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>3</Payment_x0020_No>
                                <Date>2015-07-07T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>1570</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20150708_125603.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>3</Payment_x0020_No>
                                <Date>2014-09-17T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>4174.8</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_111857.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>4</Payment_x0020_No>
                                <Date>2015-04-21T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>7050</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_112233.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>8</Payment_x0020_No>
                                <Date>2015-05-28T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>164.37</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_114353.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>9</Payment_x0020_No>
                                <Date>2015-05-29T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>868.47</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_114941.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-09T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>500</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_115439.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-09T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>368.47</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_115510.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-09T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>164.37</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_125805.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-09T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>7050</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_125912.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>11</Payment_x0020_No>
                                <Date>2015-06-19T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>446.7</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20150716_131546.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>3</Payment_x0020_No>
                                <Date>2014-09-17T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>8012.6</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_083707.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>3</Payment_x0020_No>
                                <Date>2015-05-01T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>8564.03</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_090649.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>4</Payment_x0020_No>
                                <Date>2015-04-20T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>16146.6</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_091525.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>5</Payment_x0020_No>
                                <Date>2015-05-21T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>600</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_092621.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>6</Payment_x0020_No>
                                <Date>2015-05-21T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>13.8</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_093140.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>9</Payment_x0020_No>
                                <Date>2015-05-26T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>372.6</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_094421.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>4091.53</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095326.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>4472.5</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095350.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>6806.43</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095520.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>9340.17</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095541.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>600</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095620.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>13.8</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095701.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>187.6</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095743.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2015-06-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>185</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095849.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>11</Payment_x0020_No>
                                <Date>2015-06-17T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>135.08</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20150720_095945.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1268-FLT-R</Contract_x0020_No>
                                <Payment_x0020_No>1</Payment_x0020_No>
                                <Date>2014-10-08T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>25130.5</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1268-FLT-R - 20150722_101731.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1268-FLT-R</Contract_x0020_No>
                                <Payment_x0020_No>5</Payment_x0020_No>
                                <Date>2015-04-02T00:00:00</Date>
                                <Deduction_x0020_Type>Line A Deduction</Deduction_x0020_Type>
                                <Amount>23161</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1268-FLT-R - 20150722_102936.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1268-FLT-R</Contract_x0020_No>
                                <Payment_x0020_No>2</Payment_x0020_No>
                                <Date>2014-10-28T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>25130.5</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1268-FLT-R - 20150722_131411.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1330-VFD</Contract_x0020_No>
                                <Payment_x0020_No>1</Payment_x0020_No>
                                <Date>2014-11-07T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>4400</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1330-VFD - 20150730_120043.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1330-VFD</Contract_x0020_No>
                                <Payment_x0020_No>1</Payment_x0020_No>
                                <Date>2015-03-25T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>935</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1330-VFD - 20150730_120215.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1330-VFD</Contract_x0020_No>
                                <Payment_x0020_No>2</Payment_x0020_No>
                                <Date>2015-02-09T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>5854.2</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1330-VFD - 20150730_120305.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1330-VFD</Contract_x0020_No>
                                <Payment_x0020_No>2</Payment_x0020_No>
                                <Date>2015-06-30T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>5854.2</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1330-VFD - 20150730_120351.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1330-VFD</Contract_x0020_No>
                                <Payment_x0020_No>3</Payment_x0020_No>
                                <Date>2015-06-26T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>1195</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1330-VFD - 20150730_120700.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>4</Payment_x0020_No>
                                <Date>2015-08-12T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>5495</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20150812_113107.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>13</Payment_x0020_No>
                                <Date>2015-09-28T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>2508.37</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20151002_090402.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>13</Payment_x0020_No>
                                <Date>2015-10-06T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>8332.97</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20151019_094354.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>14</Payment_x0020_No>
                                <Date>2015-11-04T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>2327.01</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20151105_082506.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>14</Payment_x0020_No>
                                <Date>2015-11-04T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>181.36</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20151105_082551.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>14</Payment_x0020_No>
                                <Date>2015-10-30T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>2715.84</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20151105_082627.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>5</Payment_x0020_No>
                                <Date>2015-11-13T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>7065</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20151118_073358.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>14</Payment_x0020_No>
                                <Date>2015-11-20T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>71.75</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20151125_075544.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>14</Payment_x0020_No>
                                <Date>2015-11-20T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>8261.22</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20151125_075608.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1351-ACS</Contract_x0020_No>
                                <Payment_x0020_No>13</Payment_x0020_No>
                                <Date>2015-11-16T00:00:00</Date>
                                <Deduction_x0020_Type>Line A Deduction</Deduction_x0020_Type>
                                <Amount>216.5</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1351-ACS - 20151207_110730.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>15</Payment_x0020_No>
                                <Date>2015-12-14T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>923.95</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20151214_113359.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1352-ACS</Contract_x0020_No>
                                <Payment_x0020_No>15</Payment_x0020_No>
                                <Date>2015-12-14T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>1791.89</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1352-ACS - 20151214_113416.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>6</Payment_x0020_No>
                                <Date>2015-12-15T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>2714.2</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20151215_100520.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>PW-86</Contract_x0020_No>
                                <Payment_x0020_No>20</Payment_x0020_No>
                                <Date>2015-12-17T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>36795</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\PW-86 - 20151217_151010.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1325-CR</Contract_x0020_No>
                                <Payment_x0020_No>15</Payment_x0020_No>
                                <Date>2015-12-21T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>20540</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1325-CR - 20151224_093522.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1372-BRN</Contract_x0020_No>
                                <Payment_x0020_No>10</Payment_x0020_No>
                                <Date>2016-01-05T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>4250</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1372-BRN - 20160106_130059.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>PW-86</Contract_x0020_No>
                                <Payment_x0020_No>21</Payment_x0020_No>
                                <Date>2016-01-08T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Deduction</Deduction_x0020_Type>
                                <Amount>129250.8</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\PW-86 - 20160111_080516.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1372-BRN</Contract_x0020_No>
                                <Payment_x0020_No>11</Payment_x0020_No>
                                <Date>2016-02-03T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>4250</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1372-BRN - 20160203_110404.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>7</Payment_x0020_No>
                                <Date>2016-02-03T00:00:00</Date>
                                <Deduction_x0020_Type>Line H Release</Deduction_x0020_Type>
                                <Amount>40.7</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20160204_073338.pdf</Memo>
                                </Deductions>
                                <Deductions>
                                <Contract_x0020_No>1350-ACT</Contract_x0020_No>
                                <Payment_x0020_No>7</Payment_x0020_No>
                                <Date>2016-02-03T00:00:00</Date>
                                <Deduction_x0020_Type>Line H to Line A</Deduction_x0020_Type>
                                <Amount>2673.5</Amount>
                                <Memo>\\lfkbwtmis\Contracts-Tracking\Deductions\1350-ACT - 20160204_073406.pdf</Memo>
                                </Deductions>
                                </dataroot>";

            var xElement = XElement.Parse(xElementString);

            mockImportFileGetter.Setup(handler => handler.GetXML(It.IsAny<string>(), It.IsAny<string>())).Returns(xElement);

            var mockXmlValidatorService = Mock.Get(autoMocker.Get<IXmlValidatorService>());

            mockXmlValidatorService.Setup(handler => handler.IsValidXml(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var directory = @"R:\dev2\ContractTrackingManagement\ContractTrackingManagement\ContractTrackingManagement.Console\bin\Debug\access-contractdb-xml";

            // Act.
            importService.ImportDeductions(directory);

            // Assert.
            Assert.IsTrue(!importService.Errors.Any());
        }
    }
}