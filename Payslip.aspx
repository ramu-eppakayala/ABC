<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payslip.aspx.cs" Inherits="PayslipGenerator.Payslip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payslip Generator</title>
    <link href="Styles/PayslipStyles.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="payslipContainer">
            <!-- Header Section with Logo and Company Details -->
            <div id="headerSection">
                <div id="logoSection">
                    <img src="Images/randstad-logo.png" alt="Randstad Logo" />
                </div>
                <div id="companyDetailsSection">
                    <h2>Randstad India Private Limited</h2>
                    <p>Deputed at SBI Cards & Payment Services</p>
                    <p>Payslip for the Month of <asp:Label ID="lblMonth" runat="server"></asp:Label></p>
                </div>
            </div>

            <!-- Employee Details Section -->
            <div id="employeeSection">
                <table>
                    <tr>
                        <td class="label">Employee Code</td>
                        <td><asp:TextBox ID="txtEmployeeCode" runat="server"></asp:TextBox></td>
                        <td class="label">Designation</td>
                        <td><asp:TextBox ID="txtDesignation" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="label">Employee Name</td>
                        <td><asp:TextBox ID="txtEmployeeName" runat="server"></asp:TextBox></td>
                        <td class="label">Location</td>
                        <td><asp:TextBox ID="txtLocation" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="label">PF Number</td>
                        <td><asp:TextBox ID="txtPFNumber" runat="server"></asp:TextBox></td>
                        <td class="label">ESIC Number</td>
                        <td><asp:TextBox ID="txtESICNumber" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="label">Worked Days</td>
                        <td><asp:TextBox ID="txtWorkedDays" runat="server" Text="31.00"></asp:TextBox></td>
                        <td class="label">LOP Days</td>
                        <td><asp:TextBox ID="txtLOPDays" runat="server" Text="0" AutoPostBack="true" OnTextChanged="txtLOPDays_TextChanged"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="label">Date Of Join</td>
                        <td><asp:TextBox ID="txtDateOfJoin" runat="server"></asp:TextBox></td>
                        <td class="label">EPF UAN</td>
                        <td><asp:TextBox ID="txtEPFUAN" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
            </div>

            <!-- Earnings and Deductions Section -->
            <div id="financialSection">
                <div id="earningsSection">
                    <h3>Earnings</h3>
                    <table>
                        <tr>
                            <th></th>
                            <th>Actual</th>
                            <th>Earned</th>
                        </tr>
                        <tr>
                            <td class="label">Basic</td>
                            <td><asp:TextBox ID="txtBasicActual" runat="server" AutoPostBack="true" OnTextChanged="Calculate_TotalEarnings"></asp:TextBox></td>
                            <td><asp:Label ID="lblBasicEarned" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label">HRA</td>
                            <td><asp:TextBox ID="txtHRAActual" runat="server" AutoPostBack="true" OnTextChanged="Calculate_TotalEarnings"></asp:TextBox></td>
                            <td><asp:Label ID="lblHRAEarned" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label">Bonus</td>
                            <td><asp:TextBox ID="txtBonusActual" runat="server" AutoPostBack="true" OnTextChanged="Calculate_TotalEarnings"></asp:TextBox></td>
                            <td><asp:Label ID="lblBonusEarned" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="label">Mid month Paid</td>
                            <td><asp:TextBox ID="txtMidMonthActual" runat="server" Text="0.00" AutoPostBack="true" OnTextChanged="Calculate_TotalEarnings"></asp:TextBox></td>
                            <td><asp:Label ID="lblMidMonthEarned" runat="server"></asp:Label></td>
                        </tr>
                        <tr class="totalRow">
                            <td class="label">Total Earnings</td>
                            <td></td>
                            <td><asp:Label ID="lblTotalEarnings" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>

                <div id="deductionsSection">
                    <h3>Deductions</h3>
                    <table>
                        <tr>
                            <td class="label">PF</td>
                            <td><asp:TextBox ID="txtPF" runat="server" AutoPostBack="true" OnTextChanged="Calculate_TotalDeductions"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="label">PT</td>
                            <td><asp:TextBox ID="txtPT" runat="server" AutoPostBack="true" OnTextChanged="Calculate_TotalDeductions"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="label">ESI</td>
                            <td><asp:TextBox ID="txtESI" runat="server" AutoPostBack="true" OnTextChanged="Calculate_TotalDeductions"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="label">Mid month Paid</td>
                            <td><asp:TextBox ID="txtMidMonthPaid" runat="server" AutoPostBack="true" OnTextChanged="Calculate_TotalDeductions"></asp:TextBox></td>
                        </tr>
                        <tr class="totalRow">
                            <td class="label">Total Deductions</td>
                            <td><asp:Label ID="lblTotalDeductions" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>
            </div>

            <!-- Net Pay Section -->
            <div id="netPaySection">
                <table>
                    <tr>
                        <td class="label">Net Pay</td>
                        <td><asp:Label ID="lblNetPay" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="label">Amount In Words</td>
                        <td><asp:Label ID="lblAmountInWords" runat="server"></asp:Label></td>
                    </tr>
                </table>
            </div>

            <!-- Bank Details Section -->
            <div id="bankDetailsSection">
                <table>
                    <tr>
                        <td class="label">Bank Name</td>
                        <td><asp:TextBox ID="txtBankName" runat="server"></asp:TextBox></td>
                        <td class="label">PaymentMethod</td>
                        <td><asp:TextBox ID="txtPaymentMethod" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="label">Account/Instrument Number</td>
                        <td><asp:TextBox ID="txtAccountNumber" runat="server"></asp:TextBox></td>
                        <td class="label">Paid Date</td>
                        <td><asp:TextBox ID="txtPaidDate" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
            </div>

            <!-- Footer Section -->
            <div id="footerSection">
                <p>This is computer generated payslip and does not require signature and stamp.</p>
            </div>

            <!-- Generate PDF Button -->
            <div id="actionSection">
                <asp:Button ID="btnGeneratePDF" runat="server" Text="Generate PDF" OnClick="btnGeneratePDF_Click" CssClass="btn-generate" />
            </div>
        </div>
    </form>
</body>
</html> 