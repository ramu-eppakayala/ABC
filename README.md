# Generator

An ASP.NET WebForm application to PDF format similar to.

## Features

- Interactive form to input employee details
- PDF generation with proper formatting
- Conversion of amounts to words

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
6. Run the application

## Project Structure

- `Payslip.aspx` - Main WebForm UI
- `Payslip.aspx.cs` - Code-behind with business logic
- `Styles/PayslipStyles.css` - CSS styles for the payslip
- `Images/` - Folder for storing the company logo
- `Web.config` - Configuration file
- `packages.config` - NuGet package references

## Note

This is a template application and may need to be customized to match your exact requirements. The default values are set for demonstration purposes only. 
