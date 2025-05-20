import React from 'react';
import type { SearchResult } from '../types/types';

interface ResultsListProps {
    results: SearchResult[];
    isLoading: boolean;
    error?: string | null;
    theme?: 'light' | 'dark';
}

const ResultsList: React.FC<ResultsListProps> = ({ results, isLoading, error, theme = 'light' }) => {
    if (isLoading) {
        return (
            <div className={`card ${theme === 'dark' ? 'bg-dark text-light border-secondary' : ''} p-4 text-center`}>
                <div className="spinner-border text-primary mx-auto" role="status">
                    <span className="visually-hidden">Loading...</span>
                </div>
                <p className="mt-3 mb-0">Searching the web...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className={`alert ${theme === 'dark' ? 'alert-danger bg-dark text-danger border-danger' : 'alert-danger'}`} role="alert">
                <h4 className="alert-heading">Search Error</h4>
                <p>{error}</p>
            </div>
        );
    }

    if (results.length === 0) {
        return (
            <div className={`alert ${theme === 'dark' ? 'alert-info bg-dark text-info border-info' : 'alert-info'}`} role="alert">
                <p className="mb-0">No results found. Try a different search term.</p>
            </div>
        );
    }

    return (
        <div className={`results-list ${theme === 'dark' ? 'text-light' : ''}`}>
            <h2 className="mb-4 h5">Search Results ({results.length})</h2>
            <div className="list-group">
                {results.map((result) => (
                    <a                         // ← Opening <a> tag
                        key={result.id}        // ← Attributes inside the element tag
                        href={result.url}      // ← No spaces around =
                        target="_blank"
                        rel="noopener noreferrer"
                        className={`list-group-item list-group-item-action ${theme === 'dark' ? 'bg-dark text-light border-secondary' : ''}`}
                    >
                    
                <div className="d-flex justify-content-between align-items-center">
                    <h5 className="mb-1">{result.name}</h5>
                    <small className={`text-${theme === 'dark' ? 'light' : 'muted'}`}>
                        {new Date(result.dateLastCrawled).toLocaleDateString()}
                    </small>
                </div>
                <p className="mb-1" dangerouslySetInnerHTML={{ __html: result.snippet }}></p>
                <small className={`${theme === 'dark' ? 'text-light opacity-75' : 'text-muted'}`}>{result.url}</small>
            </a>
                ))}
        </div>
        </div >
    );
};

export default ResultsList;