import { locationsAxios } from '../../utils/httpClient';
import type { LocationsResponse, Location } from '../../types/locationTypes';

// Raw API calls for locations
export const fetchAvailableLocations = async (day?: string): Promise<LocationsResponse> => {
    const params = day ? { day } : {};
    const response = await locationsAxios.get('/locations/available', { params });
    return response.data;
};

export const fetchLocationById = async (id: number): Promise<Location> => {
    const response = await locationsAxios.get(`/locations/${id}`);
    return response.data;
};

export const createLocation = async (location: Omit<Location, 'id'>): Promise<Location> => {
    const response = await locationsAxios.post('/locations', location);
    return response.data;
};