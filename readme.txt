Municipal Services Application (WinForms, .NET Framework 4.8)

Overview
--------
This app is a basic municipal-focused Windows Forms application based on the South African context, where residents are able to report issues like Sanitation, Roads or Utilities problems. There is a gamification element included in the app to encourage engagement (points & progress bar)

Project Structure
-----------------
- MunicipalServicesApp.csproj
- Program.cs
- MainMenuForm.cs / MainMenuForm.Designer.cs
- ReportIssuesForm.cs / ReportIssuesForm.Designer.cs
- Issue.cs
- IssueManager.cs

Requirements
------------
- Windows
- Visual Studio 2019 (or later) with .NET Framework 4.8 developer pack installed

How to Build
------------
1. Open Visual Studio.
2. File > Open > Project/Solution...
3. Select MunicipalServicesApp.csproj in this folder.
4. Build > Build Solution (Ctrl+Shift+B). The project should compile without errors.

How to Run
----------
1. Press F5 (Start Debugging) or Ctrl+F5 (Start Without Debugging).
2. Alternatively, run the built executable from bin\Debug\MunicipalServicesApp.exe after a successful build.

How to Use
----------
1. Main Menu:
   - The startup screen will display three options. Only "Report Issues" is enabled at this stage.
   - Click "Report Issues" to open the reporting form.


2. Report an Issue:
   - Enter "Location" (e.g., "Cape Town CBD", "Durban North", "Ward 23, Tshwane").
   - Select "Category": Sanitation (e.g., refuse collection, sewage leaks), Roads (e.g., potholes, traffic lights), Utilities (e.g., water outages, electricity faults).
   - Enter a detailed "Description" of the issue.
   - Optionally click the "Attach Media" button to add a photo (JPG/PNG) or document (PDF/DOCX). A confirmation will display the selected file name.
   - A progress bar fills by 25% for each completed part: Location, Category, Description, Attachment.
   - Click "Submit" to validate and send your report. You'll receive a success message with a Tracking ID (GUID).
   - Gamification: Each successful report earns 10 points. Your total amount of points will be shown in the encouragement label.
   - Click "Back to Main Menu" to return.

Data Storage
------------
Reports are stored in memory only during the application's lifetime using IssueManager.ReportedIssues. No database is used.

Notes
-----
- The two other main menu options (Local Events and Announcements, Service Request Status) are disabled in this version.
- The UI follows a consistent light blue theme with blue text buttons.

Support
-------
If you encounter issues building or running the app, ensure the .NET Framework 4.8 Developer Pack is installed and retry.



