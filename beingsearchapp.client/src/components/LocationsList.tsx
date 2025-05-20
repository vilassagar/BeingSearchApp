/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */

import React, { useState, useEffect } from 'react';
import { getAvailableLocations, filterLocationsByType } from '../services/locationService';
import type { LocationResponse } from '../types/locationTypes';

interface LocationsListProps {
    day?: string;
    type?: string;
}

const LocationsList: React.FC<LocationsListProps> = ({ day, type }) => {
    const [locations, setLocations] = useState<LocationResponse[]>([]);
    const [message, setMessage] = useState<string>('');
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchLocations = async () => {
            setIsLoading(true);
            setError(null);

            try {
                const response = await getAvailableLocations(day);

                // If type filter is provided, apply it
                let filteredLocations = response.locations;
                if (type) {
                    filteredLocations = filterLocationsByType(filteredLocations, type);
                }

                setLocations(filteredLocations);
               
                setMessage(response.message);
            } catch (err: any) {
                setError(err.response?.data?.error || 'Failed to fetch locations');
            } finally {
                setIsLoading(false);
            }
        };

        fetchLocations();
    }, [day, type]);

    if (isLoading) {
        return (
            <div className="locations-loading text-center my-5">
                <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                </div>
                <p className="mt-2">Loading locations...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="alert alert-danger" role="alert">
                <h4 className="alert-heading">Error!</h4>
                <p>{error}</p>
            </div>
        );
    }

    if (locations.length === 0) {
        return (
            <div className="alert alert-info" role="alert">
                No locations available. {message}
            </div>
        );
    }

    return (
        <div className="locations-list">
            <h2 className="mb-4">Available Locations</h2>
            <p className="mb-3">{message}</p>
            <div className="list-group">
                {locations.map((location) => (
                    <div key={location.id} className="list-group-item list-group-item-action">
                        <div className="d-flex justify-content-between align-items-center">
                            <h5 className="mb-1">{location.name}</h5>
                            <span className="badge bg-primary">{location.type}</span>
                        </div>
                        <p className="mb-1">{location.address}</p>
                        <div className="d-flex justify-content-between">
                            <small className="text-muted">
                                Hours: {formatTime(location.openTime)} - {formatTime(location.closeTime)}
                            </small>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

// Helper function to format time
const formatTime = (timeString: string): string => {
    const time = new Date(`2000-01-01T${timeString}`);
    return time.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
};

export default LocationsList;