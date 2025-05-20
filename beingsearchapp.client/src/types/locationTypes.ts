
export interface TimeSlot {
    dayOfWeek: number; // 0 = Sunday, 1 = Monday, etc.
    openTime: string;
    closeTime: string;
}

export interface Location {
    id: number;
    name: string;
    type: string;
    address: string;
    availableTimeSlots: TimeSlot[];
}

export interface LocationResponse {
    id: number;
    name: string;
    type: string;
    address: string;
    openTime: string;
    closeTime: string;
    isAvailableBetween10And1: boolean;
}

export interface LocationsResponse {
    locations: LocationResponse[];
    totalCount: number;
    message: string;
}