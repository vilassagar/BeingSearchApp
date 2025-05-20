
import React, { useState } from 'react';

interface SearchBarProps {
    onSearch: (query: string) => void;
    isLoading: boolean;
    theme?: 'light' | 'dark';
}

const SearchBar: React.FC<SearchBarProps> = ({ onSearch, isLoading, theme = 'light' }) => {
    const [query, setQuery] = useState('');
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        // Validate input
        if (!query.trim()) {
            setError('Please enter a search term');
            return;
        }

        if (query.length < 3) {
            setError('Search term must be at least 3 characters');
            return;
        }

        setError(null);
        onSearch(query);
    };

    return (
        <div className={`search-bar-container ${theme === 'dark' ? 'bg-dark text-light' : ''}`}>
            <form onSubmit={handleSubmit} className="search-form">
                <div className="input-group">
                    <input
                        type="text"
                        className={`form-control ${error ? 'is-invalid' : ''} ${theme === 'dark' ? 'bg-dark text-light border-secondary' : ''}`}
                        placeholder="Search the web..."
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        disabled={isLoading}
                        aria-label="Search query"
                    />
                    <button
                        type="submit"
                        className={`btn ${theme === 'dark' ? 'btn-outline-light' : 'btn-primary'}`}
                        disabled={isLoading}
                    >
                        {isLoading ? (
                            <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                        ) : (
                            'Search'
                        )}
                    </button>
                </div>
                {error && <div className="invalid-feedback d-block">{error}</div>}
            </form>
        </div>
    );
};

export default SearchBar;