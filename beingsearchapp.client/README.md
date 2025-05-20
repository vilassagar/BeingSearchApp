# Bing Search React Application

A React application that leverages the Bing search API to provide search results.

## Features

- Search input with validation
- Display of search results
- TypeScript implementation
- Responsive design using Bootstrap
- Error handling and loading states

## Getting Started

### Prerequisites

- Node.js (v14 or higher)
- npm or yarn
- Bing Search API key

### Installation

1. Clone the repository:
```
git clone https://github.com/vilassagar/BeingSearchApp.git
cd BeingSearchApp
```

2. Install dependencies:
```
npm install
```

3. Set up your Bing Search API key:
   - Get a Bing Search API key from [Microsoft Azure Portal](https://portal.azure.com/)
   - Replace `YOUR_BING_API_KEY` in `src/utils/httpClient.ts` with your actual key
   - For production, use environment variables instead

4. Start the development server:
```
npm run dev
```

## Usage

1. Enter a search term in the search box
2. View the results below
3. Click on any result to open the webpage in a new tab

## Testing

Run the test suite:
```
npm test
```

## Build for Production

Create a production build:
```
npm run build
```

## Technologies Used

- React.js
- TypeScript
- Bootstrap for styling
- Axios for API requests
- Jest and React Testing Library for testing

## Areas for Future Improvement

- Add pagination for search results
- Implement more advanced search filters
- Add caching for recent searches
- Improve test coverage
- Add CI/CD pipeline
- Deploy to Azure

## License

MIT