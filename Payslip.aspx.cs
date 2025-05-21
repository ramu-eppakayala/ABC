using System;
using System.IO;
using System.Web.UI;
using System.Drawing;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;


namespace PayslipGenerator
{
    public partial class Payslip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set default month as current month
                lblMonth.Text = DateTime.Now.ToString("MMMM yyyy");

                // Set default values for demonstration
                txtEmployeeCode.Text = "1612516";
                txtEmployeeName.Text = "Eppakayala Ramu";
                txtPFNumber.Text = "TNMSK03579100028756";
                txtWorkedDays.Text = "31.00";
                txtDateOfJoin.Text = "13/03/23";
                txtDesignation.Text = "Executive";
                txtLocation.Text = "SECUNDERABAD";
                txtESICNumber.Text = "5219545078";
                txtLOPDays.Text = "0";
                txtEPFUAN.Text = "101087146328";

                // Default earnings values
                txtBasicActual.Text = "12252.00";
                txtHRAActual.Text = "3473.00";
                txtBonusActual.Text = "1021.00";
                txtMidMonthActual.Text = "0.00";

                // Default deduction values
                txtPF.Text = "1470.00";
                txtPT.Text = "150.00";
                txtESI.Text = "126.00";
                txtMidMonthPaid.Text = "2500.00";

                // Default bank details
                txtBankName.Text = "STATE BANK OF INDIA";
                txtAccountNumber.Text = "38270961576";
                txtPaymentMethod.Text = "Fund Transfer";
                txtPaidDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

