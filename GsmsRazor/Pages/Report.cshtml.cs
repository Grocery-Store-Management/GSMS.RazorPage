using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjectLibrary;
using BusinessObjectLibrary.ViewModel;
using ClosedXML.Excel;
using DataAccessLibrary.BusinessEntity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace GsmsRazor.Pages
{
    [Authorize(Roles = "Store Owner")]
    public class ReportModel : PageModel
    {
        private readonly ReceiptBusinessEntity _receiptEntity;
        private readonly ImportOrderBusinessEntity _importOrderEntity;
        private readonly ReceiptDetailBusinessEntity _receiptDetailEntity;
        private readonly ImportOrderDetailBusinessEntity _importOrderDetailEntity;
        private readonly ProductBusinessEntity _productEntity;

        public ReportModel(IUnitOfWork work)
        {
            _receiptEntity = new ReceiptBusinessEntity(work);
            _importOrderEntity = new ImportOrderBusinessEntity(work);
            _receiptDetailEntity = new ReceiptDetailBusinessEntity(work);
            _importOrderDetailEntity = new ImportOrderDetailBusinessEntity(work);
            _productEntity = new ProductBusinessEntity(work);
        }

        public ChartJs RevenueChart { get; set; }
        public string RevenueChartJson { get; set; }

        public ChartJs ProductChart { get; set; }
        public string ProductChartJson { get; set; }

        public async Task<IActionResult> OnGetAsync(int? reportMonth, int? reportYear)
        {
            if (!reportMonth.HasValue)
            {
                reportMonth = DateTime.Now.Month;
            }
            if (!reportYear.HasValue)
            {
                reportYear = DateTime.Now.Year;
            }

            ViewData["periodFiveYearOfBusiness"] = new List<int> {
                DateTime.Now.Year - 5,
                DateTime.Now.Year - 4,
                DateTime.Now.Year - 3,
                DateTime.Now.Year - 2,
                DateTime.Now.Year - 1,
                DateTime.Now.Year
            };
            ViewData["reportMonth"] = reportMonth;
            ViewData["reportYear"] = reportYear;

            //Total revenue, expenditure, profit
            decimal totalRevenue = 0;
            decimal totalExpenditure = 0;
            decimal totalProfit = 0;
            IEnumerable<Receipt> receipts = await _receiptEntity.GetReceiptsBySaleMonthYearAsync(reportMonth.Value, reportYear.Value);
            List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
            foreach(Receipt receipt in receipts)
            {
                IEnumerable<ReceiptDetail> detailOfReceipt = await _receiptDetailEntity.GetReceiptDetailsByReceiptIdAsync(receipt.Id);
                receiptDetails.AddRange(detailOfReceipt);
            }
            foreach(ReceiptDetail receiptDetail in receiptDetails)
            {
                totalRevenue += receiptDetail.Price * receiptDetail.Quantity;
            }
            IEnumerable<ImportOrder> importOrders = await _importOrderEntity.GetImportOrdersBySaleMonthYearAsync(reportMonth.Value, reportYear.Value);
            List<ImportOrderDetail> importOrderDetails = new List<ImportOrderDetail>();
            foreach (ImportOrder importOrder in importOrders)
            {
                IEnumerable<ImportOrderDetail> detailOfImportOrder = await _importOrderDetailEntity.GetImportOrderDetailsByImportOrderIdAsync(importOrder.Id);
                importOrderDetails.AddRange(detailOfImportOrder);
            }
            foreach (ImportOrderDetail importOrderDetail in importOrderDetails)
            {
                totalExpenditure += importOrderDetail.Price * importOrderDetail.Quantity;
            }
            if(totalExpenditure < totalRevenue)
            {
                totalProfit = totalRevenue - totalExpenditure;
            }
            ViewData["totalRevenue"] = string.Format("{0:0}", totalRevenue);
            ViewData["totalExpenditure"] = string.Format("{0:0}", totalExpenditure);
            ViewData["totalProfit"] = string.Format("{0:0}", totalProfit);

            //Current month daily revenue (Bar chart)
            int days = DateTime.DaysInMonth(reportYear.Value, reportMonth.Value);
            Dictionary<string, int> dailyRevenue = new Dictionary<string, int>();
            IEnumerable<ReceiptDetail> tmpReceiptDetails = await _receiptDetailEntity.GetReceiptDetailsAsync();
            decimal tmpDailyRevenue;

            for (int i=1; i <= days; i++)
            {
                tmpDailyRevenue = _receiptDetailEntity.GetReceiptDailyRevenue(tmpReceiptDetails, i, reportMonth.Value, reportYear.Value);
                dailyRevenue.Add(i.ToString(), int.Parse(String.Format("{0:0}", tmpDailyRevenue)));
                tmpDailyRevenue = 0;
            }

            string revenueLabels = String.Join(",",dailyRevenue.Keys.ToArray());
            string revenueData = String.Join(",", dailyRevenue.Values.ToArray());
            var revenueChartData = @"
            {
                type: 'bar',
                responsive: true,
                data:
                {
                    labels: [" + revenueLabels + @"],
                    datasets: [{
                        label: 'Revenue (VND)',
                        data: [" + revenueData + @"],
                        backgroundColor: ['rgb(54, 162, 235)'],
                        borderColor: ['rgb(54, 162, 235)'],
                        borderWidth: 1
                    }]
                },
                options:
                {
                    scales:
                    {
                        yAxes: [{
                            ticks:
                            {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            }";

            RevenueChart = JsonConvert.DeserializeObject<ChartJs>(revenueChartData);
            RevenueChartJson = JsonConvert.SerializeObject(RevenueChart, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });

            //Products currently in stock (bar chart horizontal)
            IEnumerable<Product> products = await _productEntity.GetActiveProductsAsync();
            Product lastProduct = products.Last();
            string productLabels = "[";
            string productData = "";

            foreach(var product in products.ToArray())
            {
                if (!product.Equals(lastProduct))
                {
                    productLabels += "\"" + product.Name + "\",";
                    productData += product.StoredQuantity + ",";
                } else
                {
                    productLabels += "\"" + product.Name + "\"]";
                    productData += product.StoredQuantity;
                }
            }
            var productChartData = @"
            {
                type: 'bar',
                responsive: true,
                options:
                {
                    indexAxis: 'y',
                    elements: {
                        bar: {
                        borderWidth: 2,
                        }
                    }
                },
                data:
                {
                    labels: " + productLabels + @",
                    datasets: [{
                        label: 'Quantity',
                        data: [" + productData + @"],
                        backgroundColor: ['rgb(255, 99, 132)'],
                        borderColor: ['rgb(255, 99, 132)'],
                        borderWidth: 1
                    }]
                }
            }";

            ProductChart = JsonConvert.DeserializeObject<ChartJs>(productChartData);
            ProductChartJson = JsonConvert.SerializeObject(ProductChart, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });

            return Page();
        }

        public async Task<FileResult> OnPostExportRevenue(int exportMonth, int exportYear)
        {
            int days = DateTime.DaysInMonth(exportYear, exportMonth);
            Dictionary<string, int> dailyRevenue = new Dictionary<string, int>();
            IEnumerable<ReceiptDetail> tmpReceiptDetails = await _receiptDetailEntity.GetReceiptDetailsAsync();
            decimal tmpDailyRevenue;

            for (int i = 1; i <= days; i++)
            {
                tmpDailyRevenue = _receiptDetailEntity.GetReceiptDailyRevenue(tmpReceiptDetails, i, exportMonth, exportYear);
                dailyRevenue.Add(i.ToString(), int.Parse(String.Format("{0:0}", tmpDailyRevenue)));
                tmpDailyRevenue = 0;
            }

            DataTable dt = new DataTable("REVENUE_ " + exportMonth + "_" + exportYear);
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Date"),
                                    new DataColumn("Revenue (VND)") });

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt);
                int column = 1;
                int row = 2;
                foreach (var keyValue in dailyRevenue)
                {
                    ws.Cell(row, column).Value = keyValue.Key + "/" + exportMonth + "/" + exportYear;
                    ws.Cell(row, column + 1).Value = keyValue.Value;
                    row++;
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "REVENUE_" + exportMonth + "_" + exportYear + ".xlsx");
                }
            }
        }

        public async Task<FileResult> OnPostExportProduct()
        {
            IEnumerable<Product> products = await _productEntity.GetActiveProductsAsync();

            DataTable dt = new DataTable("PRODUCT_REPORT");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Product Name"),
                                    new DataColumn("Stored Quantity") });

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt);
                int column = 1;
                int row = 2;
                foreach(var product in products)
                {
                    ws.Cell(row, column).Value = product.Name;
                    ws.Cell(row, column + 1).Value = product.StoredQuantity;
                    row++;
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "PRODUCT_REPORT.xlsx");
                }
            }
        }
    }
}
