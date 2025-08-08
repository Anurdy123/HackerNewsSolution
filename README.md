# Hacker News Reader

A full-stack application that displays the newest stories from Hacker News using an Angular frontend and a C# .NET Core backend RESTful API.

## Features

- Display newest stories from Hacker News
- Search functionality to find stories by title
- Pagination for better user experience
- Caching of newest stories for improved performance
- Automated tests for both frontend and backend

## Technologies Used

### Backend
- C# .NET Core 9.0
- ASP.NET Core Web API
- HttpClient for external API calls
- Memory Cache for caching
- NUnit for testing
- Moq for mocking dependencies

### Frontend
- Angular 20
- TypeScript
- HTML/CSS
- Jasmine/Karma for testing

## Prerequisites

- .NET 9.0 SDK
- Node.js (v18 or higher)
- npm (v8 or higher)

## Getting Started

### Backend Setup

1. Navigate to the backend project directory:
   ```bash
   cd HackerNewsSolution/HackerNewsAPI/HackerNewsAPI
   ```

2. Restore the NuGet packages:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the backend API:
   ```bash
   dotnet run
   ```

   The API will be available at `http://localhost:5101`

### Frontend Setup

1. Navigate to the frontend project directory:
   ```bash
   cd HackerNewsSolution/hacker-news-app
   ```

2. Install the dependencies:
   ```bash
   npm install
   ```

3. Run the frontend application:
   ```bash
   npm start
   ```

   The application will be available at `http://localhost:4200`

## API Endpoints

### Get New Stories
```
GET /api/hackernews/newstories?page=1&pageSize=20
```

Returns a list of newest stories from Hacker News.

### Search Stories
```
GET /api/hackernews/search?query=angular&page=1&pageSize=20
```

Returns a list of stories matching the search query.

## Running Tests

### Backend Tests

Navigate to the backend test directory and run:
```bash
cd HackerNewsSolution/HackerNewsAPI/HackerNewsAPI.Tests
dotnet test
```

### Frontend Tests

Navigate to the frontend directory and run:
```bash
cd HackerNewsSolution/hacker-news-app
npm test
```

## Project Structure

```
HackerNewsSolution/
├── HackerNewsAPI/                 # Backend .NET Core solution
│   ├── HackerNewsAPI/             # Main API project
│   │   ├── Controllers/           # API controllers
│   │   ├── Models/                # Data models
│   │   ├── Services/              # Business logic services
│   │   └── Program.cs             # Application entry point
│   └── HackerNewsAPI.Tests/       # Backend tests
└── hacker-news-app/               # Frontend Angular application
    ├── src/
    │   ├── app/
    │   │   ├── story-list/        # Story list component
    │   │   ├── hacker-news.service.ts  # API service
    │   │   └── hacker-news-item.ts     # Data model
    │   └── assets/
    └── e2e/                       # End-to-end tests
```

## Features Implementation

### Caching
The backend implements caching using `IMemoryCache` to store the newest stories for 5 minutes, reducing the number of calls to the Hacker News API.

### Pagination
Both the backend API and frontend UI implement pagination to display a manageable number of stories per page.

### Search
The search functionality allows users to filter stories by title keywords.

### Responsive Design
The frontend is designed to be responsive and work well on different screen sizes.

## Development

### Backend Development

The backend follows clean architecture principles with:
- Dependency injection for loose coupling
- Separation of concerns with services and controllers
- Error handling and logging

### Frontend Development

The frontend follows Angular best practices with:
- Component-based architecture
- Reactive programming with RxJS
- Service for API communication
- Template-driven forms

## Deployment

To deploy this application to Azure:

1. Publish the backend:
   ```bash
   cd HackerNewsSolution/HackerNewsAPI/HackerNewsAPI
   dotnet publish -c Release
   ```

2. Build the frontend for production:
   ```bash
   cd HackerNewsSolution/hacker-news-app
   npm run build
   ```

3. Deploy the published files to Azure App Service.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a pull request

## License

This project is licensed under the MIT License.