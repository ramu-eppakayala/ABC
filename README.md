# Randstad Payslip Generator

An ASP.NET WebForm application to generate payslips in PDF format similar to Randstad's payslip format.

## Features

- Interactive form to input employee details
- Auto-calculation of earnings based on Loss of Pay (LOP) days
- PDF generation with proper formatting
- Conversion of amounts to words
- Responsive layout matching the original payslip format

## Requirements

- Visual Studio 2019 or later
- .NET Framework 4.8
- iTextSharp (for PDF generation)
- BouncyCastle (dependency for iTextSharp)

## Setup Instructions

1. Clone or download this repository
2. Open the solution in Visual Studio
3. Restore NuGet packages to download the required dependencies
4. Build the solution
5. Place the Randstad logo (randstad-logo.png) in the Images folder
6. Run the application

## Usage

1. Fill in or modify the employee details
2. Adjust the earnings and deductions as needed
3. If there are Loss of Pay (LOP) days, enter them to automatically adjust the earnings
4. Click the "Generate PDF" button to generate and download the payslip

## Project Structure

- `Payslip.aspx` - Main WebForm UI
- `Payslip.aspx.cs` - Code-behind with business logic
- `Styles/PayslipStyles.css` - CSS styles for the payslip
- `Images/` - Folder for storing the company logo
- `Web.config` - Configuration file
- `packages.config` - NuGet package references

## Note

This is a template application and may need to be customized to match your exact requirements. The default values are set for demonstration purposes only. 