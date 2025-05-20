import { fetchAvailableLocations, fetchLocationById, createLocation } from './api/locationApi';
import type { LocationsResponse, Location, LocationResponse } from '../types/locationTypes';

// Business logic for handling locations
export const getAvailableLocations = async (day?: string): Promise<LocationsResponse> => {
    try {
        const response = await fetchAvailableLocations(day);
        return response;
    } catch (error) {
        console.error('Error fetching available locations:', error);
        throw error;
    }
};

export const getLocationById = async (id: number): Promise<Location> => {
    try {
        return await fetchLocationById(id);
    } catch (error) {
        console.error(`Error fetching location with ID ${id}:`, error);
        throw error;
    }
};

export const addLocation = async (location: Omit<Location, 'id'>): Promise<Location> => {
    try {
        return await createLocation(location);
    } catch (error) {
        console.error('Error adding location:', error);
        throw error;
    }
};

// Additional business logic methods
export const filterLocationsByType = (locations: LocationResponse[], type: string): LocationResponse[] => {
    return locations.filter(location => location.type.toLowerCase() === type.toLowerCase());
};

export const getOpenLocations = async (day?: string): Promise<LocationResponse[]> => {
    const response = await getAvailableLocations(day);
    return response.locations.filter(location => location.isAvailableBetween10And1);
};

export const getLocationTypes = async (): Promise<string[]> => {
    const response = await getAvailableLocations();
    const types = new Set(response.locations.map(location => location.type));
    return Array.from(types);
};