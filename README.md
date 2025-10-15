# Western Cape Municipal Services Portal

The Western Cape Municipal Services Portal serves as a web application the municipal services for the local municipality, featuring options to report issues, see local events with the use of advanced data structures as well as algorithmic reccomendations.

## South Afrcian Overview

The Western Cape Municipal Services Portal is a web-based solution meant to bridge the gap between local municipalities and SOuth African residents. The portal provides:

1. **Issue Reporting System** - Municipal issues can be reported by citizens with the ability to upload attachments with some gamification features.
2. **Local Events & Announcements** - Community members can discover, add, and search for local events with algorithmic powered recommendations

## Features

### Issue Reporting
- **Form Validation**: Location, Category, Description with attachment support
- **Gamification**: Tracking of prgress and a points system (10 points per submission)
- **File Upload**: Secure file validation (images, PDF, Word documents, max 10MB)
- **Recent Issues**: Displays the latest  reported issues with the IDs of the issue

### Events Management
- **Add Events**: Creating new events including their title, date, category, and description
- **Advanced Search**: Searching for events, category filtration, and date range queries
- **Smart Recommendations**: Algorithmic powered search suggestions based on user search patterns
- **Analytics**: Search statistics and user preference tracking

## Technical Stack

- **Framework**: ASP.NET Core 8.0 (Razor Pages)
- **Database**: SQLite with Entity Framework Core
- **Frontend**: Bootstrap 5, JavaScript, CSS3
- **Data Structures**: Stack, Queue, LinkedList, SortedDictionary, HashSet
- **Architecture**: Service-oriented with dependency injection

## 📁 Project Structure

```
MunicipalServicesApp/
├── Data/
│   └── ApplicationDbContext.cs     # Entity Framework context
├── DataStructures/
│   ├── Event.cs                    # Event model
│   ├── Issue.cs                    # Issue model
│   ├── SearchHistory.cs            # Search tracking
│   └── UserPreference.cs           # User preferences
├── Managers/
│   ├── EventManager.cs             # Event management service
│   └── IssueManager.cs             # Issue management service
├── Services/
│   └── RecommendationService.cs    # Algorithmic recommendation engine
├── Pages/
│   ├── Index.cshtml                # Home page
│   ├── ReportIssue.cshtml          # Issue reporting
│   ├── Events.cshtml               # Events management
│   ├── SearchEvents.cshtml         # Event search
│   ├── AddEvent.cshtml             # Add new events
│   ├── Analytics.cshtml            # Search analytics
│   └── Shared/
│       └── _Layout.cshtml          # Main layout
├── wwwroot/
│   ├── css/site.css                # Custom styles
│   ├── js/site.js                  # Client-side functionality
│   └── images/                     # Application assets
└── Program.cs                      # Application configuration
```

## Advanced Data Structures

### Stack Implementation
- **Issue Management**: LIFO processing for recent issues display
- **Search History**: Undo functionality for search operations

### Queue Implementation
- **Event Search Results**: FIFO processing to display search results
- **Search History**: Returns recent searches

### LinkedList Implementation
- **Event Recommendations**: Efficient insertion/deletion for dynamic recommendations
- **User Preferences**: Dynamic preference management

### SortedDictionary Implementation
- **Event Organization**: Events sorted by date for efficient retrieval
- **Category Management**: Sorted category display

### HashSet Implementation
- **Unique Categories**: Efficient category deduplication
- **Search Terms**: Unique search term tracking

## Setup Instructions

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code (optional)
- Web browser (Chrome, Firefox, Edge, Safari)

### Installation Steps

1. **Clone or Download** the project to your local machine

2. **Navigate** to the project directory:
   ```bash
   cd MunicipalServicesApp
   ```

3. **Restore** dependencies:
   ```bash
   dotnet restore
   ```

4. **Build** the application:
   ```bash
   dotnet build
   ```

5. **Run** the application:
   ```bash
   dotnet run
   ```