                // Calculate earnings and deductions
                Calculate_TotalEarnings(null, null);
                Calculate_TotalDeductions(null, null);
            }
        }

        // Recalculate earnings when any value changes
        protected void Calculate_TotalEarnings(object sender, EventArgs e)
        {
            // Get the total days in month
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            // Get LOP days
            decimal lopDays = 0;
            decimal.TryParse(txtLOPDays.Text, out lopDays);

            // Calculate effective work days
            decimal effectiveWorkDays = totalDays - lopDays;
            decimal workRatio = effectiveWorkDays / totalDays;

            // Calculate Basic
            decimal basicActual = 0;
            decimal.TryParse(txtBasicActual.Text, out basicActual);
            decimal basicEarned = basicActual * workRatio;
            lblBasicEarned.Text = basicEarned.ToString("0.00");

            // Calculate HRA
            decimal hraActual = 0;
            decimal.TryParse(txtHRAActual.Text, out hraActual);
            decimal hraEarned = hraActual * workRatio;
            lblHRAEarned.Text = hraEarned.ToString("0.00");

            // Calculate Bonus
            decimal bonusActual = 0;
            decimal.TryParse(txtBonusActual.Text, out bonusActual);
            decimal bonusEarned = bonusActual * workRatio;
            lblBonusEarned.Text = bonusEarned.ToString("0.00");

            // Get Mid month amount
            decimal midMonthAmount = 0;
            decimal.TryParse(txtMidMonthActual.Text, out midMonthAmount);
            lblMidMonthEarned.Text = midMonthAmount.ToString("2500.00");

            // Calculate total earnings
            decimal totalEarnings = basicEarned + hraEarned + bonusEarned + midMonthAmount;
            lblTotalEarnings.Text = totalEarnings.ToString("0.00");

            // Update Net Pay
            UpdateNetPay();
        }

        // Recalculate deductions when any value changes
        protected void Calculate_TotalDeductions(object sender, EventArgs e)
        {
            // Get PF amount
            decimal pfAmount = 0;
            decimal.TryParse(txtPF.Text, out pfAmount);

            // Get PT amount
            decimal ptAmount = 0;
            decimal.TryParse(txtPT.Text, out ptAmount);

            // Get ESI amount
            decimal esiAmount = 0;
            decimal.TryParse(txtESI.Text, out esiAmount);

            // Get Mid month paid amount
            decimal midMonthPaid = 0;
            decimal.TryParse(txtMidMonthPaid.Text, out midMonthPaid);

            // Calculate total deductions
            decimal totalDeductions = pfAmount + ptAmount + esiAmount + midMonthPaid;
            lblTotalDeductions.Text = totalDeductions.ToString("0.00");

            // Update Net Pay
            UpdateNetPay();
        }

        // Handle LOP days change
        protected void txtLOPDays_TextChanged(object sender, EventArgs e)
        {
            Calculate_TotalEarnings(sender, e);
        }

        // Update net pay and amount in words
        private void UpdateNetPay()
        {
            // Get total earnings
            decimal totalEarnings = 0;
            decimal.TryParse(lblTotalEarnings.Text, out totalEarnings);

            // Get total deductions
            decimal totalDeductions = 0;
            decimal.TryParse(lblTotalDeductions.Text, out totalDeductions);

            // Calculate net pay
            decimal netPay = totalEarnings - totalDeductions;
            lblNetPay.Text = netPay.ToString("0.00");

            // Set amount in words
            string amountInWords = ConvertToWords(Math.Floor(netPay));
            if (string.IsNullOrEmpty(amountInWords))
                amountInWords = "Zero";

            // First letter capitalization is already handled in the ConvertToWords method
            lblAmountInWords.Text = amountInWords + " Rupees Only";
        }

        // Generate PDF when button is clicked
        protected void btnGeneratePDF_Click(object sender, EventArgs e)
        {
            try
            {
                // Create PDF document
                Document pdfDoc = new Document(PageSize.A4, 15, 25, 25, 10);
                
                // Create border
                iTextSharp.text.Rectangle pageRect = pdfDoc.PageSize;
                float borderWidth = 2f;
                BaseColor borderColor = BaseColor.BLACK;

                // Adjust the rectangle coordinates to create a border
                float x = pageRect.Left;
                float y = pageRect.Bottom;
                float width = pageRect.Width - 2;
                float height = pageRect.Height - 2;

                // Create a memory stream to store the PDF
                MemoryStream ms = new MemoryStream();

                // Create PDF writer
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);

                // Open document
                pdfDoc.Open();

                // Add content to the document
                AddPdfContent(pdfDoc);

                // Close document
                pdfDoc.Close();

                // Set response content type
                Response.ContentType = "application/pdf";

                // Set file name for download
                Response.AddHeader("content-disposition", "attachment;filename=Payslip_" + lblMonth.Text.Replace(" ", "_") + ".pdf");

                // Write PDF to response
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

                // End response
                Response.End();
            }
            catch (Exception ex)
            {
                // Handle exception
                Response.Write("<script>alert('Error generating PDF: " + ex.Message + "');</script>");
            }
        }

        // Add content to PDF document
        private void AddPdfContent(Document pdfDoc)
        {
            // Set fonts
            iTextSharp.text.Font titleFont = FontFactory.GetFont("Tahoma", 14, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font headerFont = FontFactory.GetFont("Tahoma", 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font normalFont = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font boldFont = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font footerFont = FontFactory.GetFont("Tahoma", 8, iTextSharp.text.Font.NORMAL);

            try
            {
                // Add Logo (if available)
                string logoPath = Server.MapPath("~/Images/randstad-logo.png");
                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(150f, 50f);
                    logo.Alignment = Element.ALIGN_LEFT;
                    pdfDoc.Add(logo);
                }

                // Add company header
                Paragraph companyName = new Paragraph("Randstad India Private Limited", titleFont);
                companyName.Alignment = Element.ALIGN_RIGHT;
                pdfDoc.Add(companyName);

                Paragraph deputedAt = new Paragraph("Deputed at SBI Cards & Payment Services", normalFont);
                deputedAt.Alignment = Element.ALIGN_RIGHT;
                pdfDoc.Add(deputedAt);

                Paragraph payslipMonth = new Paragraph("Payslip for the Month of " + lblMonth.Text, normalFont);
                payslipMonth.Alignment = Element.ALIGN_RIGHT;
                payslipMonth.SpacingAfter = 20f;
                pdfDoc.Add(payslipMonth);

                // Add employee details section
                PdfPTable employeeTable = new PdfPTable(4);
                employeeTable.WidthPercentage = 100;
                employeeTable.SpacingAfter = 20f;
                employeeTable.SetWidths(new float[] { 20f, 30f, 20f, 30f });

                // Add employee details row by row
                AddCellPair(employeeTable, "Employee Code", txtEmployeeCode.Text, boldFont, normalFont);
                AddCellPair(employeeTable, "Designation", txtDesignation.Text, boldFont, normalFont);

                AddCellPair(employeeTable, "Employee Name", txtEmployeeName.Text, boldFont, normalFont);
                AddCellPair(employeeTable, "Location", txtLocation.Text, boldFont, normalFont);

                AddCellPair(employeeTable, "PF Number", txtPFNumber.Text, boldFont, normalFont);
                AddCellPair(employeeTable, "ESIC Number", txtESICNumber.Text, boldFont, normalFont);

                AddCellPair(employeeTable, "Worked Days", txtWorkedDays.Text, boldFont, normalFont);
                AddCellPair(employeeTable, "LOP Days", txtLOPDays.Text, boldFont, normalFont);

                AddCellPair(employeeTable, "Date Of Join", txtDateOfJoin.Text, boldFont, normalFont);
                AddCellPair(employeeTable, "EPF UAN", txtEPFUAN.Text, boldFont, normalFont);

                pdfDoc.Add(employeeTable);

                // Create tables for earnings and deductions (side by side)
                PdfPTable financialTable = new PdfPTable(2);
                financialTable.WidthPercentage = 100;
                financialTable.SpacingAfter = 20f;
                financialTable.SetWidths(new float[] { 60f, 40f });

                // Earnings section
                PdfPTable earningsTable = new PdfPTable(3);
                earningsTable.WidthPercentage = 100;
                earningsTable.SetWidths(new float[] { 40f, 30f, 30f });

                // Earnings header
                PdfPCell earningsHeaderCell = new PdfPCell(new Phrase("Earnings", headerFont));
                earningsHeaderCell.Colspan = 3;
                earningsHeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                earningsHeaderCell.BackgroundColor = new BaseColor(240, 240, 240);
                earningsHeaderCell.Padding = 5f;
                earningsTable.AddCell(earningsHeaderCell);

                // Earnings column headers
                earningsTable.AddCell(new Phrase("", boldFont));
                earningsTable.AddCell(new Phrase("Actual", boldFont));
                earningsTable.AddCell(new Phrase("Earned", boldFont));

                // Earnings rows
                AddEarningsRow(earningsTable, "Basic", txtBasicActual.Text, lblBasicEarned.Text, normalFont);
                AddEarningsRow(earningsTable, "HRA", txtHRAActual.Text, lblHRAEarned.Text, normalFont);
                AddEarningsRow(earningsTable, "Bonus", txtBonusActual.Text, lblBonusEarned.Text, normalFont);
                AddEarningsRow(earningsTable, "Mid month Paid", txtMidMonthActual.Text, lblMidMonthEarned.Text, normalFont);

                // Total earnings row
                AddEarningsRow(earningsTable, "Total Earnings", "", lblTotalEarnings.Text, boldFont);

                // Deductions section
                PdfPTable deductionsTable = new PdfPTable(2);
                deductionsTable.WidthPercentage = 100;
                deductionsTable.SetWidths(new float[] { 50f, 50f });

                // Deductions header
                PdfPCell deductionsHeaderCell = new PdfPCell(new Phrase("Deductions", headerFont));
                deductionsHeaderCell.Colspan = 2;
                deductionsHeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                deductionsHeaderCell.BackgroundColor = new BaseColor(240, 240, 240);
                deductionsHeaderCell.Padding = 5f;
                deductionsTable.AddCell(deductionsHeaderCell);

                // Deductions rows
                AddDeductionsRow(deductionsTable, "PF", txtPF.Text, normalFont);
                AddDeductionsRow(deductionsTable, "PT", txtPT.Text, normalFont);
                AddDeductionsRow(deductionsTable, "ESI", txtESI.Text, normalFont);
                AddDeductionsRow(deductionsTable, "Mid month Paid", txtMidMonthPaid.Text, normalFont);

                // Total deductions row
                AddDeductionsRow(deductionsTable, "Total Deductions", lblTotalDeductions.Text, boldFont);

                // Add earnings and deductions to financial table
                PdfPCell earningsCell = new PdfPCell(earningsTable);
                earningsCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                financialTable.AddCell(earningsCell);

                PdfPCell deductionsCell = new PdfPCell(deductionsTable);
                deductionsCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                financialTable.AddCell(deductionsCell);

                // Add financial table to document
                pdfDoc.Add(financialTable);

                // Net Pay section
                PdfPTable netPayTable = new PdfPTable(2);
                netPayTable.WidthPercentage = 100;
                netPayTable.SpacingAfter = 20f;
                netPayTable.SetWidths(new float[] { 30f, 70f });

                // Net Pay value
                AddCellPair(netPayTable, "Net Pay", lblNetPay.Text, boldFont, normalFont);

                // Amount in words
                AddCellPair(netPayTable, "Amount In Words", lblAmountInWords.Text, boldFont, normalFont);

                pdfDoc.Add(netPayTable);

                // Bank Details section
                PdfPTable bankTable = new PdfPTable(4);
                bankTable.WidthPercentage = 100;
                bankTable.SpacingAfter = 20f;
                bankTable.SetWidths(new float[] { 20f, 30f, 20f, 30f });

                // Add bank details
                AddCellPair(bankTable, "Bank Name", txtBankName.Text, boldFont, normalFont);
                AddCellPair(bankTable, "PaymentMethod", txtPaymentMethod.Text, boldFont, normalFont);

                AddCellPair(bankTable, "Account/Instrument Number", txtAccountNumber.Text, boldFont, normalFont);
                AddCellPair(bankTable, "Paid Date", txtPaidDate.Text, boldFont, normalFont);

                pdfDoc.Add(bankTable);

                // Footer
                Paragraph footer = new Paragraph("This is computer generated payslip and does not require signature and stamp.", footerFont);
                footer.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(footer);
            }
            catch (Exception ex)
            {
                // Add error message to PDF
                pdfDoc.Add(new Paragraph("Error generating content: " + ex.Message, normalFont));
            }
        }

        // Helper method to add label-value pairs to tables
        private void AddCellPair(PdfPTable table, string label, string value, iTextSharp.text.Font labelFont, iTextSharp.text.Font valueFont)
        {
            PdfPCell labelCell = new PdfPCell(new Phrase(label, labelFont));
            labelCell.BorderWidth = 0.5f;
            labelCell.Padding = 5f;
            table.AddCell(labelCell);

            PdfPCell valueCell = new PdfPCell(new Phrase(value, valueFont));
            valueCell.BorderWidth = 0.5f;
            valueCell.Padding = 5f;
            table.AddCell(valueCell);
        }

        // Helper method to add rows to earnings table
        private void AddEarningsRow(PdfPTable table, string label, string actual, string earned, iTextSharp.text.Font font)
        {
            table.AddCell(new Phrase(label, font));
            table.AddCell(new Phrase(actual, font));
            table.AddCell(new Phrase(earned, font));
        }

        // Helper method to add rows to deductions table
        private void AddDeductionsRow(PdfPTable table, string label, string value, iTextSharp.text.Font font)
        {
            table.AddCell(new Phrase(label, font));
            table.AddCell(new Phrase(value, font));
        }

        // Helper method to convert amount to words
        private string ConvertToWords(decimal amount)
        {
            if (amount == 0)
                return "Zero";

            if (amount < 0)
                return "Minus " + ConvertToWords(Math.Abs(amount));

            string[] units = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                             "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            string words = "";

            int millions = (int)(amount / 1000000);
            if (millions > 0)
            {
                words += ConvertNumberToWords(millions) + " Million ";
                amount %= 1000000;
            }

            int lakhs = (int)(amount / 100000);
            if (lakhs > 0)
            {
                words += ConvertNumberToWords(lakhs) + " Lakh ";
                amount %= 100000;
            }

            int thousands = (int)(amount / 1000);
            if (thousands > 0)
            {
                words += ConvertNumberToWords(thousands) + " Thousand ";
                amount %= 1000;
            }

            int hundreds = (int)(amount / 100);
            if (hundreds > 0)
            {
                words += ConvertNumberToWords(hundreds) + " Hundred ";
                amount %= 100;
            }

            if (amount > 0)
            {
                if (words != "")
                    words += "and ";

                if (amount < 20)
                    words += units[(int)amount];
                else
                {
                    words += tens[(int)amount / 10];
                    if ((amount % 10) > 0)
                        words += " " + units[(int)amount % 10];
                }
            }

            return words;
        }

        // Helper method to convert a simple number to words
        private string ConvertNumberToWords(int number)
        {
            string[] units = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                             "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            string result = "";

            if (number < 20)
                result = units[number];
            else
            {
                result = tens[number / 10];
                if ((number % 10) > 0)
                    result += " " + units[number % 10];
            }

            return result;
        }
    }
}