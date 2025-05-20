/* eslint-disable @typescript-eslint/no-explicit-any */
// src/App.tsx
import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import SearchBar from './components/SearchBar';
import ResultsList from './components/ResultsList';
import LocationsPage from './pages/LocationsPage';
import { searchBing } from './services/api/bingService';
import type { SearchResult } from './types/types';

const App: React.FC = () => {
    // State for the search functionality
    const [results, setResults] = useState<SearchResult[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // State for the application
    const [activeTab, setActiveTab] = useState<'search' | 'locations'>('search');
    const [theme, setTheme] = useState<'light' | 'dark'>('light');

    // Handle search
    const handleSearch = async (query: string) => {
        if (!query.trim()) {
            setError('Please enter a search query');
            return;
        }

        setIsLoading(true);
        setError(null);

        try {
            const response = await searchBing(query);

            if (response.errors) {
                setError(response.errors[0].message);
                setResults([]);
            } else if (response.webPages?.value) {
                setResults(response.webPages.value);

                // If no results found
                if (response.webPages.value.length === 0) {
                    setError('No results found for your search query. Please try different keywords.');
                }
            } else {
                setResults([]);
                setError('No results found. Please try a different search term.');
            }
        } catch (err: any) {
            console.error('Search error:', err);
            setError('An unexpected error occurred. Please try again.');
            setResults([]);
        } finally {
            setIsLoading(false);
        }
    };

    // Toggle theme
    const toggleTheme = () => {
        setTheme(prevTheme => prevTheme === 'light' ? 'dark' : 'light');
    };

    // Apply theme class to body
    React.useEffect(() => {
        document.body.className = theme === 'dark' ? 'bg-dark text-light' : 'bg-light text-dark';
    }, [theme]);

    return (
        <div className={`app-container ${theme === 'dark' ? 'bg-dark text-light' : ''}`}>
            <header className={`app-header ${theme === 'dark' ? 'bg-dark' : 'bg-primary'} text-white text-center py-4 mb-4`}>
                <div className="container">
                    <div className="d-flex justify-content-between align-items-center">
                        <h1 className="mb-0">Bing Search</h1>
                        <button
                            className={`btn ${theme === 'dark' ? 'btn-light' : 'btn-dark'} btn-sm`}
                            onClick={toggleTheme}
                            aria-label={`Switch to ${theme === 'light' ? 'dark' : 'light'} mode`}
                        >
                            {theme === 'light' ? '🌙' : '☀️'}
                        </button>
                    </div>

                    <ul className="nav nav-tabs mt-3 justify-content-center">
                        <li className="nav-item">
                            <button
                                className={`nav-link ${activeTab === 'search' ? 'active bg-white text-primary' : 'text-white'}`}
                                onClick={() => setActiveTab('search')}
                            >
                                <i className="bi bi-search me-2"></i>
                                Search
                            </button>
                        </li>
                        <li className="nav-item">
                            <button
                                className={`nav-link ${activeTab === 'locations' ? 'active bg-white text-primary' : 'text-white'}`}
                                onClick={() => setActiveTab('locations')}
                            >
                                <i className="bi bi-geo-alt me-2"></i>
                                Locations
                            </button>
                        </li>
                    </ul>
                </div>
            </header>

            <main className={`container ${theme === 'dark' ? 'bg-dark text-light' : ''}`}>
                {activeTab === 'search' ? (
                    <div className="row justify-content-center">
                        <div className="col-lg-8">
                            <div className={`card ${theme === 'dark' ? 'bg-dark text-light border-secondary' : ''} mb-4`}>
                                <div className="card-body">
                                    <h2 className="card-title h4 mb-3">Web Search</h2>
                                    <SearchBar onSearch={handleSearch} isLoading={isLoading} />
                                </div>
                            </div>

                            <div className="mt-4">
                                <ResultsList
                                    results={results}
                                    isLoading={isLoading}
                                    error={error}
                                    theme={theme}
                                />
                            </div>
                        </div>
                    </div>
                ) : (
                    <LocationsPage theme={theme} />
                )}
            </main>

            <footer className={`app-footer ${theme === 'dark' ? 'bg-dark text-light border-top border-secondary' : 'bg-light'} py-3 mt-5`}>
                <div className="container text-center">
                    <p className="mb-0">
                        © {new Date().getFullYear()} Multi-App Platform |
                        <a href="#" className={`ms-2 ${theme === 'dark' ? 'text-light' : ''}`}>About</a> |
                        <a href="#" className={`ms-2 ${theme === 'dark' ? 'text-light' : ''}`}>Contact</a>
                    </p>
                </div>
            </footer>
        </div>
    );
};

export default App;