6. **Access** the application in your browser:
   - Open your web browser
   - Navigate to `http://localhost:5xxx` (port will be displayed in console)

## Usage Guide

### Home Page
- Welcome message and navigation overview
- Quick access to main features
- Application information

### Report an Issue
1. Navigate to "Report Issue" from the navbar
2. Fill in all the required fields as instructed (Location, Category, Description)
3. Optionally attach a file (images, PDF, Word documents)
4. The progress bar fills as you complete the fields
5. Submit to earn 10 points and see your issue ID

### Events Management
1. Navigate to "Events" to view all events
2. Use "Add Event" to create new events
3. Use "Search Events" for advanced filtering
4. Get personalized recommendations based on your search history
5. View analytics on the Analytics page

## Key Features

### File Validation
- **Size Limit**: 10MB maximum file size
- **File Types**: Images (JPG, PNG, GIF, BMP), PDF, Word documents
- **Security**: MIME type validation and extension checking
- **Client & Server**: Dual validation for security

### Recommendation Engine
- **User Preferences**: Tracks search patterns and preferences
- **Time Decay**: Recent searches weighted more heavily
- **Fallback System**: Popular events when no preferences exist
- **Analytics**: Search success rates and category popularity

### Gamification
- **Progress Tracking**: Real-time form completion progress
- **Points System**: 10 points per successful issue submission
- **Encouragement**: Motivational messages during form completion

## Technical Features

### Error Handling
- ModelState validation for all forms
- Try-catch blocks for exception handling
- User-friendly error messages
- Graceful degradation for missing data

### Performance
- Efficient data structure usage
- Optimized database queries
- Client-side validation for immediate feedback
- Responsive design for all devices

### Security
- Input validation and sanitization
- File upload restrictions and validation
- SQL injection prevention through Entity Framework
- XSS protection through Razor Pages

## Troubleshooting

### Common Issues

1. **Port Already in Use**
   - The application will automatically find an available port
   - Check the console output for the correct URL

2. **Database Issues**
   - The application will create the database automatically on first run
   - Sample data is loaded automatically

3. **File Upload Issues**
   - Ensure file size is under 10MB
   - Check file type is supported (images, PDF, Word documents)
   - Clear browser cache if validation errors persist

## Academic Integrity and References

### AI Assistance Declaration

The following areas received guidance from Claude (Anthropic, 2024), an AI programming assistant; all code was reviewed, adapted, and validated by the student:

- **File Validation Logic**: File type checking, size validation, and security measures
- **Advanced Data Structure Implementation**: Stack ordering, Queue processing, LinkedList operations
- **Recommendation Algorithm Structure**: Preference scoring and time decay logic
- **LINQ Query Optimization**: Search filtering and data processing

**Student Implementation**: Core business logic, UI integration, database design, service architecture, and overall application structure.

### References

**Books:**
- Troelsen, A. & Japikse, P. (2022). Pro C# 10 with .NET 6. Apress.

**Official Documentation:**
- Microsoft Corporation (2024). ASP.NET Core Razor Pages. Available at: https://docs.microsoft.com/en-us/aspnet/core/razor-pages/
- Microsoft Corporation (2024). Entity Framework Core. Available at: https://docs.microsoft.com/en-us/ef/core/

**Tutorials:**
- Code Maze (2024). ASP.NET Core Tutorials. Available at: https://code-maze.com/

**Videos:**
- IAmTimCorey (2024). Entity Framework Core Tutorials. YouTube. Available at: https://www.youtube.com/watch?v=qkJ9keBmQWo
- Raw Coding (2024). ASP.NET Core Razor Pages Tutorials. YouTube. Available at: https://youtu.be/W5T6713KRzg?si=el5RBXEhAepqnyTp

**AI Assistant:**
- Anthropic (2024). Claude AI Assistant. Available at: https://claude.ai/

---

**Western Cape Municipal Services App** - Connecting South African communities with their local government services through technology.