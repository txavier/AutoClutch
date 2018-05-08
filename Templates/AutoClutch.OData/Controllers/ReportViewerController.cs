using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class ReportViewerController : Controller
    {
        // GET: ReportViewer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KPI()
        {
            return View();
        }
        public ActionResult WorkOrderOver90Days()
        {
            return View();
        }
        public ActionResult ContractReportFull()
        {
            return View();
        }

        public ActionResult WorkOrderDetail()
        {
            return View();
        }

        public ActionResult CMWorkOrderContractSummary()
        {
            return View();
        }

        public ActionResult HighestCMWorkOrderCategory()
        {
            return View();
        }
        public ActionResult CMWorkOrderLocation()
        {
            return View();
        }

        public ActionResult MonthlyProduction()
        {
            return View();
        }

        public ActionResult WorkOrderSummary()
        {
            return View();
        }

        public ActionResult WorkOrderSummaryLocation()
        {
            return View();
        }

        public ActionResult WorkOrderRepairCostLocation()
        {
            return View();
        }

        public ActionResult WorkOrderDurationAverage()
        {
            return View();
        }
        public ActionResult ProductionReqContracts()
        {
            return View();
        }

        public ActionResult SpendingCost()
        {
            return View();
        }

        public ActionResult ActiveCMWorkOrder()
        {
            return View();
        }

        public ActionResult CMWorkOrderCompletionDuration()
        {
            return View();
        }

        public ActionResult ContractStatus()
        {
            return View();
        }
        // GET: ReportViewer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReportViewer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportViewer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportViewer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReportViewer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportViewer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReportViewer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
